using CarsAPI.Models;
using CarsAPI.Models.Dto;
using CarsAPI.Repository.IRepostiory;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarsAPI.Controllers
{
    [Route("api/v{version:apiVersion}/UsersAuth")]
    [ApiController]
    [ApiVersionNeutral]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;
        protected APIResponse _response;
        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _response = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
         {
            TokenDTO tokenDTO = await _userRepo.Login(model);
            if (tokenDTO == null || string.IsNullOrEmpty(tokenDTO.AccessToken))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName or password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = tokenDTO;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            var user = await _userRepo.Register(model);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> GetNewTokenFromRefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (ModelState.IsValid)
            {
                var tokenDtoResponse=await _userRepo.RefreshAccessToken(tokenDTO);
                if (tokenDtoResponse == null || string.IsNullOrEmpty(tokenDtoResponse.AccessToken))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Invalid Token");
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = tokenDtoResponse;
                return Ok(_response);

            }
            else
            {
                _response.IsSuccess=false;
                _response.Result = "Invalid Input";
                return BadRequest(_response);
            }
            
        }
    }
}