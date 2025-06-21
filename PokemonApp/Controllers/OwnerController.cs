using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using PokemonApp.Models.Dtos;
using PokemonApp.Repositories;

namespace PokemonApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OwnerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
        public async Task<IActionResult> GetOwners()
        {
            var owners = await _unitOfWork.Owners.GetAllAsync();
            var res = _mapper.Map<List<OwnerDto>>(owners);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(res);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]

        public async Task<IActionResult> GetOwner(int ownerId)
        {
            if (!await _unitOfWork.Owners.EntityExistsAsync(ownerId))
                return NotFound();

            var owner = await _unitOfWork.Owners.GetByIdAsync(ownerId);
            var res = _mapper.Map<OwnerDto>(owner);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(res);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(List<PokemonDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllPokemonsByOwner(int ownerId)
        {
            if (!await _unitOfWork.Owners.EntityExistsAsync(ownerId))
                return NotFound();

            var pokemons = await _unitOfWork.Owners.GetPokemonByOwnerAsync(ownerId);
            var res = _mapper.Map<List<PokemonDto>>(pokemons);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(res);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto createDto)
        {
            if (createDto == null)
                return BadRequest(ModelState);

            var owners = await _unitOfWork.Owners.GetAllAsync();
            var search = owners.Where(c => c.LastName.Trim().ToUpper() == createDto.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (search != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(createDto);

            var country = await _unitOfWork.Countries.GetByIdAsync(countryId);

            if (country == null)
            {
                ModelState.AddModelError("", "Country Not Found");
                return NotFound(ModelState);
            }

            ownerMap.Country = country;

            await _unitOfWork.Owners.AddAsync(ownerMap);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateOwner(int ownerId, OwnerUpdateDto updateDto)
        {
            if (updateDto == null)
                return BadRequest(ModelState);

            if (ownerId != updateDto.Id)
                return BadRequest(ModelState);

            if (! await _unitOfWork.Owners.EntityExistsAsync(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owner = await _unitOfWork.Owners.GetByIdAsync(ownerId);

            owner.FirstName = updateDto.FirstName;
            owner.LastName = updateDto.LastName;
            owner.Gym = updateDto.Gym;



            _unitOfWork.Owners.Update(owner);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteOwner(int ownerId)
        {
            if (!await _unitOfWork.Owners.EntityExistsAsync(ownerId))
                return NotFound();

            var owner = await _unitOfWork.Owners.GetByIdAsync(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.Owners.Delete(owner);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
