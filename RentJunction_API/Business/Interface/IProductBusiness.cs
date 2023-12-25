using Microsoft.AspNetCore.Mvc;
using RentJunction_API.Models;
using RentJunction_API.Models.ViewModels;
using System.Linq;

namespace RentJunction_API.Business.Interface
{
    public interface IProductBusiness
    {
        bool AddProduct(AddProductDTO product,string username);
        IQueryable<Product> GetProducts(string city, int? categoryId);
        IQueryable<Product> ViewListedProducts(string username);
        bool DeleteProduct(string username,int productId);
        public Product ViewProductDetail(int productId);
        bool UpdateProduct(int id, AddProductDTO product, string username);
        bool RentProduct(int id, RentProductDTO model, string username);
        bool ExtendRentPeriod(int id, [FromBody] ExtendRentDTO model, string username);
    }
}