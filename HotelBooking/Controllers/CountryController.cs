using AutoMapper;
using HotelBooking.DTO;
using HotelBooking.IRepository;
using HotelBooking.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CountryController> _logger; //Note: If you don't add <CountryController> to the Ilogger, you may have issues whilse testing

        public CountryController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CountryController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] PaggingRequest paggingRequest)
        {

            var countries = await _unitOfWork.Countries.Pagging(paggingRequest);
            var result = _mapper.Map<IList<CountryDTO>>(countries);
            return Ok(result);

        }

        [HttpGet("{id:int}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
    
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
            var result = _mapper.Map<CountryDTO>(country);
            return Ok(country);
           
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post Action in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }
           
            var country = _mapper.Map<Country>(countryDTO);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
           
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}", Name = "UpdateCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid && id < 1)
            {
                _logger.LogError($"Invalid Update Action in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if(country == null)
            {
                _logger.LogError($"Invalid action in {nameof(UpdateCountry)}");
                return BadRequest("Wrong Data Submitted");
            }

            _mapper.Map(countryDTO, country);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();

            return Ok(country);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}", Name = "DeleteCountry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (!ModelState.IsValid && id < 1)
            {
                _logger.LogError($"Invalid Delete Action in {nameof(DeleteCountry)}");
                return BadRequest(ModelState);
            }
           
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if(country == null)
            {
                _logger.LogError($"Invalid action in {nameof(DeleteCountry)}");
                return BadRequest("User with id doeas not exist");
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();

            return NoContent();

        }
    }
}
