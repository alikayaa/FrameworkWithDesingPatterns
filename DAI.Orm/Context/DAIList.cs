using DAI.Orm.Attributes;
using DAI.Orm.Provider;
using DAI.Orm.T4Template;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Context
{
    public class DAIList<T> : List<T> where T : IModel
    {
        #region Private Members
        private static List<T> _innerList = new List<T>();
        IDbProvider<T> provider;
        #endregion

        #region Constructor
        public DAIList()
        {
            provider = OrmFactory<T>.GetDbProvider();
        }
        #endregion

        #region Methods

        #region T-Sql Func
        public T First()
        {
            return provider.Select("TOP (1) *")[0];
        }

        public T FirstOrDefault()
        {
            List<T> result = provider.Select("TOP (1) *");
            if (result != null)
                return result[0];
            else
                return result.FirstOrDefault();
        }

        public DAIList<T> ToList()
        {

            if(_innerList.Count == 0)
                _innerList = provider.Select();
            return this.Converts(_innerList);
        }

        public DAIList<T> OrderBy(Expression<Func<T, object>> predicate)
        {
            string OrderByClause = ExpressionToString(predicate);
            _innerList = provider.Select("*", "",string.IsNullOrEmpty(OrderByClause) ? OrderByClause : string.Format(" ORDER BY {0}", OrderByClause));
            return this.Converts(_innerList);
        }

        public DAIList<T> OrderByDescending(Expression<Func<T, object>> predicate)
        {
            string OrderByClause = ExpressionToString(predicate);
            _innerList = provider.Select("*","", string.IsNullOrEmpty(OrderByClause) ? OrderByClause : string.Format(" ORDER BY {0} DESC", OrderByClause));
            return this.Converts(_innerList);
        }

        public DAIList<T> Where(Expression<Func<T, object>> predicate)
        {
            string whereClause = ExpressionToString(predicate);
            _innerList = provider.Select("*", "",string.IsNullOrEmpty(whereClause) ? whereClause : string.Format(" WHERE {0}", whereClause));
            DAIList<T> result = new DAIList<T>();
            result = this.Converts(_innerList);
            return result;
        }

        public decimal Max(Expression<Func<T,object>> predicate)
        {
            string MaxClause = ExpressionToString(predicate);
            string field = ExpressionToFieldString(predicate);
            if (_innerList.Count == 0)
                _innerList = provider.Select(string.Format("MAX({0}) AS {1}", MaxClause, field), field);
            return _innerList.Count > 0 ?  Convert.ToDecimal(_innerList[0].GetType().GetProperty(field).GetValue(_innerList[0])) : 0;
        }

        public decimal Min(Expression<Func<T, object>> predicate)
        {
            string MinClause = ExpressionToString(predicate);
            string field = ExpressionToFieldString(predicate);
            if (_innerList.Count == 0)
                _innerList = provider.Select(string.Format("MIN({0}) AS {1}",MinClause,field),field);
            return _innerList.Count > 0 ? Convert.ToDecimal(_innerList[0].GetType().GetProperty(field).GetValue(_innerList[0])) : 0;
        }

        public decimal Avg(Expression<Func<T, object>> predicate)
        {
            string AvgClause = ExpressionToString(predicate);
            string field = ExpressionToFieldString(predicate);
            if (_innerList.Count == 0)
                _innerList = provider.Select(string.Format("AVG({0}) AS {1}", AvgClause, field),field);
            return _innerList.Count > 0 ? Convert.ToDecimal(_innerList[0].GetType().GetProperty(field).GetValue(_innerList[0])) : 0;
        }

        public decimal Sum(Expression<Func<T, object>> predicate)
        {
            string SumClause = ExpressionToString(predicate);
            string field = ExpressionToFieldString(predicate);
            if (_innerList.Count == 0)
                _innerList = provider.Select(string.Format("SUM({0}) AS {1}", SumClause, field), field);
            return _innerList.Count > 0 ? Convert.ToDecimal(_innerList[0].GetType().GetProperty(field).GetValue(_innerList[0])) : 0;
        }

        public DAIList<T> GroupBy(Expression<Func<T, object>> predicate)
        {
            string GroupByClause = ExpressionToString(predicate);
            string field = ExpressionToFieldString(predicate);

            if (_innerList.Count == 0)
                _innerList = provider.Select(string.Format(field),string.Format("GROUP BY {0}",GroupByClause));

            return this.Converts(_innerList);

        }
        
        public DAIList<T> Take(int count)
        {
            if (_innerList.Count == 0)
                return this.Converts(provider.Select("TOP " + count + " *"));
            else
                return this.Converts(_innerList.Take(count).ToList());
        }

        public DAIList<T> Skip(int count)
        {
            if (_innerList.Count == 0)
                _innerList = provider.Select();
            _innerList = _innerList.Skip(count).ToList();
            return this.Converts(_innerList);
        }

        public static DAIList<T> GetRelation(IModel model)
        {
            int relationId = 0;
            string fkName = string.Empty;
            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                var fk = propertyInfo.GetCustomAttributes(typeof(ForeignKeyAttr), false).FirstOrDefault();
                if (fk != null)
                {
                    relationId = relationId == 0 ? Convert.ToInt32(propertyInfo.GetValue(model)) : relationId;

                    break;
                }
                
            }

            foreach (var item in typeof(T).GetProperties())
            {
                var fk = item.GetCustomAttributes(typeof(ForeignKeyAttr), false).FirstOrDefault();
                if (fk != null)
                {
                    fkName = ((ForeignKeyAttr)fk).FOREIGNKEYCOLUMN;
                    break;
                }
            }
            _innerList = new SQLServerProvider<T>().Select("*", "","Where " + fkName + " = " + relationId);
             DAIList<T> result = new DAIList<T>();
            foreach (var item in _innerList)
            {
                result.Add(item);
            }
            return result;
        }
        #endregion

        #region Crud Func
        public void AddItem(T entity)
        {
            provider.Insert(entity);
        }

        public void AddRange(List<T> entityList)
        {
            provider.MultipleInsert(entityList);
        }

        public void Delete(T entity)
        {
            provider.Delete(entity);
        }

        public void DeleteRange(List<T> entityList)
        {
            provider.MultipleDelete(entityList);
        }

        public void Update(T entity)
        {
            provider.Update(entity);
        }
        #endregion
        
        #region Private Func

        private DAIList<T> Converts(List<T> obj)
        {
            DAIList<T> result = new DAIList<T>();
            foreach (var item in obj)
            {
                result.Add(item);
            }

            return result;
        }

        private string ExpressionToString(Expression<Func<T, object>> predicate)
        {
            string result = ((LambdaExpression)predicate).Body.ToString();
            // Expression parametresi
            var paramName = predicate.Parameters[0].Name;
            // Expression parametre tipi
            var paramTypeName = predicate.Parameters[0].Type.Name;
            // Expression mantıksal operatörleri sql operatörlerine çevrilir.
            result = result.Replace(paramName + ".", paramTypeName + ".")
                             .Replace("AndAlso", "AND")
                             .Replace("OrElse","OR")
                             .Replace("\"","'")
                             .Replace("==","=")
                             .Replace("Convert","");
                             
            return result;
        }

        private string ExpressionToFieldString(Expression<Func<T, object>> predicate)
        {
            string result = ((LambdaExpression)predicate).Body.ToString();
            // Expression parametresi
            var paramName = predicate.Parameters[0].Name;
            // Expression parametre tipi
            var paramTypeName = predicate.Parameters[0].Type.Name;
            // Expression mantıksal operatörleri sql operatörlerine çevrilir.
            result = result.Replace(paramName + ".", paramTypeName + ".")
                             .Replace("AndAlso", "AND")
                             .Replace("OrElse", "OR")
                             .Replace("\"", "'")
                             .Replace("==", "=")
                             .Replace("Convert", "");
            string [] fields = result.Split('.');
            result = fields[1].Replace(")","");
            return result;
        }
        // Server Side Context
        private DAIList<T> GetServer([CallerMemberName] string callerName = "")
        {

            // Seçilen Model
            var modelProp = typeof(DbContext).GetProperty(callerName);
            // Seçilen Model Tam Adı
            var modelFullName = modelProp.GetCustomAttributes(typeof(Model), true).FirstOrDefault();

            if (modelFullName != null)
            {
                // Model Tipi
                Type ModelType = Type.GetType(((Model)(modelFullName)).ModelName);
                // SQLProvider Tipi
                Type d1 = typeof(SQLServerProvider<>);
                // Provider'in çalışacağı tip <T> 
                Type[] typeArgs = { ModelType };
                // Provider Generic Tipi
                Type makeme = d1.MakeGenericType(typeArgs);
                // Provider Nesnesi
                var o = Activator.CreateInstance(makeme);
                if (o != null)
                {
                    // Generic tipten elde edilen method
                    MethodInfo selectMethod = makeme.GetMethod("Select");
                    // Metod parametreleri
                    ParameterInfo[] paramInfo = selectMethod.GetParameters();
                    // Metod parametreleri atama
                    object[] methodParams = new object[paramInfo.Length];
                    methodParams[0] = "";
                    if (paramInfo.Length > 1)
                    {
                        methodParams[paramInfo.Length - 1] = "";
                    }
                    // Metod çağırma
                    object result = selectMethod.Invoke(o, methodParams);
                    return result as DAIList<T>;

                }
                else
                    return null;
            }
            else
                return null;

        }

        #endregion

        #endregion
    }
}
