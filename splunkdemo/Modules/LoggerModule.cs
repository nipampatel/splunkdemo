using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace splunkdemo.Modules
{
    public class LoggerModule : IHttpModule
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly Regex _regex = new Regex("(?i)\\.css|\\.js|\\.jpg|\\.jpeg|\\.png|\\.bmp|\\.gif|\\.ico|\\.pdf|\\.xml");
        private static readonly string _uniqueId = "uniqueid";
        private static readonly string _startTime = "startTime";
        private static readonly string _remoteAddress = "REMOTE_ADDR";

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
            HttpContext.Current.Items[_uniqueId] = Guid.NewGuid().ToString("N");
            HttpContext.Current.Items[_startTime] = DateTime.Now;
        }

        public void OnEndRequest(object sender, EventArgs e)
        {
            if (!IsExcludedRequest())
            {
                DateTime startTime = DateTime.MinValue;

                if (HttpContext.Current.Items[_startTime] != null)
                {
                    startTime = DateTime.Parse(HttpContext.Current.Items[_startTime].ToString());
                }

                HttpApplication context = (HttpApplication)sender;
                RouteData routeData = context.Request.RequestContext.RouteData;

                _logger.Log(LogLevel.Info, string.Format("Id={0},IpAddress={1},RequestUrl={2},Status={3},Referrer={4},BrowserType={5},ExecutionTime={6}",
                    HttpContext.Current.Items[_uniqueId],
                    context.Request.ServerVariables[_remoteAddress],
                    context.Request.Url,
                    context.Response.Status,
                    (context.Request.UrlReferrer == null ? string.Empty : context.Request.UrlReferrer.ToString()),
                    context.Request.UserAgent,
                    (DateTime.Now - startTime).TotalMilliseconds
                    ));
            }
        }

        private bool IsExcludedRequest()
        {   
            if (HttpContext.Current.Request != null)
            {
                Match match = _regex.Match(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
                return match.Success;
            }
            return false;
        }
    }
}