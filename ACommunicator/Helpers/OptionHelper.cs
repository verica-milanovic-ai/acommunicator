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
                case 2:
                case 3:
                    {
                        returnValue = DbContext.Options.Where(x => x.ParentOptionId == optionId).ToList();
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

        /// <summary>
        /// Needs to be called from AddOption method 
        /// with input param type of OptionMediaModel
        /// </summary>
        /// <param name="option"></param>
        /// <param name="endUserId"></param>
        /// <returns></returns>
        private static Option AddOption(Option option, string endUserId)
        {
            if (string.IsNullOrEmpty(option?.Name)
                ||
                (option?.Level < 1 || option?.Level > 4))
            {
                return null;
            }

            var endUser = DbContext.EndUsers.Find(endUserId);
            option.IsDefault = false;
            option.EndUsers = new List<EndUser> { endUser };

            option = DbContext.Options.Add(option);

            DbContext.SaveChanges();

            return option;
        }

        private static bool UpdateOption(Option option, string endUserId)
        {
            if (string.IsNullOrEmpty(option?.Name)
                ||
                (option?.Level < 1 || option?.Level > 4))
            {
                return false;
            }
            var original = DbContext.Options.Find(option.Id);
            if (original == null) return false;

            if (original.IsDefault)
            {
                DbContext.Options.Add(option);

                var endUserOriginal = DbContext.EndUsers.Find(endUserId);
                var endUser = new EndUser(endUserOriginal);
                endUser.Options.Remove(original);

                DbContext.Entry(endUserOriginal).CurrentValues.SetValues(endUser);
            }
            else
            {
                DbContext.Entry(original).CurrentValues.SetValues(option);
            }

            DbContext.SaveChanges();
            return true;
        }

        private static bool RemoveOption(Option option, string endUserId)
        {
            var endUserOriginal = DbContext.EndUsers.Find(endUserId);

            if (option == null) return false;


            var endUser = new EndUser(endUserOriginal);
            
            // recursion here!
            RemoveAllChildOptions(option, endUser);

            DbContext.Entry(endUserOriginal).CurrentValues.SetValues(endUser);
            DbContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Be aware that recoursion is used in this method.
        /// </summary>
        /// <param name="option">Option to be removed as well as its childOptions</param>
        /// <param name="endUser">From whom to remove Option - must be a copy of original endUser object!</param>
        private static void RemoveAllChildOptions(Option option, EndUser endUser)
        {
            var childOptionList = option.Options1;

            if (childOptionList != null && childOptionList.Count > 0)
            {
                foreach (var childOption in childOptionList)
                {
                    RemoveAllChildOptions(childOption, endUser);
                }
            }

            endUser.Options.Remove(option);
            if (!option.IsDefault)
            {
                DbContext.Options.Remove(option);
            }
        }
    }
}