using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Hugogo.Business.AccountLogin.SSOLogin;
using Hugogo.Common;
using Hugogo.IBusiness.ExternalBusiness;
using Hugogo.Injector;
using Hugogo.Model;
using Hugogo.Web.Models;

namespace Hugogo.Web.Controllers
{
    /// <summary>
    /// 项目控制器基类，所有需要登录验证的页面都需要继承这个类
    /// </summary>
    [AopRecord]
    public abstract class BaseController : Controller
    {
        ///// <summary>
        ///// 创建工作渠道的静态单一实例
        ///// </summary>
        //protected readonly IWorkChannelService _ChannelService = DependencyInjector.GetInstance<IWorkChannelService>();

        
        /// <summary>
        /// 构造函数
        /// </summary> 
        protected BaseController()
        {
            ValidateRequest = false;

        }

        #region 当前用户信息

        /// <summary>
        /// 当前登录人信息
        /// </summary>
        public UserInfoModel CurrentUserInfo
        {
            get
            {
                var service = DependencyInjector.GetInstance<ILoginService>();
                return service.CurrentUser;
            }
        }

        #endregion

        #region 重写基类Controller

        /// <summary>
        /// 重写Controller的OnActionExecuting方法，拦截Action的执行，进行自定义处理
        /// </summary>
        /// <param name="filterContext">上下文</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var returnUri = filterContext.HttpContext.Request.Url.ToString();
            bool isLoginPage = RouteData.Values["controller"].ToString().ToLower().Equals("account");
            //如果不是登录页，尚未登录，跳转到登录页面
            if (!isLoginPage && (CurrentUserInfo == null || CurrentUserInfo.UserId == 0))
            {
                    filterContext.Result = Redirect(FormsAuthentication.LoginUrl + "?returnUrl=" +
                    HttpUtility.UrlEncode(ConvertHelper.ToString(filterContext.RequestContext.HttpContext.Request.Url)));
               
                return;
            }

            //判断此次的请求，该用户是否有此页面的权限
            List<MenuModel> userMenu = CurrentUserInfo.UserMenu;
            if (userMenu == null || userMenu.Count <= 0)
            {
                filterContext.Controller.ViewData["ErrorMessage"] = "对不起，您没有此系统的任何权限！";
                filterContext.Result = new ViewResult
                {
                    ViewName = "IllegalCallError",
                    ViewData = filterContext.Controller.ViewData,
                };
                return;
            }

            //验证是否开启Action权限验证，默认是不开启
            if (HugogoConfigHelper.GetInstance().GetConfigValue("AccountLogin", "ActionLegalize", false))
            {
                //判断权限，通过比较Url和QueryString参数来实现，由于路由定义的关系，所以Url要忽略{id}
                var currURL = Url.Action(RouteData.Values["action"].ToString(), RouteData.Values["controller"].ToString(), new { id = "" });
                var currRequest = new HttpRequest("", "http://" + Request.Url.Authority + currURL, Request.Url.Query.TrimStart('?'));
                if (!userMenu.Any(m =>
                {
                    var url = m.Url;
                    if (string.IsNullOrWhiteSpace(url)) return false;
                    //每次Url修改的时候，则对UrlRequest重新赋值
                    if (!url.StartsWith("http://"))
                    {
                        //如果是相对路径，则处理成绝对路径
                        url = "http://" + Request.Url.Authority.Trim('/') + "/" + Request.ApplicationPath.Trim('/') + "/" + url.Trim('/');
                    }
                    var objUri = new Uri(url);
                    var urlRequest = new HttpRequest("", "http://" + objUri.Authority + objUri.LocalPath, objUri.Query.TrimStart('?'));
                    //域名和端口要一致
                    if (urlRequest.Url.Authority != currRequest.Url.Authority) return false;
                    //Url地址要一致，原始地址和后来拼的都比较一次
                    if (urlRequest.Url.AbsolutePath.Trim('/') != Request.Url.AbsolutePath.Trim('/')
                        && urlRequest.Url.AbsolutePath.Trim('/') != currRequest.Url.AbsolutePath.Trim('/'))
                    {
                        return false;
                    }
                    //菜单Url如果不包含Get参数，则无需继续验证，算通过
                    if (urlRequest.QueryString.Count <= 0) return true;
                    //菜单Url包含的Get参数也要一致
                    return urlRequest.QueryString.AllKeys.All(key => urlRequest.QueryString[key] == currRequest.QueryString[key]);
                }))
                {
                    filterContext.Controller.ViewData["ErrorMessage"] = "对不起，您没有此页面的访问权限！！";
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "Error",
                        ViewData = filterContext.Controller.ViewData,
                    };
                    return;
                }
            }

            //登录的用户信息
            ViewBag.CurrentUser = CurrentUserInfo;
            ViewBag.IsOnLine = AppSettingsHelper.GetBool("IsOnLine");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(ActionExecutingContext filterContext)
        {
            var token = filterContext.HttpContext.Request.Cookies[Constant.AccessToken];
            if (token != null && !string.IsNullOrEmpty(token.Value)) return;
            var returnUri = filterContext.HttpContext.Request.Url.ToString();
            filterContext.HttpContext.Response.Redirect(OauthServices.GenerateLoginUrl(returnUri));
            filterContext.Result = new HttpUnauthorizedResult();
        }

        /// <summary>
        /// 重写Controller的OnException方法,用来记录执行过程中发生的未捕获的异常信息
        /// </summary>
        /// <param name="filterContext">上下文</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            #region 异常日志记录

            string controller = ConvertHelper.ToString(RouteData.Values["controller"]);
            string action = ConvertHelper.ToString(RouteData.Values["action"]);
            string errorMsg = filterContext.Exception.Message +
                              "\n" + filterContext.Exception.StackTrace + "\n" +
                              filterContext.Exception.Source;

            string errorTitle = string.Format("发生异常，Controller：{0}，Action：{1} ", controller, action);
            //LogHelper.ExceptionLog(errorTitle, filterContext.Exception, LogType.Common);
            //LogHelper.ThrowExceptionLog(errorTitle, filterContext.Exception);

            #endregion

            //显示错误页面
            filterContext.Controller.ViewData["ErrorMessage"] = errorMsg;
            filterContext.Result = View("Error");
        }

        #endregion
    }
}