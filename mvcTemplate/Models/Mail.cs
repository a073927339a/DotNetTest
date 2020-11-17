using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Configuration;
using log4net;
using Realtek.IntraLogin.Commons;

namespace Realtek.IntraLogin.Models
{
    // 利用DB SQL發信
    public class Mail
    {
        protected static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Mail() { }

     
       
        public static string Send(string subject, string body, string to, string cc, string from)
        {
            string adminMain = WebConfigurationManager.AppSettings["MailAdmin"];
            string message = "";
            if ("".Equals(from))
                from = adminMain;

            DaoResult daoResult = DaoHelper.Create("[dbo].[SP_Send_Mail] @profile_name,  @program_key,  @recipients,  @copy_recipients, " +
                " @blind_copy_recipients, @subject,  @body, @body_format, @importance, @sensitive, @from")
                .AddParameter("@profile_name", "Hunter Alert")  // DB發信的設定名稱
                .AddParameter("@program_key", "Hunter_Models_Mail_Send")
                .AddParameter("@recipients", to)            // 收件者
                .AddParameter("@copy_recipients", cc)       // CC
                .AddParameter("@blind_copy_recipients", "") // 密件副本
                .AddParameter("@subject", subject)          //
                .AddParameter("@body", body)
                .AddParameter("@body_format", "HTML")
                .AddParameter("@importance", "NORMAL")
                .AddParameter("@sensitive", "NORMAL")
                .AddParameter("@from", from)                // from
                .SetConnection("KWSEAI")
                .ExecuteUpdate();

            if (!daoResult.IsSuccess)
                message = daoResult.Message;
            return message;
        }

     

        public static string SendAdminErrorMail(string message)
        {
            string server = WebConfigurationManager.AppSettings["ServerType"];
            string adminMain = WebConfigurationManager.AppSettings["MailAdmin"];

            try
            {
                if (server.Equals("") || server.Equals("測試區"))
                    Mail.Send("[Hunter " + server + "] 系統錯誤通知", message, adminMain, "", "");
            }
            catch (Exception ex)
            {
                Mail.Logger.Debug(ex.ToString() + "\n" + ex.StackTrace);
                return "SendAdminErrorMail Fail: " + ex.ToString();
            }

            return "";
        }

     
    }
}