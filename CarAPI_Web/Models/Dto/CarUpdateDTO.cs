using System.ComponentModel.DataAnnotations;

namespace CarAPI_Web.Models.Dto
{
    public class CarUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        public int Year { get; set; }
        public string VIN { get; set; }
        public string EngineType { get; set; }
        public string TransmissionType { get; set; }
        public string Color { get; set; }
        public double Mileage { get; set; }
        [Required]
        public decimal Price { get; set; }
        public double FuelEfficiency { get; set; }

        public string BodyType { get; set; }
        public string Condition { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageLocalPath { get; set; }
    }

}
