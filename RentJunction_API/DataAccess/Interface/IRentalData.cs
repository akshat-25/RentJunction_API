using RentJunction_API.Models;
using System.Linq;

namespace RentJunction_API.DataAccess.Interface
{
    public interface IRentalData
    {
        void AddRental(Rental rental);
        IQueryable<Rental> GetRentalData();
    }
}