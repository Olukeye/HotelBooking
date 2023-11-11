using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.DTO
{
    public class CreateHotelDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Hotel Name Too Long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "City Name Too Long")]
        public string City { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Title Name Too Long")]
        public string Title { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Type Of Room Too Long")]
        public string Type { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Address Too Long")]
        public string Address { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Distance Detail Too Long")]
        public string Distance { get; set; }


        [Required]
        public decimal CheapestRate { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Description Too Long")]
        public string Description { get; set; }

        [Required]
        [Range(1, 5)]
        public Decimal Rating { get; set; }

        public string Featured { get; set; }

        public int CountryId { get; set; }

        public DateTime DateCreated { get; set; }
    }

    public class HotelDTO : CreateHotelDTO
    {
        public int Id { get; set; }
        public CountryDTO Country { get; set; }

        public RoomDTO Room { get; set; }
    }
}
