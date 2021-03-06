﻿using Microsoft.EntityFrameworkCore;
using Routine.api.Data;
using Routine.api.DtoParameters;
using Routine.api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.api.Services
{
    public class CompanyRespository : ICompanyRespository
    {
        private readonly RoutineDbContext _context;
        public CompanyRespository(RoutineDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            company.Id = Guid.NewGuid();
            if (company.Employees != null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
            }
            _context.Companies.Add(company);
        }

        public void AddEmployee(Guid companyId, Employee employee)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employee.CompanyId = companyId;
            _context.Employees.Add(employee);
        }

        public async Task<bool> CompanyExistAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Companies.AnyAsync(x => x.Id == companyId);
        }

        public void DeleteCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            _context.Companies.Remove(company);
        }

        public void DeleteEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            _context.Employees.Remove(employee);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (string.IsNullOrWhiteSpace(parameters.CompanyName) && string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                return await _context.Companies.ToListAsync();
            }

            var queryExpression = _context.Companies as IQueryable<Company>;
            if (!string.IsNullOrWhiteSpace(parameters.CompanyName))
            {
                queryExpression=queryExpression.Where(x => x.Name.Contains(parameters.CompanyName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                queryExpression=queryExpression.Where(x => x.Name.Contains(parameters.SearchTerm.Trim()) ||
                                           x.Introduction.Contains(parameters.SearchTerm.Trim()));
            }
            return await queryExpression.ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }
            return await _context.Companies
                .Where(x => companyIds.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToListAsync();

        }

        public async Task<Company> GetCompanyAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            return await _context.Employees.Where(x => x.Id == employeeId && x.CompanyId == companyId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, string genderDisplay)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (string.IsNullOrWhiteSpace(genderDisplay))
            {
                return await _context.Employees.Where(x => x.CompanyId == companyId)
               .ToListAsync();
            }

            var genderTrim = genderDisplay.Trim();
            var gender = Enum.Parse<Gender>(genderTrim);
            return await _context.Employees.Where(x => x.CompanyId == companyId && x.Gender == gender)
                .ToListAsync();


        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) >= 0;
        }

        public void UpdateCompany(Company company)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
