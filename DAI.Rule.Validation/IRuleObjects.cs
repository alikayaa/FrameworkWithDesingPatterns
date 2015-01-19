using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Rule.Validation
{

    // Kural sınıfı arayüzü.
    // Bu arayüzden kalıtım alarak kural objeleri oluşturulabilinir. 
    // Gerekli fonksiyonlar ve alanlar bu arayüz içerisine kullanılmıştır. 
    public interface IRuleObjects
    {
        #region Propertys
        // Kural listesi
        List<BaseRule> Rules { get; }
        // Hataları alma.
        List<string> Errors { get; }
        #endregion

        #region Func
        // Kural listesine yeni kural ekleme

        void AddRule(BaseRule rule);

        // Kural doğrulaması yapar ve hataları geri döndürür

        bool IsValid();

        #endregion
    }
}
