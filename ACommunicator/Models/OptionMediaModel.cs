namespace ACommunicator.Models
{
    public class OptionMediaModel : Option
    {
        public string Image { get; set; }

        public string Sound { get; set; }

        public OptionMediaModel(Option option)
        {
            Name = option.Name;
            Description = option.Description;
            FolderID = option.FolderID;
            ParentFolderID = option.ParentFolderID;
            Level = option.Level;
            Id = option.Id;
        }
    }
}