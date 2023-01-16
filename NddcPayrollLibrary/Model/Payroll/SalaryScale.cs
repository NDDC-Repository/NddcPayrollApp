using NddcPayrollLibrary.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Payroll
{
    public class SalaryScale
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        public string GradeLevel { get; set; }
        public Categories Category { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
