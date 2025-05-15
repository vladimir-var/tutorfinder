using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tutorfinder.DTOs;
using tutorfinder.Models;

namespace tutorfinder.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SubjectService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }


        public async Task<SubjectDto> GetSubjectByIdAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto createSubjectDto)
        {
            var subject = _mapper.Map<Subject>(createSubjectDto);
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<SubjectDto> UpdateSubjectAsync(int id, UpdateSubjectDto updateSubjectDto)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return null;

            _mapper.Map(updateSubjectDto, subject);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SubjectExistsAsync(int id)
        {
            return await _context.Subjects.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> SubjectExistsByNameAsync(string name)
        {
            return await _context.Subjects.AnyAsync(e => e.Name == name);
        }

        public async Task<IEnumerable<SubjectDto>> SearchSubjectsAsync(string searchTerm)
        {
            var subjects = await _context.Subjects
                .Where(s => 
                    s.Name.Contains(searchTerm) ||
                    s.Description.Contains(searchTerm))
                .ToListAsync();
            return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }
    }
} 