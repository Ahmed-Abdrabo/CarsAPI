    using CarAPI_Web.Models.Dto;

namespace CarAPI_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate);  
        Task<T> LogOutAsync<T>(TokenDTO obj);  
    }
}
