using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Company
{
    public class MyGradeLevelModel
    {
        public int Id { get; set; }
        public string GradeLevel { get; set; }
        public string Description { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal MonthlyGross { get; set; }
        public string Rank { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
