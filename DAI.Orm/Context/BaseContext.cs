using DAI.Orm.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Context
{
    public abstract class BaseContext
    {
        public void SaveChanges()
        {
            var baseType = typeof(BaseContext).FullName;

            Type contextType = Assembly.GetCallingAssembly().GetTypes().Where(t => t.BaseType == typeof(BaseContext) && t.Name == "AutoContext").FirstOrDefault();
            FieldInfo[] fields = contextType.GetFields();
            foreach (FieldInfo field in fields)
            {
                foreach (var item in field.FieldType.GetRuntimeFields())
                {
                    MethodInfo method = item.FieldType.GetMethod("SubmitChanges");
                    if (method != null)
                    {
                        ParameterInfo[] paramInfo = method.GetParameters();
                        object[] methodParams = new object[paramInfo.Length];

                        string assemblyQualName = (field.FieldType).GenericTypeArguments[0].AssemblyQualifiedName;
                        // Model Tipi
                        Type ModelType = Type.GetType(assemblyQualName);
                        // SQLProvider Tipi
                        Type d1 = typeof(SQLServerProvider<>);
                        // Provider'in çalışacağı tip <T> 
                        Type[] typeArgs = { ModelType };
                        // Provider Generic Tipi
                        Type makeme = d1.MakeGenericType(typeArgs);
                        // Provider Nesnesi
                        var o = Activator.CreateInstance(makeme);

                       method.Invoke(o,null);
                       break;
                    }
                }
            }
        }
    }
}
