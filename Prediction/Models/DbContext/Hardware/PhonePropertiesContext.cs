using Microsoft.EntityFrameworkCore;

namespace Prediction.Models.Hardware
{
    public class PhonePropertiesContext : DbContext
    {
        public PhonePropertiesContext(DbContextOptions<PhonePropertiesContext> options) : base(options)
        {

        }

        public DbSet<PhoneProperties> PhoneProperties { get; set; }
    }
}
