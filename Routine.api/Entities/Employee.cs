using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.api.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }
        [Required]
        public string EmployeeNo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Company Company { get; set; }
    }
}
