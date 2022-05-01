using AutoMapper;
using Data.Entities;
using ViewModel.Catalog.Admin;
using ViewModel.Catalog.Company;
using ViewModel.Catalog.User;
using ViewModel.System.Users;

namespace Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<AppUser, AccountViewModel>();
            CreateMap<AccountViewModel, AppUser>();

            CreateMap<AppUser, CompanyAccountViewModel>();
            CreateMap<CompanyAccountViewModel, AppUser>();

            CreateMap<UserInformation, UserInformationViewModel>();
            CreateMap<UserInformationViewModel, UserInformation>();

            CreateMap<UserAvatar, UserAvatarViewModel>();
            CreateMap<UserAvatarViewModel, UserAvatar>();

            // Company Information
            CreateMap<CompanyInformation, CompanyInformationViewModel>().ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.UserId));
            CreateMap<CompanyInformationViewModel, CompanyInformation>();



            //Company Images
            CreateMap<CompanyImage, CompanyImagesViewModel>();
            CreateMap<CompanyImagesViewModel, CompanyImage>();

            //Company Avatar
            CreateMap<CompanyAvatar, CompanyAvatarViewModel>();
            CreateMap<CompanyAvatarViewModel, CompanyAvatar>();

            // Company Cover Image
            CreateMap<CompanyCoverImage, CompanyCoverImageViewModel>();
            CreateMap<CompanyCoverImageViewModel, CompanyCoverImage>();
            CreateMap<CompanyCoverImage, CreateCoverImageRequest>();
            CreateMap<CreateCoverImageRequest, CompanyCoverImage>();



            // Company Branch
            CreateMap<CompanyBranch, CompanyBranchViewModel>();
            CreateMap<CompanyBranchViewModel, CompanyBranch>();
            CreateMap<Branch, BranchViewModel>();
            CreateMap<BranchViewModel, Branch>();

            // company carreer
            CreateMap<Career, CareerViewModel>();
            CreateMap<CareerViewModel, Career>();

            //recruitment
            CreateMap<Recruitment, RecruitmentViewModel>();
            CreateMap<RecruitmentViewModel, Recruitment>();
            CreateMap<Recruitment, ListCompanyRecruitment>();
            CreateMap<ListCompanyRecruitment, Recruitment>();

            //chat 
            CreateMap<Chat, ListChatContent>();
            CreateMap<ListChatContent, Chat>();
        }
    }
}
