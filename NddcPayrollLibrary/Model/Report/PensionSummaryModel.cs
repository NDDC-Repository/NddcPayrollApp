using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class PensionSummaryModel
    {
        [Bindable(false)]
        public int SrNo { get; set; }
        [Bindable(false)]
        public string EmployeeCode { get; set; }
        [Bindable(false)]
        public string EmployeeName { get; set; }
        [Bindable(false)]
        public string RSAPin { get; set; }
        public string PFACode { get; set; }
        public string PFAName { get; set; }
        public int EmployeeCount { get; set; }
        public decimal EmployerArrers { get; set; }
        public decimal EmployeeArrears { get; set; }
        public decimal EmployerPension { get; set; }
        public decimal EmployeePension { get; set; }
        public decimal VoluntaryPenEmp { get; set; }
        public decimal VoluntaryPension { get; set; }
        public decimal Total { get; set; }
    }
}
