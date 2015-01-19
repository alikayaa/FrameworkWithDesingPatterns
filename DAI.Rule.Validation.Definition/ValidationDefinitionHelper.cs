using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace DAI.Rule.Validation.Definition
{
    /// <summary>
    /// Doğrulama kuralları ekranı için helper metodlar barındırır.
    /// Genellikle ekrana yeni kontrol ekleme ağırlıklı metodlardır.
    /// Kalıtıma vb. işleme sokulmayacağından dolayı metodları statiktir.
    /// </summary>
    public class ValidationDefinitionHelper
    {
        
        /// <summary>
        /// Doğrulama Kuralı Oluşturmak İçin Gerekli Alanlara Sayfa da Kontrol Ekler.
        /// </summary>
        /// <param name="definitionPage"></param>
        /// <param name="parameters"></param>
        public static void AddControlByRuleParameters(Control definitionPage,Type ruleType)
        {
            // Temel Parameterler
            List<string> paramList = new List<string>() { "propertyName", "errorMessage" };
            ParameterInfo[] parameters = GetRuleConstructorParameters(ruleType);
            foreach (var param in parameters)
            {
                if (paramList.IndexOf(param.Name) == -1)
                {
                    HtmlGenericControl paramControls = new HtmlGenericControl("fieldset");
                    HtmlGenericControl legend = new HtmlGenericControl("legend");
                    legend.InnerText = "Ekstra Kural Özelliği";
                    paramControls.Controls.Add(legend);
                    Label controlsLabel = new Label();
                    controlsLabel.Text = param.Name;
                    TextBox txtBox = new TextBox();
                    txtBox.ID = param.Name;
                    txtBox.Attributes.Add("runat", "server");
                    paramControls.Controls.Add(controlsLabel);
                    paramControls.Controls.Add(txtBox);
                    definitionPage.Controls.AddAt(definitionPage.Controls.Count-2,paramControls);
                }
            }

        }
        /// <summary>
        /// Verilen Doğrulama Kuralının Yapıcı Fonksiyon Parametreleri Döndürür.
        /// </summary>
        /// <returns></returns>
        public static ParameterInfo[] GetRuleConstructorParameters(Type ruleType)
        {
            ConstructorInfo ruleCtor = ruleType.GetConstructors()[ruleType.GetConstructors().Length - 1];
            return ruleCtor.GetParameters();
        }
    }
}