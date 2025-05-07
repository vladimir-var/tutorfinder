using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tutorfinder.Models;
using tutorfinder.Models.DTOs;

namespace tutorfinder.Services
{
    public class TutorService : ITutorService
    {
        private readonly ApplicationDbContext _context;

        public TutorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TutorDto> GetByIdAsync(int id)
        {
            var tutor = await _context.Tutors
                .Include(t => t.Subjects)
                .Include(t => t.Reviews)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tutor == null)
                return null;

            return MapToDto(tutor);
        }

        public async Task<IEnumerable<TutorDto>> GetAllAsync()
        {
            var tutors = await _context.Tutors
                .Include(t => t.Subjects)
                .Include(t => t.Reviews)
                .ToListAsync();

            return tutors.Select(MapToDto);
        }

        public async Task<TutorDto> CreateAsync(CreateTutorDto createTutorDto)
        {
            var tutor = new Tutor
            {
                UserId = createTutorDto.UserId,
                Education = createTutorDto.Education,
                Experience = createTutorDto.Experience,
                Description = createTutorDto.Description,
                HourlyRate = createTutorDto.HourlyRate,
                IsAvailable = createTutorDto.IsAvailable,
                CreatedAt = DateTime.UtcNow
            };

            if (createTutorDto.SubjectIds != null && createTutorDto.SubjectIds.Any())
            {
                var subjects = await _context.Subjects
                    .Where(s => createTutorDto.SubjectIds.Contains(s.Id))
                    .ToListAsync();

                tutor.Subjects = subjects;
            }

            _context.Tutors.Add(tutor);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(tutor.Id);
        }

        public async Task<TutorDto> UpdateAsync(int id, UpdateTutorDto updateTutorDto)
        {
            var tutor = await _context.Tutors
                .Include(t => t.Subjects)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tutor == null)
                return null;

            if (!string.IsNullOrEmpty(updateTutorDto.Education))
                tutor.Education = updateTutorDto.Education;

            if (!string.IsNullOrEmpty(updateTutorDto.Experience))
                tutor.Experience = updateTutorDto.Experience;

            if (!string.IsNullOrEmpty(updateTutorDto.Description))
                tutor.Description = updateTutorDto.Description;

            if (updateTutorDto.HourlyRate.HasValue)
                tutor.HourlyRate = updateTutorDto.HourlyRate.Value;

            if (updateTutorDto.IsAvailable.HasValue)
                tutor.IsAvailable = updateTutorDto.IsAvailable.Value;

            if (updateTutorDto.SubjectIds != null)
            {
                var subjects = await _context.Subjects
                    .Where(s => updateTutorDto.SubjectIds.Contains(s.Id))
                    .ToListAsync();

                tutor.Subjects = subjects;
            }

            await _context.SaveChangesAsync();

            return await GetByIdAsync(tutor.Id);
        }

        public async Task DeleteAsync(int id)
        {
            var tutor = await _context.Tutors.FindAsync(id);
            if (tutor != null)
            {
                _context.Tutors.Remove(tutor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TutorDto>> GetBySubjectIdAsync(int subjectId)
        {
            var tutors = await _context.Tutors
                .Include(t => t.Subjects)
                .Include(t => t.Reviews)
                .Where(t => t.Subjects.Any(s => s.Id == subjectId))
                .ToListAsync();

            return tutors.Select(MapToDto);
        }

        public async Task<IEnumerable<TutorDto>> GetAvailableTutorsAsync()
        {
            var tutors = await _context.Tutors
                .Include(t => t.Subjects)
                .Include(t => t.Reviews)
                .Where(t => t.IsAvailable)
                .ToListAsync();

            return tutors.Select(MapToDto);
        }

        private static TutorDto MapToDto(Tutor tutor)
        {
            return new TutorDto
            {
                Id = tutor.Id,
                UserId = tutor.UserId,
                Education = tutor.Education,
                Experience = tutor.Experience,
                Description = tutor.Description,
                HourlyRate = tutor.HourlyRate,
                IsAvailable = tutor.IsAvailable,
                CreatedAt = tutor.CreatedAt,
                Subjects = tutor.Subjects?.Select(s => new SubjectDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CreatedAt = s.CreatedAt
                }).ToList(),
                Reviews = tutor.Reviews?.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    TutorId = r.TutorId,
                    StudentId = r.StudentId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                }).ToList()
            };
        }
    }
} 