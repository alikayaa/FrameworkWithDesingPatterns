using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Provider
{
    // SQL Sabitleri.
    public class SQLConst
    {
        // Tablo oluşturma sorgusu.
        public static string CREATETABLE = "CREATE TABLE {0} {1} ";
        // Tablo yapısı değiştirme sorgusu.
        public static string ALTERTABLE = "DROP TABLE {0} CREATE TABLE {1} {2} ";
        // Tablo silme sorgusu.
        public static string DROPTABLE = "DROP TABLE {0} ";
        // Tablo boşaltma sorgusu.
        public static string TRUNCATETABLE = "TRUNCATE TABLE {0} ";
        // Tablo kayıt ekleme sorgusu.
        public static string INSERT = "INSERT INTO {0} ({1}) VALUES ({2}) ";
        // Tablo kayıt çekme sorgusu.
        public static string SELECT = "SELECT * FROM {0}";
        // Tablo kayıt silme sorgusu.
        public static string DELETE = "DELETE FROM {0} WHERE {1}={2} ";
        // Tablo kayıt güncelleme sorgusu.
        public static string UPDATE = "UPDATE {0} SET {1} WHERE {2} = {3}";
        // Tablo kayıt veri kaybı öncelikli güncelleme sorgusu.
        public static string PESSIMISTIC_UPDATE = "UPDATE {0} SET {1} WHERE {2}";
        // Framework tablo oluşturucu
        internal static string DAI_TABLE = "IF  NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DAI_ORM_TABLES_INSTANCE' ) BEGIN CREATE TABLE DAI_ORM_TABLES_INSTANCE(ID INT NOT NULL PRIMARY KEY IDENTITY,Name NVARCHAR(MAX),FullName NVARCHAR(MAX)) END";
    }
    // SQL Hata Sabitleri.
    public class SQLExceptionConst
    {
        // Primary Key ve AutoIncrement kontrolü hata mesajı.
        public static string DELETEPKERROR = "objesinin Primary Key yada AutoInc özelliği olmalıdır.";
    }
}
