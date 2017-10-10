using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hugogo.Injector
{
    /// <summary>
    /// 配置文件中接口信息
    /// </summary>
    internal class InjectorItem
    {
        /// <summary>
        /// 接口
        /// </summary>
        public string InterFace { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 实例类
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// 是否全局方式创建实例
        /// </summary>
        public bool IsGlobal { get; set; }
    }
}
