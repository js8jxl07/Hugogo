using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hugogo.IBusiness.Tables;
using Hugogo.Injector;
using Hugogo.Model.Tables;

namespace Hugogo.Common
{
    /// <summary>
    /// 配置信息帮助类单例
    /// </summary>
    public sealed class HugogoConfigHelper
    {
        #region single

        private static readonly HugogoConfigHelper Instance = new HugogoConfigHelper();

        private HugogoConfigHelper()
        {
        }

        public static HugogoConfigHelper GetInstance()
        {
            return Instance;
        }

        #endregion

        private readonly IConfigurationsServices _configServices =
            DependencyInjector.GetInstance<IConfigurationsServices>();

        /// <summary>
        /// 根据分组名称获取配置信息
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <returns></returns>
        public List<ConfigurationsModel> GetConfig(string groupName)
        {
            return _configServices.GetConfigByGroupName(groupName);
        }

        /// <summary>
        /// 根据分组名称及键名称获取配置信息
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <param name="configurationKey">键名称</param>
        /// <returns></returns>
        public ConfigurationsModel GetConfig(string groupName, string configurationKey)
        {
            var configInfo = _configServices.GetConfigInfo(groupName, configurationKey);
            return configInfo ?? new ConfigurationsModel();
        }

        /// <summary>
        /// 分组描述获取配置信息
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public ConfigurationsModel GetConfigByDesc(string groupName, string desc)
        {
            return GetConfig(groupName).FirstOrDefault(model => model.Description.ToUpper().Equals(desc.ToUpper())) ??
                   new ConfigurationsModel();
        }

        /// <summary>
        /// 根据分组名称获取配置信息指定类型的值集合
        /// </summary>
        /// <typeparam name="T">指定的集合类型</typeparam>
        /// <param name="groupName">分组名称</param>
        /// <param name="defaultValue">集合返回数据默认值</param>
        /// <returns></returns>
        public List<T> GetConfigValue<T>(string groupName, T defaultValue = default (T)) where T : struct
        {
            return
                GetConfig(groupName)
                    .Select(m => ValidateHelper.ConvertType(m.ConfigurationValue, defaultValue))
                    .ToList();
        }

        /// <summary>
        /// 根据分组名称获取配置信息指定类型的值集合
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <returns></returns>
        public List<string> GetConfigValue(string groupName)
        {
            return GetConfig(groupName).Select(m => m.ConfigurationValue).ToList();
        }

        /// <summary>
        /// 根据分组名称获取配置信息指定类型的值
        /// </summary>
        /// <typeparam name="T">指定值的类型</typeparam>
        /// <param name="groupName">分组名称</param>
        /// <param name="key">键名称</param>
        /// <param name="defaultValue">返回数据的默认值</param>
        /// <returns></returns>
        public T GetConfigValue<T>(string groupName, string key, T defaultValue = default (T)) where T : struct
        {
            return ValidateHelper.ConvertType(GetConfig(groupName, key).ConfigurationValue, defaultValue);
        }

        /// <summary>
        /// 根据分组名称获取配置信息指定类型的值
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <param name="key">键名称</param>
        /// <returns></returns>
        public string GetConfigValue(string groupName, string key)
        {
            return GetConfig(groupName, key).ConfigurationValue ?? string.Empty;
        }

        /// <summary>
        /// 根据分组名称及值获取配置信息
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <param name="configurationValue">值</param>
        /// <returns></returns>
        public List<ConfigurationsModel> GetConfigByGroupAndValue(string groupName, string configurationValue)
        {
            return GetConfig(groupName).Where(model => model.ConfigurationValue.Equals(configurationValue)).ToList();
        }



        /// <summary>
        /// 判断某分组中是否含有键值等于Key的配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool FilterConifgByKey(string key, string groupName)
        {
            var configList = GetConfig(groupName);
            if (configList == null || configList.Count <= 0) return false;
            if (configList.Any(m => m.ConfigurationKey.Equals(key))) return true;
            return false;
        }

        /// <summary>
        /// 新增配置
        /// </summary>
        /// <param name="Configurations"></param>
        /// <returns></returns>
        public bool Add(ConfigurationsModel Configurations)
        {
            return _configServices.Add(Configurations);
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        public bool Update(int id, ConfigurationsModel updateModel)
        {
            var updateResult = _configServices.Update(id, updateModel);
            if (updateResult)
            {
                _configServices.ClearConfigCache(updateModel.GroupName);
            }

            return updateResult;
        }
    }
}
