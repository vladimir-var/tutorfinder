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

        public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto createReviewDto)
        {
            var review = _mapper.Map<Review>(createReviewDto);
            _context.Reviews.Add(review);

            // Обновляем средний рейтинг репетитора
            var tutor = await _context.Tutors
                .Include(t => t.Reviews)
                .FirstOrDefaultAsync(t => t.Id == createReviewDto.TutorId);

            if (tutor != null)
            {
                tutor.TotalReviews++;
                tutor.AverageRating = (tutor.AverageRating * (tutor.TotalReviews - 1) + createReviewDto.Rating) / tutor.TotalReviews;
            }

            await _context.SaveChangesAsync();
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

            // Обновляем средний рейтинг репетитора
            if (updateReviewDto.Rating.HasValue && updateReviewDto.Rating.Value != oldRating)
            {
                var tutor = review.Tutor;
                tutor.AverageRating = (tutor.AverageRating * tutor.TotalReviews - oldRating + updateReviewDto.Rating.Value) / tutor.TotalReviews;
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
                // Обновляем средний рейтинг репетитора
                var tutor = review.Tutor;
                tutor.TotalReviews--;
                if (tutor.TotalReviews > 0)
                {
                    tutor.AverageRating = (tutor.AverageRating * (tutor.TotalReviews + 1) - review.Rating) / tutor.TotalReviews;
                }
                else
                {
                    tutor.AverageRating = 0;
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
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
    }
} 