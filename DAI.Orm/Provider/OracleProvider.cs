using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Provider
{
    public class OracleProvider<Table> : IDbProvider<Table>
    {
        public void CreateAlterTable(bool isCreate = true)
        {
            throw new NotImplementedException();
        }

        public void DropTable()
        {
            throw new NotImplementedException();
        }

        public void TruncateTable()
        {
            throw new NotImplementedException();
        }

        public List<Table> Select(string sqlExp = "*", string field = "", string whereClause = "")
        {
            throw new NotImplementedException();
        }

        public void Insert(Table entity)
        {
            throw new NotImplementedException();
        }

        public void MultipleInsert(List<Table> entityList)
        {
            throw new NotImplementedException();
        }

        public void Delete(Table entity)
        {
            throw new NotImplementedException();
        }

        public void MultipleDelete(List<Table> entityList)
        {
            throw new NotImplementedException();
        }

        public void Update(Table entity)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            throw new NotImplementedException();
        }
    }
}
