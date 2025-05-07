using System.Collections.Generic;
using System.Threading.Tasks;
using tutorfinder.Models.DTOs;

namespace tutorfinder.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> GetByIdAsync(int id);
        Task<IEnumerable<ReviewDto>> GetAllAsync();
        Task<ReviewDto> CreateAsync(CreateReviewDto createReviewDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<ReviewDto>> GetByTutorIdAsync(int tutorId);
        Task<IEnumerable<ReviewDto>> GetByStudentIdAsync(int studentId);
    }
} 