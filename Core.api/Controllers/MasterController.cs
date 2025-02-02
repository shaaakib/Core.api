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
        public commonResponseModel getAllEmployees()
        {
            commonResponseModel _res = new commonResponseModel();
            try
            {
                var list = _context.EmployeeMaster.ToList();
                _res.result = true;
                _res.data = list;
                return _res;
            }
            catch (Exception exp)
            {
                _res.result = false;
                _res.message = exp.Message;
                return _res;
            }


        }

        [HttpGet("getDropDwonModel")]
        public IActionResult getDropDwonModel()
        {
            try
            {
                var list = (from emp in _context.EmployeeMaster
                            select new EmployeeDropModel
                            {
                                empId = emp.empId,
                                empName = emp.empName,
                            }).ToList();
               return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
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
        public IActionResult getEmployeeById(int id)
        {
            EmployeeAddressViewModel _obj = new();
            var singleRecored = _context.EmployeeMaster.SingleOrDefault(e => e.empId == id);
            _obj.empName = singleRecored.empName;
            _obj.contactNo = singleRecored.contactNo;

            var addressList = _context.EmployeeAddress.Where(a => a.employeeId == id).ToList();
            _obj.AddressList = addressList;
            if(singleRecored == null)
            {
                return NotFound("No record found with Previded Id");
            }
            else
            {
                return Ok(_obj);
            }
            
        }

        //[HttpGet("getEmployeeByCity")]
        //public EmployeeMaster getEmployeeByCity(string city)
        //{
        //    //var singleRecored = _context.EmployeeMaster.SingleOrDefault(e => e.empId == id);
        //    var firstRecord =  _context.EmployeeMaster.FirstOrDefault(e => e.city == city);
        //    return firstRecord;
        //}

        //[HttpPost("SaveEmployee")]
        //public commonResponseModel SaveEmployee( EmployeeMaster obj)
        //{
        //    commonResponseModel _res = new commonResponseModel();
        //    try
        //    {
        //        _context.EmployeeMaster.Add(obj);
        //        _context.SaveChanges();
        //        _res.result = true;
        //        _res.message = "Employee creation successful";
        //        _res.data = obj;
        //        return _res;
        //    }
        //    catch (Exception ex)
        //    {

        //        _res.result = false;
        //        _res.message =ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //         return _res;
        //    }

        //}

        [HttpPost("CreateEmployeeWithAddress")]
        public IActionResult CreateEmployeeWithAddress(EmployeeAddressViewModel obj)
        {
            EmployeeMaster _emp = new()
            {
                contactNo = obj.contactNo,
                email = obj.email,
                empName = obj.empName
            };
            _context.EmployeeMaster.Add(_emp);
            _context.SaveChanges();

            foreach (var item in obj.AddressList)
            {
                EmployeeAddress _address = new()
                {
                    address = item.address,
                    pincode = item.pincode,
                    city = item.city,
                    state = item.state,
                    employeeId = _emp.empId
                };

                _context.EmployeeAddress.Add(_address);
                _context.SaveChanges();
            }

            return Ok("Created Success");
        }

        [HttpPost("SaveEmployeeAddress")]
        public IActionResult SaveEmployeeAddress(EmployeeViewModel obj)
        {
            EmployeeMaster _emp = new()
            {
                contactNo = obj.contactNo,
                email = obj.email,
                empName = obj.empName
            };
            _context.EmployeeMaster.Add(_emp);
            _context.SaveChanges();

            EmployeeAddress _address = new()
            {
                address = obj.address,
                pincode = obj.pincode,
                state = obj.state,
                employeeId = _emp.empId
            };

            _context.EmployeeAddress.Add(_address);
            _context.SaveChanges();

            return Ok("Created Success");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel obj)
        {
            var isExists = _context.EmployeeMaster.SingleOrDefault(e => e.email == obj.Email && e.password == obj.Password);
            if(isExists == null)
            {
                return StatusCode(401, "Invalid Creatials");
            }
            else
            {
                return Ok(isExists);
            }
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] PasswordModel obj)
        {
            var userData = _context.EmployeeMaster.SingleOrDefault(e => e.empId == obj.empId);
            if (userData == null)
            {
                return StatusCode(401, "Invalid Creatials");
            }
            else
            {
                if(userData.password == obj.ExistingPassword)
                {
                    userData.password = obj.NewPassword;
                    _context.SaveChanges();
                    return Ok(userData);
                }
                else
                {
                    return StatusCode(401, "Password does' match with existing password");
                }
            }
        }

        [HttpPost("SaveEmployee")]
        public IActionResult SaveEmployee(EmployeeMaster obj)
        {
            commonResponseModel _res = new commonResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var isEmailExist = _context.EmployeeMaster.SingleOrDefault(e => e.email == obj.email);
                    if (isEmailExist == null)
                    {
                        _context.EmployeeMaster.Add(obj);
                        _context.SaveChanges();
                        _res.result = true;
                        _res.message = "Employee creation successful";
                        _res.data = obj;
                        return StatusCode(201,obj);
                    }
                    else
                    {
                        _res.message = "Email Alreday Exist. Please try different One";
                        _res.result = true;
                        return BadRequest(_res);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
                
            }
            catch (Exception ex)
            {

                _res.result = false;
                _res.message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500,ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

        }

        [HttpPut("UpdateEmployee")]
        public EmployeeMaster UpdateEmployee(EmployeeMaster obj, int id)
        {
            var record = _context.EmployeeMaster.SingleOrDefault(e => e.empId == id);
            record.empName = obj.empName;
            record.email = obj.email;
            record.contactNo = obj.contactNo;
            //record.city = obj.city;

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
