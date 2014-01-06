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

        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult Login(string errorCode)
        {   
            _logger.Log(LogLevel.Info, string.Format("Id={0}, User={1}", HttpContext.Items["uniqueid"], errorCode));
            return new JsonResult() { Data = "Login Successful", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ThrowHttpException(string errorCode)
        {
            _logger.LogException(LogLevel.Error, string.Format("Id={0}, StatusCode={1}", HttpContext.Items["uniqueid"], errorCode), new HttpException(Int32.Parse(errorCode), "Some sort of description for exception goes here"));
            return View(Int32.Parse(errorCode));
        }

        public ActionResult LogKVP(string errorCode)
        {
            _logger.Log(LogLevel.Info, string.Format("Id={0}, Name={1}", HttpContext.Items["uniqueid"], errorCode));
            return new JsonResult() { Data = "Kvp Logged to info level.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
