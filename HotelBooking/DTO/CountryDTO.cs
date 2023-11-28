using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTO
{
    public class CreateCountryDTO 
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Country Name Too Long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 3, ErrorMessage = "ShortName Too Long")]
        public string ShortName { get; set; }

        public DateTime DateCreated { get; set; }
    }

    public class UpdateCountryDTO: CreateCountryDTO
    {

    }

    public class CountryDTO : CreateCountryDTO
    {
        public int Id { get; set; }
        public virtual IList<HotelDTO> Hotels { get; set; }
    }
}
