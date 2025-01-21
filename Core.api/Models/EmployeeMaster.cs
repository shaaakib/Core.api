using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.api.Models
{
    public class EmployeeMaster
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int empId { get; set; }
        [Required,MaxLength(50)]
        public string empName { get; set; }
        public string? email { get; set; }
        [Required,MaxLength(15)]
        public string contactNo { get; set; }
        public string? city { get; set; }
    }

    public class EmployeeDropModel
    {
        public int empId { get; set; }
        public string empName { get; set; }
    }

    public class SearchEmployeeModel
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public string? contactNo { get; set; }
    }

    public class commonResponseModel
    {
        public bool result { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public commonResponseModel(bool result = false, string Message = "", object Data = null)
        {
            result = result;
            message = Message;
            data = Data;
        }
    }
}
