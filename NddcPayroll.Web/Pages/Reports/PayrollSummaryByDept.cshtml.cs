using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Report;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;
using static Dapper.SqlMapper;

namespace NddcPayroll.Web.Pages.Reports
{
    public class PayrollSummaryByDeptModel : PageModel
    {
        private readonly IReportsData repDb;

        public List<MyPayrollSummaryByDepartmentModel> PayrollSumList { get; set; }
        public List<MyPayrollSummaryByDepartmentModel> PayrollTotals { get; set; }
        public PayrollSummaryByDeptModel(IReportsData repDb)
        {
            this.repDb = repDb;
        }
        public async Task OnGetAsync()
        {
            PayrollSumList = await Task.Run(() => repDb.GetPayrollSummaryByDeptReportAsync());
            PayrollTotals = await Task.Run(() => repDb.GetPayrollTotalsReportAsync());
        }

        public IActionResult OnPostAsync()
        {
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
            PdfDocument document = htmlConverter.Convert("https://localhost:7248/Reports/PayrollSummaryByDept");

            //Saving the PDF to the MemoryStream.
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            //Download the PDF document in the browser.
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Output5.pdf");


            ////Initialize HTML to PDF converter. 
            //HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

            ////Convert URL to PDF document. 
            //PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com/");

            ////Create the filestream to save the PDF document. 
            //FileStream fileStream = new FileStream("HTML-to-PDF.pdf", FileMode.CreateNew, FileAccess.ReadWrite);

            ////Save and closes the PDF document.
            //document.Save(fileStream);
            //document.Close(true);




            ////Initialize the HTML to PDF converter.
            //HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

            ////Convert Partial webpage to PDF
            //PdfDocument document = htmlConverter.ConvertPartialHtml("https://localhost:7248/Reports/PayrollSummaryByDept", "print");
            //MemoryStream stream = new MemoryStream();
            //document.Save(stream);
            //return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Output23.pdf");

            //FileStream fileStream = new FileStream("HTML-to-PDF.pdf", FileMode.CreateNew, FileAccess.ReadWrite);
            //Save and close the PDF document.
            //document.Save(fileStream);
            //document.Close(true);

        }
    }
}
