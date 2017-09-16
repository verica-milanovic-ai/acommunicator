using System;
using System.Web;

namespace ACommunicator.Helpers
{
    public static class CookieHelper
    {
        public const string AUserCookie = "A-User";
        public const string EndUserCookie = "End-User";
        public const string SelectedOptionsCookie = "Selected-Options";

        public static void RemoveCookie(string cookieName, HttpResponseBase response)
        {
            var aCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
            response.Cookies.Add(aCookie);
        }

        public static void AddCookie(string cookieName, string cookieValue, HttpResponseBase response)
        {
            var aCookie = new HttpCookie(cookieName, cookieValue)
            {
                HttpOnly = true
            };
            response.Cookies.Add(aCookie);
        }
    }
}