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
        private const int PageSize = 10;

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
                .Include(t => t.Reviews)
                    .ThenInclude(r => r.Student)
                .FirstOrDefaultAsync(t => t.Id == id);
            return _mapper.Map<TutorDto>(tutor);
        }

        public async Task<TutorDto> GetTutorByUserIdAsync(int userId)
        {
            var tutor = await _context.Tutors
                .Include(t => t.User)
                .Include(t => t.TutorSubjects)
                    .ThenInclude(ts => ts.Subject)
                .Include(t => t.Reviews)
                    .ThenInclude(r => r.Student)
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

            // Сначала сохраняем репетитора в базу
            _context.Tutors.Add(tutor);
            await _context.SaveChangesAsync();

            // Теперь, когда у нас есть Id, добавляем связи с предметами
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
                await _context.SaveChangesAsync();
            }

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
            return await _context.Tutors.AnyAsync(e => e.UserId == userId);
        }

        public async Task<IEnumerable<TutorDto>> SearchTutorsAdvancedAsync(
            string? term,
            int? subjectId,
            int? minExperience,
            decimal? minPrice,
            decimal? maxPrice,
            int? rating,
            string? place,
            string? sortBy = null,
            bool sortDesc = false,
            int page = 1)
        {
            var query = _context.Tutors
                .Include(t => t.User)
                .Include(t => t.TutorSubjects)
                    .ThenInclude(ts => ts.Subject)
                .AsQueryable();

            // Фільтр по текстовому пошуку
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(t =>
                    t.User.FirstName.Contains(term) ||
                    t.User.LastName.Contains(term) ||
                    t.Bio.Contains(term) ||
                    t.Education.Contains(term) ||
                    t.TeachingStyle.Contains(term));
            }

            // Фільтр по предмету
            if (subjectId.HasValue)
            {
                query = query.Where(t => t.TutorSubjects.Any(ts => ts.SubjectsId == subjectId.Value));
            }

            // Фільтр по досвіду
            if (minExperience.HasValue)
            {
                query = query.Where(t => t.YearsOfExperience >= minExperience.Value);
            }

            // Фільтр по ціні
            if (minPrice.HasValue)
            {
                query = query.Where(t => t.HourlyRate >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(t => t.HourlyRate <= maxPrice.Value);
            }

            // Фільтр по рейтингу
            if (rating.HasValue)
            {
                query = query.Where(t => t.AverageRating >= rating.Value);
            }

            // Фільтр по місцю проведення
            if (!string.IsNullOrWhiteSpace(place))
            {
                query = query.Where(t => t.TeachingStyle.Contains(place));
            }

            // Сортування
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "price" => sortDesc ? query.OrderByDescending(t => t.HourlyRate) : query.OrderBy(t => t.HourlyRate),
                    "rating" => sortDesc ? query.OrderByDescending(t => t.AverageRating) : query.OrderBy(t => t.AverageRating),
                    "experience" => sortDesc ? query.OrderByDescending(t => t.YearsOfExperience) : query.OrderBy(t => t.YearsOfExperience),
                    _ => query
                };
            }

            // Пагінація
            var tutors = await query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TutorDto>>(tutors);
        }
    }
} 