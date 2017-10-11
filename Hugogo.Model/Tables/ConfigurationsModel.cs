using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hugogo.Model.Tables
{
    public class ConfigurationsModel
    {

        /// <summary>
        /// 数据库字段：Id
        /// </summary>
        private int db_id = 0;

        /// <summary>
        /// 获取或设置主键，自增长Id
        /// </summary>
        public int Id
        {
            get { return db_id; }
            set { db_id = value; }
        }

        /// <summary>
        /// 数据库字段：ConfigurationKey
        /// </summary>
        private string db_configurationKey = string.Empty;

        /// <summary>
        /// 获取或设置配置键名称，唯一索引
        /// </summary>
        public string ConfigurationKey
        {
            get { return db_configurationKey; }
            set { db_configurationKey = value; }
        }

        /// <summary>
        /// 数据库字段：ConfigurationValue
        /// </summary>
        private string db_configurationValue = string.Empty;

        /// <summary>
        /// 获取或设置配置值
        /// </summary>
        public string ConfigurationValue
        {
            get { return db_configurationValue; }
            set { db_configurationValue = value; }
        }

        /// <summary>
        /// 数据库字段：Description
        /// </summary>
        private string db_description = string.Empty;

        /// <summary>
        /// 获取或设置描述信息
        /// </summary>
        public string Description
        {
            get { return db_description; }
            set { db_description = value; }
        }

        /// <summary>
        /// 数据库字段：Ord
        /// </summary>
        private int db_ord = 0;

        /// <summary>
        /// 获取或设置排序，数字越大越靠前
        /// </summary>
        public int Ord
        {
            get { return db_ord; }
            set { db_ord = value; }
        }

        /// <summary>
        /// 数据库字段：GroupName
        /// </summary>
        private string db_groupName = string.Empty;

        /// <summary>
        /// 获取或设置配置分组名称
        /// </summary>
        public string GroupName
        {
            get { return db_groupName; }
            set { db_groupName = value; }
        }

        /// <summary>
        /// 数据库字段：DataFlag
        /// </summary>
        private byte db_dataFlag = Convert.ToByte("0");

        /// <summary>
        /// 获取或设置是否有效 1：有效， 0：无效
        /// </summary>
        public byte DataFlag
        {
            get { return db_dataFlag; }
            set { db_dataFlag = value; }
        }

    }
}
