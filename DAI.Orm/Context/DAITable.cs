using DAI.Orm.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Context
{
    internal class DAI_ORM_TABLES_INSTANCE
    {
        [AutoInc(1)]
        public int ID { get; set; }
        [NVARCHAR]
        public string Name { get; set; }
        [NVARCHAR]
        public string FullName { get; set; }
    }
}
