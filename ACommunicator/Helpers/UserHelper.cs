using System.Linq;

namespace ACommunicator.Helpers
{
    public static class UserHelper
    {
        private static readonly ACommunicatorEntities DbContext = new ACommunicatorEntities();

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

        public static bool UsernameExists(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;
            return DbContext.AUsers.FirstOrDefault(x => x.Username.Equals(username)) != null;
        }

    }
}