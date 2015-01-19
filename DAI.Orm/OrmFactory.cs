using DAI.Orm.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm
{
    public class OrmFactory<T>
    {
        public static IDbProvider<T> GetDbProvider()
        {
            if (OrmAccessor.GetDbAcessor().DbType == OrmTypeConst.MSSQLSERVER)
                return new SQLServerProvider<T>();
            if (OrmAccessor.GetDbAcessor().DbType == OrmTypeConst.ORACLE)
                return new OracleProvider<T>();
            if (OrmAccessor.GetDbAcessor().DbType == OrmTypeConst.MSACCESS)
                return new AccessProvider<T>();
            return new SQLServerProvider<T>();
        }
    }
}
