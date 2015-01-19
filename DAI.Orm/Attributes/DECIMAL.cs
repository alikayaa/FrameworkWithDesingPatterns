using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class DECIMAL:Attribute
    {
        private int _stair;
        private int _comma;

        public DECIMAL(int stair, int comma)
        {
            this._stair = stair;
            this._comma = comma;
        }

        public virtual int STAIR
        {
            get { return _stair; }
            set { this._stair = value; }
        }

        public virtual int COMMA
        {
            get { return _comma; }
            set { this._comma = value; }
        }
    }
}
