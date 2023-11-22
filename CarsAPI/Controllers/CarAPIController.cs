using AutoMapper;
using CarsAPI.Models;
using CarsAPI.Models.Dto;
using CarsAPI.Repostiory.IRepostiory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ICarRepository _dbCar;
        private readonly IMapper _mapper;
        public CarAPIController(ICarRepository dbCar, IMapper mapper)
        {
            _dbCar = dbCar;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetCars()
        {
            try
            {
                IEnumerable<Car> cars = await _dbCar.GetAllAsync();
                _response.Result = _mapper.Map<List<CarDTO>>(cars);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("{id:int}", Name = "GetCar")]
        public async Task<ActionResult<APIResponse>> GetCar(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var car = await _dbCar.GetAsync(c => c.Id == id);
                if (car == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CarDTO>(car);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost]
         public async Task<ActionResult<APIResponse>> CreateCar([FromBody] CarCreateDTO createDTO)
         {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest();
                }

                var car = _mapper.Map<Car>(createDTO);
                //Car model = new()
                //{
                //Make = createDTO.Make,
                //Model = createDTO.Model,
                //Year = createDTO.Year,
                //VIN = createDTO.VIN,
                //EngineType = createDTO.EngineType,
                //TransmissionType = createDTO.TransmissionType,
                //Color = createDTO.Color,
                //Mileage = createDTO.Mileage,
                //Price = createDTO.Price,
                //FuelEfficiency = createDTO.FuelEfficiency,
                //BodyType = createDTO.BodyType,
                //Condition = createDTO.Condition,
                //ImageUrl = createDTO.ImageUrl
                // };
                await _dbCar.CreateAsync(car);
                _response.Result = _mapper.Map<CarDTO>(car);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetCar", new { id = car.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> UpdateCar(int id, [FromBody] CarUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                var model = _mapper.Map<Car>(updateDTO);

                //Car model = new()
                //{
                //    Make = updateDTO.Make,
                //    Model = updateDTO.Model,
                //    Year = updateDTO.Year,
                //    VIN = updateDTO.VIN,
                //    EngineType = updateDTO.EngineType,
                //    TransmissionType = updateDTO.TransmissionType,
                //    Color = updateDTO.Color,
                //    Mileage = updateDTO.Mileage,
                //    Price = updateDTO.Price,
                //    FuelEfficiency = updateDTO.FuelEfficiency,
                //    BodyType = updateDTO.BodyType,
                //    Condition = updateDTO.Condition,
                //    ImageUrl = updateDTO.ImageUrl
                //};
                await _dbCar.UpdateAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> DeleteCar(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var car = await _dbCar.GetAsync(c => c.Id == id);
                if (car == null)
                {
                    return NotFound();
                }
                await _dbCar.RemoveAsync(car);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
