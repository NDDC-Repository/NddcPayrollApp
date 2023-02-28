using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class MyPayeReportSummaryModel
    {
        public string Location { get; set; }
        public int EmployeeCount { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal Tax { get; set; }
    }
}
