using AutoMapper;
using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Domain.PocoModels;
using DomainCategory = NWCodeFirstMVC.Domain.PocoModels.Category;
using PgCategory = NWCodeFirstMVC.Infrastructure.PgModels.Category;


namespace NWCodeFirstMVC.Api.Configurations
{
    public class MapperConfig: Profile
    {
        public MapperConfig()
        {
            CreateMap<ProductModel, ProductDto>().ReverseMap();
            CreateMap<ProductModel, GetProductDto>().ReverseMap();
            CreateMap<ProductModel, UpdateProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryDto>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<SalesByCategory, GetSalesDto>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, AdminUserDto>().ReverseMap();

            // Map Postgres entity directly to Poco User to ensure mapping is available
            CreateMap<NWCodeFirstMVC.Infrastructure.PgModels.User, NWCodeFirstMVC.Domain.PocoModels.User>()
                .ForMember(dest => dest.PKID, opt => opt.MapFrom(src => (int)src.Pkid))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Passowrd, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.admin, opt => opt.MapFrom(src => src.Admin))
                .ForMember(dest => dest.firstName, opt => opt.MapFrom(src => src.Firstname))
                .ForMember(dest => dest.occupation, opt => opt.MapFrom(src => src.Occupation))
                ;
            CreateMap<PgCategory, DomainCategory>().ReverseMap();


        }
    }
}
