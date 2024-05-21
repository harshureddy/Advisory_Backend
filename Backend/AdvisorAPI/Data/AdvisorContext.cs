using AdvisorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvisorAPI.Data
{
    public class AdvisorContext : DbContext
    {
        public AdvisorContext(DbContextOptions<AdvisorContext> options) : base(options) { }

        public DbSet<Advisor> Advisors { get; set; }
    }
}
