using Microsoft.AspNetCore.Identity;

namespace CarsAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
