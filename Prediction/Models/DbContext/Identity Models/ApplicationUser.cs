using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() 
        {

        }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        public string City { get; set; }

        [EnumDataType(typeof(Country))]
        public Country Country { get; set; }
    }
}
