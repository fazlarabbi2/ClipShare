using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClipShare.ViewModels.Video
{
    public class VideoAddEdit_Vm
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Upload Thumnail Here")]
        public IFormFile ImageUpload { get; set; }

        [Display(Name = "Upload Video Here")]
        public IFormFile VideoUpload { get; set; }

        [Display(Name = "Choose the category for your video")]
        [Required(ErrorMessage = "Please chose a category")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> CategoryDropDown { get; set; }
        public string ImageContentTypes { get; set; }
        public string VideoContentTypes { get; set; }
        public string ImageUrl { get; set; }
    }
}
