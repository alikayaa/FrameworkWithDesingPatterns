using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheFramework
{
    public class ExpritionDbCache:IExpritionLeoCache
    {

        protected override void PurgeCache(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool AddCache(object key, object value, DateTime expritionDate)
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

        public override object this[object key, DateTime exprition]
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
