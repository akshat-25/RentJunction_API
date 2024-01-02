using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RentJunction_API.Business.Interface;
using RentJunction_API.Data;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using RentJunction_API.Models.Enums;
using RentJunction_API.Models.ViewModels;
using System;
using System.Linq;


namespace RentJunction_API.Business
{

    public class ProductBusiness : IProductBusiness
    {
        private readonly IProductsData productsData;
        private readonly IUserData userData;
        private readonly IRentalData rentalData;

        public ProductBusiness(IProductsData productsData, IUserData userData,IRentalData rentalData)
        {
            this.productsData = productsData;
            this.userData = userData;
            this.rentalData = rentalData;
        }
   
        public IQueryable<Product> GetProducts(string city, int? categoryId)
        {
            IQueryable<Product> products = productsData.GetProducts();

            if (!string.IsNullOrEmpty(city))
            {
                products = products.Where(p => p.City == city);
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == (ProductCategoryEnum)categoryId);
            }

            if (string.IsNullOrEmpty(city) && !categoryId.HasValue)
            {
                products = productsData.GetProducts();
            }

            if(products.Count() == 0)
            {
                throw new Exception("No Products Available..");
            }

            return products;

        }
        public void AddProduct(AddProductDTO product,string username)
        {   
                var userId = userData.GetUsers().FirstOrDefault(user => user.UserName.Equals(username)).Id;
            
                Product newProduct = new Product()
                {
                    Name = product.Name,
                    City = product.City,
                    Description = product.Description,
                    Rent = product.Rent,
                    CategoryId = product.CategoryId,
                    UserId = userId
                };

                productsData.AddProduct(newProduct);
                    
        }

        public IQueryable<Product> ViewListedProducts(string username)
        {
            var userId = userData.GetUsers().FirstOrDefault(user => user.UserName.Equals(username)).Id;

            var products = productsData.GetProducts().Where(p => p.UserId.Equals(userId));

            if(products.Count() == 0)
            {
                throw new Exception("No Products Available..");
            }
            return products;
        }
      
        public void DeleteProduct(string username,int productId)
        {
            var userId = userData.GetUsers().FirstOrDefault(user => user.UserName.Equals(username)).Id;

            var products = productsData.GetProducts().Where(p => p.UserId.Equals(userId));
            
            var deleteProduct = products.FirstOrDefault(p => p.Id.Equals(productId));
            if ( deleteProduct == null)
            {
              throw new Exception("No Products available !!");    
            }

            else
            {
                productsData.DeleteProduct(deleteProduct);
            }
            
        }

        public Product ViewProductDetail(int productId)
        {
           var product = productsData.GetProducts().FirstOrDefault(p => p.Id == productId);

           if(rentalData.GetRentalData().FirstOrDefault(rental => rental.ProductId  == productId) != null)
           {
                return null;
           }
            return product;
        }

        public void UpdateProduct(int id, AddProductDTO product, string username)
        {
            var userId = userData.GetUsers().FirstOrDefault(user => user.UserName.Equals(username)).Id;

            var products = productsData.GetProducts().Where(product => product.UserId.Equals(userId));

            var updateProduct = products.FirstOrDefault(product => product.Id == id);

            if (updateProduct == null)
                throw new Exception("No Product Found");


            updateProduct.Name = product.Name;
            updateProduct.City = product.City;
            updateProduct.Description = product.Description;
            updateProduct.Rent = product.Rent;
            productsData.SaveChanges();
            

        }

        public void RentProduct(int id, RentProductDTO model, string username)
        {
            var userId = userData.GetUsers().FirstOrDefault(user => user.UserName.Equals(username)).Id;

            var product = productsData.GetProducts().FirstOrDefault(product => product.Id == id);

            DateTime startDate;
            DateTime endDate;

            var isValidstartDate = DateTime.TryParse(model.StartDate, out startDate);
            var isValidEndDate = DateTime.TryParse(model.EndDate, out endDate);

            if (!isValidstartDate && ! isValidEndDate && !(startDate < DateTime.Today) && (startDate > endDate))
            {
                throw new Exception("Start/End date is invalid..");
            }

            if (startDate.Equals(endDate))
            {
                throw new Exception("Start and end date cannot be same..");
            }

            var rental = new Rental()
            {
                StartDate = startDate.ToString(),
                EndDate = endDate.ToString(),
                Price = (endDate.Day - startDate.Day) * (product.Rent),
                UserId = userId,
                ProductId = product.Id
            };

            rentalData.AddRental(rental);
           

        }

        public void ExtendRentPeriod(int id, [FromBody] ExtendRentDTO model,string username)
        {
            var userId = userData.GetUsers().FirstOrDefault(user => user.UserName.Equals(username)).Id;

            var rentalProduct = rentalData.GetRentalData().FirstOrDefault(product => product.ProductId == id);

            var product = productsData.GetProducts().FirstOrDefault(product => product.Id == id);

            if(rentalProduct == null) { throw new Exception("No Product Found.."); }

            DateTime prevEndDate;
            DateTime newEndDate;

            var isValidPrevEndDate = DateTime.TryParse(rentalProduct.EndDate, out prevEndDate);
            var isValidNewEndDate = DateTime.TryParse(model.NewEndDate, out newEndDate);

            if(!isValidPrevEndDate && !isValidNewEndDate || (prevEndDate > newEndDate) )
            {
                throw new Exception("Invalid Start/End Date");
            }

            rentalProduct.EndDate = newEndDate.ToString();
            rentalProduct.Price = ((product.Rent) * newEndDate.Subtract(prevEndDate).Days) + rentalProduct.Price;

            productsData.SaveChanges();

          

        }
    }
}