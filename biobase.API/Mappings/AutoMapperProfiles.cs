
using AutoMapper;
using biobase.API.Models.Domain;
using biobase.API.Models.DTO;

namespace biobase.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // TAXA MAPPING
            CreateMap<Taxa, TaxaDto>()
                .ForMember(dest => dest.species_id, opt => opt.MapFrom(src => src.soortnummer))
                .ForMember(dest => dest.taxon_class, opt => opt.MapFrom(src => src.groep))
                .ForMember(dest => dest.sci_name, opt => opt.MapFrom(src => src.wetnaam))
                .ForMember(dest => dest.nl_name, opt => opt.MapFrom(src => src.nednaam))
                .ForMember(dest => dest.family, opt => opt.MapFrom(src => src.familie))
                .ForMember(dest => dest.red_list_status, opt => opt.MapFrom(src => src.rl))
                .ForMember(dest => dest.rareness, opt => opt.MapFrom(src => src.zzz))
                .ForMember(dest => dest.taxon_subclass, opt => opt.MapFrom(src => src.deelgroep))
                .ForMember(dest => dest.author, opt => opt.MapFrom(src => src.auteur));


            // TRAITS CATEGORIES
            CreateMap<TraitsCategories, TraitsCategoriesDto>()
                .ForMember(dest => dest.category_id, opt => opt.MapFrom(src => src.rubriekid))
                .ForMember(dest => dest.category_name, opt => opt.MapFrom(src => src.rubriek))
                .ForMember(dest => dest.taxon_class, opt => opt.MapFrom(src =>src.soortgroep))
                .ForMember(dest => dest.source, opt => opt.MapFrom(src => src.bron));


            // HABITAT CLASSES TAXA
            CreateMap<HabitatClassesTaxa, HabitatClassesTaxaDto>()
                .ForMember(dest => dest.sci_name, opt => opt.MapFrom(src => src.wetnaam))
                .ForMember(dest => dest.nl_name, opt => opt.MapFrom(src => src.nednaam))
                .ForMember(dest => dest.red_list_status, opt => opt.MapFrom(src => src.rl))
                .ForMember(dest => dest.taxon_class, opt => opt.MapFrom(src => src.soortgroep));
        }
    }
}
