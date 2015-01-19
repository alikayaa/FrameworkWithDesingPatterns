using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class VARCHAR:Attribute
    {
         private const string max_length = "MAX";
        private int _length;

        public VARCHAR(int length = 0)
        {
            this._length = length;
        }


        public virtual string MAX
        {
            get { return max_length; }
        }

        public virtual int LENGTH
        {
            get { return _length; }
            set { this._length = value; }
        }
    }
}
