using CarsAPI.Data;
using CarsAPI.Models.Dto;
using CarsAPI.Models;
using CarsAPI.Repository.IRepostiory;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace CarsAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _roleManager = roleManager;
        }

        public bool IsUniqueUser(string userName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<TokenDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);


            if (user == null || isValid == false)
            {
                return new TokenDTO()
                {
                    AccessToken = ""
                };
            }

            var jwtTokenId = $"JTI{Guid.NewGuid()}";
            var accessToken = await GetAccessToken(user, jwtTokenId);
            var refreshToken = await CreateNewRefreshToken(user.Id,jwtTokenId);
            TokenDTO tokenDTO = new TokenDTO()
            {
                AccessToken = accessToken,
                RefreshToken= refreshToken
            };
            return tokenDTO;
        }

        public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.UserName,
                Email = registrationRequestDTO.UserName,
                NormalizedEmail = registrationRequestDTO.UserName.ToUpper(),
                Name = registrationRequestDTO.Name
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(registrationRequestDTO.Role).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(registrationRequestDTO.Role));
                    }
                    await _userManager.AddToRoleAsync(user, registrationRequestDTO.Role);
                    var userToReturn = _db.ApplicationUsers
                        .FirstOrDefault(u => u.UserName == registrationRequestDTO.UserName);
                    return _mapper.Map<UserDTO>(userToReturn);

                }
            }
            catch (Exception e)
            {

            }

            return new UserDTO();
        }

        private async Task<string> GetAccessToken(ApplicationUser user, string jwtTokenId)
        {
            //if user was found generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim(JwtRegisteredClaimNames.Jti, jwtTokenId),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenStr = tokenHandler.WriteToken(token);
            return tokenStr;
        }

        public async Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO)
        {
            var existingRefreshToken= await _db.RefreshTokens.FirstOrDefaultAsync(u=>u.Refresh_Token==tokenDTO.RefreshToken);
            if (existingRefreshToken==null)
            {
                return new TokenDTO();
            }
            var accessTokenData = GetAccessTokenData(tokenDTO.AccessToken);
            if (!accessTokenData.isSuccessful||accessTokenData.userId!=existingRefreshToken.UserId
                ||accessTokenData.tokenId!=existingRefreshToken.JwtTokenId)
            {
                existingRefreshToken.IsValid = false;
                _db.SaveChanges();
                return new TokenDTO();
            }
            
            if(!existingRefreshToken.IsValid)
            {
                var chainRecords =await _db.RefreshTokens.Where(u => u.UserId == existingRefreshToken.UserId
                && u.JwtTokenId == existingRefreshToken.JwtTokenId).ExecuteUpdateAsync(u => u.SetProperty(refreshToken => refreshToken.IsValid, false));
              
                return new TokenDTO();
            }

            if (existingRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                existingRefreshToken.IsValid = false;
                _db.SaveChanges();
                return new TokenDTO();
            }
            var newRefreshToken=await CreateNewRefreshToken(existingRefreshToken.UserId,existingRefreshToken.JwtTokenId);
            existingRefreshToken.IsValid = false;
            _db.SaveChanges();


            var appUser=_db.ApplicationUsers.FirstOrDefault(u=>u.Id==existingRefreshToken.UserId);
            if(appUser==null)
            {
                return new TokenDTO();
            }
            var newAccessToken=await GetAccessToken(appUser,existingRefreshToken.JwtTokenId);

            return new TokenDTO()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }

        private async Task<string> CreateNewRefreshToken(string userId,string tokenId)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                IsValid = true,
                UserId = userId,
                JwtTokenId = tokenId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(3),
                Refresh_Token=Guid.NewGuid()+"-"+Guid.NewGuid()
            };
            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();
            return refreshToken.Refresh_Token;
        }

        private (bool isSuccessful, string userId, string tokenId) GetAccessTokenData(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(accessToken);
                var jwtTokenId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti).Value;
                var userId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value;
                return (true, userId, jwtTokenId);
            }
            catch (Exception ex)
            {
                return (false, null, null);
            }
        }
    }
}