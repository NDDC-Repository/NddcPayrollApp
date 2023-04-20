using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class PensionSummaryModel
    {
        public int SrNo { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string RSAPin { get; set; }
        public string PFACode { get; set; }
        public string PFAName { get; set; }
        public int EmployeeCount { get; set; }
        public decimal VoluntaryPension { get; set; }
        public decimal EmployeePension { get; set; }
        public decimal EmployerPension { get; set; }
        public decimal Total { get; set; }
    }
}
