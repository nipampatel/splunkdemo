﻿using NLog;
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
            _logger.Log(LogLevel.Info, string.Format("Url : {0}", System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath));
        }        

        public ActionResult Index()
        {
            _logger.Log(LogLevel.Warn, "Index page hit");
            return View();
        }

        public ActionResult Login(string errorCode)
        {
            // errorCode refers to user name for this demo
            _logger.Log(LogLevel.Info, "Login page is called from ip 127.0.0.1 for user {0}", errorCode);
            return new JsonResult() { Data = "Login Successful", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ThrowHttpException(string errorCode)
        {
            _logger.LogException(LogLevel.Error, string.Format("Http status code {0}", errorCode), new HttpException(Int32.Parse(errorCode), "Some sort of description for exception goes here"));
            return View(Int32.Parse(errorCode));
        }

        public ActionResult LogKVP(string value)
        {
            _logger.Log(LogLevel.Info, string.Format("Name={0}, ReferrerUrl={1}", value, System.Web.HttpContext.Current.Request.UrlReferrer));
            return new JsonResult() { Data = "Kvp Logged to info level.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
