using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tutorfinder.DTOs;
using tutorfinder.Models;

namespace tutorfinder.Services
{
    public class TutorService : ITutorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TutorService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TutorDto>> GetAllTutorsAsync()
        {
            var tutors = await _context.Tutors
                .Include(t => t.User)
                .Include(t => t.TutorSubjects)
                    .ThenInclude(ts => ts.Subject)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TutorDto>>(tutors);
        }

        public async Task<TutorDto> GetTutorByIdAsync(int id)
        {
            var tutor = await _context.Tutors
                .Include(t => t.User)
                .Include(t => t.TutorSubjects)
                    .ThenInclude(ts => ts.Subject)
                .FirstOrDefaultAsync(t => t.Id == id);
            return _mapper.Map<TutorDto>(tutor);
        }

        public async Task<TutorDto> GetTutorByUserIdAsync(int userId)
        {
            var tutor = await _context.Tutors
                .Include(t => t.User)
                .Include(t => t.TutorSubjects)
                    .ThenInclude(ts => ts.Subject)
                .FirstOrDefaultAsync(t => t.UserId == userId);
            return _mapper.Map<TutorDto>(tutor);
        }

        public async Task<IEnumerable<TutorDto>> GetTutorsBySubjectAsync(int subjectId)
        {
            var tutors = await _context.Tutors
                .Include(t => t.User)
                .Include(t => t.TutorSubjects)
                    .ThenInclude(ts => ts.Subject)
                .Where(t => t.TutorSubjects.Any(ts => ts.SubjectsId == subjectId))
                .ToListAsync();
            return _mapper.Map<IEnumerable<TutorDto>>(tutors);
        }

        public async Task<IEnumerable<TutorDto>> SearchTutorsAsync(string searchTerm)
        {
            var tutors = await _context.Tutors
                .Include(t => t.User)
                .Include(t => t.TutorSubjects)
                    .ThenInclude(ts => ts.Subject)
                .Where(t => 
                    t.User.FirstName.Contains(searchTerm) ||
                    t.User.LastName.Contains(searchTerm) ||
                    t.Bio.Contains(searchTerm) ||
                    t.Education.Contains(searchTerm) ||
                    t.TeachingStyle.Contains(searchTerm))
                .ToListAsync();
            return _mapper.Map<IEnumerable<TutorDto>>(tutors);
        }

        public async Task<TutorDto> CreateTutorAsync(CreateTutorDto createTutorDto)
        {
            var tutor = _mapper.Map<Tutor>(createTutorDto);

            if (createTutorDto.SubjectIds != null)
            {
                foreach (var subjectId in createTutorDto.SubjectIds)
                {
                    tutor.TutorSubjects.Add(new TutorSubject
                    {
                        TutorsId = tutor.Id,
                        SubjectsId = subjectId
                    });
                }
            }

            _context.Tutors.Add(tutor);
            await _context.SaveChangesAsync();

            return await GetTutorByIdAsync(tutor.Id);
        }

        public async Task<TutorDto> UpdateTutorAsync(int id, UpdateTutorDto updateTutorDto)
        {
            var tutor = await _context.Tutors
                .Include(t => t.TutorSubjects)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tutor == null) return null;

            _mapper.Map(updateTutorDto, tutor);

            if (updateTutorDto.SubjectIds != null)
            {
                // Удаляем старые связи
                tutor.TutorSubjects.Clear();

                // Добавляем новые связи
                foreach (var subjectId in updateTutorDto.SubjectIds)
                {
                    tutor.TutorSubjects.Add(new TutorSubject
                    {
                        TutorsId = tutor.Id,
                        SubjectsId = subjectId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return await GetTutorByIdAsync(tutor.Id);
        }

        public async Task DeleteTutorAsync(int id)
        {
            var tutor = await _context.Tutors.FindAsync(id);
            if (tutor != null)
            {
                _context.Tutors.Remove(tutor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TutorExistsAsync(int id)
        {
            return await _context.Tutors.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> TutorExistsByUserIdAsync(int userId)
        {
            return await _context.Tutors.AnyAsync(t => t.UserId == userId);
        }

        public async Task<IEnumerable<CertificateDto>> GetTutorCertificatesAsync(int tutorId)
        {
            var certificates = await _context.Certificates
                .Where(c => c.TutorId == tutorId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CertificateDto>>(certificates);
        }

        public async Task<IEnumerable<ReviewDto>> GetTutorReviewsAsync(int tutorId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.Student)
                .Where(r => r.TutorId == tutorId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }
    }
} 