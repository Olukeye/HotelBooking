using AutoMapper;
using HotelBookings.DTO;
using HotelBookings.Model;

namespace HotelBookings.Configuration
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Country, CreateCountryDTO>().ReverseMap();
            CreateMap<Hotel, HotelDTO>().ReverseMap();
            CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
            CreateMap<Room, CreateRoomDTO>().ReverseMap();

        }
    }
}
