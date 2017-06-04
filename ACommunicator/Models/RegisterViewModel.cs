using System.ComponentModel.DataAnnotations;
using ACommunicator.Properties;

namespace ACommunicator.Models
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RequiredConfirmPassword")]
        [Display(ResourceType = typeof(Resources), Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RequiredEmail")]
        [Display(ResourceType = typeof(Resources), Name = "Email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "WrongEmailFormat")]
        public string Email { get; set; }
    }
}