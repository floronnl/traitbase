
using AutoMapper;
using biobase.API.Models.Domain;
using biobase.API.Models.DTO;

namespace biobase.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // HABITAT CLASSES
            CreateMap<HabitatClasses, HabitatClassesDto>()
                .ForMember(dest => dest.HabitatCode, opt => opt.MapFrom(src => src.habitat_code))
                .ForMember(dest => dest.HabitatClassification, opt => opt.MapFrom(src => src.habitat_classification))
                .ForMember(dest => dest.HabitatDescription, opt => opt.MapFrom(src => src.habitat_description));

            // EU HABITAT CLASSES
            CreateMap<EuHabitatClasses, EuHabitatClassesDto>()
                .ForMember(dest => dest.HabitatCode, opt => opt.MapFrom(src => src.habitat_code))
                .ForMember(dest => dest.EuHabitatCode, opt => opt.MapFrom(src => src.eu_habitat_code))
                .ForMember(dest => dest.HabitatDescription, opt => opt.MapFrom(src => src.habitat_description))
                .ForMember(dest => dest.HabitatDescriptionFull, opt => opt.MapFrom(src => src.habitat_description_full))
                .ForMember(dest => dest.HabitatDescriptionEn, opt => opt.MapFrom(src => src.habitat_description_en))
                .ForMember(dest => dest.AnnexIPriority, opt => opt.MapFrom(src => src.annex_i_priority));

            // HABITAT CLASSES TAXA
            CreateMap<HabitatTaxa, HabitatTaxaDto>()
                .ForMember(dest => dest.HabitatCode, opt => opt.MapFrom(src => src.habitat_code))
                .ForMember(dest => dest.HabitatClassification, opt => opt.MapFrom(src => src.habitat_classification))
                .ForMember(dest => dest.HabitatDescription, opt => opt.MapFrom(src => src.habitat_description))
                .ForMember(dest => dest.ScientificName, opt => opt.MapFrom(src => src.wetnaam))
                .ForMember(dest => dest.VernacularName, opt => opt.MapFrom(src => src.nednaam))
                .ForMember(dest => dest.ThreatStatus, opt => opt.MapFrom(src => src.rl))
                .ForMember(dest => dest.TaxaGroup, opt => opt.MapFrom(src => src.soortgroep))
                .ForMember(dest => dest.TaxonCategory, opt => opt.MapFrom(src => src.taxon_category))
                .ForMember(dest => dest.ExtraInfo, opt => opt.MapFrom(src => src.extra_info))
                .ForMember(dest => dest.NdffIdentity, opt => opt.MapFrom(src => src.identity));

            // EU HABITAT CLASSES TAXA
            CreateMap<EuHabitatTaxa, EuHabitatTaxaDto>()
                .ForMember(dest => dest.HabitatCode, opt => opt.MapFrom(src => src.habitat_code))
                .ForMember(dest => dest.VernacularName, opt => opt.MapFrom(src => src.nednaam))
                .ForMember(dest => dest.ScientificName, opt => opt.MapFrom(src => src.wetnaam))
                .ForMember(dest => dest.TaxaGroup, opt => opt.MapFrom(src => src.taxa_group))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.category))
                .ForMember(dest => dest.NdffIdentity, opt => opt.MapFrom(src => src.identity));
                
            // TAXA GROUPS
            CreateMap<TaxaGroups, TaxaGroupsDto>()
                .ForMember(dest => dest.TaxaGroup, opt => opt.MapFrom(src => src.taxa_group))
                .ForMember(dest => dest.TaxaGroupDescription, opt => opt.MapFrom(src => src.taxa_group_description));

            // TAXA 
            CreateMap<Taxa, TaxaDto>()
                .ForMember(dest => dest.TaxonId, opt => opt.MapFrom(src => src.soortnummer))
                .ForMember(dest => dest.TaxaGroup, opt => opt.MapFrom(src => src.groep))
                .ForMember(dest => dest.ScientificName, opt => opt.MapFrom(src => src.wetnaam))
                .ForMember(dest => dest.VernacularName, opt => opt.MapFrom(src => src.nednaam))
                .ForMember(dest => dest.Family, opt => opt.MapFrom(src => src.familie))
                .ForMember(dest => dest.TaxonLabel, opt => opt.MapFrom(src => src.label))
                .ForMember(dest => dest.NdffIdentity, opt => opt.MapFrom(src => src.identity))
                .ForMember(dest => dest.HabitatDirective, opt => opt.MapFrom(src => src.habitatrichtlijn))
                .ForMember(dest => dest.ThreatStatus, opt => opt.MapFrom(src => src.rl))
                .ForMember(dest => dest.OccurenceStatus, opt => opt.MapFrom(src => src.zzz))
                .ForMember(dest => dest.TaxaSubgroup, opt => opt.MapFrom(src => src.deelgroep))
                .ForMember(dest => dest.IsAcceptedName, opt => opt.MapFrom(src => src.acc))
                .ForMember(dest => dest.ScientificNameAuthorship, opt => opt.MapFrom(src => src.auteur))
                .ForMember(dest => dest.Updated, opt => opt.MapFrom(src => src.updated));

            // EU Taxa
            CreateMap<EuTaxa, EuTaxaDto>()
                .ForMember(dest => dest.EuSpeciesCode, opt => opt.MapFrom(src => src.speciescode))
                .ForMember(dest => dest.TaxonId, opt => opt.MapFrom(src => src.taxon_id))
                .ForMember(dest => dest.VernacularName, opt => opt.MapFrom(src => src.nednaam))
                .ForMember(dest => dest.ScientificName, opt => opt.MapFrom(src => src.wetnaam))
                .ForMember(dest => dest.ScientificNameArt17, opt => opt.MapFrom(src => src.name_art17))
                .ForMember(dest => dest.ScientificNameHdBd, opt => opt.MapFrom(src => src.name_hd_bd))
                .ForMember(dest => dest.Directive, opt => opt.MapFrom(src => src.directive))
                .ForMember(dest => dest.AnnexII, opt => opt.MapFrom(src => src.annex_ii))
                .ForMember(dest => dest.AnnexIV, opt => opt.MapFrom(src => src.annex_iv))
                .ForMember(dest => dest.AnnexV, opt => opt.MapFrom(src => src.annex_v))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.priority))
                .ForMember(dest => dest.TaxaGroup, opt => opt.MapFrom(src => src.taxa_group))
                .ForMember(dest => dest.Occurrence, opt => opt.MapFrom(source => source.occurrence))
                .ForMember(dest => dest.Season, opt => opt.MapFrom(source => source.season))
                .ForMember(dest => dest.NdffIdentity, opt => opt.MapFrom(source => source.ndff_uri));


            // TRAITS CATEGORIES
            CreateMap<TraitsCategories, TraitsCategoriesDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.rubriekid))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.rubriek))
                .ForMember(dest => dest.TaxaGroup, opt => opt.MapFrom(src => src.soortgroep))
                .ForMember(dest => dest.DefaultCopyright, opt => opt.MapFrom(src => src.default_copyright))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.bron));
            
        }
    }
}
