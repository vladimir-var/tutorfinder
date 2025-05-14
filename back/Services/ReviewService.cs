using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tutorfinder.DTOs;
using tutorfinder.Models;

namespace tutorfinder.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReviewService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Student)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByTutorIdAsync(int tutorId)
        {
            await RecalculateAllTutorsRatingsAsync();
            var reviews = await _context.Reviews
                .Include(r => r.Student)
                .Where(r => r.TutorId == tutorId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByStudentIdAsync(int studentId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.Student)
                .Where(r => r.StudentId == studentId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto> GetReviewByIdAsync(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Student)
                .FirstOrDefaultAsync(r => r.Id == id);
            return _mapper.Map<ReviewDto>(review);
        }

        private void UpdateTutorRatingAndReviewsCount(Tutor tutor)
        {
            // Підрахунок тільки по реальних відгуках з бази для цього репетитора
            var allReviews = _context.Reviews.Where(r => r.TutorId == tutor.Id).ToList();
            tutor.TotalReviews = allReviews.Count;
            tutor.AverageRating = tutor.TotalReviews > 0 ? (decimal)allReviews.Average(r => r.Rating) : 0;
        }

        public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto createReviewDto, int studentId)
        {
            // Перевірка на дубль
            if (await HasStudentReviewedTutorAsync(studentId, createReviewDto.TutorId))
                throw new InvalidOperationException("Ви вже залишали відгук цьому репетитору.");

            var review = _mapper.Map<Review>(createReviewDto);
            review.StudentId = studentId;
            _context.Reviews.Add(review);

            // Оновлюємо середній рейтинг репетитора
            var tutor = await _context.Tutors
                .Include(t => t.Reviews)
                .FirstOrDefaultAsync(t => t.Id == createReviewDto.TutorId);

            if (tutor != null)
            {
                UpdateTutorRatingAndReviewsCount(tutor);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Логування помилки
                Console.WriteLine($"Помилка при збереженні відгуку: {ex.Message}");
                throw;
            }
            return await GetReviewByIdAsync(review.Id);
        }

        public async Task<ReviewDto> UpdateReviewAsync(int id, UpdateReviewDto updateReviewDto)
        {
            var review = await _context.Reviews
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null) return null;

            var oldRating = review.Rating;
            _mapper.Map(updateReviewDto, review);

            // Оновлюємо середній рейтинг репетитора
            var tutor = review.Tutor;
            if (tutor != null)
            {
                UpdateTutorRatingAndReviewsCount(tutor);
            }

            await _context.SaveChangesAsync();
            return await GetReviewByIdAsync(review.Id);
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review != null)
            {
                var tutor = review.Tutor;
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                if (tutor != null)
                {
                    UpdateTutorRatingAndReviewsCount(tutor);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> ReviewExistsAsync(int id)
        {
            return await _context.Reviews.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> HasStudentReviewedTutorAsync(int studentId, int tutorId)
        {
            return await _context.Reviews.AnyAsync(r => r.StudentId == studentId && r.TutorId == tutorId);
        }

        // Тимчасовий метод для повного перерахунку всіх рейтингів (використати один раз!)
        public async Task RecalculateAllTutorsRatingsAsync()
        {
            var tutors = await _context.Tutors.ToListAsync();
            foreach (var tutor in tutors)
            {
                var allReviews = _context.Reviews.Where(r => r.TutorId == tutor.Id).ToList();
                tutor.TotalReviews = allReviews.Count;
                tutor.AverageRating = tutor.TotalReviews > 0 ? (decimal)allReviews.Average(r => r.Rating) : 0;
            }
            await _context.SaveChangesAsync();
        }
    }
} 