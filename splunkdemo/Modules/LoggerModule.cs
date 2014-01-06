using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace splunkdemo.Modules
{
    public class LoggerModule : IHttpModule
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            
            context.BeginRequest += OnStartRequest;
            context.EndRequest += OnEndRequest;
        }

        public void OnStartRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items["uniqueid"] = Guid.NewGuid().ToString("N");
        }

        public void OnEndRequest(object sender, EventArgs e)
        {
            HttpApplication context = (HttpApplication)sender;
            RouteData routeData = context.Request.RequestContext.RouteData;

            _logger.Log(LogLevel.Info, string.Format("Id={0},IpAddress={1},RequestUrl={2},Status={3},Referrer={4},BrowserType={5}", 
                HttpContext.Current.Items["uniqueid"],
                context.Request.ServerVariables["REMOTE_ADDR"],
                context.Request.Url,
                context.Response.Status,
                (context.Request.UrlReferrer == null ? string.Empty : context.Request.UrlReferrer.ToString()),
                context.Request.UserAgent));
        }
    }
}