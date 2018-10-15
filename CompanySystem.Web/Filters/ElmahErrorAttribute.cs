﻿using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Diagnostics;

namespace CompanySystem.Web.Filters
{
    public class ElmahErrorAttribute : ExceptionFilterAttribute, System.Web.Mvc.IExceptionFilter
    {
        public override void OnException(
             HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
                Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
                Elmah.ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
            //base.OnException(actionExecutedContext);
            using (EventLog eventLog = new EventLog("eLearning"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(filterContext.Exception.Message, EventLogEntryType.Information, 101, 1);
            }
        }        
    }
}