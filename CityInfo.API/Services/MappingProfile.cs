using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.City, Models.CityWithoutPointOfInterestDto>().ReverseMap();
            CreateMap<Entities.City, Models.CityDto>().ReverseMap();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>().ReverseMap();
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>().ReverseMap();
            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>().ReverseMap();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>().ReverseMap();
        }
    }
}
