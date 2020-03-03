using Microsoft.EntityFrameworkCore;

namespace Prediction.Models.Time_Series_Forecasting
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
    }
}
