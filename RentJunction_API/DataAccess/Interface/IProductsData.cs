using RentJunction_API.Models;
using System.Linq;

namespace RentJunction_API.DataAccess.Interface
{
    public interface IProductsData
    {
        void AddProduct(Product product);
        void DeleteProduct(Product product);
        IQueryable<Product> GetProducts();
        public void SaveChanges();
    }
}