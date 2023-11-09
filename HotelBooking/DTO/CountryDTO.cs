using HotelBookings.Model;
using System.ComponentModel.DataAnnotations;

namespace HotelBookings.DTO
{
    public class CreateCountryDTO : CountryDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Country Name Too Long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 3, ErrorMessage = "ShortName Too Long")]
        public string ShortName { get; set; }

        public DateTime DateCreated { get; set; }
    }
    public class CountryDTO
    {
        public int Id { get; set; }
        public IList<HotelDTO> Hotels { get; set; }
    }
}
