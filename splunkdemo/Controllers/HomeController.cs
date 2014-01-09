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
            AuthenticateUser(errorCode, "DummyPassword");
            return new JsonResult() { Data = "Login Successful", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ThrowHttpException(string errorCode)
        {
            _logger.LogException(LogLevel.Error, string.Format("Id={0}, StatusCode={1}", HttpContext.Items["uniqueid"], errorCode), new HttpException(Int32.Parse(errorCode), "Some sort of description for exception goes here"));
            throw new HttpException(Int32.Parse(errorCode), "Dummy exception being thrown.");            
        }

        public ActionResult LogKVP(string errorCode)
        {
            _logger.Log(LogLevel.Info, string.Format("Id={0}, Name={1}", HttpContext.Items["uniqueid"], errorCode));
            return new JsonResult() { Data = "Kvp Logged to info level.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private bool AuthenticateUser(string userName, string password)
        {            
            _logger.Log(LogLevel.Info, string.Format("Id={0}, Method={1}, User={2}", HttpContext.Items["uniqueid"], "AuthenticateUser", userName));
            return true;
        }
    }
}
