using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Core.api.Models
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<EmployeeMaster> EmployeeMaster { get; set; }
        public DbSet<EmployeeAddress> EmployeeAddress { get; set; }
        public DbSet<EmployeeCount> EmployeeCount { get; set; }
        public DbSet<SearchEmployee> SearchEmployee { get; set; }

        public IQueryable<EmployeeCount> getEmployeeCount()
        {
            return this.EmployeeCount.FromSqlRaw("execute GetEmployeeCount");
        }

        public IQueryable<SearchEmployee> searchEmployeeByName(string searchText)
        {
            SqlParameter str = new SqlParameter("@searchString", searchText);
            return this.SearchEmployee.FromSqlRaw("execute SearchEmployee @searchString", str);
        }
    }
}
