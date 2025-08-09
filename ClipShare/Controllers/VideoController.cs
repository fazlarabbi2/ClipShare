using ClipShare.Core.DTOs;
using ClipShare.Core.Entities;
using ClipShare.Core.Pagination;
using ClipShare.Entities;
using ClipShare.Extensions;
using ClipShare.Services.IServices;
using ClipShare.Utility;
using ClipShare.ViewModels;
using ClipShare.ViewModels.Video;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Controllers
{
    [Authorize(Roles = SD.UserRole)]
    public class VideoController : CoreController
    {
        private readonly IConfiguration configuration;
        private readonly IPhotoService photoService;

        public VideoController(IConfiguration configuration, IPhotoService photoService)
        {
            this.configuration = configuration;
            this.photoService = photoService;
        }

        public async Task<IActionResult> Watch(int id)
        {
            var fetchedVideo = await UnitOfWork.VideoRepo.GetFirstOrDefaultAsync(x => x.Id == id);

            if (fetchedVideo != null)
            {
                var toReturn = new VideoWatch_vm();

                toReturn.Id = fetchedVideo.Id;
                toReturn.Title = fetchedVideo.Title;
                toReturn.Description = fetchedVideo.Description;
                toReturn.CreatedAt = fetchedVideo.CreatedAt;
                toReturn.ChannelId = fetchedVideo.ChannelId;

                //toReturn.IsLiked = true;
                //toReturn.IsDisliked = true;

                toReturn.SubscribersCount = SD.GetRandomNumber(1, 4000, id);
                toReturn.ViewersCount = SD.GetRandomNumber(1000, 5000, id);
                toReturn.LikesCount = SD.GetRandomNumber(5, 1000, id);
                toReturn.DisLikesCount = SD.GetRandomNumber(1, 500, id);

                return View(toReturn);
            }
            TempData["notification"] = "false; Not Found; Requested video was not found";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GetVideoFile(int videoId)
        {
            var fetchedVideoFile = await UnitOfWork.VideoFileRepo.GetFirstOrDefaultAsync(x => x.VideoId == videoId);

            if (fetchedVideoFile != null)
            {
                return File(fetchedVideoFile.Contents, fetchedVideoFile.ContentType);
            }

            TempData["notification"] = "false; Not Found; Requested video was not found";
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> DownloadVideoFile(int videoId)
        {
            var fetchedVideo = await UnitOfWork.VideoRepo.GetFirstOrDefaultAsync(x => x.Id == videoId, "VideoFile");

            if (fetchedVideo != null)
            {
                string fileDownloadName = fetchedVideo.Title + fetchedVideo.VideoFile.Extension;
                return File(fetchedVideo.Contents, fetchedVideo.VideoFile.ContentType, fileDownloadName);
            }

            TempData["notification"] = "false; Not Found; Requested video was not found";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateEditVideo(int id)
        {
            if (!await UnitOfWork.ChannelRepo.AnyAsync(x => x.AppUserId == User.GetUserId()))
            {
                TempData["notification"] =
                    "false; Not Found; No Channel associated with you account was found";
                return RedirectToAction("Index", "Channel");
            }

            var toReturn = new VideoAddEdit_Vm
            {
                ImageContentTypes = string.Join(",", AcceptableContentTypes("image")),
                VideoContentTypes = string.Join(",", AcceptableContentTypes("video"))
            };

            if (id > 0)
            {
                // Edit part
                var userId = await UnitOfWork.VideoRepo.GetUserIdByVideoId(id);

                if (!userId.Equals(User.GetUserId()))
                {
                    TempData["notification"] = "false;Not Found; Requested video not found.";
                    return RedirectToAction("Index", "Channel");
                }

                var fetchedVideo = await UnitOfWork.VideoRepo.GetByIdAsync(id);

                if (fetchedVideo == null)
                {
                    TempData["notification"] = "false;Not Found; Requested video not found.";
                    return RedirectToAction("Index", "Channel");
                }

                toReturn.Id = fetchedVideo.Id;
                toReturn.Title = fetchedVideo.Title;
                toReturn.Description = fetchedVideo.Description;
                toReturn.CategoryId = fetchedVideo.CategoryId;
                toReturn.ImageUrl = fetchedVideo.ThumbnailUrl;
            }

            toReturn.CategoryDropDown = await GetCategoryDropdownAsync();

            return View(toReturn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEditVideo(VideoAddEdit_Vm model)
        {
            if (ModelState.IsValid)
            {
                bool proceed = true;

                if (model.Id == 0)
                {
                    if (model.ImageUpload == null)
                    {
                        ModelState.AddModelError("ImageUpload", "Please upload thumbnail");
                        proceed = false;
                    }
                    if (proceed && model.VideoUpload == null)
                    {
                        ModelState.AddModelError("VideoUpload", "Please upload your video");
                        proceed = false;
                    }
                }

                if (model.ImageUpload != null)
                {
                    if (proceed && !IsAcceptableContentType("image", model.ImageUpload.ContentType))
                    {
                        ModelState.AddModelError(
                            "ImageUpload",
                            string.Format(
                                "Invalid content type. It must be one of the following: {0}",
                                string.Join(", ", AcceptableContentTypes("image"))
                            )
                        );
                        proceed = false;
                    }
                    if (
                        proceed
                        && model.ImageUpload.Length
                            > int.Parse(Configuration["FileUpload:ImageMaxSizeInMB"]) * SD.MB
                    )
                    {
                        ModelState.AddModelError(
                            "ImageUpload",
                            string.Format(
                                "File too large. Max: {0} MB",
                                Configuration["FileUpload:ImageMaxSizeInMB"]
                            )
                        );
                        proceed = false;
                    }
                }

                if (model.VideoUpload != null)
                {
                    if (proceed && !IsAcceptableContentType("video", model.VideoUpload.ContentType))
                    {
                        ModelState.AddModelError(
                            "VideoUpload",
                            $"Invalid content type. Allowed: {string.Join(", ", AcceptableContentTypes("video"))}"
                        );
                        proceed = false;
                    }
                    if (proceed && model.VideoUpload.Length
                            > int.Parse(Configuration["FileUpload:VideoMaxSizeInMB"]) * SD.MB
                    )
                    {
                        ModelState.AddModelError(
                            "VideoUpload",
                            $"File too large. Max: {Configuration["FileUpload:VideoMaxSizeInMB"]} MB"
                        );
                        proceed = false;
                    }
                }

                if (proceed)
                {
                    string title;
                    string message;

                    if (model.Id == 0)
                    {
                        // For create
                        var videoToAdd = new Video()
                        {
                            Title = model.Title,
                            Description = model.Description,
                            ContentType = model.VideoUpload.ContentType, 
                            Contents = GetContentsAsync(model.VideoUpload).GetAwaiter().GetResult(), 
                            VideoFile = new VideoFile
                            {
                                ContentType = model.VideoUpload.ContentType,
                                Contents = GetContentsAsync(model.VideoUpload).GetAwaiter().GetResult(),
                                Extension = SD.GetFileExtension(model.VideoUpload.ContentType)
                            },
                            CategoryId = model.CategoryId,
                            ChannelId = UnitOfWork.ChannelRepo.GetChannelIdByUserId(User.GetUserId()).GetAwaiter().GetResult(),
                            ThumbnailUrl = photoService.UploadPhotoLocally(model.ImageUpload), // some url that we going to provide
                        };

                        UnitOfWork.VideoRepo.Add(videoToAdd);

                        title = "Created";
                        message = "New video has been created";
                    }
                    else
                    {
                        // for update
                        var fetchedVideo = await UnitOfWork.VideoRepo.GetByIdAsync(model.Id);

                        if (fetchedVideo == null)
                        {
                            TempData["notification"] =
                                "false; Not Found; Requested video was not found";
                            return RedirectToAction("Index", "Channel");
                        }

                        fetchedVideo.Title = model.Title;
                        fetchedVideo.Description = model.Description;
                        fetchedVideo.CategoryId = model.CategoryId;

                        if (model.ImageUpload != null)
                        {
                            // handle re uploading the image file
                            fetchedVideo.ThumbnailUrl = photoService.UploadPhotoLocally(model.ImageUpload, fetchedVideo.ThumbnailUrl);
                        }

                        title = "Editted";
                        message = "Video has been Updated";
                    }

                    TempData["notification"] = $"true;{title};{message}";
                    await UnitOfWork.CompleteAsync();
                    return RedirectToAction("Index", "Channel");
                }
            }

            // Repopulate dropdown and content types before returning view
            model.CategoryDropDown = await GetCategoryDropdownAsync();
            model.ImageContentTypes = string.Join(",", AcceptableContentTypes("image"));
            model.VideoContentTypes = string.Join(",", AcceptableContentTypes("video"));

            return View(model);
        }

        #region API Endpoints
        [HttpGet]
        public async Task<IActionResult> GetVideosForChannelGrid(BaseParameters parameters)
        {
            var userForChannelId = await UnitOfWork.ChannelRepo.GetChannelIdByUserId(User.GetUserId());
            var videosForGrid = await UnitOfWork.VideoRepo.GetVideosForChannelGridAsync(userForChannelId, parameters);
            var paginatedResults = new PaginatedResult<VideoGridChannelDto>(videosForGrid, videosForGrid.TotalItemsCount, videosForGrid.PageNumber, videosForGrid.PageSize, videosForGrid.TotalPages);

            return Json(new ApiResponse(200, result: paginatedResults));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var video = await UnitOfWork.VideoRepo.GetFirstOrDefaultAsync(x => x.Id == id && x.Channel.AppUserId == User.GetUserId());

            if (video != null)
            {
                UnitOfWork.VideoRepo.Remove(video);
                await UnitOfWork.CompleteAsync();

                return Json(new ApiResponse(200, "Deleted", "Your Video of " + video.Title + " has been deleted"));
            }

            return Json(new ApiResponse(404, message: "The requested video was not Found"));
        }

        [HttpPut]
        public async Task<IActionResult> SubscribeChannel(int channelId)
        {
            var channel = await UnitOfWork.ChannelRepo.GetFirstOrDefaultAsync(x => x.Id == channelId, "Subscribers");

            if (channel != null)
            {
                int userId = User.GetUserId();

                var fetchedSubscribe = channel.Subscribers.Where(x => x.ChannelId == channelId && x.AppUserId == userId).FirstOrDefault();

                if (fetchedSubscribe == null)
                {
                    // Subscribe
                    channel.Subscribers.Add(new Subscribe(userId, channelId));

                    await UnitOfWork.CompleteAsync();
                    return Json(new ApiResponse(200, "Subscribed", "Subscribed"));
                }
                else
                {
                    // Unsubscribe
                    channel.Subscribers.Remove(fetchedSubscribe);
                    await UnitOfWork.CompleteAsync();
                    return Json(new ApiResponse(200, "UnSubscribed", "Subscribed"));
                }
            }

            return Json(new ApiResponse(404, message: "Channel was not found"));
        }
        #endregion

        #region Private Methods
        public async Task<IEnumerable<SelectListItem>> GetCategoryDropdownAsync()
        {
            var allCategories = await UnitOfWork.CategoryRepo.GetAllAsync();

            return allCategories.Select(category => new SelectListItem()
            {
                Text = category.Name,
                Value = category.Id.ToString(),
            });
        }

        private string[] AcceptableContentTypes(string type)
        {
            if (type.Equals("image"))
            {
                return Configuration.GetSection("FileUpload:ImageContentTypes").Get<string[]>();
            }
            else
            {
                return Configuration.GetSection("FileUpload:VideoContentTypes").Get<string[]>();
            }
        }

        private bool IsAcceptableContentType(string type, string contentType)
        {
            var allowedContentTypes = AcceptableContentTypes(type);

            foreach (var allowedContentType in allowedContentTypes)
            {
                if (contentType.ToLower().Equals(allowedContentType.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }


        private async Task<byte[]> GetContentsAsync(IFormFile file)
        {
            byte[] contents;

            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);
            contents = memoryStream.ToArray();

            return contents;
        }
        #endregion
    }
}
