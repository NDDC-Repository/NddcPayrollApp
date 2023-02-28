using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class MyStaffPayeDeductionsModel
    {
        public int SrNo { get; set; }
        public string StaffTinNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal Tax { get; set; }

    }
}
