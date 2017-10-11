using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hugogo.Model
{
    [Serializable]
    public class UserInfoModel
    {
        /// <summary>
        /// 工号
        /// </summary>

        public string JobNumber { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 分机号
        /// </summary>
        public string ExtensionNumber { get; set; }

        /// <summary>
        /// 是否记住密码（保存两周）
        /// </summary>
        public bool IsRemember { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 用户拥有的菜单信息
        /// </summary>
        public List<MenuModel> UserMenu { get; set; }

        /// <summary>
        /// 坐席工号
        /// </summary>
        public int SeatJobNumber { get; set; }

        /// <summary>
        /// 当前排班Id
        /// </summary>
        public int CurrentScheduling_Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentWorkStatus { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 最小部门Id
        /// </summary>
        public int Dptid { get; set; }

        /// <summary>
        /// 最小部门名称
        /// </summary>
        public string DptName { get; set; }

        /// <summary>
        /// 区域归属梯队
        /// </summary>
        public string AreaIds { get; set; }

        /// <summary>
        /// 部门所属于梯队
        /// </summary>
        public string DeptsTeam { get; set; }
    }

    /// <summary>
    /// 常用用户信息静态类
    /// </summary>
    public static class UserInfoHelper
    {
        static UserInfoHelper()
        {
            SystemUser = new UserInfoModel { RealName = "系统", JobNumber = string.Empty };
            PayNoticeUser = new UserInfoModel { RealName = "支付回写", JobNumber = string.Empty };
        }

        /// <summary>
        /// 系统用户
        /// </summary>
        public static UserInfoModel SystemUser { get; private set; }

        /// <summary>
        /// 支付回写用户
        /// </summary>
        public static UserInfoModel PayNoticeUser { get; private set; }

        /// <summary>
        /// 客户用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static UserInfoModel CustomerUser(string name = "客户")
        {
            if (string.IsNullOrWhiteSpace(name)) name = "客户";
            return new UserInfoModel { RealName = name, JobNumber = string.Empty };
        }
    }
}
