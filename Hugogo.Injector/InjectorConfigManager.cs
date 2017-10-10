using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Configuration;
namespace Hugogo.Injector
{
    class InjectorConfigManager
    {
        /// <summary>
        /// 依赖注入反射文件路径
        /// </summary>
        public static readonly string InjectorReflectPath = ConfigurationManager.AppSettings["InjectorReflectPath"];
        
        /// <summary>
        /// 锁定对象
        /// </summary>
        private static readonly object ConfigLock = new object();

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string ConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "Injector.config";

        /// <summary>
        /// 反射文件路径
        /// </summary>
        private static readonly string BusinessFilePath = AppDomain.CurrentDomain.BaseDirectory + InjectorReflectPath;

        /// <summary>
        /// 接口集合
        /// </summary>
        private static Dictionary<string, InjectorItem> BusinessInters = null;

        /// <summary>
        /// 监控文件
        /// </summary>
        private static readonly FileSystemWatcher configWatcher = new FileSystemWatcher();

        /// <summary>
        /// 监控反射文件
        /// </summary>
        private static readonly FileSystemWatcher businessWatcher = new FileSystemWatcher();

        /// <summary>
        /// 修改配置的时间控制
        /// </summary>
        private static System.Threading.Timer timer;

        /// <summary>
        /// 初始化
        /// </summary>
        static InjectorConfigManager()
        {
            if (BusinessInters == null)
            {
                lock (ConfigLock)
                {
                    LoadDatabaseConfig();
                }

                configWatcher.Path = Path.GetDirectoryName(ConfigFilePath);
                configWatcher.Filter = Path.GetFileName(ConfigFilePath);
                configWatcher.NotifyFilter = NotifyFilters.LastWrite;
                configWatcher.EnableRaisingEvents = true;
                configWatcher.Changed += new FileSystemEventHandler(ConfigWatcher_Changed);

                businessWatcher.Path = BusinessFilePath;
                businessWatcher.NotifyFilter = NotifyFilters.LastWrite;
                businessWatcher.EnableRaisingEvents = true;
                businessWatcher.IncludeSubdirectories = false;
                businessWatcher.Changed += new FileSystemEventHandler(BusinessWatcher_Changed);

                timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnWatchedFileChange), null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            }
        }

        /// <summary>
        /// 反射文件创建
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private static void BusinessWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(e.FullPath))
                {
                    AppDomain.Unload(AppDomain.CurrentDomain);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 文件修改时
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private static void ConfigWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            timer.Change(500, System.Threading.Timeout.Infinite);
        }

        /// <summary>
        ///  文件监控改变
        /// </summary>
        /// <param name="state">附加信息</param>
        private static void OnWatchedFileChange(object state)
        {
            ReloadConfigFile();
        }

        /// <summary>
        /// 清理所有缓存
        /// </summary>
        private static void ClearAllCache()
        {
            foreach (KeyValuePair<string, InjectorItem> item in BusinessInters)
            {
                CacheInjector.RemoveCatche(item.Key);
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        private static void LoadDatabaseConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ConfigFilePath);
                BusinessInters = new Dictionary<string, InjectorItem>();

                // Modified by Michael Feng at 2013-05-08
                XmlNodeList injectNodesList = doc.DocumentElement.SelectNodes("./Injector");
                foreach (XmlNode node in injectNodesList)
                {
                    bool isGlobal = true;
                    if (node != null && node.Attributes["global"] != null)
                    {
                        isGlobal = bool.Parse(node.Attributes["global"].Value);
                    }

                    if (node.Attributes["interface"] != null
                        && node.Attributes["instance"] != null
                        && node.Attributes["file"] != null
                        && !string.IsNullOrEmpty(node.Attributes["interface"].Value)
                        && !string.IsNullOrEmpty(node.Attributes["instance"].Value)
                        && !string.IsNullOrEmpty(node.Attributes["file"].Value)
                        )
                    {
                        InjectorItem item = new InjectorItem();
                        item.InterFace = node.Attributes["interface"].Value;
                        item.Instance = node.Attributes["instance"].Value;
                        item.File = node.Attributes["file"].Value;
                        item.IsGlobal = isGlobal;
                        BusinessInters.Add(node.Attributes["interface"].Value, item);
                    }
                }
            }
        }

        /// <summary>
        /// 重新加载配置信息
        /// </summary>
        private static void ReloadConfigFile()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(ConfigFilePath);

                    // Modified by Michael Feng at 2013-05-08
                    XmlNodeList injectNodesList = doc.DocumentElement.SelectNodes("./Injector");
                    foreach (XmlNode node in injectNodesList)
                    {
                        bool isGlobal = true;
                        if (node.Attributes["global"] != null)
                        {
                            isGlobal = bool.Parse(node.Attributes["global"].Value);
                        }

                        if (node.Attributes["interface"] != null
                           && node.Attributes["instance"] != null
                           && node.Attributes["file"] != null
                           && !string.IsNullOrEmpty(node.Attributes["interface"].Value)
                           && !string.IsNullOrEmpty(node.Attributes["instance"].Value)
                           && !string.IsNullOrEmpty(node.Attributes["file"].Value)
                           && !string.IsNullOrEmpty(node.Attributes["file"].Value))
                        {
                            if (BusinessInters.ContainsKey(node.Attributes["interface"].Value)
                                && (!BusinessInters[node.Attributes["interface"].Value].Instance.Equals(node.Attributes["instance"].Value)
                                || !BusinessInters[node.Attributes["interface"].Value].File.Equals(node.Attributes["file"].Value)))
                            {
                                BusinessInters[node.Attributes["interface"].Value].Instance = node.Attributes["instance"].Value;
                                BusinessInters[node.Attributes["interface"].Value].File = node.Attributes["file"].Value;
                                BusinessInters[node.Attributes["interface"].Value].IsGlobal = isGlobal;
                                CacheInjector.RemoveCatche(node.Attributes["interface"].Value);
                            }

                            if (!BusinessInters.ContainsKey(node.Attributes["interface"].Value))
                            {
                                InjectorItem item = new InjectorItem();
                                item.InterFace = node.Attributes["interface"].Value;
                                item.Instance = node.Attributes["instance"].Value;
                                item.File = node.Attributes["file"].Value;
                                item.IsGlobal = isGlobal;
                                BusinessInters.Add(node.Attributes["interface"].Value, item);
                            }
                        }
                    }

                    string[] keys = new string[BusinessInters.Count];
                    BusinessInters.Keys.CopyTo(keys, 0);

                    foreach (string key in keys)
                    {
                        bool isExist = false;
                        foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                        {
                            if (node.Attributes["interface"] != null
                                && key.Equals(node.Attributes["interface"].Value))
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist == false)
                        {
                            BusinessInters.Remove(key);
                            CacheInjector.RemoveCatche(key);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="interFaceName">接口名称</param>
        /// <returns>返回配置项</returns>
        public static InjectorItem GetInjectorItem(string interFaceName)
        {
            if (BusinessInters != null && BusinessInters.ContainsKey(interFaceName))
            {
                return BusinessInters[interFaceName];
            }
            return null;
        }
    }
}
