using iText.Html2pdf;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayroll.Web.Renderers;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Report;

using System.Net.Mime;

namespace NddcPayroll.Web.Pages.PdfPages
{
    public class PayrollSummaryByDeptRptModel : PageModel
    {
        private readonly IReportsData repDb;
        private readonly IRazorTemplateRenderer renderer;

        public List<MyPayrollSummaryByDepartmentModel> PayrollSumList { get; set; }
        public List<MyPayrollSummaryByDepartmentModel> PayrollTotals { get; set; }
        public PayrollSummaryByDeptRptModel(IReportsData repDb, IRazorTemplateRenderer renderer)
        {
            this.repDb = repDb;
            this.renderer = renderer;
        }
       
        string BaseHref => $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        public async Task<FileResult> OnGetReportFromPartialAsync()
        {
            string printDate = DateTime.Now.ToString();

            PayrollSumList = await Task.Run(() => repDb.GetPayrollSummaryByDeptReportAsync());
            PayrollTotals = await Task.Run(() => repDb.GetPayrollTotalsReportAsync());
            var html = await renderer.RenderPartialToStringAsync("_PayrollSummaryRpt", this);
            ConverterProperties converterProperties = new();
            converterProperties.SetBaseUri(BaseHref);
            using var stream = new MemoryStream();
            HtmlConverter.ConvertToPdf(html, stream, converterProperties);
            return File(stream.ToArray(), MediaTypeNames.Application.Pdf, $"PayrollByDept-{printDate}.pdf");
        }
    }
}
