using iText.Html2pdf;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayroll.Web.Renderers;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.PayrollJournal;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Payroll;
using System.Net.Mime;

namespace NddcPayroll.Web.Pages
{
    public class payslipdeptModel : PageModel
    {
        private readonly IPayrollJournalData payJoDb;
        private readonly ICompanyData compDb;
        private readonly IEmployeeData empDb;
        private readonly IRazorTemplateRenderer renderer;

        public List<MyPayrollJournalTitleModel> JournalTitles { get; set; }
        public List<DepartmentModel> Departments { get; set; }
        public List<MyPaySlipModel> PaySlips { get; set; }

        public payslipdeptModel(IPayrollJournalData payJoDb, ICompanyData compDb, IEmployeeData empDb, IRazorTemplateRenderer renderer)
        {
            this.payJoDb = payJoDb;
            this.compDb = compDb;
            this.empDb = empDb;
            this.renderer = renderer;
        }
        string BaseHref => $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        public async Task<FileResult> OnGetReportFromPartialAsync(int? departmentId, int? payrollJournalTitleId)
        {
            string printDate = DateTime.Now.ToString();

            PaySlips = await Task.Run(() => empDb.GetEmployeePaySlipByDept(departmentId.Value, payrollJournalTitleId.Value));
            //PaySlips = empDb.GetEmployeePaySlipByDept(departmentId.Value, payrollJournalTitleId.Value);
            //JournalTitles = payJoDb.GetPayrollJournalTitles();
           // Departments = compDb.GetAllDepartments();

            var html = await renderer.RenderPartialToStringAsync("_PayslipDeptRpt", this);
            ConverterProperties converterProperties = new();
            converterProperties.SetBaseUri(BaseHref);
            using var stream = new MemoryStream();
            HtmlConverter.ConvertToPdf(html, stream, converterProperties);
            return File(stream.ToArray(), MediaTypeNames.Application.Pdf, $"PayslipByDept-{printDate}.pdf");
        }
    }
}
