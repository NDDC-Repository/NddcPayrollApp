using NddcPayrollLibrary.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Company
{
    public class MyGradeLevelGridModel
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        public string GradeLevel { get; set; }
        public string Description { get; set; }
        public double BasicSalary { get; set; }
    }
}
