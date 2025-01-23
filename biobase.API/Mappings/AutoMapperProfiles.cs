
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
            CreateMap<Taxa, TaxaDto>().ReverseMap();

            // TRAITS CATEGORIES
            CreateMap<TraitsCategories, TraitsCategoriesDto>().ReverseMap();

            // HABITAT CLASSES TAXA
            CreateMap<HabitatClassesTaxa, HabitatClassesTaxaDto>().ReverseMap();
        }
    }
}
