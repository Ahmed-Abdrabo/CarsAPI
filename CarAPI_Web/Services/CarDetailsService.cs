using Car_Utility;
using CarAPI_Web.Models;
using CarAPI_Web.Models.Dto;
using CarAPI_Web.Services.IServices;

namespace CarAPI_Web.Services
{
    public class CarDetailsService : BaseService, ICarDetailsService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string carUrl;

        public CarDetailsService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            carUrl = configuration.GetValue<string>("ServiceUrls:CarAPI");

        }
        public Task<T> CreateAsync<T>(CarDetailsCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = carUrl + "/api/CarDetailsAPI"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = carUrl + "/api/CarDetailsAPI/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = carUrl + "/api/CarDetailsAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = carUrl + "/api/CarDetailsAPI/" + id
            });
        }

        public Task<T> UpdateAsync<T>(CarDetailsUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto, 
                Url = carUrl + "/api/CarDetailsAPI/" + dto.CarDetailsId
            });
        }
    }
}

