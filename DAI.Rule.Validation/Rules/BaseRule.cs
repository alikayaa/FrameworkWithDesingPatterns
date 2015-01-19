using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Rule.Validation
{
    // Kurallar için soyut temel sınıf. Doğrulama Kurallar için temel fonksiyonları barındırır.
    // Bu sınıftan kalıtım alarak kendi doğrulama kurallarınızı yazabilirsiniz.
    public abstract class BaseRule
    {

        #region Members
        const string ERRORMESSAGE = " geçerli değil.";
        #endregion

        #region Propertys
        // Özellik
        public string Property { get; set; }
        // Hata mesajı.
        public string Error { get; set; }
        #endregion

        #region Constructors
        // Geçerli ayarlar ile yapıcı oluşturma
        public BaseRule(string property)
        {
            // Özellik adı ve tanımlı hata mesajı ayarlanır.
            Property = property;
            Error = property + ERRORMESSAGE;
        }
        // Hata mesajını değiştirerek yapıcı oluşturma
        public BaseRule(string property,string errorMessage)
        {
            // Özellik adı ve tanımlı hata mesajı ayarlanır.
            Property = property;
            Error = errorMessage;
        }
        #endregion

        #region Func
        // Doğrulama yöntemi, türetilmiş sınıflar ezerek kullanacaktır. Kendi kurallarını yazacaklardır.
        public abstract bool Validate(object ruleObject);

        // Yansıma(reflection) ile verilen RuleObjects objesini döndürür. Reflection ile çok azda olsa performans kaybı yaşanır.
        // Projenizin büyüklüğüne göre türetilmiş sınıfınızda kendi metodunuzu yazabilirsiniz.
        // Milisaniyeler ile ölçüm yaptığınız projelere kadar göz ardı edilebilir bir kayıptır.
        public object GetPropertyValue(object ruleObject)
        {
            return ruleObject.GetType().GetProperty(Property).GetValue(ruleObject, null);
        }
        #endregion
    }
}
