using CarsAPI.Data;
using CarsAPI.Models;
using CarsAPI.Repostiory.IRepostiory;

namespace CarsAPI.Repostiory
{
    public class CarDetailsRepository : Repository<CarDetails>, ICarDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public CarDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<CarDetails> UpdateAsync(CarDetails entity)
        {
            _db.CarDetails.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
