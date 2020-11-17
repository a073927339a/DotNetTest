using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using log4net;
using Realtek.IntraLogin.Models;

namespace Realtek.IntraLogin.Filters
{
    public class AuthorityAttribute : ActionFilterAttribute
    {
        protected static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string[] BYPASS_CONTROLLERS = new string[] { "Base", "Form", "Home", "Git" };
        private static readonly string[] BYPASS_AUTH_CONTROLLERS = new string[] { "Report" };

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CheckAuthority(filterContext);
        }

        // 判斷是否轉頁"無權限"頁面
        private void CheckAuthority(ActionExecutingContext filterContext)
        {
            string controller = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();
            string userId = GetUserId(System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            bool pass = true;

            if (!BYPASS_CONTROLLERS.Contains<string>(controller))
            {
                if (userId == null || "".Equals(userId))
                {
                    pass = false;
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        // Request by ajax 
                        HttpContext.Current.Response.AddHeader("timeout", "Y");
                        HttpContext.Current.Response.AddHeader("ReturnUrl", controller);
                        HttpContext.Current.Response.StatusCode = 401;

                        filterContext.Result = new JsonResult
                        {
                            Data = "Please Login!",
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {   // 直接導向首頁
                        // save query param string
                        string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                        System.Web.HttpContext.Current.Session["returnUrl"] = url;
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Login" },
                            { "returnUrl", url } });
                    }
                }
                else if (filterContext.HttpContext.Request.IsAjaxRequest())
                {

                }
                else if (!checkPageAuthority(controller, userId))
                {
                    pass = false;
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Login" }, { "returnUrl", controller }, { "message", "No Authority" } });
                }

                if (pass)
                {
                    AuthorityAttribute.Logger.Debug("Enter Function => /" + controller + "/" + action);
                }
            }


        }


        // 檢查此User是否有進入此頁面或是使用api的權限
        private bool checkPageAuthority(string controller, string userId)
        {
            if (BYPASS_AUTH_CONTROLLERS.Contains<string>(controller))
            {
                return true;
            }

            return true;
        }

        protected string GetUserId(HttpRequest request, HttpSessionState session)
        {
            string userId = (string)session["SessionUserId"];
            return userId;
        }
    }
}