using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using tutorfinder.DTOs;
using tutorfinder.Services;
using System.Security.Claims;

namespace tutorfinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;

        public StudentsController(IUserService userService, IReviewService reviewService)
        {
            _userService = userService;
            _reviewService = reviewService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return int.Parse(userIdClaim.Value);
        }

        // GET: api/Students/profile
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetStudentProfile()
        {
            var userId = GetCurrentUserId();
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/Students/reviews
        [HttpGet("reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetStudentReviews()
        {
            var userId = GetCurrentUserId();
            var reviews = await _reviewService.GetReviewsByStudentIdAsync(userId);
            return Ok(reviews);
        }
    }
} 