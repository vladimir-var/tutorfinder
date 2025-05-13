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
    public class TutorsController : ControllerBase
    {
        private readonly ITutorService _tutorService;
        private readonly IReviewService _reviewService;

        public TutorsController(ITutorService tutorService, IReviewService reviewService)
        {
            _tutorService = tutorService;
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

        // GET: api/Tutors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TutorDto>>> GetTutors()
        {
            var tutors = await _tutorService.GetAllTutorsAsync();
            return Ok(tutors);
        }

        // GET: api/Tutors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TutorDto>> GetTutor(int id)
        {
            var tutor = await _tutorService.GetTutorByIdAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }
            return Ok(tutor);
        }

        // GET: api/Tutors/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<TutorDto>> GetTutorByUserId(int userId)
        {
            var tutor = await _tutorService.GetTutorByUserIdAsync(userId);
            if (tutor == null)
            {
                return NotFound();
            }
            return Ok(tutor);
        }

        // GET: api/Tutors/subject/5
        [HttpGet("subject/{subjectId}")]
        public async Task<ActionResult<IEnumerable<TutorDto>>> GetTutorsBySubject(int subjectId)
        {
            var tutors = await _tutorService.GetTutorsBySubjectAsync(subjectId);
            return Ok(tutors);
        }

        // GET: api/Tutors/search?term=math
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TutorDto>>> SearchTutors([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest("Поисковый запрос не может быть пустым");
            }

            var tutors = await _tutorService.SearchTutorsAsync(term);
            return Ok(tutors);
        }

        // POST: api/Tutors
        [HttpPost]
        public async Task<ActionResult<TutorDto>> CreateTutor(CreateTutorDto createTutorDto)
        {
            if (await _tutorService.TutorExistsByUserIdAsync(createTutorDto.UserId))
            {
                return BadRequest("Репетитор с таким UserId уже существует");
            }

            var tutor = await _tutorService.CreateTutorAsync(createTutorDto);
            return CreatedAtAction(nameof(GetTutor), new { id = tutor.Id }, tutor);
        }

        // PUT: api/Tutors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTutor(int id, UpdateTutorDto updateTutorDto)
        {
            if (!await _tutorService.TutorExistsAsync(id))
            {
                return NotFound();
            }

            var tutor = await _tutorService.UpdateTutorAsync(id, updateTutorDto);
            if (tutor == null)
            {
                return BadRequest();
            }

            return Ok(tutor);
        }

        // DELETE: api/Tutors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutor(int id)
        {
            if (!await _tutorService.TutorExistsAsync(id))
            {
                return NotFound();
            }

            await _tutorService.DeleteTutorAsync(id);
            return NoContent();
        }

        // GET: api/Tutors/profile
        [HttpGet("profile")]
        public async Task<ActionResult<TutorDto>> GetTutorProfile()
        {
            var userId = GetCurrentUserId();
            var tutor = await _tutorService.GetTutorByUserIdAsync(userId);
            if (tutor == null)
            {
                return NotFound();
            }
            return Ok(tutor);
        }

        // GET: api/Tutors/certificates
        [HttpGet("certificates")]
        public async Task<ActionResult<string>> GetTutorCertificates()
        {
            var userId = GetCurrentUserId();
            var tutor = await _tutorService.GetTutorByUserIdAsync(userId);
            if (tutor == null)
            {
                return NotFound();
            }
            return Ok(tutor.Certifications);
        }

        // POST: api/Tutors/certificates
        [HttpPost("certificates")]
        public async Task<ActionResult<string>> AddCertificate([FromBody] CertificateNameDto dto)
        {
            var userId = GetCurrentUserId();
            var tutor = await _tutorService.GetTutorByUserIdAsync(userId);
            if (tutor == null)
            {
                return NotFound();
            }

            string newCerts;
            if (string.IsNullOrWhiteSpace(tutor.Certifications))
                newCerts = dto.Name;
            else
                newCerts = tutor.Certifications + "\n" + dto.Name;

            var updateDto = new UpdateTutorDto
            {
                Certifications = newCerts
            };

            var updatedTutor = await _tutorService.UpdateTutorAsync(tutor.Id, updateDto);
            return Ok(updatedTutor.Certifications);
        }

        // DELETE: api/Tutors/certificates/{id}
        [HttpDelete("certificates/{id}")]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            var userId = GetCurrentUserId();
            var tutor = await _tutorService.GetTutorByUserIdAsync(userId);
            if (tutor == null)
            {
                return NotFound();
            }

            var certificates = tutor.Certifications?.Split('\n').ToList() ?? new List<string>();
            if (id >= 0 && id < certificates.Count)
            {
                certificates.RemoveAt(id);
                var updateDto = new UpdateTutorDto
                {
                    Certifications = string.Join("\n", certificates)
                };

                await _tutorService.UpdateTutorAsync(tutor.Id, updateDto);
                return NoContent();
            }

            return NotFound();
        }

        // GET: api/Tutors/reviews
        [HttpGet("reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetTutorReviews()
        {
            var userId = GetCurrentUserId();
            var tutor = await _tutorService.GetTutorByUserIdAsync(userId);
            if (tutor == null)
            {
                return NotFound();
            }
            var reviews = await _reviewService.GetReviewsByTutorIdAsync(tutor.Id);
            return Ok(reviews);
        }
    }
} 