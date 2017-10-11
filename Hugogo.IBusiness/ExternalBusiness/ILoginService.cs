using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Hugogo.Model;

namespace Hugogo.IBusiness.ExternalBusiness
{
    /// <summary>
    /// 登录接口
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// 登录初始化
        /// </summary>
        /// <returns>跳转地址（如果有）</returns>
        ActionResult LoginInit();

        /// <summary>
        /// 登录
        /// </summary>
        ///<param name="userInfo">UserInfoModel</param>
        ///<param name="errorInfo">如果错误，返回错误信息</param>
        /// <returns>UserInfoModel</returns>
        ActionResult LogOn(UserInfoModel userInfo, out string errorInfo);

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        UserInfoModel CurrentUser { get; }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        ActionResult LogOut();
    }
}
