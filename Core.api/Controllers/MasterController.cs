using Core.api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        public MasterController( EmployeeDbContext empDbContext)
        {
            this._context = empDbContext;
        }

        [HttpGet("getAllEmployees")]
        public List<EmployeeMaster> getAllEmployees()
        {
            var list = _context.EmployeeMaster.ToList();
            return list;
        }

        [HttpGet("getEmployeeById")]
        public EmployeeMaster getEmployeeById(int id)
        {
            var singleRecored = _context.EmployeeMaster.SingleOrDefault(e => e.empId == id);
            return singleRecored;
        }

        [HttpGet("getEmployeeByCity")]
        public EmployeeMaster getEmployeeByCity(string city)
        {
            //var singleRecored = _context.EmployeeMaster.SingleOrDefault(e => e.empId == id);
            var firstRecord =  _context.EmployeeMaster.FirstOrDefault(e => e.city == city);
            return firstRecord;
        }

        [HttpPost("SaveEmployee")]
        public EmployeeMaster SaveEmployee( EmployeeMaster obj)
        {
            _context.EmployeeMaster.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        [HttpPut("UpdateEmployee")]
        public EmployeeMaster UpdateEmployee(EmployeeMaster obj, int id)
        {
            var record = _context.EmployeeMaster.SingleOrDefault(e => e.empId == id);
            record.empName = obj.empName;
            record.email = obj.email;
            record.contactNo = obj.contactNo;
            record.city = obj.city;

            _context.SaveChanges();

            return record;
        }

        [HttpDelete("DeleteEmployeeById")]
        public bool DeleteEmployeeById(int id)
        {
            try
            {
                var data = _context.EmployeeMaster.SingleOrDefault(e => e.empId == id);
                _context.Remove(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }
    }
}
