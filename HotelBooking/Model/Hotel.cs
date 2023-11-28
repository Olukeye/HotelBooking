using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Distance { get; set; }

        public virtual IList<Room> Rooms { get; set; }

        public decimal CheapestRate { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public string Featured { get; set; }
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
