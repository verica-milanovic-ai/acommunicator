using System;
using System.Web;

namespace ACommunicator.Helpers
{
    public static class CookieHelper
    {
        public const string AUserCookie = "A-User";
        public const string EndUserCookie = "End-User";

        public static void RemoveCookie(string cookieName, HttpResponseBase response)
        {
            var aCookie = new HttpCookie(cookieName) {Expires = DateTime.Now.AddDays(-1)};
            response.Cookies.Add(aCookie);
        }
    }
}