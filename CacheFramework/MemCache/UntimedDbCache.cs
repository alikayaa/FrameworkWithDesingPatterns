using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheFramework
{
    public class UntimedDbCache:IUntimedLeoCache
    {
        protected override bool AddCache(object key, object value)
        {
            throw new NotImplementedException();
        }

        protected override bool RemoveCache(object key)
        {
            throw new NotImplementedException();
        }

        protected override object GetCache(object key)
        {
            throw new NotImplementedException();
        }

        public override object this[object key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
