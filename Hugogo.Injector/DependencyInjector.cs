using System;
using System.Reflection;

namespace Hugogo.Injector
{
    public class DependencyInjector
    {
        /// <summary>
        /// 获取接口实例
        /// </summary>
        /// <typeparam name="T">业务接口</typeparam>
        /// <returns>返回配置中中实现类的实例</returns>
        public static T GetInstance<T>()
        {
            string interfacetype = typeof(T).ToString();
            InjectorItem item = InjectorConfigManager.GetInjectorItem(interfacetype);
            if (item != null)
            {
                object obj = CacheInjector.GetFromCache(interfacetype);
                //object obj = null;
                if (obj == null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + InjectorConfigManager.InjectorReflectPath + item.File;
                    obj = (item.IsGlobal ? Assembly.LoadFrom(path).CreateInstance(item.Instance) : Assembly.LoadFrom(path).GetType(item.Instance));
                    CacheInjector.SaveToCache(interfacetype, obj);
                }

                return (item.IsGlobal ? (T)obj : (T)Activator.CreateInstance((Type)obj));
            }

            return default(T);
        }

        /// <summary>
        /// 获取接口实例
        /// </summary>
        /// <typeparam name="T">业务接口</typeparam>
        /// <param name="args">参数集合</param>
        /// <returns>返回配置中中实现类的实例</returns>
        public static T GetInstance<T>(object[] args)
        {
            string interfacetype = typeof(T).ToString();
            InjectorItem item = InjectorConfigManager.GetInjectorItem(interfacetype);
            if (item != null)
            {
                object obj = CacheInjector.GetFromCache(interfacetype);
                //object obj = null;
                if (obj == null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + InjectorConfigManager.InjectorReflectPath + item.File;
                    obj = (item.IsGlobal ? Assembly.LoadFrom(path).CreateInstance(item.Instance, true, BindingFlags.CreateInstance, null, args, null, null) : Assembly.LoadFrom(path).GetType(item.Instance));
                    CacheInjector.SaveToCache(interfacetype, obj);
                }

                return (item.IsGlobal ? (T)obj : (T)Activator.CreateInstance((Type)obj, args));
            }

            return default(T);
        }
    }
}
