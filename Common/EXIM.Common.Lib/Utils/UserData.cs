using System.Web;

namespace EXIM.Common.Lib.Utils
{
    public class UserData
    {
        public string GetUserNameWithoutDomain()
        {
            string userName = string.Empty;
            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                userName = HttpContext.Current.User.Identity.Name.Split('|').Length > 2 ? HttpContext.Current.User.Identity.Name.Split('|')[2].Split('@')[0] : HttpContext.Current.User.Identity.Name.Split('\\')[1];

            }
            return userName;
        }
    }

}
