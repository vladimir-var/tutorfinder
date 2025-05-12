using tutorfinder.DTOs;

namespace tutorfinder.Services
{
    public interface ITutorService
    {
        Task<IEnumerable<TutorDto>> GetAllTutorsAsync();
        Task<TutorDto> GetTutorByIdAsync(int id);
        Task<TutorDto> GetTutorByUserIdAsync(int userId);
        Task<IEnumerable<TutorDto>> GetTutorsBySubjectAsync(int subjectId);
        Task<IEnumerable<TutorDto>> SearchTutorsAsync(string searchTerm);
        Task<TutorDto> CreateTutorAsync(CreateTutorDto createTutorDto);
        Task<TutorDto> UpdateTutorAsync(int id, UpdateTutorDto updateTutorDto);
        Task DeleteTutorAsync(int id);
        Task<bool> TutorExistsAsync(int id);
        Task<bool> TutorExistsByUserIdAsync(int userId);
    }
} 