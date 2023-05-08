using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.PayrollJournal;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Payroll;

using System.IO;
using System.Text;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace NddcPayroll.Web.Pages.Reports
{
    public class PayslipByDeptModel : PageModel
    {
        private readonly IPayrollJournalData payJoDb;
        private readonly ICompanyData compDb;
        private readonly IEmployeeData empDb;

        public List<MyPayrollJournalTitleModel> JournalTitles { get; set; }
        public List<DepartmentModel> Departments { get; set; }
        public List<MyPaySlipModel> PaySlips { get; set; }


        [BindProperty]
        public int PayrollJournalTitleId { get; set; }
        [BindProperty]
        public int DepartmentId { get; set; }

        public PayslipByDeptModel(IPayrollJournalData payJoDb, ICompanyData compDb, IEmployeeData empDb)
        {
            this.payJoDb = payJoDb;
            this.compDb = compDb;
            this.empDb = empDb;
        }
        public void OnGet()
        {
            JournalTitles = payJoDb.GetPayrollJournalTitles();
            Departments = compDb.GetAllDepartments();
        }
        public void OnPost()
        {
            PaySlips = empDb.GetEmployeePaySlipByDept(DepartmentId, PayrollJournalTitleId);

            JournalTitles = payJoDb.GetPayrollJournalTitles();
            Departments = compDb.GetAllDepartments();
        }

        public FileResult OnPostExport(string GridHtml)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(GridHtml)))
            {
                ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
                PdfWriter writer = new PdfWriter(byteArrayOutputStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                pdfDocument.SetDefaultPageSize(PageSize.A4);
                HtmlConverter.ConvertToPdf(stream, pdfDocument);
                pdfDocument.Close();
                return File(byteArrayOutputStream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }
    }
}
