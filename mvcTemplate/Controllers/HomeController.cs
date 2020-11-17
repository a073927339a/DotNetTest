using System;
using System.Web.Mvc;
using Realtek.IntraLogin.Models;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.IO;

namespace Realtek.IntraLogin.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            string empId = this.GetUserEmpId();
            string userId = this.GetUserId(true); // ex. vinlykuo
            string userMail = "";
            if (!String.IsNullOrEmpty(userId))
            {
                if (Session["returnUrl"] != null)
                {
                    string returnUrl = Session["returnUrl"].ToString();
                    Session.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                userMail = Session["Account"].ToString();
                Session["Account"] = "";
                if (!string.IsNullOrEmpty(userMail))
                {
                    return Redirect(WebConfigurationManager.AppSettings["Intranet"] + Encode(userMail));
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public ActionResult Login(string returnUrl = "", string message = "")
        {
            string userId = this.GetUserId(true);
            
            if (!String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return View();
        }

        /// <summary>
        /// 網頁語言設置
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult SetCulture(string culture, string returnUrl)
        {
            // Save culture in a cookie 
            HttpCookie cookie = Request.Cookies["_culture"];

            if (cookie != null)
            {
                // update cookie value 
                cookie.Value = culture;
            }
            else
            {
                // create cookie value 
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }

            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        [HttpPost]
        public JsonResult CheckAccount(string account, string password)
        {
            bool pass = true;
            bool isAuthorized = false;

            if (account.Contains("@"))
            {
                // 有些人登入會打全部帳號"vinlykuo@realtek.com"
                // 要把後面的信箱部份去掉
                account = account.Split('@')[0];
            }

            string message = new Account().ValidateLogin(account, password);
            string userIp = this.GetUserIp();
            // string empId = Account.GetUserEmpId(account);  //取得工號
            //if ("".Equals(empId)) 
            //{
            //    // 查不到工號就當作登入錯誤
            //    message = "找不到此使用者，請用簽核帳密登入(不用帶@realtek.com)";
            //}

            pass = "pass".Equals(message) ? pass : false;

            if (pass)
            {
                message = "";
                this.SetUserId(account);
                isAuthorized = true;
                Session["Account"] = account;
            }

            return Json(new
            {
                Pass = pass,
                Message = message,
                IsAuthorized = isAuthorized
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "Home");
        }

        private string Encode(string encodeStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(WebConfigurationManager.AppSettings["Key"]);
            byte[] iv = Encoding.ASCII.GetBytes(WebConfigurationManager.AppSettings["IV"]);
            byte[] dataByteArray = Encoding.UTF8.GetBytes(encodeStr);

            des.Key = key;
            des.IV = iv;
            string encrypt = "";
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                encrypt = Convert.ToBase64String(ms.ToArray());
            }
            encrypt = encrypt.Replace("+", "%2B");
            return encrypt;
        }

        public string Controller()
        {
            string str = "Dev branch";
            return str;
        }

    }
}
