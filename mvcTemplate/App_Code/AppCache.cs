using System.Web;

namespace Realtek.IntraLogin.Commons
{
    public class AppCache
    {
        public static void Set(string key, object value)
        {
            HttpContext.Current.Application[key] = value;
        }

        public static object Get(string key)
        {
            return HttpContext.Current.Application[key];
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Application.Remove(key);
        }

    }
}