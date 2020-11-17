using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Configuration;
using Realtek.IntraLogin.Commons;
using log4net;
using System.Net;
using System.Xml;
using System.DirectoryServices;
using Realtek.Commons;

namespace Realtek.IntraLogin.Models
{
    public class Account
    {
        protected static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 屬性
        public string Id { get; set; }              // 編號
        public string Name { get; set; }            // User姓名
        public string UserAccount { get; set; }     // 登入帳號
        public string Mail { get; set; }            //
        public string CompanyId { get; set; }       // 


        #endregion

        public Account() { }

        public string ValidateLogin(string user, string password)
        {
            // SpResponse accountStatus = CheckAccount(user);
            string debugMode = WebConfigurationManager.AppSettings["Debug"];
            string debugPassword = WebConfigurationManager.AppSettings["DebugPassword"];


            // 驗證帳號
            if (debugMode != null && "YES".Equals(debugMode))
                if (debugPassword != null && debugPassword.Equals(password))
                    return "pass";

            if (this.ValidateLadpLogin(user, password))
                return "pass";

            return "Login Error: 登入錯誤，請使用簽核系統密碼";
        }

        /// <summary>
        /// 到LDAP做認證
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool ValidateLadpLogin(string user, string password)
        {
            bool pass = false;
            string ldapServer = Config.GetConfigValue("IntraLogin", "LDAP");
            try
            {
                DirectoryEntry root = new DirectoryEntry(ldapServer, user, password, AuthenticationTypes.None);
                DirectorySearcher searcher = new DirectorySearcher(root);
                searcher.Filter = "(&(objectClass=dominoPerson)(|(cn=" + user + ")(uid=" + user + ")(mail=" + user + ")))";
                SearchResult rs = searcher.FindOne();
                pass = true;
            }
            catch
            {
                pass = false;
            }
            return pass;
        }

        private string Branch()
        {
            string str = "Hello Dev branch";
            return str;
        }
    }
}