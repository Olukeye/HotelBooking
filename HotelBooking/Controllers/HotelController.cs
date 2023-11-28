using AutoMapper;
using HotelBooking.DTO;
using HotelBooking.IRepository;
using HotelBooking.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HotelController> _logger;


        public HotelController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HotelController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels([FromQuery] PaggingRequest paggingRequest)
        {
            var hotel = await _unitOfWork.Hotels.Pagging(paggingRequest);
            var result = _mapper.Map<IList<Hotel>>(hotel);

            return Ok(result);

        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string> { "Rooms" });
            var result = _mapper.Map<HotelDTO>(hotel);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO createlDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post Action in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            var hotel = _mapper.Map<Hotel>(createlDTO);
            await _unitOfWork.Hotels.Insert(hotel);

            await _unitOfWork.Save();
            return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO updateDTO)
        {

            if (!ModelState.IsValid && id < 1)
            {
                _logger.LogError($"Invalid Post Action in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
            if(hotel == null)
            {
                _logger.LogError($"Invalid action in {nameof(UpdateHotel)}");
                return BadRequest("Wrong Data Submitted");
            }

            var result = _mapper.Map(updateDTO, hotel);
            _unitOfWork.Hotels.Update(result);
            await _unitOfWork.Save();

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (!ModelState.IsValid && id < 1)
            {
                _logger.LogError($"Invalid Post Action in {nameof(DeleteHotel)}");
                return BadRequest(ModelState);
            }

            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid action in {nameof(UpdateHotel)}");
                return BadRequest("Wrong Data Submitted");
            }

            await _unitOfWork.Hotels.Delete(id);
            await _unitOfWork.Save();

            return NoContent();
        }
    }
}
