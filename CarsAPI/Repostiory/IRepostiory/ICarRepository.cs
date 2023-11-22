using CarsAPI.Models;

namespace CarsAPI.Repostiory.IRepostiory
{
    public interface ICarRepository: IRepository<Car>
    {
        Task<Car> UpdateAsync(Car entity);
    }
}
