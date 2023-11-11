using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTO
{
    public class LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(14, ErrorMessage = "Your Password Is Limited To {2} To {1} Characters", MinimumLength = 4)]
        public string Password { get; set; }
    }
    public class UserDTO : LoginDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string phoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(14, ErrorMessage = "Your Password Is Limited To {2} To {1} Characters", MinimumLength = 4)]
        public string Password { get; set; }
        public ICollection<string> Roles { get; set; }

    }
}
