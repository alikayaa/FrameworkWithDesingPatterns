using DAI.Configuration;
using DAI.Orm.Context;
using DAI.Orm.Provider;
using DAI.Orm.T4Template;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm
{
    public class OrmEngine
    {
        #region Members
        private static OrmEngine _engine;
        public SqlConnection Conn;
        public SqlCommand Command;
        public List<SqlParameter> SqlParameters = new List<SqlParameter>();
        public string SqlCommandText = string.Empty;
        public bool PessimisticUpdate { get; set; }
        #endregion

        #region Singleton
        private OrmEngine()
        {
            this.PessimisticUpdate = ConfigManager.Instance().GetValue<bool>("PessimisticUpdate");
        }

        public static OrmEngine Instance()
        {
            if (_engine == null)
                _engine = new OrmEngine();

            return _engine;
        }
        #endregion

        #region Model Methods

        private List<Type> GetModelClass(Assembly callAssembly)
        {
            // Çağırılan Assembly yükle
            var baseType = typeof(IModel).FullName;
            var assembly = callAssembly;
            // IModel türemiş sınıfları getir.
            return assembly.GetTypes().Where(t => t.GetInterface("IModel") != null && baseType == t.GetInterface("IModel").FullName).ToList();
        }

        public void InitializeDatabase()
        {
            // DAI Tablolarını kontrol et.
            IDbProvider<DAI_ORM_TABLES_INSTANCE> Provider = OrmFactory<DAI_ORM_TABLES_INSTANCE>.GetDbProvider();
            Provider.CreateAlterTable(false);
            List<DAI_ORM_TABLES_INSTANCE> ModelClass = TypeConvertDaiTable(GetModelClass(Assembly.GetCallingAssembly()));
            Provider.MultipleInsert(ModelClass);
            Provider.SubmitChanges();
        }

        private  void InitOrmTable()
        {
            
        }

        private List<DAI_ORM_TABLES_INSTANCE> TypeConvertDaiTable(List<Type> types)
        {
            List<DAI_ORM_TABLES_INSTANCE> tableRecords = new List<DAI_ORM_TABLES_INSTANCE>();

            foreach (var item in types)
            {
                tableRecords.Add(new DAI_ORM_TABLES_INSTANCE() { Name = item.Name, FullName = item.AssemblyQualifiedName });
            }

            return tableRecords;
        }

        #endregion

        #region Code First Generation Engine
        private void codeGeneration(List<Type> models)
        {

            // Kod üretecek c# derleyici ünitesi.
            var compileUnit = new CodeCompileUnit();
            // Sınıfın üretileceği assembly.
            var assembly = Assembly.GetCallingAssembly();
            // Sınıfın üretileceği namespace.
            var ns = new CodeNamespace(assembly.GetType().Namespace);
            // Namespace Assembly'e eklenir.
            compileUnit.Namespaces.Add(ns);
            // Oluşturulacak sınıf.
            var type = new CodeTypeDeclaration("AutoContext");
            // Sınıf özellikleri ayarlanır.
            type.TypeAttributes = TypeAttributes.Class | TypeAttributes.Public;
            // Sınıf Namespace'e eklenir.
            ns.Types.Add(type);
            CodeMemberProperty prop = new CodeMemberProperty();
            prop.Attributes = MemberAttributes.Public;
            prop.Name = "propName";
            prop.Type = new CodeTypeReference(Type.GetType("DAI.Orm.Test.Category, DAI.Orm.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
            type.Members.Add(prop);

            var provider = new CSharpCodeProvider();
            String sourceFile;
            if (provider.FileExtension[0] == '.')
            {
                sourceFile = "AutoContext" + provider.FileExtension;
            }
            else
            {
                sourceFile = "AutoContext." + provider.FileExtension;
            }
            CodeGeneratorOptions cp = new CodeGeneratorOptions();
            IndentedTextWriter tw = new IndentedTextWriter(new StreamWriter(sourceFile, false), "    ");
            provider.GenerateCodeFromCompileUnit(compileUnit, tw, cp);
            tw.Close();

            
        }
        
        #endregion

    }
}
