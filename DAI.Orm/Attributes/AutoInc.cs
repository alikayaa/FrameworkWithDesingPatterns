using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class AutoInc:Attribute
    {
        private int _length;
        private int _startPoint;

        public AutoInc(int length)
        {
            this._length = length;
            this._startPoint = 1;
        }

        public AutoInc(int length,int startPoint)
        {
            this._length = length;
            this._startPoint = startPoint;
        }

        public virtual int LENGTH
        {
            get { return _length; }
            set { this._length = value; }
        }

        public virtual int STARTPOINT
        {
            get { return _startPoint; }
            set { this._startPoint = value; }
        }
    }
}
