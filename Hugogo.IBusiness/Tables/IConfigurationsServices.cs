using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hugogo.Model.Tables;

namespace Hugogo.IBusiness.Tables
{
    public partial interface IConfigurationsServices
        
    {
         /// <summary>
         ///Configurations 添加操作
         /// </summary>
         /// <param name="configurations">Configurations实体类</param>
         /// <returns>执行成功返回true,否则为false</returns>
         bool Add(ConfigurationsModel configurations);


        /// <summary>
        /// Configurations修改操作
        /// </summary>
        /// <param name="id">主键，自增长Id</param>
        /// <param name="updateModel">要修改Configurations实体类</param>
        /// <returns>执行成功返回true,否则为false</returns>
        bool Update(int id,ConfigurationsModel updateModel);
  
 
        ///// <summary>
        ///// 获取配置信息列表
        ///// </summary>
        ///// <param name="model">配置信息展示Model</param>
        ///// <returns></returns>
        //ConfigurationViewModel GetConfigurationList(ConfigurationViewModel model);

        ///// <summary>
        ///// Configurations查询操作
        ///// </summary>
        ///// <param name="id">主键自增长Id</param>
        ///// <returns>返回Configurations实体列表</returns>
        //ConfigurationsModel GetConfigById(int id);

        /// <summary>
        /// 根据分组获取配置信息
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <returns></returns>
        List<ConfigurationsModel> GetConfigByGroupName(string groupName);

        /// <summary>
        /// 根据分组名称和Key获取配置信息
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        ConfigurationsModel GetConfigInfo(string groupName, string key);

        /// <summary>
        /// 获取所有分组名称
        /// </summary>
        /// <returns></returns>
        DataTable GetAllGroupName();

        /// <summary>
        /// 清除配置信息缓存
        /// </summary>
        /// <returns>清除成功返回True，否则返回False</returns>
        bool ClearConfigCache(string groupName);

        /// <summary>
        /// 获取配置VALUE
        /// </summary>
        /// <param name="key"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        string GetConfigurationValue(string key, string groupName);

        /// <summary>
        /// Sql依赖注入配置更新
        /// </summary>
        /// <param name="chkId">Sql配置值</param>
        /// <returns></returns>
        bool ConfigValueUpdate(int chkId);

        /// <summary>
        /// 获取及时配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        string GetTimelyConfiguration(string key, string groupName);
    }
}
