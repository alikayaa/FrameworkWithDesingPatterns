using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAI.Rule.Validation
{

    // Kredi kartı doğrulama kuralı.
    // 16 haneli 4'er haneden sonra boşluk olacak şekilde kontrol yapar.

    public class ValidateCreditcard : ValidateRegex
    {
        #region Members
        const string ERRORMESSAGE = " geçersiz kredi kartı numarası.";
        #endregion

        #region Constructors
        // Varsayılan ayarlar ile yapıcı oluşturma
        public ValidateCreditcard(string propertyName) :
            base(propertyName, @"^((\d{4}[- ]?){3}\d{4})$")
        {
            Error = propertyName + ERRORMESSAGE;
        }
        // Hata mesajını değiştirerek yapıcı oluşturma
        public ValidateCreditcard(string propertyName, string errorMessage) :
            this(propertyName)
        {
            Error = errorMessage;
        }

        #endregion
    }
}
