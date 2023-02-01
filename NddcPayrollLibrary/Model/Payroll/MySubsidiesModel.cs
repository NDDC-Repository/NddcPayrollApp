using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Payroll
{
    public class MySubsidiesModel
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        public string SubsidyType { get; set; }
        public double Amount { get; set; }
        public string Cycle { get; set; }
        public int GradeLevelId { get; set; }
    }
}
