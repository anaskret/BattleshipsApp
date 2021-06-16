using AutoMapper;
using Battleships.DataAccess.Migrations;
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
            CreateMap<DataAccess.Migrations.Example, ExampleDto>();
            CreateMap<ExampleDto, DataAccess.Migrations.Example>();
        }
    }
}
