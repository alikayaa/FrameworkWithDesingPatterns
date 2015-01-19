using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAI.Orm.Attributes;
using DAI.Orm.Context;
namespace DAI.Orm.Test
{
    public class Product : IModel
    {
        [AutoInc(1, 1)]
        [PrimaryKeyAttr]
        public int Id { get; set; }
        [NVARCHAR(255)]
        public string ProductName { get; set; }
        [VARCHAR(255)]
        public string ProductCode { get; set; }
        [ForeignKeyAttr("Category", "Id", "int")]
        public int CategoryId { get; set; }
        public DAIList<Category> Categories { get { return DAIList<Category>.GetRelation(this); } }
    }


    public class ProductNew : IModel
    {
        [AutoInc(1, 1)]
        public int Id { get; set; }
        [PrimaryKeyAttr]
        [NVARCHAR(255)]
        [NOTNULL]
        public string ProductName { get; set; }
        [VARCHAR(255)]
        [NOTNULL]
        public string ProductCode { get; set; }
        [ForeignKeyAttr("Category", "Id", "int")]
        public int CategoryId { get; set; }
    }
}
