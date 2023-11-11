using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Model
{
    public class ApiUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
    }
}
