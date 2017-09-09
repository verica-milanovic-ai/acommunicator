namespace ACommunicator.Models
{
    public class OptionItemViewModel : OptionMediaModel
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
        public OptionItemViewModel(OptionMediaModel option, string action, string controller) : base(option)
        {
            Image = option.Image;
            Sound = option.Sound;
            Action = action;
            Controller = controller;
        }

        public OptionItemViewModel(OptionMediaModel option) : base(option)
        {
            Image = option.Image;
            Sound = option.Sound;
        }
    }
}