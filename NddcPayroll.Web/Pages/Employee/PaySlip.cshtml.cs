using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.PayrollJournal;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Payroll;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace NddcPayroll.Web.Pages.Employee
{
    public class PaySlipModel : PageModel
    {
        private readonly IEmployeeData empDb;
        private readonly IPayrollJournalData payJoDb;

        public List<MyPayrollJournalTitleModel> PayrollJournalTitles { get; set; }

        [BindProperty]
        public EmployeeModel Employee { get; set; }
        [BindProperty]
        public int PayrollJournalTitleId { get; set; }
        public MyPaySlipModel MyPayslip { get; set; }
        public bool Show { get; set; }
        public int MyEmpId { get; set; }

        public PaySlipModel(IEmployeeData empDb, IPayrollJournalData payJoDb)
        {
            this.empDb = empDb;
            this.payJoDb = payJoDb;
        }
        public void OnGet(int? EmpId)
        {
            //Employee = empDb.GetEmployeeDetails(EmpId.Value);
            PayrollJournalTitles = payJoDb.GetPayrollJournalTitles();
            MyEmpId = EmpId.Value;

        }

        public IActionResult OnPostAsync(int? EmpId)
        {
            Show = true;
           
            MyPayslip = empDb.GetEmployeePaySlip(EmpId.Value, PayrollJournalTitleId);
            PayrollJournalTitles = payJoDb.GetPayrollJournalTitles();



            string cookieName = ".AspNetCore.Identity.Application";
            //Get cookie value from HttpRequest object for the requested page.
            string cookieValue = string.Empty;
            if (Request.Cookies[cookieName] != null)
            {
                cookieValue = Request.Cookies[cookieName];
            }

            //Initialize the HTML to PDF converter with the Blink rendering engine.
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

            BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();

            blinkConverterSettings.ViewPortSize = new Syncfusion.Drawing.Size(1440, 0);

            //Add cookies as name and value pair.
            blinkConverterSettings.Cookies.Add(cookieName, cookieValue);

            //Assign Blink converter settings to HTML converter.
            htmlConverter.ConverterSettings = blinkConverterSettings;

            string url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(HttpContext.Request);
            url = url.Substring(0, url.LastIndexOf('/'));

            //Convert existing current page URL to PDF.
            PdfDocument document = htmlConverter.Convert($"https://payroll.nddc.gov.ng/pdfpages/employeepayslip/{EmpId.Value}/{PayrollJournalTitleId}");

            //Saving the PDF to the MemoryStream.
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            //Download the PDF document in the browser.
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Output5.pdf");
        }

       
    }
}
