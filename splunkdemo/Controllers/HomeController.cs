using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace splunkdemo.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public HomeController()
        {            
        }        

        public ActionResult Index()
        {
            _logger.Log(LogLevel.Warn, "Index page hit");
            return View();
        }

        public ActionResult Login(string errorCode)
        {
            // errorCode refers to user name for this demo
            _logger.Log(LogLevel.Info, string.Format("User={0}, IP=127.0.0.1", errorCode));
            return new JsonResult() { Data = "Login Successful", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ThrowHttpException(string errorCode)
        {
            _logger.LogException(LogLevel.Error, string.Format("StatusCode={0}", errorCode), new HttpException(Int32.Parse(errorCode), "Some sort of description for exception goes here"));
            return View(Int32.Parse(errorCode));
        }

        public ActionResult LogKVP(string errorCode)
        {
            _logger.Log(LogLevel.Info, string.Format("Name={0}, ReferrerUrl={1}", errorCode, System.Web.HttpContext.Current.Request.UrlReferrer));
            return new JsonResult() { Data = "Kvp Logged to info level.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
