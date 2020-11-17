using System;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Resources;
using Realtek.IntraLogin.Models;
using log4net;

namespace Realtek.IntraLogin.Controllers
{
    public class BaseController : Controller
    {
        protected static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static ResourceManager MessageRm; // 定義API回傳的訊息Resource Manager

        public BaseController()
        {
            MessageRm = new ResourceManager("Realtek.IntraLogin.App_GlobalResources.ApiMessage", typeof(BaseController).Assembly);

            // set LayoutMaster ViewBage;
            ViewBag.ServerType = Config.GetConfigValue("IntraLogin", "SERVERTYPE");
            ViewBag.AdminContact = Config.GetConfigValue("IntraLogin", "MAIL_ADMIN");

            try
            {

            }
            catch (Exception e)
            {

            }
        }

        protected void LogException(string methodName, string exception)
        {
            Logger.Error("[" + methodName + "] Error => " + exception);
        }

        /// <summary>
        /// 取得現在登入員工帳號 ex. vinlykuo
        /// </summary>
        /// <returns></returns>
        protected string GetUserId()
        {
            return this.GetUserId(false);
        }

        protected string GetUserId(bool refresh)
        {
            string userId = (string)Session["SessionUserId"];
            return userId;
        }

        /// <summary>
        /// 取得現在登入員工工號  ex. R2555
        /// </summary>
        /// <returns></returns>
        protected string GetUserEmpId()
        {
            string userEmpId = (string)Session["SessionEmpId"];
            return userEmpId;
        }

        protected string GetUserMail()
        {
            string userMail = (string)Session["SessionUserMail"];
            return userMail;
        }

        protected void SetUserEmpId(string empId)
        {
            Session["SessionEmpId"] = empId;
        }

        protected void SetUserId(string userId)
        {
            Session["SessionUserId"] = userId;
        }

        protected void SetUserMail(string userMail)
        {
            Session["SessionUserMail"] = userMail;
        }

        protected JsonResult CreateJsonResult(object data)
        {
            return new JsonResult()
            {
                Data = data,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        protected JsonResult CreateErrorJsonResult(string errorMsg, string stack)
        {
            string serverType = Config.GetConfigValue("IntraLogin", "ServerType");
            Response.StatusCode = 500;
            Logger.Debug(errorMsg + "\n" + stack);

            Mail.SendAdminErrorMail(errorMsg + "\n" + stack);

            return new JsonResult()
            {
                Data = errorMsg,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        protected void HandleErrorStringResult(string errorMsg, string stack)
        {
            string serverType = Config.GetConfigValue("IntraLogin", "ServerType");
            Response.StatusCode = 500;
            Logger.Debug(errorMsg + "\n" + stack);
            if ("".Equals(serverType))
            {
                Mail.SendAdminErrorMail(errorMsg + "\n" + stack);
                //mail to admin
            }
        }

        protected string GetUserIp()
        {
            string tClientIP = Request.ServerVariables["REMOTE_ADDR"].ToString();
            // 開發機的localhost IP會變成::1
            if (tClientIP.Equals("::1"))
                tClientIP = "172.21.17.141";
            return tClientIP;
        }

        protected string GetSystemMessageArg(string name)
        {
            string temp = MessageRm.GetString(name);
            return temp;
        }
    }
}
