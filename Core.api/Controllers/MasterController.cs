using Core.api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        [HttpGet("getDropDwonModel")]
        public List<EmployeeDropModel> getDropDwonModel()
        {
            var list = (from emp in _context.EmployeeMaster
                        select new EmployeeDropModel
                        {
                            empId = emp.empId,
                            empName = emp.empName,
                        }).ToList();

            return list;
        }

        [HttpGet("searchEmployee")]
        public List<EmployeeMaster> getSearchEmployee(string searchText)
        {
            var filterData = _context.EmployeeMaster.Where(s => s.empName == searchText).ToList();
            return filterData;
        }

        [HttpGet("searchByNameEmployee")]
        public List<EmployeeMaster> searchByNameEmployee(string searchText)
        {
            var filterData = _context.EmployeeMaster.Where(s => s.empName.StartsWith(searchText)).ToList();
            var contextList = _context.EmployeeMaster.Where(t => t.empName.Contains(searchText)).ToList();
            return filterData;
        }

        [HttpGet("searchMemberEmployee")]
        public List<EmployeeMaster> searchMemberEmployee(string? name, string? email, string? contactNo)
        {
            var _memberList = (from emp in _context.EmployeeMaster
                               where emp.empName != ""
                               && (name == null || emp.empName.Contains(name))
                               && (email == null || emp.email.StartsWith(email))
                               && (contactNo == null || emp.contactNo.StartsWith(contactNo))
                               select emp
                               ).ToList();

            return _memberList;
        }

        [HttpPost("searchMemberEmployeeObj")]
        public List<EmployeeMaster> searchMemberEmployeeObj(SearchEmployeeModel obj)
        {
            var _memberList = (from emp in _context.EmployeeMaster
                               where emp.empName != ""
                               && (obj.name == null || emp.empName.Contains(obj.name))
                               && (obj.email == null || emp.email.StartsWith(obj.email))
                               && (obj.contactNo == null || emp.contactNo.StartsWith(obj.contactNo))
                               select emp
                               ).ToList();

            return _memberList;
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
