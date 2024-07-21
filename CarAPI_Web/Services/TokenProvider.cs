using Car_Utility;
using CarAPI_Web.Models.Dto;
using CarAPI_Web.Services.IServices;

namespace CarAPI_Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.AccessToken);
        }

        public TokenDTO GetToken()
        {
            try
            {
                bool hasAccessToken=_contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string accessToken);
                TokenDTO tokenDto = new()
                {
                    AccessToken = accessToken
                };
                return hasAccessToken ? tokenDto : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void SetToken(TokenDTO tokenDTO)
        {
            var cookieOptions=new CookieOptions { Expires=DateTime.UtcNow.AddDays(60) };
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.AccessToken,tokenDTO.AccessToken,cookieOptions);
        }
    }
}
