﻿using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Routine.api.DtoModel;
using Routine.api.DtoParameters;
using Routine.api.Entities;
using Routine.api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.api.Controllers
{
    [ApiController]
    [Route("v1/api/companies")]
    [EnableCors("any")]
    public class CompaniesController:ControllerBase
    {
        public ICompanyRespository _companyRespository { get; }
        public IMapper _mapper { get; }

        public CompaniesController(ICompanyRespository companyRespository,IMapper mapper)
        {
            _companyRespository = companyRespository?? throw new ArgumentNullException(nameof(companyRespository));
            _mapper = mapper??throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 根据条件获得公司集合
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery] CompanyDtoParameters parameters)
        {
            var companies = await _companyRespository.GetCompaniesAsync(parameters);
            var companyDtos= _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companyDtos);
        }

        /// <summary>
        /// 根据公司Id获得公司
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("{companyId}",Name =nameof(GetCompany))]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId)
        {
            var company = await _companyRespository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }
            var companyDto = _mapper.Map<CompanyDto>(company);
            return Ok(companyDto);
        }

        /// <summary>
        /// 创建公司信息
        /// </summary>
        /// <param name="companyDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyAddDto companyDto)
        {
            var company = _mapper.Map<Company>(companyDto);
            _companyRespository.AddCompany(company);
            await _companyRespository.SaveAsync();
            var returnDto = _mapper.Map<CompanyDto>(company);
            return CreatedAtRoute(nameof(GetCompany), new { companyId = company.Id }, returnDto);
        }

    }
}
