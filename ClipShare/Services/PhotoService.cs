using ClipShare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace ClipShare.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public PhotoService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public string UploadPhotoLocally(IFormFile photo, string oldPhotoUrl = "")
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            string uploadsDirectory = Path.Combine(webRootPath, @"images\thumbnails");

            // Check if the directory exists, otherwise create
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            string fileName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(photo.FileName);

            if (!string.IsNullOrEmpty(oldPhotoUrl))
            {
                // replace the image
                var oldImagePath = Path.Combine(webRootPath, oldPhotoUrl.TrimStart('\\'));

                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }

            using var fileStream = new FileStream(Path.Combine(uploadsDirectory, fileName + extension), FileMode.Create);

            photo.CopyTo(fileStream);

            return @"\images\thumbnails\" + fileName + extension;
        }
    }
}
