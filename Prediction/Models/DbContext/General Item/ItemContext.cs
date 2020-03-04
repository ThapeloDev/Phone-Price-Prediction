using Microsoft.EntityFrameworkCore;
using Prediction.Models.Hardware;

namespace Prediction.Models.Time_Series_Forecasting
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Prediction.Models.Hardware.PhoneProperties> PhoneProperties { get; set; }
    }
}
