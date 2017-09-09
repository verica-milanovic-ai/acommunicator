using System.Collections.Generic;
using System.Linq;

namespace ACommunicator.Helpers
{
    public static partial class OptionHelper
    {
        private static readonly ACommunicatorEntities DbContext = new ACommunicatorEntities();

        /// <summary>
        /// get options for user on specified level
        /// </summary>
        /// <param name="endUserId"></param>
        /// <param name="optionLevel">
        /// 1 - primary (root) options
        /// 2 - node options
        /// 3 - leaf options
        /// </param>
        /// <returns></returns>
        public static List<Option> GetOptinonListForEndUser(int endUserId, int optionLevel = 1)
        {
            var endUser = DbContext.EndUsers.FirstOrDefault(x => x.Id == endUserId);
            // TODO: check if it is possible to have NullPointerException for x.EndUsers.Contains method call
            //var returnValue = DbContext.Options.Where(x => x.Level == optionLevel && x.EndUsers.Contains(endUser)).ToList();
            var returnValue = endUser.Options.Where(x=>x.Level == optionLevel).ToList();

            return returnValue;
        }

        public static Option GetOption(int optionId)
        {
            return DbContext.Options.FirstOrDefault(x => x.Id == optionId);
        }

        public static List<Option> GetChildOptionList(int optionId)
        {
            var option = DbContext.Options.FirstOrDefault(x => x.Id == optionId);
            // TODO: check if it is possible to have NullPointerException for x.ParentFolderID.Equals method call
            var returnValue = DbContext.Options.Where(x => x.ParentFolderID.Equals(option.FolderID)).ToList();

            return returnValue;
        }

    }
}