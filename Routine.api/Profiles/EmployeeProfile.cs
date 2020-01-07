using AutoMapper;
using Routine.api.DtoModel;
using Routine.api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.api.Profiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dst=>dst.Age,opt=>opt.MapFrom(source=>DateTime.Now.Year-source.DateOfBirth.Year))
                .ForMember(dst=>dst.GenderDisplay,opt=>opt.MapFrom(source=>source.Gender.ToString()))
                .ForMember(dst=>dst.Name,opt=>opt.MapFrom(source=>($"{source.FirstName}{source.LastName}")));

            CreateMap<EmployeeAddDto, Employee>();
        }
    }
}
