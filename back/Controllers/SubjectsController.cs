using Microsoft.AspNetCore.Mvc;
using tutorfinder.DTOs;
using tutorfinder.Services;

namespace tutorfinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        // GET: api/Subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
        {
            var subjects = await _subjectService.GetAllSubjectsAsync();
            return Ok(subjects);
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubject(int id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return Ok(subject);
        }

        // GET: api/Subjects/search?term=math
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> SearchSubjects([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest("Поисковый запрос не может быть пустым");
            }

            var subjects = await _subjectService.SearchSubjectsAsync(term);
            return Ok(subjects);
        }

        // POST: api/Subjects
        [HttpPost]
        public async Task<ActionResult<SubjectDto>> CreateSubject(CreateSubjectDto createSubjectDto)
        {
            if (await _subjectService.SubjectExistsByNameAsync(createSubjectDto.Name))
            {
                return BadRequest("Предмет с таким названием уже существует");
            }

            var subject = await _subjectService.CreateSubjectAsync(createSubjectDto);
            return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
        }

        // PUT: api/Subjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, UpdateSubjectDto updateSubjectDto)
        {
            if (!await _subjectService.SubjectExistsAsync(id))
            {
                return NotFound();
            }

            var subject = await _subjectService.UpdateSubjectAsync(id, updateSubjectDto);
            if (subject == null)
            {
                return BadRequest();
            }

            return Ok(subject);
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            if (!await _subjectService.SubjectExistsAsync(id))
            {
                return NotFound();
            }

            await _subjectService.DeleteSubjectAsync(id);
            return NoContent();
        }
    }
} 