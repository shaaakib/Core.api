using Microsoft.EntityFrameworkCore;

namespace Core.api.Models
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<EmployeeMaster> EmployeeMaster { get; set; }
    }
}
