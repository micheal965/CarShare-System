using AutoMapper;
using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Shared.DTOs.Identity;
using Youth_Innovation_System.Shared.DTOs.Post;

namespace Youth_Innovation_System.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserToReturnDto>();

            CreateMap<UpdateUserDto, ApplicationUser>();

            CreateMap<CarPost, PostResponseDto>()
                .ForMember(dest => dest.ImageUrls,
                        opt => opt.MapFrom(src => src.postImages.Select(pi => pi.imageUrl)))
                       .ForMember(dest => dest.Feedbacks,
                       opt => opt.MapFrom(src => src.CarFeedbacks));


            CreateMap<UpdatePostDto, CarPost>();
            CreateMap<CreatePostDto, CarPost>();
        }
    }
}
