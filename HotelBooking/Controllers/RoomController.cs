using AutoMapper;
using HotelBooking.DTO;
using HotelBooking.IRepository;
using HotelBooking.Model;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoomController> _logger;
        private readonly IMapper _mapper;

        public RoomController(IUnitOfWork unitOfWork, ILogger<RoomController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRooms([FromQuery] PaggingRequest paggingRequest)
        {
            var rooms = await _unitOfWork.Rooms.Pagging(paggingRequest);
            var result = _mapper.Map<IList<Room>>(rooms);
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetRoom")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _unitOfWork.Rooms.Get(q => q.Id == id);
            var result = _mapper.Map<RoomDTO>(room);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDTO createRoom)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post Action in {nameof(CreateRoom)}");
                return BadRequest(ModelState);
            }

            var room = _mapper.Map<Room>(createRoom);
            await _unitOfWork.Rooms.Insert(room);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetRoom", new { id = room.Id }, room);

        }

        [HttpPut("{id:int}", Name = "UpdateRoom")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRoom(int id,[FromBody] UpdateRoomDTO updateRoom)
        {
            if(!ModelState.IsValid && id < 1)
            {
                _logger.LogError($"Invalid Put Action in {nameof(UpdateRoom)}");
                return BadRequest(ModelState);
            }

            var room = await _unitOfWork.Rooms.Get(q => q.Id == id);
            if(room == null)
            {
                _logger.LogError($"Invalid action in {nameof(UpdateRoom)}");
                return BadRequest("User with {id} does not exist");
            }

            var result = _mapper.Map(UpdateRoom, room);
            _unitOfWork.Rooms.Update(result);
            await _unitOfWork.Save();

            return Ok(result);
        }

        [HttpDelete("{id:int}", Name = "DeleteRoom")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            if (!ModelState.IsValid && id < 1)
            {
                _logger.LogError($"Invalid Delete Action in {nameof(DeleteRoom)}");
                return BadRequest(ModelState);
            }

            var room = await _unitOfWork.Rooms.Get(q => q.Id == id);
            if (room == null)
            {
                _logger.LogError($"Invalid action in {nameof(DeleteRoom)}");
                return BadRequest("Wrong Data Submitted");
            }

            await  _unitOfWork.Rooms.Delete(id);
            await _unitOfWork.Save();

            return NoContent();
        }
    }
}
