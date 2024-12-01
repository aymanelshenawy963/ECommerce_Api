using Microsoft.AspNetCore.Identity;

namespace E_Commerce507Api.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Adderss { get; set; }
    }
}
