using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAI.Rule.Validation
{
    
    // Veri Uzunluk doğrulama
    // Verilen iki değer arası doğrulama yapar
    
    public class ValidateLength : BaseRule
    {
        #region Members
        private int _min;
        private int _max;
        const string ERRORMESSAGE1 = " en az";
        const string ERRORMESSAGE2 = " en çok";
        const string ERRORMESSAGE3 = " olmalıdır.";
        #endregion

        #region Constructors
        // Varsayılan değerler ile yapıcı oluşturma
        public ValidateLength(string propertyName, int min, int max)
            : base(propertyName)
        {
            _min = min;
            _max = max;

            Error = propertyName + ERRORMESSAGE1 + _min + ERRORMESSAGE2 + _max + ERRORMESSAGE3;
        }
        // Hata mesajını değiştirerek yapıcı oluşturma
        public ValidateLength(string propertyName, string errorMessage, int min, int max)
            : this(propertyName, min, max)
        {
            Error = errorMessage;
        }
        #endregion

        #region Func
        // Doğrulama kuralı
        public override bool Validate(object businessObject)
        {
            // Özellik değer uzunluğu
            int length = GetPropertyValue(businessObject).ToString().Length;
            return length >= _min && length <= _max;
        }

        #endregion
    }
}
