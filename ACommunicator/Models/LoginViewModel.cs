using System.ComponentModel.DataAnnotations;
using ACommunicator.Properties;

namespace ACommunicator.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RequiredUsername")]
        [Display(ResourceType = typeof(Resources), Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RequiredPassword")]
        [Display(ResourceType = typeof(Resources), Name = "Password")]
        public string Password { get; set; }
    }
}