using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CacheFramework
{
    public class UntimedMemCache:IUntimedLeoCache
    {
        private static Dictionary<object, object> m_cacheList = new Dictionary<object, object>();



        protected override bool AddCache(object key, object value)
        {
            if (m_cacheList.ContainsKey(key))
                m_cacheList.Remove(key);
            m_cacheList.Add(key, value);
            return true;
            
        }

        protected override bool RemoveCache(object key)
        {
            if (!m_cacheList.ContainsKey(key))
                return false;
            m_cacheList.Remove(key);
            return true;
        }


        protected override object GetCache(object key)
        {
            if (m_cacheList.ContainsKey(key))
                return m_cacheList[key];
            return null;
        }

        public override object this[object key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Add(key, value);
            }
        }
    }
}
