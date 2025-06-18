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
    public class ReviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            if (reviews == null)
                return NotFound();

            var res = _mapper.Map<List<ReviewDto>>(reviews);

            return Ok(res);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]

        public async Task<IActionResult> GetOwner(int reviewId)
        {
            if (reviewId == 0)
                return BadRequest();

            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);

            if (review == null)
                return NotFound();

            var res = _mapper.Map<ReviewDto>(review);
            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateReview([FromQuery] int reviewerId,
            [FromQuery] int pokemonId,[FromBody] ReviewDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var reviews = await _unitOfWork.Reviews.GetAllAsync();

            var review = reviews.Where(c => c.Title.Trim().ToUpper() 
                            == createDto.Title.TrimEnd().ToUpper()).FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            var pokemon = await _unitOfWork.Pokemons.GetByIdAsync(pokemonId);
            var reviewer = await _unitOfWork.Reviewers.GetByIdAsync(reviewerId);

            if (pokemon == null || reviewer == null)
                return BadRequest();

            var newReview = _mapper.Map<Review>(createDto);

            newReview.Reviewer = reviewer;
            newReview.Pokemon = pokemon;

            await _unitOfWork.Reviews.AddAsync(newReview);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");

        }


        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReviewsForPokemon(int pokemonId)
        {
            if (pokemonId == 0)
                return BadRequest();

            var reviews = await _unitOfWork.Reviews.GetReviewsOfAPokemonAsync(pokemonId);

            if (reviews == null)
                return NotFound();

            var res = _mapper.Map<List<ReviewDto>>(reviews);

            return Ok(res);
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewUpdateDto updateDto)
        {
            if (updateDto == null)
                return BadRequest();

            if (reviewId != updateDto.Id)
                return BadRequest();

            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);

            if (review == null)
                return NotFound();

            review.Title = updateDto.Title;
            review.Text = updateDto.Text;
            review.Rating = updateDto.Rating;

            _unitOfWork.Reviews.Update(review);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            if (reviewId == 0)
                return BadRequest();

            if (!await _unitOfWork.Reviews.EntityExistsAsync(reviewId))
                NotFound();

            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);

            _unitOfWork.Reviews.Delete(review);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
                return StatusCode(500, ModelState);

            }

            return NoContent();
        }

        [HttpDelete("/DeleteReviewsByReviewer/{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReviewsByReviewer(int reviewerId)
        {
            if (!await _unitOfWork.Reviewers.EntityExistsAsync(reviewerId))
                return NotFound();

            var reviews = await _unitOfWork.Reviewers.GetReviewsByReviewerAsync(reviewerId);
            var reviewsList = reviews.ToList();

            if (!ModelState.IsValid)
                return BadRequest();

            _unitOfWork.Reviews.DeleteReviews(reviewsList);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "error deleting reviews");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
