using Microsoft.EntityFrameworkCore;

namespace GCloud_Test.Models
{
    public class OtherObjectContext : DbContext
    {
        public DbSet<OtherObject> OtherObjects { get; set; }

        public OtherObjectContext(DbContextOptions<OtherObjectContext> options) : base(options)
        {

        }
    }
}
