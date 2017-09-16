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
        /// 2 - node options whit childs type of node
        /// 3 - node options whit childs type of leaf
        /// 4 - leaf options
        /// </param>
        /// <returns></returns>
        public static List<Option> GetOptinonListForEndUser(int endUserId, int optionLevel = 1)
        {
            var endUser = DbContext.EndUsers.FirstOrDefault(x => x.Id == endUserId);
            // TODO: check if it is possible to have NullPointerException for x.EndUsers.Contains method call
            //var returnValue = DbContext.Options.Where(x => x.Level == optionLevel && x.EndUsers.Contains(endUser)).ToList();
            var returnValue = endUser.Options.Where(x => x.Level == optionLevel).ToList();

            return returnValue;
        }

        public static Option GetOption(int optionId)
        {
            return DbContext.Options.FirstOrDefault(x => x.Id == optionId);
        }

        /// <summary>
        /// Gets child options for option with Id as in param;
        /// It's currently implemented to support options with four levels:
        /// 1 - primary (root) options
        /// 2 - node options whit childs type of node
        /// 3 - node options whit childs type of leaf
        /// 4 - leaf options
        /// </summary>
        /// <param name="optionId"></param>
        /// <returns></returns>
        public static List<Option> GetChildOptionList(int optionId)
        {
            var option = DbContext.Options.FirstOrDefault(x => x.Id == optionId);

            // TODO: check if it is possible to have NullPointerException for x.ParentFolderID.Equals method call
            var returnValue = new List<Option>();
            
            switch (option?.Level)
            {
                case 1:
                    {
                        returnValue = DbContext.Options.Where(x => x.ParentFolderID.Equals(option.FolderID)).ToList();
                        break;
                    }
                case 2:
                case 3:
                    {
                        returnValue =
                            DbContext.Options.Where(
                                x =>
                                    x.ParentFolderID.Equals(option.FolderID) &&
                                    (x.Level == option.Level || x.Level == option.Level + 1)).ToList();
                        break;
                    }
                case 4:
                    {
                        returnValue.Add(option);
                        break;
                    }
                default:
                {
                    break;
                }
            }

            return returnValue;
        }
    }
}