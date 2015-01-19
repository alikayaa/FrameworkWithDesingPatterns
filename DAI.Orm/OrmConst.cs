using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm
{
    #region Orm Genel Const
    public class OrmConst
    {
        public const string ORMTABLE = "DAI_ORM_TABLES_INSTANCE";
    }

    #endregion

    #region DbType Const
    public class OrmTypeConst
    {
        public const string MSSQLSERVER = "MSSQL";
        public const string ORACLE = "ORACLE";
        public const string MSACCESS = "ACCESS";
    }
    #endregion
}
