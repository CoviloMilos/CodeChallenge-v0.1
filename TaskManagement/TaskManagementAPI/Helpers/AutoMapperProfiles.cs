using AutoMapper;
using TaskManagementAPI.Dtos;

namespace TaskManagementAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<DtoCreateCase, Models.Case>()
                .ForMember(dest => dest.CaseId, opt => opt.Ignore())
                .ForMember(dest => dest.CaseNum, opt => opt.Ignore())
                .ForMember(dest => dest.TaskGuid, opt => opt.Ignore());

            CreateMap<DtoCreateTask, Models.Task>()
                .ForMember(dest => dest.TaskId, opt => opt.Ignore())
                .ForMember(dest => dest.TaskNum, opt => opt.Ignore())
                .ForMember(dest => dest.TaskGuid, opt => opt.Ignore())
                .ForMember(dest => dest.Cases, opt => opt.MapFrom(src => src.CreateCases));   

            CreateMap<DtoUpdateTask, Models.Task>()
                .ForMember(dest => dest.TaskId, opt => opt.Ignore())
                .ForMember(dest => dest.TaskNum, opt => opt.Ignore())
                .ForMember(dest => dest.TaskGuid, opt => opt.Ignore());

            CreateMap<DtoUpdateCase, Models.Case>()
                .ForMember(dest => dest.CaseId, opt => opt.Ignore())
                .ForMember(dest => dest.CaseNum, opt => opt.Ignore())
                .ForMember(dest => dest.TaskGuid, opt => opt.Ignore());
                
            CreateMap<Models.Case, DtoGetCase>();

            CreateMap<Models.Task, DtoGetTask>();
        }
    }
}