using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using PokemonApp.Models.Dtos;

namespace PokemonApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public async Task<IActionResult> GetReviewers()
        {
            var reviewers = await _unitOfWork.Reviewers.GetAllAsync();

            if (reviewers == null)
                return BadRequest();

            var res = _mapper.Map<List<ReviewerDto>>(reviewers);
            return Ok(res);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewer(int reviewerId)
        {
            if (reviewerId == 0)
                return BadRequest();

            if (!await _unitOfWork.Reviewers.EntityExistsAsync(reviewerId))
                return NotFound();

            var reviewer = await _unitOfWork.Reviewers.GetByIdAsync(reviewerId);
            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public async Task<IActionResult> GetReviewsByAReviewer(int reviewerId)
        {
            if (reviewerId == 0)
                return BadRequest();

            if (!await _unitOfWork.Reviewers.EntityExistsAsync(reviewerId))
                return NotFound();

            var reviews = await _unitOfWork.Reviewers.GetReviewsByReviewerAsync(reviewerId);
            var res = _mapper.Map<List<ReviewDto>>(reviews);

            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReviewer([FromBody] ReviewerDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var reviewer = _mapper.Map<Reviewer>(createDto);

            var reviewerList = await _unitOfWork.Reviewers.GetAllAsync();
            var reviewerExists = reviewerList.Where(r => r.LastName.Trim().ToUpper()
            == createDto.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (reviewerExists != null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            await _unitOfWork.Reviewers.AddAsync(reviewer);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReviewer(int reviewerId, [FromBody] ReviewerUpdateDto updateDto)
        {
            if (reviewerId != updateDto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                BadRequest(ModelState);

            if (!await _unitOfWork.Reviewers.EntityExistsAsync(reviewerId))
                return NotFound();

            var reviewer = await _unitOfWork.Reviewers.GetByIdAsync(reviewerId);
            reviewer.FirstName = updateDto.FirstName;
            reviewer.LastName = updateDto.LastName;

            _unitOfWork.Reviewers.Update(reviewer);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReviewer(int reviewerId)
        {
            if (reviewerId == 0)
                return BadRequest();

            if (!await _unitOfWork.Reviewers.EntityExistsAsync(reviewerId))
                return NotFound();

            var reviewer = await _unitOfWork.Reviewers.GetByIdAsync(reviewerId);
            _unitOfWork.Reviewers.Delete(reviewer);
            var res = await _unitOfWork.SaveAsync();

            if (res <= 0)
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
