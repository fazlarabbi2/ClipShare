using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClipShare.ViewModels.Channel
{
    public class ChannelAddEdit_vm
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[a-zA-Z]{3,15}$", ErrorMessage = "Name must be between 3 and 15 characters long and can only contain letters (A-Z, a-z)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "About Field is Required")]
        [StringLength(200, MinimumLength = 20, ErrorMessage = "About must be at least 20, and maximum 200 characters")]
        [Display(Name = "About your channel")]
        public string About { get; set; }

        public List<ModelError_vm> Errors { get; set; } = [];
    }
}
