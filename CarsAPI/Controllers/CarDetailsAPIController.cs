using AutoMapper;
using CarsAPI.Models;
using CarsAPI.Models.Dto;
using CarsAPI.Repostiory;
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

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetCarDetails()
        {
            try
            {
                IEnumerable<CarDetails> carDetails = await _dbCarDetails.GetAllAsync(includeProperties: "Car");
                _response.Result = _mapper.Map<List<CarDetailsDTO>>(carDetails);
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


        [HttpGet("{id:int}", Name = "GetCarDetails")]
        public async Task<ActionResult<APIResponse>> GetCarDetail(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var carDetails = await _dbCarDetails.GetAsync(c => c.CarDetailsId == id);
                if (carDetails == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CarDetailsDTO>(carDetails);
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
         public async Task<ActionResult<APIResponse>> CreateCarDetails([FromBody] CarDetailsCreateDTO createDTO)
         {
            try
            {
                if (await _dbCarDetails.GetAsync(u => u.CarDetailsId == createDTO.CarDetailsId) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Car Details already Exists!");
                    return BadRequest(ModelState);
                }
                if (await _dbCar.GetAsync(u => u.Id == createDTO.CarId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Car ID is Invalid!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest();
                }

                var carDetails = _mapper.Map<CarDetails>(createDTO);
                await _dbCarDetails.CreateAsync(carDetails);
                _response.Result = _mapper.Map<CarDetailsDTO>(carDetails);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetCarDetails", new { id = carDetails.CarDetailsId }, _response);
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
        public async Task<ActionResult<APIResponse>> UpdateCarDetails(int id, [FromBody] CarDetailsUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.CarDetailsId)
                {
                    return BadRequest();
                }
                if (await _dbCar.GetAsync(u => u.Id == updateDTO.CarId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Car ID is Invalid!");
                    return BadRequest(ModelState);
                }
                var model = _mapper.Map<CarDetails>(updateDTO);
                await _dbCarDetails.UpdateAsync(model);

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
        public async Task<ActionResult<APIResponse>> DeleteCarDetails(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var carDetails = await _dbCarDetails.GetAsync(c => c.CarDetailsId == id);
                if (carDetails == null)
                {
                    return NotFound();
                }
                await _dbCarDetails.RemoveAsync(carDetails);
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
