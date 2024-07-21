using CarsAPI.Models;

namespace CarsAPI.Repository.IRepostiory
{
    public interface ICarDetailsRepository : IRepository<CarDetails>
    {
        Task<CarDetails> UpdateAsync(CarDetails entity);
    }
}
