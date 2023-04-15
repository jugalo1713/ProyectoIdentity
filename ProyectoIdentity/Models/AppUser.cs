using Microsoft.AspNetCore.Identity;

namespace ProyectoIdentity.Models
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int CountryCode { get; set; }
        public string Telephone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public bool State { get; set; }
    }
}
