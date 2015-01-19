using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAI.Rule.Validation
{
    
    // Bu sınıf verilen iki veri türü ve operatör ile karşılaştırma yapar.
    
    public class ValidateCompare : BaseRule
    {
        #region Member
        // Hata mesajı.
        const string ERRORMESSAGE = " olmalıdır.";
        #endregion

        #region Propertys
        // Karşılaştırılacak özellik adı
        string OtherPropertyName { get; set; }
        // Karşılaştırma veri tipi
        ValidationDataType DataType { get; set; }
        // Karşılaştırma operatörü
        ValidationOperator Operator { get; set; }
        #endregion

        #region Constructors
        // Varsayılan hata mesajlı yapıcı
        public ValidateCompare(string propertyName, string otherPropertyName, 
            ValidationOperator @operator, ValidationDataType dataType )
            : base(propertyName)
        {
            
            OtherPropertyName = otherPropertyName;
            Operator = @operator;
            DataType = dataType;

            Error = propertyName + otherPropertyName + Operator.ToString() + ERRORMESSAGE;
        }

        // Değiştirilmiş hata mesajlı yapıcı
        public ValidateCompare(string propertyName, string otherPropertyName, string errorMessage,
            ValidationOperator @operator, ValidationDataType dataType )
            : this(propertyName, otherPropertyName, @operator, dataType)
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
                // 1.Özellik 
                string propValue1 = businessObject.GetType().GetProperty(Property).GetValue(businessObject, null).ToString();
                // 2.Özellik 
                string propValue2 = businessObject.GetType().GetProperty(OtherPropertyName).GetValue(businessObject, null).ToString();
                
                // Veritipine göre karar verme
                switch(DataType)
                {
                    case ValidationDataType.Integer:

                        // 1.Özellik değerini alma
                        int ival1 = int.Parse(propValue1);
                        // 2.Özellik değerini alma
                        int ival2 = int.Parse(propValue2);

                        // Operatöre göre karar verme
                        switch(Operator)
                        {
                            // Operatöre göre sonuç geri döner.
                            case ValidationOperator.Equal: return ival1 == ival2; 
                            case ValidationOperator.NotEqual: return ival1 != ival2; 
                            case ValidationOperator.GreaterThan: return ival1 > ival2; 
                            case ValidationOperator.GreaterThanEqual: return ival1 >= ival2; 
                            case ValidationOperator.LessThan: return ival1 < ival2; 
                            case ValidationOperator.LessThanEqual: return ival1 <= ival2; 
                        }
                        break;
                        
                    case ValidationDataType.Double:

                        // 1.Özellik değerini alma
                        double dval1 = double.Parse(propValue1);
                        // 2.Özellik değerini alma
                        double dval2 = double.Parse(propValue2);

                        // Operatöre göre karar verme
                        switch(Operator)
                        {
                            // Operatöre göre sonuç geri döner.
                            case ValidationOperator.Equal: return dval1 == dval2; 
                            case ValidationOperator.NotEqual: return dval1 != dval2; 
                            case ValidationOperator.GreaterThan: return dval1 > dval2; 
                            case ValidationOperator.GreaterThanEqual: return dval1 >= dval2; 
                            case ValidationOperator.LessThan: return dval1 < dval2; 
                            case ValidationOperator.LessThanEqual: return dval1 <= dval2; 
                        }
                        break;
                        
                    case ValidationDataType.Decimal:

                        // 1.Özellik değerini alma
                        decimal cval1 = decimal.Parse(propValue1);
                        // 2.Özellik değerini alma
                        decimal cval2 = decimal.Parse(propValue2);

                        // Operatöre göre karar verme
                        switch(Operator)
                        {
                            // Operatöre göre sonuç geri döner.
                            case ValidationOperator.Equal: return cval1 == cval2; 
                            case ValidationOperator.NotEqual: return cval1 != cval2; 
                            case ValidationOperator.GreaterThan: return cval1 > cval2; 
                            case ValidationOperator.GreaterThanEqual: return cval1 >= cval2; 
                            case ValidationOperator.LessThan: return cval1 < cval2; 
                            case ValidationOperator.LessThanEqual: return cval1 <= cval2; 
                        }
                        break;

                    case ValidationDataType.Date:

                        // 1.Özellik değerini alma
                        DateTime tval1 = DateTime.Parse(propValue1);
                        // 2.Özellik değerini alma
                        DateTime tval2 = DateTime.Parse(propValue2);

                        // Operatöre göre karar verme
                        switch(Operator)
                        {
                            // Operatöre göre sonuç geri döner.
                            case ValidationOperator.Equal: return tval1 == tval2; 
                            case ValidationOperator.NotEqual: return tval1 != tval2; 
                            case ValidationOperator.GreaterThan: return tval1 > tval2; 
                            case ValidationOperator.GreaterThanEqual: return tval1 >= tval2; 
                            case ValidationOperator.LessThan: return tval1 < tval2; 
                            case ValidationOperator.LessThanEqual: return tval1 <= tval2; 
                        }
                        break;

                    case ValidationDataType.String:

                        // string karşılaştırma sonucu
                        int result = string.Compare(propValue1, propValue2, StringComparison.CurrentCulture);

                        // Operatöre göre karar verme
                        switch(Operator)
                        {
                            // Operatöre göre sonuç geri döner.
                            case ValidationOperator.Equal: return result == 0; 
                            case ValidationOperator.NotEqual: return result != 0; 
                            case ValidationOperator.GreaterThan: return result > 0; 
                            case ValidationOperator.GreaterThanEqual: return result >= 0; 
                            case ValidationOperator.LessThan: return result < 0; 
                            case ValidationOperator.LessThanEqual: return result <= 0; 
                        }
                        break;

                }
                return false;
            }
            catch{ return false; }
        }

        #endregion
    }
}
