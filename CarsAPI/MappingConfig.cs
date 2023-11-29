using AutoMapper;
using CarsAPI.Models;
using CarsAPI.Models.Dto;

namespace CarsAPI
{
    public class MappingConfig: Profile
    {
      public MappingConfig() {
            CreateMap<Car, CarDTO>();
            CreateMap<CarDTO, Car>();

            CreateMap<Car, CarCreateDTO>().ReverseMap();
            CreateMap<Car, CarUpdateDTO>().ReverseMap();


            CreateMap<CarDetails, CarDetailsDTO>().ReverseMap();
            CreateMap<CarDetails, CarDetailsCreateDTO>().ReverseMap();
            CreateMap<CarDetails, CarDetailsUpdateDTO>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
