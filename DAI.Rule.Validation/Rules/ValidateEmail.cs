using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAI.Rule.Validation
{
    
    // Email doğrulma kuralı.
    
    public class ValidateEmail : ValidateRegex
    {
        #region Constructors
        // Varsayılan ayarlar ile yapıcı oluşturma
        public ValidateEmail(string propertyName) :
            base(propertyName, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
        {
            Error = propertyName + " is not a valid email address";
        }
        // Hata mesajını değiştirerek yapıcı oluşturma
        public ValidateEmail(string propertyName, string errorMessage) :
            this(propertyName)
        {
            Error = errorMessage;
        }

        #endregion
    }
}
