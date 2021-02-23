using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
     
        public bool StatusOnline { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }





      
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public int? Age { get; set; }
        public string Phone { get; set; }
     
        public int SexId { get; set; }
        public int WorkTime { get; set; }

      //  [Display(Name = "Password")]


    
        public virtual WorkTime WorkTimeNavigation { get; set; }
        public virtual ICollection<EmployeeTask> Tasks { get; set; }




    }
}
