using Car_Utility;
using CarAPI_Web.Models;
using CarAPI_Web.Models.Dto;
using CarAPI_Web.Services.IServices;

namespace CarAPI_Web.Services
{
    public class CarService: ICarService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseService _baseService;
        private string carUrl;

        public CarService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
        {
            _clientFactory = clientFactory;
            _baseService = baseService;
            carUrl = configuration.GetValue<string>("ServiceUrls:CarAPI");

        }
        public async Task<T> CreateAsync<T>(CarCreateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarAPI",
                ContentType=SD.ContentType.MultipartFormData
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarAPI/" + id,
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarAPI",
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarAPI/" + id
            });
        }

        public async Task<T> UpdateAsync<T>(CarUpdateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto, 
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarAPI/" + dto.Id,
                ContentType = SD.ContentType.MultipartFormData
            });
        }
    }
}

