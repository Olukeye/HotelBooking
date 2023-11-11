using HotelBooking.DTO;

namespace HotelBooking.AuthServices
{
    public interface IAuth
    {
        Task<bool> ValidateUser(LoginDTO loginDTO);

        Task<string> CreateToken();
    }
}
