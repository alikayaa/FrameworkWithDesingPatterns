using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAI.Orm.Provider;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DAI.Orm.Context;
using DAI.Orm.T4Template;
using System.Configuration;
namespace DAI.Orm.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Insert()
        {
            Product a = new Product() { ProductName = "mac", ProductCode = "pro", CategoryId = 7 };
            SQLServerProvider<Product> provider = new SQLServerProvider<Product>();
            provider.Insert(a);
            provider.SubmitChanges();

        }

        [TestMethod]
        public void MultipleInsert()
        {
            List<Product> products = new List<Product>();
            products.Add(new Product() { ProductName = "yosemite", ProductCode = "pro", CategoryId = 8 });
            products.Add(new Product() { ProductName = "mavericks", ProductCode = "pro", CategoryId = 9 });
            SQLServerProvider<Product> provider = new SQLServerProvider<Product>();
            provider.MultipleInsert(products);
            provider.SubmitChanges();

        }

        [TestMethod]
        public void Delete()
        {
            Category cat = new Category() { Id = 11 };
            cat.CategoryName = "dai";
            SQLServerProvider<Category> provider = new SQLServerProvider<Category>();
            provider.Delete(cat);
            provider.SubmitChanges();
        }

        [TestMethod]
        public void MultipleDelete()
        {
            List<Category> cats = new List<Category>();
            cats.Add(new Category() { Id = 11 });
            cats.Add(new Category() { Id = 12 });
            SQLServerProvider<Category> provider = new SQLServerProvider<Category>();
            provider.MultipleDelete(cats);
            provider.SubmitChanges();
        }

        [TestMethod]
        public void Truncate()
        {
            SQLServerProvider<Product> provider = new SQLServerProvider<Product>();
            provider.TruncateTable();
            provider.SubmitChanges();
        }

        [TestMethod]
        public void Drop()
        {
            SQLServerProvider<ProductNew> provider = new SQLServerProvider<ProductNew>();
            provider.DropTable();
            provider.SubmitChanges();
        }

        [TestMethod]
        public void Update()
        {
            OrmEngine.Instance().InitializeDatabase();
            AutoContext db = new AutoContext();
            DAIList<Category> categories = db.Category.ToList();
            Category category = categories[0];
            category.CategoryName = "pc3";

            db.Category.Update(category);
            //provider.Update(new Product() { Id = 2, ProductCode = "Ali", ProductName= "Kaya" });
            db.SaveChanges();
        }

        [TestMethod]
        public void GetModelClass()
        {
            // Test başarılı oldu. Method Private çekildi.
            //List<Type> types = OrmEngine.Instance().GetModelClass();
        }

        [TestMethod]
        public void CodeGeneration()
        {
            List<Category> dai = new List<Category>();
            //Category a = new Category();
            //a.CategoryCode = "dai";
            //DbContext context = new DbContext();

            //OrmEngine.Instance().InitializeDatabase();
            //Product b = new Product();
            //b.ProductCode = "a";
            //context.Product.Add(b);
            //DAIList<Product> cat = context.Product.Where(p => p.ProductCode == "a");

            AutoContext db = new AutoContext();
            DAIList<Category> cat = db.Category.Where(p => p.CategoryName == "mac");

            DAIList<Product> Products = db.Product.ToList().OrderByDescending(p => p.Id);
            Product a = Products[0];
            var b = a.Categories;

        }

        [TestMethod]
        public void SaveChanges()
        {
            AutoContext db = new AutoContext();
            db.Category.AddItem(new Category() { CategoryCode = "cd", CategoryName = "dvd" });
            db.SaveChanges();
        }

        [TestMethod]
        public void CreateTable()
        {
            SQLServerProvider<ProductNew> provider = new SQLServerProvider<ProductNew>();
            provider.CreateAlterTable();
        }

        [TestMethod]
        public void AlterTable()
        {
            SQLServerProvider<ProductNew> provider = new SQLServerProvider<ProductNew>();
            provider.CreateAlterTable(false);
        }

        [TestMethod]
        public void First()
        {
            AutoContext db = new AutoContext();

            Category dai = db.Category.First();
        }
        
        [TestMethod]
        public void Max()
        {
            AutoContext db = new AutoContext();
            decimal maxId = db.Category.Max(p => p.Id);
        }

        [TestMethod]
        public void Min()
        {
            AutoContext db = new AutoContext();
            decimal minId = db.Product.Min(p => p.Id);
        }

        [TestMethod]
        public void OrderBy()
        {
            AutoContext db = new AutoContext();
            DAIList<Product> prs = db.Product.OrderBy(i => i.ProductName);
        }

        [TestMethod]
        public void OrderByDesc()
        {
            AutoContext db = new AutoContext();
            DAIList<Product> prs = db.Product.OrderByDescending(i => i.ProductName);
        }

        [TestMethod]
        public void Avg()
        {
            AutoContext db = new AutoContext();
            decimal avg = db.Product.Avg(i => i.Id);
        }

        [TestMethod]
        public void GroupBy()
        {
            AutoContext db = new AutoContext();
            DAIList<Category> cats = db.Category.GroupBy(i => i.CategoryName);
        }

        [TestMethod]
        public void Sum()
        {
            AutoContext db = new AutoContext();
            decimal sumID = db.Product.Sum(c => c.Id);
        }

        [TestMethod]
        public void Last()
        {
            AutoContext db = new AutoContext();
            //Product p = db.Product.Last();
        }

        [TestMethod]
        public void LastFieldValue()
        {
            AutoContext db = new AutoContext();
            //string lastCatName = db.Category.LastFieldValue("CategoryName").ToString();
        }

        [TestMethod]
        public void Take()
        {
            DAIList<Product> product = new AutoContext().Product.Take(1);
        }

        [TestMethod]
        public void Skip()
        {
            DAIList<Product> product = new AutoContext().Product.ToList().Skip(1);

        }

        [TestMethod]
        public void SkipTake()
        {
            DAIList<Category> cats = new AutoContext().Category.ToList().Skip(1).Take(2);
        }

        [TestMethod]
        public void RelationMap()
        {
            AutoContext db = new AutoContext();
            DAIList<Product> products = db.Product.ToList();
            DAIList<Category> cat = products[0].Categories;
        }

        [TestMethod]
        public void InitDb()
        {
            OrmEngine.Instance().InitializeDatabase();
        }

        [TestMethod]
        public void WhereClause()
        {
            AutoContext db = new AutoContext();
            DAIList<Product> products = new DAIList<Product>();
            products = db.Product.Where(i => i.Id == 1 && i.ProductName == "Mac" || i.CategoryId != 3).ToList();
        }

        [TestMethod]
        public void InitOrmTable()
        {
            //OrmEngine.Instance().InitOrmTable();
        }

        [TestMethod]
        public void ContextExtensionTest()
        {
            ConfigurationManager.ConnectionStrings["DEFAULT"].ToString();
            AutoContext db = new AutoContext();
            DAIList<Category> cats = db.Category.Where(i=> i.Id == 8).ToList();
            DbContext a = new DbContext();
            a.Adi = "dai";
            a.TransformText();
        }
    }
}
