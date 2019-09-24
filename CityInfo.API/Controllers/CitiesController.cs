using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            var cityEntities = _cityInfoRepository.GetCities();
            var results = _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult Getcity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if(city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = _mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterestResult = _mapper.Map<CityWithoutPointOfInterestDto>(city);
            return Ok(cityWithoutPointsOfInterestResult);
        }
    }
}
