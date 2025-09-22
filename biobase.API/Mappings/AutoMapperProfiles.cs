
using AutoMapper;
using biobase.API.Models.Domain;
using biobase.API.Models.DTO;
using System.Xml.Linq;

namespace biobase.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // TAXA MAPPING
            CreateMap<Taxa, TaxaDto>()
                .ForMember(dest => dest.TaxonId, opt => opt.MapFrom(src => src.soortnummer))
                .ForMember(dest => dest.TaxaGroup, opt => opt.MapFrom(src => src.groep))
                .ForMember(dest => dest.ScientificName, opt => opt.MapFrom(src => src.wetnaam))
                .ForMember(dest => dest.VernacularName, opt => opt.MapFrom(src => src.nednaam))
                .ForMember(dest => dest.Family, opt => opt.MapFrom(src => src.familie))
                .ForMember(dest => dest.TaxonLabel, opt => opt.MapFrom(src => src.label))
                .ForMember(dest => dest.NdffIdentity, opt => opt.MapFrom(src => src.identity))
                .ForMember(dest => dest.ThreatStatus, opt => opt.MapFrom(src => src.rl))
                .ForMember(dest => dest.OccurenceStatus, opt => opt.MapFrom(src => src.zzz))
                .ForMember(dest => dest.TaxaSubgroup, opt => opt.MapFrom(src => src.deelgroep))
                .ForMember(dest => dest.IsAcceptedName, opt => opt.MapFrom(src => src.acc))
                .ForMember(dest => dest.ScientificNameAuthorship, opt => opt.MapFrom(src => src.auteur))
                .ForMember(dest => dest.Updated, opt => opt.MapFrom(src => src.updated));


            // TRAITS CATEGORIES
            CreateMap<TraitsCategories, TraitsCategoriesDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.rubriekid))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.rubriek))
                .ForMember(dest => dest.TaxaGroup, opt => opt.MapFrom(src =>src.soortgroep))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.bron));


            // HABITAT CLASSES TAXA
            CreateMap<HabitatClassesTaxa, HabitatClassesTaxaDto>()
                .ForMember(dest => dest.ScientificName, opt => opt.MapFrom(src => src.wetnaam))
                .ForMember(dest => dest.VernacularName, opt => opt.MapFrom(src => src.nednaam))
                .ForMember(dest => dest.ThreatStatus, opt => opt.MapFrom(src => src.rl))
                .ForMember(dest => dest.TaxaGroup, opt => opt.MapFrom(src => src.soortgroep));

            CreateMap<HabitatCodes, HabitatCodesDto>()
                .ForMember(dest => dest.HabitatCode, opt => opt.MapFrom(src => src.habitat_code))
                .ForMember(dest => dest.HabitatClassification, opt => opt.MapFrom(source => source.habitat_classification))
                .ForMember(dest => dest.HabitatDescription, opt => opt.MapFrom(source => source.habitat_description));

            CreateMap<TaxaGroups, TaxaGroupsDto>();
        }
    }
}
