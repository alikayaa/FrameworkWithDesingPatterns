using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class ForeignKeyAttr:Attribute
    {
        private string _foreignTable;
        private string _foreignKeyColumn;
        private string _foreignKeyType;
        private bool _listOrOne;

        public ForeignKeyAttr(string foreignTable,string foreignKeyColumn,string foreignKeyType)
        {
            this._foreignTable = foreignTable;
            this._foreignKeyColumn = foreignKeyColumn;
            this._foreignKeyType = foreignKeyType;
        }

        public ForeignKeyAttr(string foreignTable,string foreignKeyColumn,string foreignKeyType,bool list)
        {
            this._foreignTable = foreignTable;
            this._foreignKeyColumn = foreignKeyColumn;
            this._foreignKeyType = foreignKeyType;
            this._listOrOne = list;
        }

        public virtual string FOREIGNTABLE
        {
            get { return _foreignTable; }
            set { this._foreignTable = value; }
        }

        public virtual string FOREIGNKEYCOLUMN
        {
            get { return _foreignKeyColumn; }
            set { this._foreignKeyColumn = value; }
        }

        public virtual string FOREIGNKEYTYPE
        {
            get { return _foreignKeyType; }
            set { this._foreignKeyType = value; }
        }

        public virtual bool LISTORONE
        {
            get { return _listOrOne; }
            set { this._listOrOne = value; }
        }
    }
}
