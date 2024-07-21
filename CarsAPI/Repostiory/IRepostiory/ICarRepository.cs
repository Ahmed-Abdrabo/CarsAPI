using CarsAPI.Models;

namespace CarsAPI.Repository.IRepostiory
{
    public interface ICarRepository: IRepository<Car>
    {
        Task<Car> UpdateAsync(Car entity);
    }
}
