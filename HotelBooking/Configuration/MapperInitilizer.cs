using AutoMapper;
using HotelBooking.DTO;
using HotelBooking.Model;

namespace HotelBooking.Configuration
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            CreateMap<ApiUser, UserDTO>().ReverseMap();
            CreateMap<ApiUser, LoginDTO>().ReverseMap();

            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Country, CreateCountryDTO>().ReverseMap();

            CreateMap<Hotel, HotelDTO>().ReverseMap();
            CreateMap<Hotel, CreateHotelDTO>().ReverseMap();

            CreateMap<Room, RoomDTO>().ReverseMap();
            CreateMap<Room, CreateRoomDTO>().ReverseMap();
        }
    }
}
