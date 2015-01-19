using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAI.Rule.Validation
{
    
    // identity validation rule. 
    // value must be integer and greater than zero
    
    public class ValidateId : BaseRule
    {

        #region Members
        const string ERRORMESSAGE = " geçersiz id";
        #endregion

        #region Constructors
        // Varsayılan ayarlar ile yapıcı oluşturma
        public ValidateId(string propertyName)
            : base(propertyName)
        {
            Error = propertyName + ERRORMESSAGE;
        }
        // Hata mesajını değiştirerek yapıcı oluşturma
        public ValidateId(string propertyName, string errorMessage)
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
                int id = int.Parse(GetPropertyValue(businessObject).ToString());
                return id >= 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
