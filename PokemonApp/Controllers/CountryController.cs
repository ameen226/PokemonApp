using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using PokemonApp.Models.Dtos;
using PokemonApp.Repositories;

namespace PokemonApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public async Task<IActionResult> GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(await _unitOfWork.Countries.GetAllAsync());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountry(int countryId)
        {
            if (!await _unitOfWork.Countries.EntityExistsAsync(countryId))
            {
                return NotFound();
            }

            var countryDto = _mapper.Map<CountryDto>(await _unitOfWork.Countries.GetByIdAsync(countryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countryDto);
        }

        [HttpGet("/Country/{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public async Task<IActionResult> GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(await _unitOfWork.Countries.GetCountryByOwnerAsync(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCountry(CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);


            var countries = await _unitOfWork.Countries.GetAllAsync();
            var country = countries.Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);

            await _unitOfWork.Countries.AddAsync(countryMap);

            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public async Task<IActionResult> UpdateCountry(int countryId, [FromBody] CountryUpdateDto countryDto)
        {
            if (countryDto == null)
                return BadRequest(ModelState);

            if (countryId != countryDto.Id)
                return BadRequest(ModelState);
        
            if (! await _unitOfWork.Countries.EntityExistsAsync(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(countryDto);

            _unitOfWork.Countries.Update(countryMap);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            if (! await _unitOfWork.Countries.EntityExistsAsync(countryId))
            {
                return NotFound();
            }

            var country = await _unitOfWork.Countries.GetByIdAsync(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.Countries.Delete(country);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
