using System.Collections.Generic;
using System.Threading.Tasks;
using tutorfinder.Models.DTOs;

namespace tutorfinder.Services
{
    public interface ITutorService
    {
        Task<TutorDto> GetByIdAsync(int id);
        Task<IEnumerable<TutorDto>> GetAllAsync();
        Task<TutorDto> CreateAsync(CreateTutorDto createTutorDto);
        Task<TutorDto> UpdateAsync(int id, UpdateTutorDto updateTutorDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<TutorDto>> GetBySubjectIdAsync(int subjectId);
        Task<IEnumerable<TutorDto>> GetAvailableTutorsAsync();
    }
} 