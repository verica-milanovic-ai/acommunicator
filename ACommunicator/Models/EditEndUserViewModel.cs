using System.ComponentModel.DataAnnotations;
using System.Web;
using ACommunicator.Properties;
using ACommunicator.ValidationAttributes;

namespace ACommunicator.Models
{
    public class EditEndUserViewModel
    {
        public EndUser EndUser { get; set; }
        public bool? IsSuccessfullySaved { get; set; }
        [FileSize(1024000)]
        [FileTypes("jpg,jpeg,png")]
        [Display(ResourceType = typeof(Resources), Name = "UploadPhoto")]
        public HttpPostedFileBase NewPicture { get; set; }
    }
}