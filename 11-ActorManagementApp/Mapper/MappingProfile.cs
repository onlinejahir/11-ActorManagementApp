using _11_ActorManagementApp.ViewModels.ActorVM;
using _11_ActorManagementApp.ViewModels.BiographyVM;
using ActorManagement.Models.EntityModels;
using AutoMapper;

namespace _11_ActorManagementApp.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ActorCreateVM, Actor>().ReverseMap();
            CreateMap<ActorEditVM, Actor>().ReverseMap();
            CreateMap<ActorDetailsVM, Actor>().ReverseMap();
            CreateMap<ActorIndexVM, Actor>().ReverseMap();
            CreateMap<BiographyAddVM, Biography>().ReverseMap();
            CreateMap<BiographyEditVM, Biography>()
                .ForMember(dest => dest.Actor, opt => opt.Ignore()) // Ignore clearing Actor
                .ForMember(dest => dest.BiographyImages, opt => opt.Ignore()).ReverseMap(); //Ignore BiographyImages
        }
    }
}
