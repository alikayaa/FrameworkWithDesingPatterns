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
using DAI.Orm.Test;
namespace DAI.Orm.Context
{
    public class AutoContext : BaseContext
    {
        public DAIList<Category> Category = new DAIList<Category>();
        public DAIList<Person> Person = new DAIList<Person>();
        public DAIList<Product> Product = new DAIList<Product>();
        public DAIList<ProductNew> ProductNew = new DAIList<ProductNew>();
    }
}
