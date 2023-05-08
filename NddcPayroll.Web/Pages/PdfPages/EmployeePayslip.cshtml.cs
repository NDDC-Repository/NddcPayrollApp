using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Model.Employee;

using System.IO;
using System.Text;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using System.Reflection;

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

        public FileResult OnPostExport(string GridHtml)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(GridHtml)))
            {
                ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
                PdfWriter writer = new PdfWriter(byteArrayOutputStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                pdfDocument.SetDefaultPageSize(PageSize.A4);
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(stream, pdfDocument, converterProperties);
                pdfDocument.Close();
                return File(byteArrayOutputStream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }
    }
}
