using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hugogo.Model
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    [Serializable]
    public class MenuModel
    {
        /// <summary>
        /// 英文名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int MenuParentId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Url地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 菜单（权限）类别
        /// </summary>
        public string MenuType { get; set; }

        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool HasPermission { get; set; }

        /// <summary>
        /// 默认页面地址
        /// </summary>
        public string FirstUrl { get; set; }
    }
}
