using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

namespace ConsoleUI
{
    //SOLID
    //Open Closed Principle
    class Program
    {
        static void Main(string[] args)
        {
            //Data Transformation Object
            ProductTest();
            //IoC 
            //CategoryTest();
        }

        private static void CategoryTest()
        {
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            foreach (var category in categoryManager.GetAll())
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductTest()
        {
            ProductManager productManager = new ProductManager(new EfProductDal());

            var result = productManager.GetProductDetails();

            if (result.Success)
            {
                Console.WriteLine(" ---------------------------------------------------------------------------------------------------------");
                Console.WriteLine("| " + "{0,-50}" + " | " + "{1,-50}" + " | ", "Ürün Adı", "Kategori Adı");
                Console.WriteLine(" ---------------------------------------------------------------------------------------------------------");
                foreach (var product in result.Data)
                {
                    Console.WriteLine("| " + "{0,-50}" + " | " + "{1,-50}" + " | ", product.ProductName, product.CategoryName);
                }
                Console.WriteLine(" ---------------------------------------------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine(result.Message);
            }

        }
    }
}
