using AutoMapper;
using HotelBooking.DTO;
using HotelBooking.IRepository;
using HotelBooking.Model;
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
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                var result = _mapper.Map<IList<CountryDTO>>(countries);
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong In The {nameof(GetCountries)}");
                return StatusCode(500, $"Internal Server Error. Please Try Again Later");
            }
        }

        [HttpGet("{id:int}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
                var result = _mapper.Map<CountryDTO>(country);
                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong In The {nameof(GetCountries)}");
                return StatusCode(500, $"Internal Server Error. Please Try Again Later");
            }
        }

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
            try
            {
                var country = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
                //return CreatedAtAction("CreateCountry", country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong In The {nameof(CreateCountry)}");
                return StatusCode(500, $"Internal Server Error. Please Try Again Later");
            }
        }

        [HttpPut("{id:int}")]
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
            try
            {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong In The {nameof(UpdateCountry)}");
                return StatusCode(500, $"Internal Server Error. Please Try Again Later");
            }
        }

        [HttpDelete("{id:int}")]
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
            try
            {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong In The {nameof(DeleteCountry)}");
                return StatusCode(500, $"Internal Server Error. Please Try Again Later");
            }
        }
    }
}
