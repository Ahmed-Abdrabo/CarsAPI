using AutoMapper;
using CarsAPI.Models;
using CarsAPI.Models.Dto;
using CarsAPI.Repostiory.IRepostiory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarsAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CarAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ICarRepository _dbCar;
        private readonly IMapper _mapper;
        public CarAPIController(ICarRepository dbCar, IMapper mapper)
        {
            _dbCar = dbCar;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCars([FromQuery(Name = "filterYear")] int? year, [FromQuery] string? search
            , int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Car> cars;

                if (year > 0)
                {
                    cars = await _dbCar.GetAllAsync(u => u.Year == year, pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
                    cars = await _dbCar.GetAllAsync(pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    cars = cars.Where(u => u.Make.ToLower().Contains(search));
                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCar(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var car = await _dbCar.GetAsync(c => c.Id == id);
                if (car == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCar([FromBody] CarCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
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


        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateCar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCar(int id, [FromBody] CarUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.IsSuccess = false;
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

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteCar")]
        [Authorize(Roles = "admin")]
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
                    _response.IsSuccess = false;
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
