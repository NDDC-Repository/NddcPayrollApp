using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class MyNHFReportModel
    {
        public int Id { get; set; }
        public string SrNo { get; set; }
        public string PayPoint { get; set; }
        public string EmployeeCode { get; set; }
        public string NHFNumber { get; set; }
        public string FirstNAme { get; set; }
        public string LastName { get; set; }
        public decimal NHFAmount { get; set; }
        public decimal BasicSalary { get; set; }
    }
}
