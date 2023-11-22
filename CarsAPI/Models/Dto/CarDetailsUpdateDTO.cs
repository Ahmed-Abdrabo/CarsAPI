using System.ComponentModel.DataAnnotations;

namespace CarsAPI.Models.Dto
{
    public class CarDetailsUpdateDTO
    {
        [Required]
        public int CarDetailsId { get; set; }
        [Required]
        public int CarId { get; set; }
        public string AdditionalFeature { get; set; }
        public string SpecialDetails { get; set; }
    }
}
