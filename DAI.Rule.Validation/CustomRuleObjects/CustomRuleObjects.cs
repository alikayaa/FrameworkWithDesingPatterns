using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAI.Rule.Validation;


namespace DAI.Rule.Validation.CustomRuleObjects
{
    public abstract class CustomRuleObjects:IRuleObjects
    {
        #region Members
        // Kural listesi
        List<BaseRule> rules = new List<BaseRule>();
        // Hata listesi
        List<string> errors = new List<string>();
        #endregion

        #region Propertys
        public List<BaseRule> Rules
        {
            get
            {
                return rules;
            }

        }

        public List<string> Errors
        {
            get
            {
                return errors;
            }

        }
        #endregion

        #region Func
        // Kural listesine yeni kural ekleme

        public void AddRule(BaseRule rule)
        {
            Rules.Add(rule);
        }
        // Kural doğrulaması yapar ve hataları geri döndürür

        public bool IsValid()
        {
            bool valid = true;

            Errors.Clear();

            foreach (var rule in Rules)
            {
                if (!rule.Validate(this))
                {
                    valid = false;
                    Errors.Add(rule.Error);
                }
            }
            return valid;
        }

        #endregion
    }
}
