using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Rule.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class DomainObjectName:Attribute
    {
        public string Name { get; set; }
        public DomainObjectName(string name)
        {
            this.Name = name;
        }
    }
}
