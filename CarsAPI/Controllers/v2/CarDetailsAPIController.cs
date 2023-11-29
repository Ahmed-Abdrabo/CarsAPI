using AutoMapper;
using CarsAPI.Models;
using CarsAPI.Models.Dto;
using CarsAPI.Repostiory;
using CarsAPI.Repostiory.IRepostiory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CarsAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CarDetailsAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ICarDetailsRepository _dbCarDetails;
        private readonly ICarRepository _dbCar;
        private readonly IMapper _mapper;
        public CarDetailsAPIController(ICarDetailsRepository dbCarDetails, ICarRepository dbCar, IMapper mapper)
        {
            _dbCarDetails = dbCarDetails;
            _dbCar = dbCar;
            _mapper = mapper;
            this._response = new();
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
