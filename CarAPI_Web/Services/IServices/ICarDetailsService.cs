using CarAPI_Web.Models.Dto;

namespace CarAPI_Web.Services.IServices
{
    public interface ICarDetailsService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(CarDetailsCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(CarDetailsUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
