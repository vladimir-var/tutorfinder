using tutorfinder.DTOs;

namespace tutorfinder.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<IEnumerable<ReviewDto>> GetReviewsByTutorIdAsync(int tutorId);
        Task<IEnumerable<ReviewDto>> GetReviewsByStudentIdAsync(int studentId);
        Task<ReviewDto> GetReviewByIdAsync(int id);
        Task<ReviewDto> CreateReviewAsync(CreateReviewDto createReviewDto);
        Task<ReviewDto> UpdateReviewAsync(int id, UpdateReviewDto updateReviewDto);
        Task DeleteReviewAsync(int id);
        Task<bool> ReviewExistsAsync(int id);
        Task<bool> HasStudentReviewedTutorAsync(int studentId, int tutorId);
    }
} 