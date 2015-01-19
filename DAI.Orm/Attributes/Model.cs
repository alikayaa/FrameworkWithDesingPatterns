using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class Model : Attribute
    {
        private string _modelName;
        public Model(string modelName)
        {
            this._modelName = modelName;
        }

        public string ModelName
        {
            get
            {
                return this._modelName;
            }

        }

        public string GetModelName()
        {
            return this._modelName;
        }
        //public Type ModelType
        //{
        //    get
        //    {
        //        return this.GetModelType();
        //    }
        //}

        //private Type GetModelType()
        //{

        //    return Type;

        //}


    }
}
