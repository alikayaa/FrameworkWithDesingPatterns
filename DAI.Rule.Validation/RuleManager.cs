using DAI.Orm.Context;
using DAI.Rule.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DAI.Rule.Validation
{
    public class RuleManager
    {

        /// <summary>
        /// Doğrulama kuralı hatalarını tutan liste.
        /// </summary>
        public static List<string> ErrorList = new List<string>();

        /// <summary>
        /// Business üzerinde tanımlanmış entity sınıflarını getirir.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> GetDomainObjects()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var asm in assembly.GetTypes())
                {
                    var domainObj = asm.GetCustomAttributes(typeof(DomainObjectName), false).FirstOrDefault();
                    if (domainObj != null)
                    {
                        result.Add(asm.AssemblyQualifiedName,((DomainObjectName)domainObj).Name);
                    }
                }

            }

            return result;

        }

        /// <summary>
        /// Business üzerinde tanımlanmış entity sınıflarını DropDownList'e ekler.
        /// </summary>
        /// <returns></returns>
        public void GetDomainObjects(DropDownList HtmlControl)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var asm in assembly.GetTypes())
                {
                    var domainObj = asm.GetCustomAttributes(typeof(DomainObjectName), false).FirstOrDefault();
                    if (domainObj != null)
                    {
                        HtmlControl.Items.Add(new ListItem() { Value = asm.AssemblyQualifiedName, Text = ((DomainObjectName)domainObj).Name });
                    }
                }

            }

        }

        /// <summary>
        /// Business Domain Entity Üzerindeki Alanları Getirir.(Property,Field)
        /// </summary>
        /// <param name="classFullName"></param>
        /// <returns></returns>
        public List<string> GetDomainObjectFields(string classFullName)
        {
            List<string> result = new List<string>();

            var classType = Type.GetType(classFullName);

            foreach (var property in classType.GetProperties())
            {
                result.Add(property.Name);
            }

            foreach (var field in classType.GetFields())
            {
                result.Add(field.Name);
            }
            return result;
        }

        /// <summary>
        /// Business Domain Entity Üzerindeki Alanları DropDownList'e ekler.(Property,Field)
        /// </summary>
        /// <param name="classFullName"></param>
        /// <returns></returns>
        public void GetDomainObjectFields(DropDownList HtmlControl, string classFullName)
        {
            var classType = Type.GetType(classFullName);

            foreach (var property in classType.GetProperties())
            {
                HtmlControl.Items.Add(new ListItem() {Value = property.Name,Text = property.Name });
            }

            foreach (var field in classType.GetFields())
            {
                HtmlControl.Items.Add(new ListItem() { Value = field.Name, Text = field.Name });
            }
        }

        /// <summary>
        /// Business Validation katmanında tanımlanmış doğrulama kurallarını getirir.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetValidationRule()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var asm in assembly.GetTypes().Where(t => t.BaseType != null && t.BaseType.Name == "BaseRule").ToList())
                {
                    result.Add(asm.AssemblyQualifiedName, asm.Name);
                }

            }
            return result;

        }


        /// <summary>
        /// Business Validation katmanında tanımlanmış doğrulama kurallarını DropDownList'e ekler.
        /// </summary>
        /// <returns></returns>
        public void GetValidationRule(DropDownList HtmlControl)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var asm in assembly.GetTypes().Where(t => t.BaseType != null && t.BaseType.Name == "BaseRule").ToList())
                {
                    HtmlControl.Items.Add(new ListItem() { Value=asm.AssemblyQualifiedName, Text=asm.Name});
                }
            }
        }

        /// <summary>
        /// Verilen Domain Object'i kurallara göre doğrulama yapar.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DomainObject"></param>
        /// <returns></returns>
        public static bool IsValid<T>(T DomainObject)
        {
            ErrorList.Clear();
            // Kural Listesi
            List<BaseRule> Rules = new List<BaseRule>();
            bool result = true;
            Type types = DomainObject.GetType();
            AutoContext db = new AutoContext();
            string className = types.AssemblyQualifiedName;
            DAIList<DAI_VALIDATION_RULES> rules = db.Rules.ToList();
            foreach (var item in rules)
            {
                if(item.ClassName == className)
                    Rules.Add(getRule(item.ClassName, item.FieldName, item.RuleName, item.ErrorMessage, item.OtherParams));
            }

            foreach (var rule in Rules)
            {

                if (!rule.Validate(DomainObject))
                {
                    result = false;
                    ErrorList.Add(rule.Error);
                }
            }

            return result;
        }

        /// <summary>
        /// Verilen kural adına göre kural sınıflarını döner.
        /// </summary>
        /// <param name="RuleName"></param>
        /// <returns></returns>
        private static BaseRule getRule(string ClassName, string FieldName, string RuleName, string ErrorMessage, string OtherParam)
        {
            int j = 0;
            object[] OtherParams = OtherParam.Split(',');
            Type ruleType = Type.GetType(RuleName);
            ConstructorInfo method = ruleType.GetConstructors()[ruleType.GetConstructors().Length - 1];
            ParameterInfo[] parameters = method.GetParameters();
            object[] parameterCtor = new object[parameters.Length];
            parameterCtor[0] = FieldName;
            parameterCtor[1] = ErrorMessage;
            for (int i = 2; i < parameterCtor.Length; i++, j++)
            {
                Type parameterType = parameters[i].ParameterType;
                parameterCtor[i] = Convert.ChangeType(OtherParams[j], parameterType);
            }
            return (BaseRule)Activator.CreateInstance(ruleType, parameterCtor);
        }
    }
}
