using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAI.Rule.Validation
{
    
    // IP Adresi doğrulama
    // Ip adresi eşleştirme yöntemi ile doğrulanır
    public class ValidateIPAddress : ValidateRegex
    {
        #region Constructors
        // Varsayılan ayarlar ile yapıcı oluşturma
        public ValidateIPAddress(string propertyName) :
            base(propertyName, @"^([0-2]?[0-5]?[0-5]\.){3}[0-2]?[0-5]?[0-5]$")
        {
            Error = propertyName + " is not a valid IP Address";
        }
        // Hata mesajını değiştirerek yapıcı oluşturma
        public ValidateIPAddress(string propertyName, string errorMessage) :
            this(propertyName)
        {
            Error = errorMessage;
        }

        #endregion
    }
}
