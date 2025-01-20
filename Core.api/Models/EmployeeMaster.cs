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
}
