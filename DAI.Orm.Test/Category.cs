using DAI.Orm.Attributes;
using DAI.Orm.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Test
{

    public class Category:IModel
    {
        [AutoInc(1)]
        [PrimaryKeyAttr]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        [ForeignKeyAttr("Product","Id","int")]
        public DAIList<Product> CatProduct { get; set; }
    }
}
