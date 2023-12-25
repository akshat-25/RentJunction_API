using RentJunction_API.Data;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using System.Linq;

namespace RentJunction_API.DataAccess
{
    public class ProductsData : IProductsData
    {
        private readonly AppDbContext appDbContext;

        public ProductsData(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public IQueryable<Product> GetProducts()
        {
            return appDbContext.Products;
        }
        public void AddProduct(Product product)
        {
            appDbContext.Products.Add(product);
            appDbContext.SaveChanges();
        }
        public void DeleteProduct(Product product)
        {
            appDbContext.Products.Remove(product);
            appDbContext.SaveChanges();
        }
        public void SaveChanges()
        {
            appDbContext.SaveChanges();
        }

    }
}
