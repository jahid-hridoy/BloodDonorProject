using AutoMapper;
using BloodDonorProject.Models;
using BloodDonorProject.Services.Implementations;
using BloodDonorProject.Utilities;

namespace BloodDonorProject.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BloodDonorCreateViewModel, BloodDonor>();

            CreateMap<BloodDonor, BloodDonorListViewModel>()
                .ForMember(dest => dest.BloodGroup, opt => opt.MapFrom(src => src.BloodGroup.ToString()))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateHelper.CalculateAge(src.DateOfBirth)))
                .ForMember(dest => dest.LastDonationDate, opt => opt.MapFrom(src => DateHelper.GetLastDonationDate(src.LastDonationDate)))
                .ForMember(dest => dest.IsEligibleForDonation, opt => opt.MapFrom(src => BloodDonorService.IsEligible(src)));

            CreateMap<BloodDonor, BloodDonorEditViewModel>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
                .ForMember(dest => dest.ExistingProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));

            CreateMap<BloodDonorEditViewModel, BloodDonor>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ExistingProfilePicture));

        }
    }
}
