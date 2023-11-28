using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTO
{
    public class CreateRoomDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Room Name Too Long")]
        public string Name { get; set; }

        [Required]
        public int RoomNumber { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 2)]
        public int MaxGuestPerRoom { get; set; }

        [Required]
        [StringLength(maximumLength: 200, ErrorMessage = "Room Description Name Too Long")]
        public string Description { get; set; }

        [Required]
        public bool Available { get; set; }

        [Required]
        public int HotelId { get; set; }

        public DateTime DateCreated { get; set; }

    }

    public class RoomDTO : CreateRoomDTO
    {
        public int Id { get; set; }
        public HotelDTO Hotel { get; set; }

    }

    public class UpdateRoomDTO : CreateRoomDTO
    {

    }
}
