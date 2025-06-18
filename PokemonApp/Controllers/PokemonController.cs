using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using PokemonApp.Models.Dtos;

namespace PokemonApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PokemonController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemons()
        {
            var pokemons = await _unitOfWork.Pokemons.GetAllAsync();

            if (pokemons == null)
                return NotFound();

            var res = _mapper.Map<List<PokemonDto>>(pokemons); 
            return Ok(res);
        }

        [HttpGet("{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(PokemonDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemon(int pokemonId)
        {
            if (pokemonId == 0)
                return BadRequest();

            if (!await _unitOfWork.Pokemons.EntityExistsAsync(pokemonId))
                return NotFound();

            var pokemon = await _unitOfWork.Pokemons.GetByIdAsync(pokemonId);
            var res = _mapper.Map<PokemonDto>(pokemon);

            return Ok(res);

        }

        [HttpGet("{pokemonId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemonRating(int pokemonId)
        {
            if (pokemonId == 0)
                return BadRequest();

            if (!await _unitOfWork.Pokemons.EntityExistsAsync(pokemonId))
                return NotFound();

            var res = await _unitOfWork.Pokemons.GetPokemonRatingAsync(pokemonId);
            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId,[FromBody] PokemonDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonExist = await _unitOfWork.Pokemons.GetPokemonTrimToUpperAsync(createDto);

            if (pokemonExist != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            var pokemon = _mapper.Map<Pokemon>(createDto);

            if (!await _unitOfWork.Owners.EntityExistsAsync(ownerId))
            {
                ModelState.AddModelError("", "Owner Not Found");
                return StatusCode(422, ModelState);
            }

            if (!await _unitOfWork.Categories.EntityExistsAsync(categoryId))
            {
                ModelState.AddModelError("", "Category Not Found");
                return StatusCode(422, ModelState);
            }

            await _unitOfWork.Pokemons.CreatePokemonAsync(ownerId ,categoryId ,pokemon);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpPut("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePokemon(int pokemonId, [FromBody] PokemonUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (pokemonId != updateDto.Id)
                return BadRequest();

            if (!await _unitOfWork.Pokemons.EntityExistsAsync(pokemonId))
                return NotFound();

            var pokemon = await _unitOfWork.Pokemons.GetByIdAsync(pokemonId);

            pokemon.Name = updateDto.Name;
            _unitOfWork.Pokemons.Update(pokemon);
            var res = await _unitOfWork.SaveAsync();
            
            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePokemon(int pokemonId)
        {
            if (pokemonId == 0)
                return BadRequest();

            if (!await _unitOfWork.Pokemons.EntityExistsAsync(pokemonId))
            {
                ModelState.AddModelError("", "Pokemon Not Found");
                return StatusCode(404, ModelState);
            }

            var pokemon = await _unitOfWork.Pokemons.GetByIdAsync(pokemonId);
            _unitOfWork.Pokemons.Delete(pokemon);

            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
