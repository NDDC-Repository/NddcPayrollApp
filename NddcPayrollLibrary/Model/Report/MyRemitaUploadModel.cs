using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class MyRemitaUploadModel
    {
        public int Id { get; set; }
        public string BankCode { get; set; }
        public string EmployeeCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; } = "20";
        public decimal PayableAmount { get; set; }
        public string ShortDescription { get; set; } = DateTime.Now.ToString("Y");
        public string LongDescription { get; set; } = DateTime.Now.ToString("Y");
        public string EmployeeName { get; set; }
        public string Mobile { get; set; } = "0";
        public string Email { get; set; } = "0";
        public string WitholdingTax { get; set; } = "0";
        public string TaxOffice { get; set; } = "0";
    }
}
