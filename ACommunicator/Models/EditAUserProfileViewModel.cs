using System.ComponentModel.DataAnnotations;
using ACommunicator.Properties;

namespace ACommunicator.Models
{
    public class EditAUserProfileViewModel
    {
        public AUser AUser { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Password")]
        public string NewPassword { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}