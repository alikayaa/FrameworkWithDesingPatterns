using DAI.Orm.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.EntityModel;
namespace DAI.Orm.Test
{
    public class Person : IModel
    {
        [AutoInc(1,1)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

    }
}
