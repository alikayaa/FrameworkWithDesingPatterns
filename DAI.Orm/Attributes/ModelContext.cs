using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Class)]
    public class ModelContext:Attribute
    {
        private string[]  _models;

        public ModelContext(params string[] models)
        {
            this._models = models;
        }

        public string[] Models
        {
            get
            {
                return this._models;
            }
        }
    }
}
