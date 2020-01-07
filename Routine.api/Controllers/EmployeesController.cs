using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.api.DtoModel;
using Routine.api.Entities;
using Routine.api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRespository _companyRespository;

        public EmployeesController(IMapper mapper,ICompanyRespository companyRespository)
        {
            this._mapper = mapper??throw new ArgumentNullException(nameof(mapper));
            this._companyRespository = companyRespository ?? throw new ArgumentNullException(nameof(companyRespository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId,[FromQuery]string genderDisplay)
        {
            if (!await _companyRespository.CompanyExistAsync(companyId))
            {
                return NotFound();
            }
            var employees = await _companyRespository.GetEmployeesAsync(companyId,genderDisplay);
            if (employees == null)
            {
                return NotFound();
            }
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeesDto);
        }

        [Route("{employeeId}",Name =nameof(GetEmployee))]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid companyId, Guid employeeId)
        {
            if (!await _companyRespository.CompanyExistAsync(companyId))
            {
                return NotFound();
            }
            var employee= await _companyRespository.GetEmployeeAsync(companyId, employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId,EmployeeAddDto employeeAddDto)
        {
            var employee = _mapper.Map<Employee>(employeeAddDto);
            _companyRespository.AddEmployee(companyId, employee);
            await _companyRespository.SaveAsync();

            var dtoReturn = _mapper.Map<EmployeeDto>(employee);
            return CreatedAtRoute(nameof(GetEmployee), new { companyId = companyId, employeeId = employee.Id }, dtoReturn);
        }

    }
}
