using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Hardware
{
    public class PhonePropertiesContext: DbContext
    {
        public PhonePropertiesContext(DbContextOptions<PhonePropertiesContext> options) : base(options)
        {

        }

        public DbSet<PhoneProperties> PhoneProperties { get; set; }
    }
}
