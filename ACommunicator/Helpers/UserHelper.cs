using System.Collections.Generic;
using System.Linq;

namespace ACommunicator.Helpers
{
    public static class UserHelper
    {
        private static readonly ACommunicatorEntities DbContext = new ACommunicatorEntities();
        private static readonly string DefaultPicutrePath = "./Content/imgs/default.png";

        public static AUser GetAUserByUsername(string username)
        {
            return string.IsNullOrEmpty(username) ? null : DbContext.AUsers.FirstOrDefault(x => x.Username.Equals(username));
        }

        public static void AddAUser(AUser aUser)
        {
            if (string.IsNullOrEmpty(aUser?.Username) || string.IsNullOrEmpty(aUser.Password) ||
                string.IsNullOrEmpty(aUser.Email)) return;
            DbContext.AUsers.Add(aUser);
            DbContext.SaveChanges();
        }

        public static ICollection<EndUser> GetEndUserList(string username)
        {
            return string.IsNullOrEmpty(username) ? null : DbContext.AUsers.FirstOrDefault(x => x.Username.Trim().Equals(username.Trim()))?.EndUsers;
        }

        public static EndUser AddEndUser(EndUser endUser)
        {
            if (string.IsNullOrEmpty(endUser?.Username)) { return null; }

            if (string.IsNullOrEmpty(endUser.Name)) { endUser.Name = endUser.Username; }
            if (string.IsNullOrEmpty(endUser.PicturePath)) { endUser.PicturePath = DefaultPicutrePath; }

            DbContext.EndUsers.Add(endUser);
            DbContext.SaveChanges();

            return endUser;
        }

        public static EndUser GetEndUserById(int id)
        {
            return id == -1 ? null : DbContext.EndUsers.FirstOrDefault(x => x.Id == id);
        }

        public static bool UsernameExists(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;
            return DbContext.AUsers.FirstOrDefault(x => x.Username.Equals(username)) != null;
        }

    }
}