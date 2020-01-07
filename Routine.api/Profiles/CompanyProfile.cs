using AutoMapper;
using Routine.api.DtoModel;
using Routine.api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.api.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(dst => dst.CompanyName, opt => opt.MapFrom(src => src.Name));

            CreateMap<CompanyAddDto, Company>();
        }
    }
}
