using Microsoft.AspNetCore.Mvc;
using tutorfinder.DTOs;
using tutorfinder.Services;

namespace tutorfinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        // GET: api/Reviews/tutor/5
        [HttpGet("tutor/{tutorId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByTutor(int tutorId)
        {
            var reviews = await _reviewService.GetReviewsByTutorIdAsync(tutorId);
            return Ok(reviews);
        }

        // GET: api/Reviews/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByUser(int userId)
        {
            var reviews = await _reviewService.GetReviewsByStudentIdAsync(userId);
            return Ok(reviews);
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> CreateReview(CreateReviewDto createReviewDto)
        {
            var review = await _reviewService.CreateReviewAsync(createReviewDto);
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, UpdateReviewDto updateReviewDto)
        {
            if (!await _reviewService.ReviewExistsAsync(id))
            {
                return NotFound();
            }

            var review = await _reviewService.UpdateReviewAsync(id, updateReviewDto);
            if (review == null)
            {
                return BadRequest();
            }

            return Ok(review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (!await _reviewService.ReviewExistsAsync(id))
            {
                return NotFound();
            }

            await _reviewService.DeleteReviewAsync(id);
            return NoContent();
        }
    }
} 