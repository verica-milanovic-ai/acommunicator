using System.Collections.Generic;

namespace ACommunicator.Models
{
    public class EndUserItemViewModel : EndUser
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public List<OptionItemViewModel> SelectedOptions { get; set; }
    }
}