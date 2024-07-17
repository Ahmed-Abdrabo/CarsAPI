using Car_Utility;
using CarAPI_Web.Models;
using CarAPI_Web.Models.Dto;
using CarAPI_Web.Services.IServices;

namespace CarAPI_Web.Services
{
    public class CarService: BaseService, ICarService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string carUrl;

        public CarService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            carUrl = configuration.GetValue<string>("ServiceUrls:CarAPI");

        }
        public Task<T> CreateAsync<T>(CarCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarAPI",
                Token = token,
                ContentType=SD.ContentType.MultipartFormData
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(CarUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto, 
                Url = carUrl + $"/api/{SD.CurrentApiVersion}/CarAPI/" + dto.Id,
                Token = token,
                ContentType = SD.ContentType.MultipartFormData
            });
        }
    }
}

