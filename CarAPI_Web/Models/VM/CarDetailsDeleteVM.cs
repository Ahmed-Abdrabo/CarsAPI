using CarAPI_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarAPI_Web.Models.VM
{
    public class CarDetailsDeleteVM
    {
        public CarDetailsDeleteVM()
        {
            CarDetails = new CarDetailsDTO();
        }
        public CarDetailsDTO CarDetails { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }
    }
}
