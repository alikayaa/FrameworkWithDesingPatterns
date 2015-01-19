using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Orm.Provider
{
    // Sağlayıcı ara yüzü.
    public interface IDbProvider<Table>
    {
        // Tablo oluştur veya değiştir.
        void CreateAlterTable(bool isCreate=true);
        // Tablo sil.
        void DropTable();
        // Tablo boşalt.
        void TruncateTable();
        // Tablodan veri çek.
        List<Table> Select(string sqlExp="*",string field="",string whereClause = "");
        // Tabloya kayıt ekle.
        void Insert(Table entity);
        // Tabloya toplu kayıt ekle.
        void MultipleInsert(List<Table> entityList);
        // Tablodan kayıt sil.
        void Delete(Table entity);
        // Tablodan toplu kayıt sil.
        void MultipleDelete(List<Table> entityList);
        // Tabloda kayıt güncelle.
        void Update(Table entity);
        // Değişiklikleri uygula.
        void SubmitChanges();
        
    }
}
