using System.ComponentModel.DataAnnotations;
using System.Web;
using ACommunicator.Properties;
using ACommunicator.ValidationAttributes;

namespace ACommunicator.Models
{
    public class RegisterChildViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RequiredUsername")]
        [Display(ResourceType = typeof(Resources), Name = "Username")]
        public string Username { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Name")]
        public string Name { get; set; }

        // TODO: decide if this is Image after all?
        [FileSize(10240)]
        [FileTypes("jpg,jpeg,png")]
        public HttpPostedFileBase Picture { get; set; }
    }
}