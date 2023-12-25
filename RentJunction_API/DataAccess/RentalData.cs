using RentJunction_API.Data;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using System.Linq;

namespace RentJunction_API.DataAccess
{
    public class RentalData : IRentalData
    {
        private readonly AppDbContext appDbContext;
        public RentalData(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public IQueryable<Rental> GetRentalData()
        {
            return appDbContext.Rentals;
        }

        public void AddRental(Rental rental)
        {
            appDbContext.Rentals.Add(rental);
            appDbContext.SaveChanges();
        }
    }
}
