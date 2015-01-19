using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DAI.Rule.Validation
{
    
    // Regex doğrulama kuralları için temel sınıf
    
    public class ValidateRegex : BaseRule
    {
        #region Propertys
        protected string Pattern { get; set; }
        #endregion 

        #region Constructors
        // Varsayılan değerler ile yapıcı oluşturma
        public ValidateRegex(string propertyName, string pattern)
            : base(propertyName)
        {
            Pattern = pattern;
        }
        // Hata mesajını değiştirerek yapıcı oluşturma
        public ValidateRegex(string propertyName, string errorMessage, string pattern)
            : this(propertyName, pattern)
        {
            Error = errorMessage;
        }
        #endregion

        #region Func
        public override bool Validate(object businessObject)
        {
            return Regex.Match(GetPropertyValue(businessObject).ToString(), Pattern).Success;
        }
        #endregion
    }
}
