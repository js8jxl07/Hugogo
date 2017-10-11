using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Hugogo.Common;

namespace Hugogo.Business.AccountLogin.SSOLogin
{
    public class Constant
    {
        #region 获取配置信息

        ///  /// <summary>
        /// 域名加端口号
        /// </summary>
        public static readonly string Authority = HttpContext.Current.Request.Url.Authority.ToLower();
        /// <summary>
        /// 二级虚拟目录
        /// </summary>
        public static readonly string WebRootPath = HttpContext.Current.Request.ApplicationPath.ToLower();
        /// <summary>
        /// 授权登录回调本系统地址
        /// </summary>
        public static string ApplyTokenRedirectUri = "http://" + Authority + WebRootPath.TrimEnd('/') + "/oauth/authorizationCodeCallBack";
        public static string StateApplyForCodeUri = AppSettingsHelper.GetString("StateApplyForCodeUri");
        public static string GetUserInfoByTokenUri = AppSettingsHelper.GetString("GetUserInfoByTokenUri");
        public static string CodeApplyForTokenUri = AppSettingsHelper.GetString("CodeApplyForTokenUri");
        public static string SSOLogoutUri = AppSettingsHelper.GetString("SSOLogoutUri");
        public static string ClientId = AppSettingsHelper.GetString("ClientId");
        public static string ClientSecret = AppSettingsHelper.GetString("ClientSecret");
        #endregion

        #region  客户端属性

        public const string Scope = "read";

        public const string ResponseType = "code";
        public const string GrantType = "authorization_code";
        public const string AccessToken = "access_token";

        #endregion

        #region 用户属性

        public const string UserId = "userId";
        public const string UserName = "username";
        public const string JobNumber = "workId";
        public const string DepartmentId = "departmentId";
        public const string Department = "department";
        public const string PhoneNumber = "phoneNumber";

        #endregion
    }

}
