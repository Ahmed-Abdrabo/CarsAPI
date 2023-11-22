using CarsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsAPI.Models
{
    public class CarDetails
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CarDetailsId { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }

        public Car Car { get; set; }

        public string AdditionalFeature { get; set; }
        public string SpecialDetails { get; set; }
    }
}


//modelBuilder.Entity<CarDetails>().HasData(
//    new CarDetails
//    {
//        CarDetailsId = 1,
//        CarId = 1,
//        AdditionalFeature = "Sunroof",
//        SpecialDetails = "Well-maintained, single owner",
//        CreatedDate = DateTime.Now,
//        UpdatedDate = DateTime.Now
//    },
//    new CarDetails
//    {
//        CarDetailsId = 2,
//        CarId = 2,
//        AdditionalFeature = "Leather seats",
//        SpecialDetails = "Low mileage, great condition",
//        CreatedDate = DateTime.Now,
//        UpdatedDate = DateTime.Now
//    },
//    new CarDetails
//    {
//        CarDetailsId = 3,
//        CarId = 3,
//        AdditionalFeature = "Performance exhaust",
//        SpecialDetails = "Brand new, showroom condition",
//        CreatedDate = DateTime.Now,
//        UpdatedDate = DateTime.Now
//    },
//    new CarDetails
//    {
//        CarDetailsId = 4,
//        CarId = 4,
//        AdditionalFeature = "Towing package",
//        SpecialDetails = "Minor scratches, fully loaded",
//        CreatedDate = DateTime.Now,
//        UpdatedDate = DateTime.Now
//    },
//    new CarDetails
//    {
//        CarDetailsId = 5,
//        CarId = 5,
//        AdditionalFeature = "Navigation system",
//        SpecialDetails = "Excellent interior, high-tech features",
//        CreatedDate = DateTime.Now,
//        UpdatedDate = DateTime.Now
//    }
//);