using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Payroll
{
    public class MyLinkedBenefitsModel
    {
        public int Id { get; set; }
        public int GradeLevelId { get; set; }
        public string GradeLevel { get; set; }
        public int LinkedGradeLevelId { get; set; }
        public string BenefitType { get; set; }
        public int MultiplyBy { get; set; }
        public string BenefitTypeId { get; set; }
        public double Amount { get; set; }
        public string Cycle { get; set; }
    }
}
