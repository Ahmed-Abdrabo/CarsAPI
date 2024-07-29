using CarAPI_Web.Models;

namespace CarAPI_Web.Services.IServices
{
    public interface IApiMessageRequestBuilder
    {
        HttpRequestMessage Build (APIRequest apiRequest);
    }
}
