using AutoMapper;
using Battleships.Models.Dtos;
using Battleships.Models.Entities.Example;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.WebApi.Extensions.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Example, ExampleDto>();
            CreateMap<ExampleDto, Example>();
        }
    }
}
