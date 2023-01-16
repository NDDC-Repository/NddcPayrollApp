using NddcPayrollLibrary.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Payroll
{
    public class MyBenefitsModel
    {
        public int Id { get; set; }
        public int GradeLevelID { get; set; }
        public BenefitTypes Benefit { get; set; }
        public double Percentage { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}

