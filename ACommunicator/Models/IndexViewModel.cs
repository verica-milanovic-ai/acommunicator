using System.Collections.Generic;

namespace ACommunicator.Models
{
    public class IndexViewModel
    {
        public int SelectedEndUserId { get; set; }

        public ICollection<EndUser> EndUserList { get; set; }
    }
}