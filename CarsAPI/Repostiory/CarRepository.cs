using CarsAPI.Data;
using CarsAPI.Models;
using CarsAPI.Repostiory.IRepostiory;

namespace CarsAPI.Repostiory
{
    public class CarRepository: Repository<Car>, ICarRepository
    {
        private readonly ApplicationDbContext _db;
        public CarRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Car> UpdateAsync(Car entity)
        {
            _db.Cars.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
