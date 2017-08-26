using System.Configuration;

namespace ACommunicator.Helpers
{
    public class AppSettings
    {
        public static string EndUserProfilePictureNamePattern { get; set; }
        public static string EndUserProfilePictureDirectory { get; set; }
        public static string DefaultProfilePictureFileName { get; set; }

        public static string ACommunicatorPhotosDriveFolderId { get; set; }
        public static string ACommunicatorOptionPhotosDriveFolderId { get; set; }

        static AppSettings()
        {
            EndUserProfilePictureNamePattern = ConfigurationManager.AppSettings["EndUserProfilePictureNamePattern"] ?? string.Empty;
            EndUserProfilePictureDirectory = ConfigurationManager.AppSettings["EndUserProfilePictureDirectory"] ?? string.Empty;
            DefaultProfilePictureFileName = ConfigurationManager.AppSettings["DefaultProfilePictureFileName"] ?? string.Empty;

            ACommunicatorPhotosDriveFolderId = ConfigurationManager.AppSettings["ACommunicatorPhotosDriveFolderId"] ?? string.Empty;
            ACommunicatorOptionPhotosDriveFolderId = ConfigurationManager.AppSettings["ACommunicatorOptionPhotosDriveFolderId"] ?? string.Empty;
        }
    }
}