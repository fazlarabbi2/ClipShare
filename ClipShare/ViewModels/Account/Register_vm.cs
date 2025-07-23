using System.ComponentModel.DataAnnotations;

namespace ClipShare.ViewModels.Account
{
    public class Register_vm
    {
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Display(Name = ("User Name Required"))]
        [Required(ErrorMessage = "Name (UserName) is required)")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Name must be atleast {2}, and maximum {1} characters")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Name must contain only letters and numbers")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])[0-9a-zA-Z]{6,15}$", ErrorMessage = "Password must be 6-15 characters long, contain only letters and numbers, and include at least one letter and one number.")]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is Required")]
        public string ConfirmPassword { get; set; }
    }
}
