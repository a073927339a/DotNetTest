using System.Web.Mvc;

namespace Realtek.IntraLogin.Controllers
{
    public class FormController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RenderMessage()
        {
            return PartialView("_RenderMessage");
        }

        public ActionResult LoginPartial()
        {
            string userId = this.GetUserId();
            ViewBag.UserId = userId;
            return PartialView("_LoginPartial");
        }

        public ActionResult SearchAccountWindow()
        {
            return PartialView("_SearchAccountWindow");
        }

        public ActionResult AnnounceWindow()
        {
            return PartialView("_AnnounceWindow");
        }

        public ActionResult CheckYesNoWindow()
        {
            return PartialView("_CheckYesNoWindow");
        }

        public ActionResult ShowSelectDeptMemberWindow()
        {
            return PartialView("_ShowSelectDeptMemberWindow"); 
        }

        public ActionResult ShowSelectDeptWindow()
        {
            return PartialView("_ShowSelectDeptWindow");
        }
    }
}