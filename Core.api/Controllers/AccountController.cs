using Core.api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Core.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _config;
        private readonly EmployeeDbContext _context;
        public AccountController(IConfiguration config, EmployeeDbContext empDbContext)
        {
            _config = config;
            _context = empDbContext;
        }


        [HttpPost("Login")]
        public IActionResult Login(LoginModel obj)
        {
            var isExist = _context.EmployeeMaster.SingleOrDefault(m => m.email == obj.Email && m.password == obj.Password);
            if (isExist == null)
            {
                return StatusCode(401, "Invalid Credentials");
            }
            else
            {
                var token = GenerateAccessToken();
                EmployeeMasterView _obj = new EmployeeMasterView()
                {
                    contactNo = isExist.contactNo,
                    email = isExist.email,
                    empId = isExist.empId,
                    empName = isExist.empName,
                    password = isExist.password,
                    token = token
                };

                return Ok(_obj);
            }
        }

        private string GenerateAccessToken()
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));

            //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["JwtSettings:Issuer"], _config["JwtSettings:Issuer"], null,
             expires: DateTime.Now.AddMinutes(5),
             signingCredentials: signinCredentials
             );


            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
