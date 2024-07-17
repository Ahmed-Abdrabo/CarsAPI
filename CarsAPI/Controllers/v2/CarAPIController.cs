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

namespace CarsAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
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

      //  [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCar([FromForm] CarCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                var car = _mapper.Map<Car>(createDTO);
               
                await _dbCar.CreateAsync(car);
                if (createDTO.Image != null)
                {
                    string fileName=car.Id+Path.GetExtension(createDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImage\" + fileName;

                    var directoryLocation=Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    FileInfo file = new FileInfo(directoryLocation);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    using(var fileStream=new FileStream(directoryLocation, FileMode.Create))
                    {
                        createDTO.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    car.ImageUrl = baseUrl+ "/ProductImage/"+fileName;
                    car.ImageLocalPath = filePath;
                }
                else
                {
                    car.ImageUrl = "https://placehold.co/600x400";
                }
                await _dbCar.UpdateAsync(car);
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
        public async Task<ActionResult<APIResponse>> UpdateCar(int id, [FromForm] CarUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.IsSuccess = false;
                    return BadRequest();
                }
                var model = _mapper.Map<Car>(updateDTO);

                if (updateDTO.Image != null)
                {
                    if (!string.IsNullOrEmpty(model.ImageLocalPath))
                    {
                        var oldFilePathDirectory=Path.Combine(Directory.GetCurrentDirectory(),model.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    string fileName = updateDTO.Id + Path.GetExtension(updateDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImage\" + fileName;

                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                   
                    using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                    {
                        updateDTO.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    model.ImageUrl = baseUrl + "/ProductImage/" + fileName;
                    model.ImageLocalPath = filePath;
                }
               
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
                if (!string.IsNullOrEmpty(car.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), car.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
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
