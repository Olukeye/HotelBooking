using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoomNumber { get; set; }
        public decimal Price { get; set; }
        public int MaxGuestPerRoom { get; set; }
        public string Description { get; set; }
        public bool Available { get; set; }
        [ForeignKey(nameof(Hotel))]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
