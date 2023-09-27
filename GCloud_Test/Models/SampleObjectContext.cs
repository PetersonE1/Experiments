using Microsoft.EntityFrameworkCore;

namespace GCloud_Test.Models
{
    public class SampleObjectContext : DbContext
    {
        public DbSet<SampleObject> SampleObjects { get; set; }

        public SampleObjectContext(DbContextOptions<SampleObjectContext> options) : base(options)
        {

        }
    }
}
