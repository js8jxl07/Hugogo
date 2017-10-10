using System.Web;
using System.Web.Caching;

namespace Hugogo.Injector
{
    internal class CacheInjector
    {
        /// <summary>
        /// 将对象加入到缓存中
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cacheObject">缓存对象</param>
        public static void SaveToCache(string cacheKey, object cacheObject)
        {
            Cache cache = HttpRuntime.Cache;
            if (!(cacheObject == null))
            {
                cache.Insert(cacheKey, cacheObject);
            }
        }

        /// <summary>
        /// 从缓存中取得对象，不存在则返回null
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <returns>获取的缓存对象</returns>
        public static object GetFromCache(string cacheKey)
        {
            Cache cache = HttpRuntime.Cache;
            return cache[cacheKey];
        }

        /// <summary>
        /// 移除缓冲对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        public static void RemoveCatche(string cacheKey)
        {
            Cache cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }
    }
}
