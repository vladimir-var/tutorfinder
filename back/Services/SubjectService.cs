using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tutorfinder.Models;
using tutorfinder.Models.DTOs;

namespace tutorfinder.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDbContext _context;

        public SubjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SubjectDto> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
                return null;

            return MapToDto(subject);
        }

        public async Task<IEnumerable<SubjectDto>> GetAllAsync()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return subjects.Select(MapToDto);
        }

        public async Task<SubjectDto> CreateAsync(CreateSubjectDto createSubjectDto)
        {
            var subject = new Subject
            {
                Name = createSubjectDto.Name,
                Description = createSubjectDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return MapToDto(subject);
        }

        public async Task DeleteAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }

        private static SubjectDto MapToDto(Subject subject)
        {
            return new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Description = subject.Description,
                CreatedAt = subject.CreatedAt
            };
        }
    }
} 