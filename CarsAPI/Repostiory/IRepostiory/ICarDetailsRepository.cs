using CarsAPI.Models;

namespace CarsAPI.Repostiory.IRepostiory
{
    public interface ICarDetailsRepository : IRepository<CarDetails>
    {
        Task<CarDetails> UpdateAsync(CarDetails entity);
    }
}
