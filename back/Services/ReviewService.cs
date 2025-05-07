using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tutorfinder.Models;
using tutorfinder.Models.DTOs;

namespace tutorfinder.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewDto> GetByIdAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return null;

            return MapToDto(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await _context.Reviews.ToListAsync();
            return reviews.Select(MapToDto);
        }

        public async Task<ReviewDto> CreateAsync(CreateReviewDto createReviewDto)
        {
            var review = new Review
            {
                TutorId = createReviewDto.TutorId,
                StudentId = createReviewDto.StudentId,
                Rating = createReviewDto.Rating,
                Comment = createReviewDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return MapToDto(review);
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ReviewDto>> GetByTutorIdAsync(int tutorId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.TutorId == tutorId)
                .ToListAsync();

            return reviews.Select(MapToDto);
        }

        public async Task<IEnumerable<ReviewDto>> GetByStudentIdAsync(int studentId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.StudentId == studentId)
                .ToListAsync();

            return reviews.Select(MapToDto);
        }

        private static ReviewDto MapToDto(Review review)
        {
            return new ReviewDto
            {
                Id = review.Id,
                TutorId = review.TutorId,
                StudentId = review.StudentId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
} 