using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Model.Employee;

namespace NddcPayroll.Web.Pages.PdfPages
{
    public class EmployeePayslipModel : PageModel
    {
        private readonly IEmployeeData empDb;

        public MyPaySlipModel MyPayslip { get; set; }
        //public int MyEmpId { get; set; }
        //public int MyPayrollJoId { get; set; }

        public EmployeePayslipModel(IEmployeeData empDb)
        {
            this.empDb = empDb;
        }
        public void OnGet(int? EmpId, int? PayrollJoId)
        {
            //MyEmpId = EmpId.Value;
            //MyPayrollJoId = PayrollJoId.Value;
            MyPayslip = empDb.GetEmployeePaySlip(EmpId.Value, PayrollJoId.Value);
        }
    }
}
