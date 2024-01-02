using Microsoft.AspNetCore.Mvc;
using RentJunction_API.Models;
using RentJunction_API.Models.ViewModels;
using System.Linq;

namespace RentJunction_API.Business.Interface
{
    public interface IProductBusiness
    {
        void AddProduct(AddProductDTO product,string username);
        IQueryable<Product> GetProducts(string city, int? categoryId);
        IQueryable<Product> ViewListedProducts(string username);
        void DeleteProduct(string username,int productId);
        public Product ViewProductDetail(int productId);
        void UpdateProduct(int id, AddProductDTO product, string username);
        void RentProduct(int id, RentProductDTO model, string username);
        void ExtendRentPeriod(int id, [FromBody] ExtendRentDTO model, string username);
    }
}