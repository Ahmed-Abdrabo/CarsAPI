using Car_Utility;
using CarAPI_Web.Models;
using CarAPI_Web.Models.Dto;
using CarAPI_Web.Services.IServices;

namespace CarAPI_Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseService _baseService;
        private string carUrl;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
        {
            _clientFactory = clientFactory;
            carUrl = configuration.GetValue<string>("ServiceUrls:CarAPI");
            _baseService = baseService;
        }

        public async Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/UsersAuth/login"
            },withBearer:false);
        }

        public async Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/UsersAuth/register"
            }, withBearer: false);
        }
        public async Task<T> LogOutAsync<T>(TokenDTO obj)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/UsersAuth/revoke"
            });
        }
    }
}