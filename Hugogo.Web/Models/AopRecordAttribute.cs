using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hugogo.Common;
using Hugogo.Injector;
using Hugogo.Model;
using Hugogo.Web.Controllers;

namespace Hugogo.Web.Models
{
    /// <summary>
    /// 自定义的一个简单AOP，用来记录系统Action的执行时间 
    /// </summary>
    public class AopRecordAttribute : ActionFilterAttribute
    {
        private const string StopWatchName = "AopStopWatch"; //计时器名

        /// <summary>
        /// 构造函数，初始化
        /// </summary>
        public AopRecordAttribute()
        {
            this.Order = int.MaxValue; //总是最后一个filter
        }

        /// <summary>
        /// 重写Action开始执行时进行的操作
        /// </summary>
        /// <param name="filterContext">ActionExecutingContext</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //检查检测开关，为0即为关闭，就不继续执行检测代码逻辑了
            var pageExcuteLog = HugogoConfigHelper.GetInstance().GetConfigValue("PageExcuteLog", "PageExcuteLog");
            if (pageExcuteLog != "1") return;

            var monitorpages = HugogoConfigHelper.GetInstance().GetConfig("PageMonitor");
            /*没有需要监控的页面，直接返回*/
            if (monitorpages == null || monitorpages.Count == 0) return;

            //如果此页面不需要监控，直接返回
            string scontroller = ConvertHelper.ToString(filterContext.RouteData.Values["controller"]);
            string action = ConvertHelper.ToString(filterContext.RouteData.Values["action"]);
            string pageurl = string.Format("{0}/{1}", scontroller, action).ToLower();
            if (monitorpages.All(t => t.ConfigurationValue.ToLower() != pageurl)) return;

            ControllerBase controller = filterContext.Controller;
            if (controller != null)
            {
                Stopwatch stopWatch = new Stopwatch();

                //为当前执行controller建立一个计时对象
                controller.ViewData[StopWatchName] = stopWatch;
                //启动计时
                stopWatch.Start();
            }
        }

        /// <summary>
        /// 重写Action执行完成后的操作
        /// </summary>
        /// <param name="filterContext">ResultExecutedContext</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            HttpContextBase httpContext = filterContext.HttpContext;
            //当前执行的Controller
            ControllerBase controller = filterContext.Controller;
            string scontroller = ConvertHelper.ToString(filterContext.RouteData.Values["controller"]);
            string action = ConvertHelper.ToString(filterContext.RouteData.Values["action"]);
            //请求方法，Get，Post等
            string httptype = httpContext.Request.HttpMethod;
            bool isajax = httpContext.Request.IsAjaxRequest();//是否Ajax异步请求
            string urlrefer = ConvertHelper.ToString(httpContext.Request.UrlReferrer);//来源地址 

            //当前登录人
            string userName = "未登录";
            int userid = 0;

            var bcontroller = controller as BaseController;
            if (bcontroller != null && bcontroller.CurrentUserInfo != null)
            {
                userName = bcontroller.CurrentUserInfo.RealName + "[" + bcontroller.CurrentUserInfo.JobNumber + "]";
                userid = bcontroller.CurrentUserInfo.UserId;
            }

            //得到当前Action的计时器对象
            var stopWatch = filterContext.Controller.ViewData[StopWatchName] as Stopwatch;

            if (stopWatch == null) return;

            //停止计时
            stopWatch.Stop();

            #region 记录执行结果

            #endregion


        }
    }
}