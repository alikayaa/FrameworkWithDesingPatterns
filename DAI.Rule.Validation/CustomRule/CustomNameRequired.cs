using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAI.Rule.Validation;

namespace DAI.Rule.Validation.CustomRule
{
    public class CustomNameRequired : BaseRule
    {
        #region Members
        const string ERRORMESSAGE = " ad alanı DAI olmalıdır.";
        const string ReqName = "DAI";
        #endregion

        #region Constructors
        // Varsayılan değerler ile yapıcı oluşturma
        public CustomNameRequired(string propertyName)
            : base(propertyName)
        {
            Error = propertyName + ERRORMESSAGE;
        }
        // Hata mesajını değiştirerek yapıcı oluşturma
        public CustomNameRequired(string propertyName, string errorMessage)
            : this(propertyName)
        {
            Error = errorMessage;
        }
        #endregion

        #region Func
        // Doğrulama kuralı
        public override bool Validate(object businessObject)
        {
            // Özellik değer uzunluğu
            string Name = GetPropertyValue(businessObject).ToString();
            return Name == ReqName;

        }

        #endregion
    }
}
