using AutoMapper;
using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.DTOs.Identity;
using Youth_Innovation_System.Shared.DTOs.Identity;
using Youth_Innovation_System.Shared.DTOs.Post;
using Youth_Innovation_System.Shared.DTOs.Rental;

namespace Youth_Innovation_System.Helpers
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<ApplicationUser, LoginResponseDto>()
                .ForMember(dest => dest.profilePicture, opt => opt.MapFrom(src => src.pictureUrl));

            CreateMap<ApplicationUser, UserToReturnDto>();

            CreateMap<UpdateUserDto, ApplicationUser>();

            CreateMap<CarPost, PostResponseDto>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.postImages.Select(pi => pi.imageUrl)))
                .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.CarFeedbacks))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => EncryptionHelper.Decrypt(src.Description)));

            CreateMap<CarFeedback, CarFeedbackDto>();

            CreateMap<UpdatePostDto, CarPost>()
          .ForMember(dest => dest.Description, opt => opt.MapFrom((src) =>
              EncryptionHelper.Encrypt(src.Description)));

            CreateMap<CreatePostDto, CarPost>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) =>
              EncryptionHelper.Encrypt(src.Description)));



            CreateMap<ApplicationUser, AccountResponseDto>()
                .ForMember(dest => dest.profilePicture,
                opt => opt.MapFrom(src => src.pictureUrl));

            CreateMap<RentalApplication, RentalApplicationResponseDto>();

        }
    }
}
