using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheFramework
{
    public class UntimedFileCache:IUntimedLeoCache
    {
        private static Hashtable m_hastable = new Hashtable();
        protected override bool AddCache(object key, object value)
        {
            if (m_hastable.ContainsKey(key))
                m_hastable.Remove(key);
            m_hastable.Add(key, value);
            return true;
        }

        protected override bool RemoveCache(object key)
        {
            if (!m_hastable.ContainsKey(key))
                return false;
            m_hastable.Remove(key);
            return true;
        }

        protected override object GetCache(object key)
        {
            if (m_hastable.ContainsKey(key))
                return m_hastable[key];
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
