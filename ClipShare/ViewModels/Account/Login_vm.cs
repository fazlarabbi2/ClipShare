using System.ComponentModel.DataAnnotations;

namespace ClipShare.ViewModels.Account
{
    public class Login_vm
    {
        private string _userName;
        [Display(Name = "Put your username or Email")]
        [Required(ErrorMessage = "Username is Required")]
        public string UserName
        {
            get => _userName;
            set => _userName = value?.ToLower();
        }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
