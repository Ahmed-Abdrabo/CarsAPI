using CarsAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarsAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarDetails> CarDetails { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");
                // You can adjust the precision and scale as needed
            });
            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    Id = 1,
                    Make = "Toyota",
                    Model = "Camry",
                    Year = 2020,
                    VIN = "1HGCM82633A004352",
                    EngineType = "Gasoline",
                    TransmissionType = "Automatic",
                    Color = "Silver",
                    Mileage = 25000,
                    Price = 20000,
                    FuelEfficiency = 28.5,
                    BodyType = "Sedan",
                    Condition = "Used",
                    ImageUrl = "https://example.com/toyota_camry_image.jpg"
                },
                new Car
                {
                    Id = 2,
                    Make = "Honda",
                    Model = "CR-V",
                    Year = 2018,
                    VIN = "5J6RW1H58HL000568",
                    EngineType = "Gasoline",
                    TransmissionType = "CVT",
                    Color = "Blue",
                    Mileage = 30000,
                    Price = 23000,
                    FuelEfficiency = 30.2,
                    BodyType = "SUV",
                    Condition = "Used",
                    ImageUrl = "https://example.com/honda_crv_image.jpg"
                },
                new Car
                {
                    Id = 3,
                    Make = "Ford",
                    Model = "Mustang",
                    Year = 2022,
                    VIN = "1FA6P8TH6M5107921",
                    EngineType = "Gasoline",
                    TransmissionType = "Manual",
                    Color = "Red",
                    Mileage = 10000,
                    Price = 35000,
                    FuelEfficiency = 25.0,
                    BodyType = "Coupe",
                    Condition = "New",
                    ImageUrl = "https://example.com/ford_mustang_image.jpg"
                },
                new Car
                {
                    Id = 4,
                    Make = "Chevrolet",
                    Model = "Tahoe",
                    Year = 2021,
                    VIN = "1GNSKCKC3MR379084",
                    EngineType = "Gasoline",
                    TransmissionType = "Automatic",
                    Color = "Black",
                    Mileage = 15000,
                    Price = 45000,
                    FuelEfficiency = 18.6,
                    BodyType = "SUV",
                    Condition = "Used",
                    ImageUrl = "https://example.com/chevrolet_tahoe_image.jpg"
                },
                new Car
                {
                    Id = 5,
                    Make = "BMW",
                    Model = "X5",
                    Year = 2019,
                    VIN = "5UXCR6C56KLL31682",
                    EngineType = "Gasoline",
                    TransmissionType = "Automatic",
                    Color = "White",
                    Mileage = 20000,
                    Price = 50000,
                    FuelEfficiency = 24.5,
                    BodyType = "SUV",
                    Condition = "Used",
                    ImageUrl = "https://example.com/bmw_x5_image.jpg"
                }
            );
        }
    }
}
