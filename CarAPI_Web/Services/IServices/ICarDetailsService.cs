using CarAPI_Web.Models.Dto;

namespace CarAPI_Web.Services.IServices
{
    public interface ICarDetailsService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(CarDetailsCreateDTO dto);
        Task<T> UpdateAsync<T>(CarDetailsUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
