namespace ACommunicator.Models
{
    public class OptionItemViewModel : Option
    {
        public string Action { get; set; }
        public string Controller { get; set; }

        /// <summary>
        /// Constructor for OptionItemViewModel class
        /// </summary>
        /// <param name="option">Option object</param>
        /// <param name="action">Controller Action name that needs to be called
        ///  when user clicks on this option</param>
        /// <param name="controller">Controller name (without Controller suffix)</param>
        public OptionItemViewModel(Option option, string action, string controller)
        {
            Id = option.Id;
            Title = option.Title;
            Description = option.Description;
            PicturePath = option.PicturePath;
            SoundPath = option.SoundPath;
            Action = action;
            Controller = controller;
        }
    }
}