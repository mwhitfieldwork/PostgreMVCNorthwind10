using AutoMapper;
using NWCodeFirstMVC.Domain.PocoModels;
using NWCodeFirstMVC.Infrastructure.PgModels;

namespace NWCodeFirstMVC.Infrastructure.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductModel>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => (int)src.ProductId))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId.HasValue ? (int?)src.SupplierId : null))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId.HasValue ? (int?)src.CategoryId : null))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.HasValue ? (decimal?)src.UnitPrice : null))
                .ForMember(dest => dest.Discontinued, opt => opt.MapFrom(src => src.Discontinued == 1))
                ;

            CreateMap<ProductModel, Product>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => (short)src.ProductId))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId.HasValue ? (short?)src.SupplierId : null))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId.HasValue ? (short?)src.CategoryId : null))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.HasValue ? (float?)src.UnitPrice : null))
                .ForMember(dest => dest.Discontinued, opt => opt.MapFrom(src => src.Discontinued ? 1 : 0))
                ;

            // Map PgModels.User <-> PocoModels.User
            CreateMap<NWCodeFirstMVC.Infrastructure.PgModels.User, NWCodeFirstMVC.Domain.PocoModels.User>()
                .ForMember(dest => dest.PKID, opt => opt.MapFrom(src => (int)src.Pkid))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Passowrd, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.admin, opt => opt.MapFrom(src => src.Admin))
                .ForMember(dest => dest.firstName, opt => opt.MapFrom(src => src.Firstname))
                .ForMember(dest => dest.occupation, opt => opt.MapFrom(src => src.Occupation))
                ;

            CreateMap<NWCodeFirstMVC.Domain.PocoModels.User, NWCodeFirstMVC.Infrastructure.PgModels.User>()
                .ForMember(dest => dest.Pkid, opt => opt.MapFrom(src => (short)src.PKID))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Passowrd))
                .ForMember(dest => dest.Admin, opt => opt.MapFrom(src => src.admin))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.firstName))
                .ForMember(dest => dest.Occupation, opt => opt.MapFrom(src => src.occupation))
                ;
        }
    }
}
