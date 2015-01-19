using DAI.Orm.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DAI.Orm.Provider
{
    public class SQLServerProvider<Table> : IDbProvider<Table>
    {

        #region Private Members
        private static Type entityType
        {
            get { return typeof(Table); }
        }
        // Auto Increment Sql sorgusu
        string autoIncSql = string.Empty;
        // Primary Key Sql Sorgusu
        string pkSql = string.Empty;
        // İlişkisel Id
        int relationId = 0;
        // Tablo Index
        int tableIndex = 0;
        // Kolon Tipi 
        bool affectedColumn = false;
        // Primitive Tipler
        List<string> primitiveTypes = new List<string>() { "String", "Int32", "Int16", "Double", "Float", "DateTime" };
        Dictionary<int, Table> IdentityMap = new Dictionary<int, Table>();
        #endregion

        #region Db Func

        #region Table Func
        // Tablo oluştur veya değiştir.
        public void CreateAlterTable(bool isCreate = true)
        {
            string createSql = "(";
            string oneRow = string.Empty;
            string pkColumn = string.Empty;
            var type = new object();
            int i = 0;
            foreach (var property in entityType.GetProperties())
            {
                // Değişken Adı
                oneRow = property.Name;
                affectedColumn = false;
                #region Tablo Alanları
                // AutoInc alanlar kontrol edilir.
                var autoInc = property.GetCustomAttributes(typeof(AutoInc), false).FirstOrDefault();
                if (autoInc != null)
                {
                    pkColumn += oneRow;
                    oneRow += " int IDENTITY(" + ((AutoInc)(autoInc)).STARTPOINT + "," + ((AutoInc)(autoInc)).LENGTH + ") ,";
                    createSql += oneRow;
                    continue;

                }
                // Değişken tipi bulma

                // INT
                type = property.GetCustomAttributes(typeof(INT), false).FirstOrDefault();
                if (type != null)
                    oneRow += " int";

                // Binary
                type = property.GetCustomAttributes(typeof(BINARY), false).FirstOrDefault();
                if (type != null)
                    oneRow += " binary";

                // Boolean
                type = property.GetCustomAttributes(typeof(BOOLEAN), false).FirstOrDefault();
                if (type != null)
                    oneRow += " byte";

                // DateTime
                type = property.GetCustomAttributes(typeof(DATETIME), false).FirstOrDefault();
                if (type != null)
                    oneRow += " DATETIME";

                // Decimal
                type = property.GetCustomAttributes(typeof(DECIMAL), false).FirstOrDefault();
                if (type != null)
                {
                    oneRow += " DECIMAL(" + ((DECIMAL)(type)).STAIR + "," + ((DECIMAL)(type)).COMMA + ")";
                }

                // Nvarchar
                type = property.GetCustomAttributes(typeof(NVARCHAR), false).FirstOrDefault();
                if (type != null)
                {
                    if (((NVARCHAR)type).LENGTH > 0)
                        oneRow += " nvarchar(" + ((NVARCHAR)type).LENGTH + ")";
                    else
                        oneRow += " nvarchar(" + ((NVARCHAR)type).MAX + ")";
                }

                // Varchar
                type = property.GetCustomAttributes(typeof(VARCHAR), false).FirstOrDefault();
                if (type != null)
                {
                    if (((VARCHAR)type).LENGTH > 0)
                        oneRow += " nvarchar(" + ((VARCHAR)type).LENGTH + ")";
                    else
                        oneRow += " nvarchar(" + ((VARCHAR)type).MAX + ")";
                }

                type = property.GetCustomAttributes(typeof(ForeignKeyAttr), false).FirstOrDefault();
                if (type != null)
                {
                    oneRow += " " + ((ForeignKeyAttr)type).FOREIGNKEYTYPE + " NULL,";
                    affectedColumn = true;
                }

                // Kolon tipi

                // NULL
                type = property.GetCustomAttributes(typeof(NULL), false).FirstOrDefault();
                if (type != null)
                {
                    oneRow += " NULL,";
                    affectedColumn = true;
                }

                // NOT NULL
                type = property.GetCustomAttributes(typeof(NOTNULL), false).FirstOrDefault();
                if (type != null && !affectedColumn)
                {
                    oneRow += " NOT NULL,";
                    affectedColumn = true;
                }

                if (!affectedColumn)
                    oneRow += ",";
                #endregion
                if (oneRow != property.Name)
                    createSql += oneRow;
                i++;
            }
            i = 0;
            foreach (var property in entityType.GetProperties())
            {
                oneRow = "";
                #region Indeksler
                // Primary Key alanlar kontrol edilir.
                type = property.GetCustomAttributes(typeof(PrimaryKeyAttr), false).FirstOrDefault();
                if (type != null)
                    pkColumn += "," + property.Name;

                // Unique Index
                type = property.GetCustomAttributes(typeof(UNIQUEINDEX), false).FirstOrDefault();
                if (type != null)
                    oneRow += " CREATE UNIQUE CLUSTERED INDEX " + entityType.Name + "x" + i + " ON " + entityType.Name + "(" + property.Name + "),";

                // NonClustered Index
                type = property.GetCustomAttributes(typeof(NONCLUSTEREDINDEX), false).FirstOrDefault();
                if (type != null)
                    oneRow += " CREATE NONCLUSTERED INDEX " + entityType.Name + "x" + i + " ON " + entityType.Name + "(" + property.Name + "),";

                // Foreign Key
                type = property.GetCustomAttributes(typeof(ForeignKeyAttr), false).FirstOrDefault();
                if (type != null)
                {
                    oneRow += "FOREIGN KEY (" + property.Name + ") REFERENCES " + ((ForeignKeyAttr)type).FOREIGNTABLE + "(" + ((ForeignKeyAttr)type).FOREIGNKEYCOLUMN + "),";
                }
                createSql += oneRow;
                i++;
                #endregion
            }

            createSql += "PRIMARY KEY (" + pkColumn + "))";
            if (isCreate)
                OrmEngine.Instance().SqlCommandText += string.Format(SQLConst.CREATETABLE, entityType.Name, createSql);
            else
                OrmEngine.Instance().SqlCommandText += string.Format(SQLConst.ALTERTABLE, entityType.Name, entityType.Name, createSql);
            SubmitChanges();
        }
        // Tablo sil.
        public void DropTable()
        {
            OrmEngine.Instance().SqlCommandText += string.Format(SQLConst.DROPTABLE, entityType.Name);
        }
        // Tablo boşalt.
        public void TruncateTable()
        {
            OrmEngine.Instance().SqlCommandText += string.Format(SQLConst.TRUNCATETABLE, entityType.Name);
        }

        #endregion

        #region Data Func
        // Tablodan veri çek.
        public List<Table> Select(string sqlExp = "*", string field = "", string whereClause = "")
        {

            // Bağlantı açma
            this.Connection();
            // Komut objesi oluşturulur.
            OrmEngine.Instance().Command = new SqlCommand(OrmEngine.Instance().SqlCommandText, OrmEngine.Instance().Conn);
            OrmEngine.Instance().Command.CommandText = string.Format("SELECT {0} FROM {1} {2}", sqlExp, entityType.Name, whereClause);

            var reader = OrmEngine.Instance().Command.ExecuteReader();
            var resultList = new List<Table>();
            while (reader.Read())
            {
                Table entity = Activator.CreateInstance<Table>();
                Table identityEntity = Activator.CreateInstance<Table>();
                foreach (var propertyInfo in entityType.GetProperties())
                {
                    if (propertyInfo.PropertyType.Namespace == "System")
                    {
                        if (InTableColumn(propertyInfo.Name, field))
                        {
                            if (reader[propertyInfo.Name] != DBNull.Value)
                            {
                                propertyInfo.SetValue(entity, reader[propertyInfo.Name]);
                                propertyInfo.SetValue(identityEntity, reader[propertyInfo.Name]);
                            }
                        }

                    }
                    var pk = propertyInfo.GetCustomAttributes(typeof(PrimaryKeyAttr), false).FirstOrDefault();
                    if (pk != null)
                    {
                        if (InTableColumn(propertyInfo.Name, field))
                        {
                            if (reader[propertyInfo.Name] != DBNull.Value)
                            {
                                if (!IdentityMap.ContainsKey(Convert.ToInt32(reader[propertyInfo.Name])))
                                    IdentityMap.Add(Convert.ToInt32(reader[propertyInfo.Name]), identityEntity);
                            }
                        }
                    }
                    var ai = propertyInfo.GetCustomAttributes(typeof(AutoInc), false).FirstOrDefault();
                    if (ai != null)
                    {
                        if (InTableColumn(propertyInfo.Name, field))
                        {
                            if (reader[propertyInfo.Name] != DBNull.Value)
                            {
                                if (!IdentityMap.ContainsKey(Convert.ToInt32(reader[propertyInfo.Name])))
                                    IdentityMap.Add(Convert.ToInt32(reader[propertyInfo.Name]), identityEntity);
                            }
                        }
                    }
                }

                resultList.Add(entity);
            }
            return resultList;
        }


        // Tablodan son kaydı çek.
        public Table Last()
        {

            string pkName = string.Empty;
            foreach (var item in entityType.GetProperties())
            {
                var pk = item.GetCustomAttributes(typeof(AutoInc), false).FirstOrDefault();
                if (pk != null)
                {
                    pkName = item.Name;
                    break;
                }
            }

            // Bağlantı açma
            this.Connection();
            // Komut objesi oluşturulur.
            OrmEngine.Instance().Command = new SqlCommand(OrmEngine.Instance().SqlCommandText, OrmEngine.Instance().Conn);

            OrmEngine.Instance().Command.CommandText = string.Format("SELECT TOP (1) * FROM {0} ORDER BY {1} DESC", entityType.Name, pkName);

            var reader = OrmEngine.Instance().Command.ExecuteReader();
            var resultList = new List<Table>();
            while (reader.Read())
            {
                // k= new Kisi()
                var entity = Activator.CreateInstance<Table>();
                foreach (var propertyInfo in entityType.GetProperties())
                {
                    // k.KisiID= reader["KisiID"]; 
                    if (propertyInfo.PropertyType.Namespace == "System")
                        propertyInfo.SetValue(entity, reader[propertyInfo.Name]);
                }
                resultList.Add(entity);
            }
            return resultList.FirstOrDefault();
        }
        // Tablodaki son kayıt
        public object LastFieldValue(string Field)
        {
            // Bağlantı açma
            this.Connection();
            // Komut objesi oluşturulur.
            OrmEngine.Instance().Command = new SqlCommand(OrmEngine.Instance().SqlCommandText, OrmEngine.Instance().Conn);
            OrmEngine.Instance().Command.CommandText = string.Format("SELECT TOP 1 {0} FROM {1} ORDER BY {2} DESC ", Field, entityType.Name, Field);
            var reader = OrmEngine.Instance().Command.ExecuteReader();
            while (reader.Read())
            {
                return reader[0];
            }
            return null;
        }
        // Tabloya kayıt ekle.
        public void Insert(Table entity)
        {
            var columnNames = new List<string>();
            var values = new List<string>();
            // Data Primary Key kontrolu
            this.CheckPrimaryKey(entity);

            foreach (var property in entityType.GetProperties())
            {
                // AutoInc kontrolu, AutoInc insert sorgusuna eklenmez. 
                var autoInc = property.GetCustomAttributes(typeof(AutoInc), false).FirstOrDefault();
                if (autoInc != null)
                {
                    continue;
                }

                // primary key kontrolu, AutoInc insert sorgusuna eklenmez. 
                var pk = property.GetCustomAttributes(typeof(PrimaryKeyAttr), false).FirstOrDefault();
                if (pk != null)
                {
                    continue;
                }

                // tip kontrolu
                if (primitiveTypes.Contains(property.PropertyType.Name))
                {

                    // database parametresi oluşturma
                    var dbParam = new SqlParameter();
                    // Değer dizine atama yapma @AlanAdı şeklinde
                    values.Add("@" + (dbParam.ParameterName = "p" + OrmEngine.Instance().SqlParameters.Count));
                    // Parametre değer atama
                    dbParam.Value = property.GetValue(entity);
                    // Hafızadaki Sql Parametreleri objesine eklenir.
                    OrmEngine.Instance().SqlParameters.Add(dbParam);
                    // Alan adı dizine ekleme
                    columnNames.Add(property.Name);
                }


            }
            // Sql çıktı atama String Join ile dizi elemanları arasına ',' eklenir.
            OrmEngine.Instance().SqlCommandText += string.Format(SQLConst.INSERT, entityType.Name, string.Join(",", columnNames), string.Join(",", values));

        }
        // Tabloya toplu kayıt ekle.
        public void MultipleInsert(List<Table> entityList)
        {
            foreach (var entity in entityList)
            {
                Insert(entity);
            }
        }
        // Tablodan kayıt sil.
        public void Delete(Table entity)
        {
            // Where koşul değeri
            var clauseValue = new object();
            var clauseObject = new object();
            // Silinecek tabloda primary key veya AutoInc alanı bulunur.
            foreach (var property in entityType.GetProperties())
            {
                // AutoInc alanı aranır.
                var autoInc = property.GetCustomAttributes(typeof(AutoInc), false).FirstOrDefault();
                // Alan bulunursa değeri alınır.
                if (autoInc != null)
                {
                    clauseObject = property.Name;
                    clauseValue = property.GetValue(entity);
                    break;
                }
                // Primary Key alanı aranır.
                var pk = property.GetCustomAttributes(typeof(PrimaryKeyAttr), false).FirstOrDefault();
                // Alan bulunursa değeri alınır.
                if (pk != null)
                {
                    clauseObject = property.Name;
                    clauseValue = property.GetValue(entity);
                    break;
                }

            }
            // Obje üzerinde Pk veya AutoInc özelliği yoksa hata fırlatılır.
            if (clauseObject.GetType() == typeof(object))
                throw new Exception(string.Format("{0} {1}", entityType.Name, SQLExceptionConst.DELETEPKERROR));
            OrmEngine.Instance().SqlCommandText += string.Format(SQLConst.DELETE, entityType.Name, clauseObject, clauseValue);
        }
        // Tablodan toplu kayıt sil.
        public void MultipleDelete(List<Table> entityList)
        {
            foreach (var entity in entityList)
            {
                Delete(entity);
            }
        }
        // Tabloda kayıt güncelle.
        public void Update(Table entity)
        {
            // Where koşul değeri
            var clauseValue = new object();
            var clauseObject = new object();
            int Id = 0;
            bool isUpdate = false;
            // güncelleme değeri
            string updateSql = "";
            // Güncellenecek tabloda primary key veya AutoInc alanı bulunur.
            foreach (var property in entityType.GetProperties())
            {
                if (property.PropertyType.Namespace == "System")
                {
                    // AutoInc alanı aranır.
                    var autoInc = property.GetCustomAttributes(typeof(AutoInc), false).FirstOrDefault();
                    // Alan bulunursa değeri alınır.
                    if (autoInc != null)
                    {
                        clauseObject = property.Name;
                        clauseValue = property.GetValue(entity);
                        Id = Convert.ToInt32(property.GetValue(entity));

                    }
                    // Primary Key alanı aranır.
                    var pk = property.GetCustomAttributes(typeof(PrimaryKeyAttr), false).FirstOrDefault();
                    // Alan bulunursa değeri alınır.
                    if (pk != null)
                    {
                        clauseObject = property.Name;
                        clauseValue = property.GetValue(entity);
                        Id = Convert.ToInt32(property.GetValue(entity));
                    }
                    // Identity Map Entity'sini Getir.
                    if (IdentityMap.ContainsKey(Id))
                    {
                        Table identityEntity = IdentityMap[Id];
                        PropertyInfo identityProp = identityEntity.GetType().GetProperty(property.Name);
                        // Değişenleri Uygula
                        if (!identityProp.GetValue(identityEntity).Equals(property.GetValue(entity)))
                        {
                            isUpdate = true;
                            if (property.PropertyType.Name == "String")
                                updateSql += string.Format("{0} = '{1}',", property.Name, property.GetValue(entity));
                            else
                                updateSql += string.Format("{0} = {1},", property.Name, property.GetValue(entity));
                        }
                    }
                    else
                    {
                        isUpdate = false;
                    }
                }
            }
            // Obje üzerinde Pk veya AutoInc özelliği yoksa hata fırlatılır.
            if (clauseObject.GetType() == typeof(object))
                throw new Exception(string.Format("{0} {1}", entityType.Name, SQLExceptionConst.DELETEPKERROR));
            if (isUpdate)
            {
                updateSql = updateSql.Substring(0, updateSql.Length - 1);
                if (OrmEngine.Instance().PessimisticUpdate)
                    OrmEngine.Instance().SqlCommandText += string.Format(SQLConst.PESSIMISTIC_UPDATE, entityType.Name, updateSql, this.PessimisticUpdateWhereClause(IdentityMap[Id]));
                else
                    OrmEngine.Instance().SqlCommandText += string.Format(SQLConst.UPDATE, entityType.Name, updateSql, clauseObject, clauseValue);
            }
        }

        #endregion

        #region Submit Changes
        // Değişiklikleri uygula.
        public void SubmitChanges()
        {
            using (var ts = new TransactionScope())
            {
                // Bağlantı açma
                object lockObject = new object();
                lock (lockObject)
                {
                    this.Connection();
                    // Komut objesi oluşturulur.
                    OrmEngine.Instance().Command = new SqlCommand(OrmEngine.Instance().SqlCommandText, OrmEngine.Instance().Conn);
                    // Sql parametreler komut objesine eklenir.
                    if (OrmEngine.Instance().SqlParameters != null)
                    {
                        OrmEngine.Instance().Command.Parameters.AddRange(OrmEngine.Instance().SqlParameters.ToArray());
                    }
                    // Sql komut çalıştırılır.
                    try
                    {
                        int result = 0;
                        if (!string.IsNullOrEmpty(OrmEngine.Instance().SqlCommandText))
                            result = OrmEngine.Instance().Command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Veritabanı işlemi sırasında bir hata oluştu. " + ex.Message, ex.InnerException);
                    }
                    finally
                    {
                        // Sql Komut boşaltma
                        OrmEngine.Instance().SqlCommandText = string.Empty;
                        OrmEngine.Instance().SqlParameters.Clear();
                        OrmEngine.Instance().Command = null;
                        // Bağlantı kapat
                        this.CloseConnection();
                        ts.Complete();
                    }
                }
            }
           
        }
        #endregion

        #endregion

        #region Connection Func
        // Bağlantı Açma.
        public void Connection()
        {
            OrmEngine.Instance().Conn = new SqlConnection(OrmAccessor.GetDbAcessor().ConnectionString);
            OrmEngine.Instance().Conn.Open();
        }

        public void CloseConnection()
        {
            OrmEngine.Instance().Conn.Close();
        }
        #endregion

        #region Index Func
        public void CheckPrimaryKey(Table entity)
        {
            bool result = false;

            foreach (var property in entityType.GetProperties())
            {
                var primaryKey = property.GetCustomAttributes(typeof(PrimaryKeyAttr), false).FirstOrDefault();
                if (primaryKey != null)
                    result = true;
                var autoInc = property.GetCustomAttributes(typeof(AutoInc), false).FirstOrDefault();
                if (autoInc != null)
                    result = true;
            }

            if (!result)
            {
                throw new NullReferenceException("Primary Key Alanı veya AutoInc Alanı Olmak Zorundadır.");
            }
        }
        #endregion

        #region Helper
        private bool InTableColumn(string columnName, string sqlField)
        {
            if (string.IsNullOrEmpty(sqlField))
                return true;
            else if (string.Join(",", sqlField).IndexOf(columnName) != -1)
                return true;
            else
                return false;
        }
        private string PessimisticUpdateWhereClause(Table entity)
        {
            string pessimisticWhere = string.Empty;
            if (entity == null)
                return pessimisticWhere;
            foreach (var property in entityType.GetProperties())
            {
                if (property.PropertyType.Namespace == "System")
                {
                    if (property.PropertyType.Name == "String")
                        pessimisticWhere += string.Format(" {0} = '{1}' AND", property.Name, property.GetValue(entity));
                    else
                        pessimisticWhere += string.Format(" {0} = {1} AND", property.Name, property.GetValue(entity));
                }
            }
            pessimisticWhere = pessimisticWhere.Substring(0, pessimisticWhere.Length - 3);
            return pessimisticWhere;
        }
        #endregion

    }
}
