using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.api.Entities
{
    public class Company
    {
        public Guid Id { get; set;}
        [Required,MaxLength(100)]
        public string Name { get; set; }

        public string Introduction { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
