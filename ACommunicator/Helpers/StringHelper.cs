using System;

namespace ACommunicator.Helpers
{
    public static class StringHelper
    {
        public static string GetFileExtension(this string fileName)
        {
            if(string.IsNullOrEmpty(fileName) || !fileName.Contains("."))
                return "";

            var lastDotIndex = fileName.LastIndexOf(".", StringComparison.Ordinal);

            return fileName.Substring(lastDotIndex);
        }
    }
}