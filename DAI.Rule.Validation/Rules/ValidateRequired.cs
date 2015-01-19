using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Rule.Validation
{
    public class ValidateRequired : BaseRule
    {
        #region Members
        const string ERRORMESSAGE = " zorunlu alan";
        #endregion

        #region Constructors
        // Geçerli ayarlar ile yapıcı oluşturma
        public ValidateRequired(string propertyName)
            : base(propertyName)
        {
            Error = propertyName + ERRORMESSAGE;
        }
        // Hata mesajını değiştirerek yapıcı oluşturma
        public ValidateRequired(string propertyName, string errorMessage)
            : base(propertyName)
        {
            Error = errorMessage;
        }
        #endregion

        #region Func
        // Doğrulama kuralı
        public override bool Validate(object businessObject)
        {
            try
            {
                return GetPropertyValue(businessObject).ToString().Length > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
