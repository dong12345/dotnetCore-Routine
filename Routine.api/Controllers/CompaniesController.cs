using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.api.DtoModel;
using Routine.api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.api.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController:ControllerBase
    {
        public ICompanyRespository _companyRespository { get; }
        public IMapper _mapper { get; }

        public CompaniesController(ICompanyRespository companyRespository,IMapper mapper)
        {
            _companyRespository = companyRespository?? throw new ArgumentNullException(nameof(companyRespository));
            _mapper = mapper??throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            var companies = await _companyRespository.GetCompaniesAsync();
            var companyDtos= _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companyDtos);
        }

        [HttpGet("{companyId}")]
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

    }
}
