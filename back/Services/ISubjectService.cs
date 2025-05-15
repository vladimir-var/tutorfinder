using tutorfinder.DTOs;

namespace tutorfinder.Services
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
        Task<SubjectDto> GetSubjectByIdAsync(int id);
        Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto createSubjectDto);
        Task<SubjectDto> UpdateSubjectAsync(int id, UpdateSubjectDto updateSubjectDto);
        Task DeleteSubjectAsync(int id);
        Task<bool> SubjectExistsAsync(int id);
        Task<bool> SubjectExistsByNameAsync(string name);
        Task<IEnumerable<SubjectDto>> SearchSubjectsAsync(string searchTerm);
    }
} 