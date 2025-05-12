using Microsoft.AspNetCore.Mvc;
using tutorfinder.DTOs;
using tutorfinder.Services;

namespace tutorfinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TutorProfileController : ControllerBase
    {
        private readonly ITutorService _tutorService;
        private readonly IUserService _userService;

        public TutorProfileController(ITutorService tutorService, IUserService userService)
        {
            _tutorService = tutorService;
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<TutorProfileDto>> GetProfile()
        {
            // TODO: Получать ID пользователя из токена
            var userId = 1; // Временное решение

            var tutor = await _tutorService.GetTutorByUserIdAsync(userId);
            if (tutor == null)
            {
                return NotFound("Репетитор не найден");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден");
            }

            var profile = new TutorProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Bio = tutor.Bio,
                Education = tutor.Education,
                YearsOfExperience = tutor.YearsOfExperience,
                HourlyRate = tutor.HourlyRate,
                TeachingStyle = tutor.TeachingStyle,
                Certifications = tutor.Certifications
            };

            return Ok(profile);
        }

        [HttpGet("certificates")]
        public async Task<ActionResult<IEnumerable<CertificateDto>>> GetCertificates()
        {
            // TODO: Получать ID пользователя из токена
            var userId = 1; // Временное решение

            var tutor = await _tutorService.GetTutorByUserIdAsync(userId);
            if (tutor == null)
            {
                return NotFound("Репетитор не найден");
            }

            var certificates = await _tutorService.GetTutorCertificatesAsync(tutor.Id);
            return Ok(certificates);
        }

        [HttpGet("reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {
            // TODO: Получать ID пользователя из токена
            var userId = 1; // Временное решение

            var tutor = await _tutorService.GetTutorByUserIdAsync(userId);
            if (tutor == null)
            {
                return NotFound("Репетитор не найден");
            }

            var reviews = await _tutorService.GetTutorReviewsAsync(tutor.Id);
            return Ok(reviews);
        }
    }
} 