using Car_Utility;
using CarAPI_Web.Models;
using CarAPI_Web.Models.Dto;
using CarAPI_Web.Services.IServices;

namespace CarAPI_Web.Services
{
    public class CarDetailsService :  ICarDetailsService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseService _baseService;
        private string carUrl;

        public CarDetailsService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
        {
            _clientFactory = clientFactory;
            _baseService = baseService;
            carUrl = configuration.GetValue<string>("ServiceUrls:CarAPI");

        }


        public async Task<T> CreateAsync<T>(CarDetailsCreateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarDetailsAPI",
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = carUrl + "/api/v1/CarDetailsAPI/" + id,
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarDetailsAPI"
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarDetailsAPI/" + id
            });
        }

        public async Task<T> UpdateAsync<T>(CarDetailsUpdateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto, 
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarDetailsAPI/" + dto.CarDetailsId
            });
        }
    }
}

