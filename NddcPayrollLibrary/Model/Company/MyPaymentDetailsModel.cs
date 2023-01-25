using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Company
{
    public class MyPaymentDetailsModel
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string PayInfo1 { get; set; }
        public string PayInfo2 { get; set; }
        public string SortCode { get; set; }
        public int EmployeeId { get; set; }
    }
}
