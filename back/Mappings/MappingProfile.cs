using AutoMapper;
using tutorfinder.DTOs;
using tutorfinder.Models;

namespace tutorfinder.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User маппинги
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Пароль будет хешироваться отдельно
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Tutor маппинги
            CreateMap<Tutor, TutorDto>()
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.TutorSubjects.Select(ts => ts.Subject)))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Reviews, opt => opt.Ignore());
            CreateMap<CreateTutorDto, Tutor>();
            CreateMap<UpdateTutorDto, Tutor>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Subject маппинги
            CreateMap<Subject, SubjectDto>();
            CreateMap<CreateSubjectDto, Subject>();
            CreateMap<UpdateSubjectDto, Subject>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Review маппинги
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Tutor, opt => opt.MapFrom(src => src.Tutor));
            CreateMap<CreateReviewDto, Review>()
                .ForMember(dest => dest.StudentId, opt => opt.Ignore());
            CreateMap<UpdateReviewDto, Review>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
} 