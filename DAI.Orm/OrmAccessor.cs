using DAI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm
{
    // Orm Bağlantı Konfig Dosyaları Okuyucu ve Bağlantı Oluşturucu Sınıf
    public class OrmAccessor
    {
        private static OrmAccessor _accessor;
        public string ConnectionString { get; set; }
        public string DbType { get; set; }

        private OrmAccessor()
        {
            this.ConnectionString = ConfigManager.Instance().GetConnectionString("DEFAULT");
            this.DbType = ConfigManager.Instance().GetValue<string>("DBTYPE");

        }
        public static OrmAccessor GetDbAcessor()
        {
            if(_accessor == null)
                _accessor = new OrmAccessor();
            
            return _accessor;

        }
    }
}
