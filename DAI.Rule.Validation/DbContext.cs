using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DAI.Orm;
using DAI.Orm.Context;
using DAI.Orm.Attributes;
namespace DAI.Rule.Validation
{
    public class AutoContext : BaseContext
    {
        public DAIList<DAI_VALIDATION_RULES> Rules = new DAIList<DAI_VALIDATION_RULES>();

    }
}
