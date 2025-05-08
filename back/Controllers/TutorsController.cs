using Microsoft.AspNetCore.Mvc;
using tutorfinder.DTOs;
using tutorfinder.Services;

namespace tutorfinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TutorsController : ControllerBase
    {
        private readonly ITutorService _tutorService;

        public TutorsController(ITutorService tutorService)
        {
            _tutorService = tutorService;
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
    }
} 