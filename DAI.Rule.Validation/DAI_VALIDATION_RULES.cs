using DAI.Orm;
using DAI.Orm.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Rule.Validation
{
    public class DAI_VALIDATION_RULES:IModel
    {
        [PrimaryKeyAttr]
        public int Id { get; set; }
        [NVARCHAR]
        public string ClassName { get; set; }
        [NVARCHAR]
        public string FieldName { get; set; }
        [NVARCHAR]
        public string RuleName { get; set; }
        [NVARCHAR]
        public string ErrorMessage { get; set; }
        [NVARCHAR]
        public string OtherParams { get; set; }
    }
}
