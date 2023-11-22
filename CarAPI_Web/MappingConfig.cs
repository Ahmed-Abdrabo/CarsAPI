using AutoMapper;
using CarAPI_Web.Models;
using CarAPI_Web.Models.Dto;

namespace CarAPI_Web
{
    public class MappingConfig: Profile
    {
      public MappingConfig() {
        

            CreateMap<CarDTO, CarCreateDTO>().ReverseMap();
            CreateMap<CarDTO, CarUpdateDTO>().ReverseMap();


            CreateMap<CarDetailsDTO, CarDetailsCreateDTO>().ReverseMap();
            CreateMap<CarDetailsDTO, CarDetailsUpdateDTO>().ReverseMap();
        }
    }
}
