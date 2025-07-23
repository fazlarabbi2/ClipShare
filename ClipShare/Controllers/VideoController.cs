using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using ClipShare.Entities;
using ClipShare.Extensions;
using ClipShare.Services.IServices;
using ClipShare.Utility;
using ClipShare.ViewModels.Video;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

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

        public async Task<IActionResult> CreateEditVideo(int id)
        {
            if (!await UnitOfWork.ChannelRepo.AnyAsync(x => x.AppUserId == User.GetUserId()))
            {
                TempData["notification"] =
                    "false; Not Found; No Channel associated with you account was found";
                return RedirectToAction("Index", "Channel");
            }

            var toReturn = new VideoAddEdit_Vm();
            toReturn.ImageContentTypes = string.Join(",", AcceptableContentTypes("image"));
            toReturn.VideoContentTypes = string.Join(",", AcceptableContentTypes("video"));

            if (id > 0)
            {
                // Edit part
            }

            toReturn.CategoryDropDown = await GetCategoryDropdownAsync();

            return View(toReturn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEditVideo(VideoAddEdit_Vm model)
        {
            // Handle null model case
            //if (model == null)
            //{
            //    model = new VideoAddEdit_Vm();
            //}

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
                return Configuration.GetSection("FileUpload:VideoContentType").Get<string[]>();
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
