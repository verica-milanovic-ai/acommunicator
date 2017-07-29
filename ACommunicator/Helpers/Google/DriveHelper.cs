using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using log4net;

namespace ACommunicator.Helpers.Google
{
    /// <summary>
    /// Helper class for Google Drive
    /// Use it to get file list or to upload files
    /// </summary>
    public class DriveHelper
    {
        public static readonly ILog Log = LogManager.GetLogger("LogInfo");
        public static DriveService DriveService { get; set; }

        // Static constructor - initialzes static data - performed only once
        // Called automatically before the first instance is created or any static members are referenced
        static DriveHelper()
        {
            var scopes = new[] { DriveService.Scope.Drive }; // Full access


            var keyFilePath = HttpContext.Current.Server.MapPath("..\\ACommunicator-c4d6e0c7cd69.p12"); // Downloaded from https://console.developers.google.com
            var serviceAccountEmail = "acommunicator@acommunicator-175210.iam.gserviceaccount.com"; // found https://console.developers.google.com

            //loading the Key file
            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = scopes
            }.FromCertificate(certificate));

            DriveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ACommunicator"
            });

        }

        /// <param name="maxResults">Used  to tell number of maximum results to return.
        /// If it's equal or less than 0, reutrns all files</param>
        /// <param name="search">Used to filter file list by file name containing string.
        /// If it's null, returns all files</param>
        /// <returns>File list</returns>
        public static FileList GetFileList(int maxResults = 1000, string search = null)
        {
            var request = DriveService.Files.List();
            if (maxResults > 0)
            {
                request.MaxResults = maxResults;
            }

            if (!string.IsNullOrEmpty(search))
            {
                request.Q = "name contains " + search;
            }

            var files = request.Execute();
            return files;
        }

        public static IEnumerable<File> GetFiles(string search = null)
       {
            var request = DriveService.Files.List();
            request.Q = "'" + AppSettings.ACommunicatorPhotosDriveFolderId + "' in parents";
            request.MaxResults = 1;
            
            var files = request.Execute();

            return !string.IsNullOrEmpty(search) ? files.Items.Where(x=>x.Title.Contains(search)) : files.Items.ToList();
        }

        /// <summary>
        /// Creates folder
        /// </summary>
        /// <param name="title">Name for folder</param>
        /// <param name="description">Description for folder</param>
        /// <param name="parent">Parent folder</param>
        /// <returns>Newly created folder as File type</returns>
        public static File CreateDirectory(string title, string description, string parent)
        {
            File newDirectory = null;

            // Create metaData for a new Directory
            var body = new File
            {
                Title = title,
                Description = description,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<ParentReference>() { new ParentReference() { Id = parent } }
            };
            try
            {
                var request = DriveService.Files.Insert(body);
                newDirectory = request.Execute();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return newDirectory;
        }

        /// <summary>
        /// Uploads file
        /// </summary>
        /// <param name="uploadFile">File to be uploaded</param>
        /// <param name="parent">Direcroty where to upload</param>
        /// <returns>Uploaded file</returns>
        public static File UploadFile(string uploadFile, string parent)
        {
            if (System.IO.File.Exists(uploadFile))
            {
                var body = new File
                {
                    Title = System.IO.Path.GetFileName(uploadFile),
                    Description = "description",
                    MimeType = GetMimeType(uploadFile),
                    Parents = new List<ParentReference>() { new ParentReference() { Id = parent } }
                };

                // File's content.
                var byteArray = System.IO.File.ReadAllBytes(uploadFile);
                var stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    var request = DriveService.Files.Insert(body, stream, GetMimeType(uploadFile));
                    request.Upload();
                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    Log.Error("An error occurred: " + e.Message);
                    return null;
                }
            }

            Log.Error("File does not exist: " + uploadFile);
            return null;
        }

        /// <summary>
        /// Used to update existing file
        /// </summary>
        /// <param name="uploadFile">New file to be uploaded to override existing</param>
        /// <param name="parent"></param>
        /// <param name="fileId">Id of a file to be updated</param>
        /// <returns>Updated file</returns>
        public static File UpdateFile(string uploadFile, string parent, string fileId)
        {
            if (System.IO.File.Exists(uploadFile))
            {
                var body = new File
                {
                    Title = System.IO.Path.GetFileName(uploadFile),
                    Description = "File updated by Diamto Drive Sample",
                    MimeType = GetMimeType(uploadFile),
                    Parents = new List<ParentReference>() { new ParentReference() { Id = parent } }
                };

                // File's content.
                var byteArray = System.IO.File.ReadAllBytes(uploadFile);
                var stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    var request = DriveService.Files.Update(body, fileId, stream, GetMimeType(uploadFile));
                    request.Upload();
                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    Log.Error("An error occurred: " + e.Message);
                    return null;
                }
            }

            Log.Error("File does not exist: " + uploadFile);
            return null;
        }


        // Tries to figure out the mime type of the file.
        private static string GetMimeType(string fileName)
        {
            var mimeType = "application/unknown";
            var extension = System.IO.Path.GetExtension(fileName);

            if (extension == null) return mimeType;

            var ext = extension.ToLower();
            var regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

            if (regKey?.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }

            return mimeType;
        }


    }
}