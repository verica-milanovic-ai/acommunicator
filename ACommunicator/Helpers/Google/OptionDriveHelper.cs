using System.Linq;
using ACommunicator.Helpers.Google;
using ACommunicator.Models;

namespace ACommunicator.Helpers
{
    public static partial class OptionHelper
    {
        public static OptionMediaModel GetOption(Option option)
        {
            var files = DriveHelper.GetFileList(0, option.Name, option.FolderID);
            var returnValue = new OptionMediaModel(option);

            if (files == null) return null;

            returnValue.Image = files.Items.Count > 0 ? files.Items.FirstOrDefault(x => x.MimeType == "image/jpeg")?.ThumbnailLink : "~/Content/imgs/default.png";
            returnValue.Sound = files.Items.Count > 0 ? files.Items.FirstOrDefault(x => x.MimeType == "audio/wav")?.DownloadUrl : null;
            return returnValue;
        }

        public static OptionMediaModel AddOption(OptionMediaModel optionMediaModel, string endUserId)
        {
            //TODO: save media on drive

            AddOption(new Option(optionMediaModel), endUserId);
            return optionMediaModel;
        }

        public static bool UpdateOption(OptionMediaModel optionMediaModel, string endUserId)
        {
            // TODO: edit media on drive

            return UpdateOption(new Option(optionMediaModel), endUserId);
        }

        public static bool RemoveOption(OptionMediaModel optionMediaModel, string endUserId)
        {
            // TODO: remove media from drive if option is not default one

            return RemoveOption(new Option(optionMediaModel), endUserId);
        }


    }
}