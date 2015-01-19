using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace DAI.Rule.Validation.Definition
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                InitRulePage();
        }

        private void InitRulePage()
        {
            RuleManager ruleManager = new RuleManager();
            ruleManager.GetDomainObjects(dominObjList);
            ruleManager.GetValidationRule(validationRule);
        }
        
        protected void dominObjList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RuleManager ruleManager = new RuleManager();
            fieldList.Items.Clear();
            ruleManager.GetDomainObjectFields(fieldList,dominObjList.SelectedItem.Value);
            
        }

        protected void validationRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ruleFullName = validationRule.SelectedItem.Value;
            Type ruleType = Type.GetType(ruleFullName);
            ValidationDefinitionHelper.AddControlByRuleParameters(form1, ruleType);
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            string parameters = string.Empty;
            // Request Key 0-9 düzenli parametreler
            List<string> AllKeys = Request.Form.AllKeys.ToList();
            List<string> NewKeys = new List<string>();
            if (AllKeys.Count > 9)
            {
                for (int i = 10; i < AllKeys.Count; i++)
                {
                    NewKeys.Add(AllKeys[i]);
                }
            }

            if (NewKeys.Count > 0)
            {
                foreach (var item in NewKeys)
                {
                    parameters += Request.Form[item] + ",";    
                }
                parameters = parameters.Substring(0, parameters.Length - 1);     
            }

            DAI_VALIDATION_RULES validationRuleEnt = new DAI_VALIDATION_RULES();
            validationRuleEnt.ClassName = dominObjList.SelectedItem.Value;
            validationRuleEnt.FieldName = fieldList.SelectedItem.Value;
            validationRuleEnt.RuleName = validationRule.SelectedItem.Value;
            validationRuleEnt.OtherParams = parameters;
            validationRuleEnt.ErrorMessage = errTxt.Text;
            AutoContext db = new AutoContext();
            db.DAI_VALIDATION_RULES.AddItem(validationRuleEnt);
            db.SaveChanges();
        }


    }
}