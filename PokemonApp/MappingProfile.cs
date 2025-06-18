using AutoMapper;
using PokemonApp.Models;
using PokemonApp.Models.Dtos;

namespace PokemonApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CountryDto, Country>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<ReviewDto, Review>();
            CreateMap<ReviewerDto, Reviewer>();
            CreateMap<Country, CountryDto>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Reviewer, ReviewerDto>();


            CreateMap<PokemonUpdateDto, Pokemon>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<CountryUpdateDto, Country>();
            CreateMap<OwnerUpdateDto, Owner>();
            CreateMap<ReviewUpdateDto, Review>();
            CreateMap<ReviewerUpdateDto, Reviewer>();







        }
    }
}
