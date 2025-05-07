using System.Collections.Generic;
using System.Threading.Tasks;
using tutorfinder.Models.DTOs;

namespace tutorfinder.Services
{
    public interface ISubjectService
    {
        Task<SubjectDto> GetByIdAsync(int id);
        Task<IEnumerable<SubjectDto>> GetAllAsync();
        Task<SubjectDto> CreateAsync(CreateSubjectDto createSubjectDto);
        Task DeleteAsync(int id);
    }
} 