using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Report;
using Syncfusion.Drawing;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
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

            //Initialize the HTML to PDF converter with the Blink rendering engine.
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

            BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();

            blinkConverterSettings.ViewPortSize = new Syncfusion.Drawing.Size(800, 0);

            //Assign Blink converter settings to HTML converter.
            htmlConverter.ConverterSettings = blinkConverterSettings;

            string url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(HttpContext.Request);
            url = url.Substring(0, url.LastIndexOf('/'));

            //Convert existing current page URL to PDF.
            PdfDocument document = htmlConverter.Convert("https://payroll.nddc.gov.ng/Reports/PayrollSummaryByDept");

            //Saving the PDF to the MemoryStream.
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            //Download the PDF document in the browser.
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Output5.pdf");


        }

        public async Task<IActionResult> OnPostExcelExportAsync()
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;

                //Create a workbook
                IWorkbook workbook = application.Workbooks.Create(0);
                IWorksheet worksheet = workbook.Worksheets.Create("SUMMARY");

                worksheet.IsGridLinesVisible = true;
                worksheet.Workbook.StandardFont = "Arial";
                worksheet.Workbook.StandardFontSize = 10;
                worksheet.Zoom = 80;

                worksheet.Range["A1"].Text = "PAYROLL SUMMARY PER DEPARTMENT";
                worksheet.Range["A1"].CellStyle.Font.Bold = true;
                worksheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(127, 127, 127);
                worksheet.Range["A1"].CellStyle.Font.Size = 24;
                worksheet.Range["A1"].CellStyle.Font.FontName = "Segoe UI Light";
                worksheet.Range["A1"].RowHeight = 50;

                worksheet.Range["A3"].Text = "001 NIGER DELTA DEVELOPMENT COMMISSION";
                worksheet.Range["A3"].CellStyle.Font.Bold = true;
                worksheet.Range["A3"].CellStyle.Font.RGBColor = Color.FromArgb(127, 127, 127);
                worksheet.Range["A3"].CellStyle.Font.FontName = "Segoe UI Light";
                worksheet.Range["A3"].CellStyle.Font.Size = 14;

                worksheet.Range["A4"].Text = $"Period Ending: {DateTime.Now.ToString("MMMM yyyy")}";
                worksheet.Range["A4"].CellStyle.Font.Bold = true;
                worksheet.Range["A4"].CellStyle.Font.RGBColor = Color.FromArgb(127, 127, 127);
                worksheet.Range["A4"].CellStyle.Font.FontName = "Segoe UI Light";
                worksheet.Range["A4"].CellStyle.Font.Size = 12;

                PayrollSumList = await Task.Run(() => repDb.GetPayrollSummaryByDeptReportAsync());
                int i = 6;
                foreach (var p in PayrollSumList)
                {
                    worksheet.Range[$"A{i}"].Text = $"Department: {p.DepartmentName}";
                    worksheet.Range[$"A{i}:E{i}"].Merge();
                    worksheet.Range[$"A{i}:E{i}"].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[$"A{i}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"A{i}:E{i}"].CellStyle.Color = Color.FromArgb(194, 214, 155);

                    worksheet.Range[$"A{i + 1}:E{i + 1}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"A{i + 1}:E{i + 1}"].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[$"A{i + 1}:E{i + 1}"].CellStyle.Color = Color.FromArgb(194, 214, 155);
                    worksheet.Range[$"A{i + 1}"].Text = "EARNING";
                    worksheet.Range[$"A{i + 1}"].ColumnWidth = 25;
                    worksheet.Range[$"B{i + 1}"].Text = "AMOUNT";
                    worksheet.Range[$"B{i + 1}"].ColumnWidth = 20;
                    worksheet.Range[$"C{i + 1}"].Text = "DEDUCTION";
                    worksheet.Range[$"C{i + 1}"].ColumnWidth = 25;
                    worksheet.Range[$"D{i + 1}"].Text = "AMOUNT";
                    worksheet.Range[$"D{i + 1}"].ColumnWidth = 15;
                    worksheet.Range[$"E{i + 1}"].Text = "CC AMOUNT";
                    worksheet.Range[$"E{i + 1}"].ColumnWidth = 15;

                    worksheet.Range[$"A{i + 2}"].Text = "Basic Salary";
                    worksheet.Range[$"B{i + 2}"].Value2 = p.BasicSalary;
                    worksheet.Range[$"B{i + 2}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 2}"].Text = "Tax";
                    worksheet.Range[$"D{i + 2}"].Value2 = p.Tax;
                    worksheet.Range[$"D{i + 2}"].NumberFormat = "#,###.##";


                    worksheet.Range[$"A{i + 3}"].Text = "Transport Allowance";
                    worksheet.Range[$"B{i + 3}"].Value2 = p.TransportAllow;
                    worksheet.Range[$"B{i + 3}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 3}"].Text = "Nat. Housing Fund";
                    worksheet.Range[$"D{i + 3}"].Value2 = p.NHF;
                    worksheet.Range[$"D{i + 3}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 4}"].Text = "Meal Allowance";
                    worksheet.Range[$"B{i + 4}"].Value2 = p.MealAllow;
                    worksheet.Range[$"B{i + 4}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 4}"].Text = "Pension";
                    worksheet.Range[$"D{i + 4}"].Value2 = p.Pension;
                    worksheet.Range[$"D{i + 4}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"E{i + 4}"].Value2 = p.EmployerPension;
                    worksheet.Range[$"E{i + 4}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 5}"].Text = "Utility Allowance";
                    worksheet.Range[$"B{i + 5}"].Value2 = p.UtilityAllow;
                    worksheet.Range[$"B{i + 5}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 5}"].Text = "NDDC AUPTRE JSA";
                    worksheet.Range[$"D{i + 5}"].Value2 = p.JSA;
                    worksheet.Range[$"D{i + 5}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 6}"].Text = "Education Allowance";
                    worksheet.Range[$"B{i + 6}"].Value2 = p.EducationAllow;
                    worksheet.Range[$"B{i + 6}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 6}"].Text = "Cooperative";
                    worksheet.Range[$"D{i + 6}"].Value2 = p.CooporativeDed;
                    worksheet.Range[$"D{i + 6}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 7}"].Text = "Security Allowance";
                    worksheet.Range[$"B{i + 7}"].Value2 = p.SecurityAllow;
                    worksheet.Range[$"B{i + 7}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 7}"].Text = "NDDC AUPTRE SSA";
                    worksheet.Range[$"D{i + 7}"].Value2 = p.SSA;
                    worksheet.Range[$"D{i + 7}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 8}"].Text = "Domestic Servant";
                    worksheet.Range[$"B{i + 8}"].Value2 = p.DomesticServantAllow;
                    worksheet.Range[$"B{i + 8}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 9}"].Text = "Medical";
                    worksheet.Range[$"B{i + 9}"].Value2 = p.MedicalAllow;
                    worksheet.Range[$"B{i + 9}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 10}"].Text = "Driver's Allowance";
                    worksheet.Range[$"B{i + 10}"].Value2 = p.DriverAllow;
                    worksheet.Range[$"B{i + 10}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 11}"].Text = "Vehicle Maintenance";
                    worksheet.Range[$"B{i + 11}"].Value2 = p.VehicleAllow;
                    worksheet.Range[$"B{i + 11}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 12}"].Text = "Hazard";
                    worksheet.Range[$"B{i + 12}"].Value2 = p.HazardAllow;
                    worksheet.Range[$"B{i + 12}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 13}"].Text = "Shift Allowance";
                    worksheet.Range[$"B{i + 13}"].Value2 = 0.00;
                    worksheet.Range[$"B{i + 13}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 14}"].Text = "Secretarial Allowance";

                    worksheet.Range[$"A{i + 16}"].Text = "TOTALS";
                    worksheet.Range[$"A{i + 16}:E{i + 16}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 16}"].Value2 = $"=SUM(B{i + 2}:B{i + 14})";
                    worksheet.Range[$"B{i + 16}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"D{i + 16}"].Value2 = $"=SUM(D{i + 2}:D{i + 7})";
                    worksheet.Range[$"D{i + 16}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"E{i + 16}"].Value2 = $"=E{i + 4}";
                    worksheet.Range[$"E{i + 16}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 18}"].Text = "NET PAY";
                    worksheet.Range[$"A{i + 18}:E{i + 18}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 18}"].Value2 = p.NetPay;
                    worksheet.Range[$"B{i + 18}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 20}"].Text = "SALARY COST";
                    worksheet.Range[$"A{i + 20}:E{i + 20}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 20}"].Value2 = $"=(B{i + 16} + E{i + 16})";
                    worksheet.Range[$"B{i + 20}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 22}"].Text = "EMP COUNT";
                    worksheet.Range[$"A{i + 22}:E{i + 22}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 22}"].Value2 = p.EmployeeCount;

                    worksheet.Range[$"A{i + 2}:E{i + 22}"].BorderAround(ExcelLineStyle.Thin);

                    i = i + 24;
                }

                //Create Summary
                worksheet.Range[$"A{i}"].Text = "TOTAL SUMMARY FOR ALL DEPARTMENTS";
                worksheet.Range[$"A{i}"].CellStyle.Font.Bold = true;
                worksheet.Range[$"A{i}"].CellStyle.Font.RGBColor = Color.FromArgb(127, 127, 127);
                worksheet.Range[$"A{i}"].CellStyle.Font.Size = 16;
                worksheet.Range[$"A{i}"].CellStyle.Font.FontName = "Segoe UI Light";
                worksheet.Range[$"A{i}"].RowHeight = 50;

                worksheet.Range[$"A{i + 2}"].Text = "001 NIGER DELTA DEVELOPMENT COMMISSION";
                worksheet.Range[$"A{i + 2}"].CellStyle.Font.Bold = true;
                worksheet.Range[$"A{i + 2}"].CellStyle.Font.RGBColor = Color.FromArgb(127, 127, 127);
                worksheet.Range[$"A{i + 2}"].CellStyle.Font.FontName = "Segoe UI Light";
                worksheet.Range[$"A{i + 2}"].CellStyle.Font.Size = 14;

                worksheet.Range[$"A{i + 4}"].Text = $"Period Ending: {DateTime.Now.ToString("MMMM yyyy")}";
                worksheet.Range[$"A{i + 4}"].CellStyle.Font.Bold = true;
                worksheet.Range[$"A{i + 4}"].CellStyle.Font.RGBColor = Color.FromArgb(127, 127, 127);
                worksheet.Range[$"A{i + 4}"].CellStyle.Font.FontName = "Segoe UI Light";
                worksheet.Range[$"A{i + 4}"].CellStyle.Font.Size = 12;

                List<MyPayrollSummaryByDepartmentModel> pList = new();
                pList = await Task.Run(() => repDb.GetPayrollTotalsReportAsync());
                i = i + 6;
                foreach (var p in pList)
                {
                    worksheet.Range[$"A{i}"].Text = "Summary: Grand Totals";
                    worksheet.Range[$"A{i}:E{i}"].Merge();
                    worksheet.Range[$"A{i}:E{i}"].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[$"A{i}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"A{i}:E{i}"].CellStyle.Color = Color.FromArgb(194, 214, 155);

                    worksheet.Range[$"A{i + 1}:E{i + 1}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"A{i + 1}:E{i + 1}"].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[$"A{i + 1}:E{i + 1}"].CellStyle.Color = Color.FromArgb(194, 214, 155);
                    worksheet.Range[$"A{i + 1}"].Text = "EARNING";
                    worksheet.Range[$"A{i + 1}"].ColumnWidth = 25;
                    worksheet.Range[$"B{i + 1}"].Text = "AMOUNT";
                    worksheet.Range[$"B{i + 1}"].ColumnWidth = 20;
                    worksheet.Range[$"C{i + 1}"].Text = "DEDUCTION";
                    worksheet.Range[$"C{i + 1}"].ColumnWidth = 25;
                    worksheet.Range[$"D{i + 1}"].Text = "AMOUNT";
                    worksheet.Range[$"D{i + 1}"].ColumnWidth = 15;
                    worksheet.Range[$"E{i + 1}"].Text = "CC AMOUNT";
                    worksheet.Range[$"E{i + 1}"].ColumnWidth = 15;

                    worksheet.Range[$"A{i + 2}"].Text = "Basic Salary";
                    worksheet.Range[$"B{i + 2}"].Value2 = p.BasicSalary;
                    worksheet.Range[$"B{i + 2}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 2}"].Text = "Tax";
                    worksheet.Range[$"D{i + 2}"].Value2 = p.Tax;
                    worksheet.Range[$"D{i + 2}"].NumberFormat = "#,###.##";


                    worksheet.Range[$"A{i + 3}"].Text = "Transport Allowance";
                    worksheet.Range[$"B{i + 3}"].Value2 = p.TransportAllow;
                    worksheet.Range[$"B{i + 3}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 3}"].Text = "Nat. Housing Fund";
                    worksheet.Range[$"D{i + 3}"].Value2 = p.NHF;
                    worksheet.Range[$"D{i + 3}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 4}"].Text = "Meal Allowance";
                    worksheet.Range[$"B{i + 4}"].Value2 = p.MealAllow;
                    worksheet.Range[$"B{i + 4}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 4}"].Text = "Pension";
                    worksheet.Range[$"D{i + 4}"].Value2 = p.Pension;
                    worksheet.Range[$"D{i + 4}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"E{i + 4}"].Value2 = p.EmployerPension;
                    worksheet.Range[$"E{i + 4}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 5}"].Text = "Utility Allowance";
                    worksheet.Range[$"B{i + 5}"].Value2 = p.UtilityAllow;
                    worksheet.Range[$"B{i + 5}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 5}"].Text = "NDDC AUPTRE JSA";
                    worksheet.Range[$"D{i + 5}"].Value2 = p.JSA;
                    worksheet.Range[$"D{i + 5}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 6}"].Text = "Education Allowance";
                    worksheet.Range[$"B{i + 6}"].Value2 = p.EducationAllow;
                    worksheet.Range[$"B{i + 6}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 6}"].Text = "Cooperative";
                    worksheet.Range[$"D{i + 6}"].Value2 = p.CooporativeDed;
                    worksheet.Range[$"D{i + 6}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 7}"].Text = "Security Allowance";
                    worksheet.Range[$"B{i + 7}"].Value2 = p.SecurityAllow;
                    worksheet.Range[$"B{i + 7}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"C{i + 7}"].Text = "NDDC AUPTRE SSA";
                    worksheet.Range[$"D{i + 7}"].Value2 = p.SSA;
                    worksheet.Range[$"D{i + 7}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 8}"].Text = "Domestic Servant";
                    worksheet.Range[$"B{i + 8}"].Value2 = p.DomesticServantAllow;
                    worksheet.Range[$"B{i + 8}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 9}"].Text = "Medical";
                    worksheet.Range[$"B{i + 9}"].Value2 = p.MedicalAllow;
                    worksheet.Range[$"B{i + 9}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 10}"].Text = "Driver's Allowance";
                    worksheet.Range[$"B{i + 10}"].Value2 = p.DriverAllow;
                    worksheet.Range[$"B{i + 10}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 11}"].Text = "Vehicle Maintenance";
                    worksheet.Range[$"B{i + 11}"].Value2 = p.VehicleAllow;
                    worksheet.Range[$"B{i + 11}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 12}"].Text = "Hazard";
                    worksheet.Range[$"B{i + 12}"].Value2 = p.HazardAllow;
                    worksheet.Range[$"B{i + 12}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 13}"].Text = "Shift Allowance";
                    worksheet.Range[$"B{i + 13}"].Value2 = 0.00;
                    worksheet.Range[$"B{i + 13}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 14}"].Text = "Secretarial Allowance";

                    worksheet.Range[$"A{i + 16}"].Text = "TOTALS";
                    worksheet.Range[$"A{i + 16}:E{i + 16}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 16}"].Value2 = $"=SUM(B{i + 2}:B{i + 14})";
                    worksheet.Range[$"B{i + 16}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"D{i + 16}"].Value2 = $"=SUM(D{i + 2}:D{i + 7})";
                    worksheet.Range[$"D{i + 16}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"E{i + 16}"].Value2 = $"=E{i + 4}";
                    worksheet.Range[$"E{i + 16}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 18}"].Text = "NET PAY";
                    worksheet.Range[$"A{i + 18}:E{i + 18}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 18}"].Value2 = p.NetPay;
                    worksheet.Range[$"B{i + 18}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 20}"].Text = "SALARY COST";
                    worksheet.Range[$"A{i + 20}:E{i + 20}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 20}"].Value2 = $"=(B{i + 16} + E{i + 16})";
                    worksheet.Range[$"B{i + 20}"].NumberFormat = "#,###.##";

                    worksheet.Range[$"A{i + 22}"].Text = "EMP COUNT";
                    worksheet.Range[$"A{i + 22}:E{i + 22}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 22}"].Value2 = p.EmployeeCount;

                    worksheet.Range[$"A{i + 2}:E{i + 22}"].BorderAround(ExcelLineStyle.Thin);
                }


                worksheet.PageSetup.Orientation = ExcelPageOrientation.Portrait;
                worksheet.PageSetup.FitToPagesTall = 0;

                worksheet.PageSetup.LeftFooter = "&KFF0000 Dynamics 365 - Simple Payroll";
                worksheet.PageSetup.CenterFooter = $"&KFF0000 Printed On: &D at &T";
                worksheet.PageSetup.RightFooter = "&KFF0000 pages &P- of &N";



                //Saving the Excel to the MemoryStream 
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                //Set the position as '0'.
                stream.Position = 0;

                //Download the Excel file in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");
                fileStreamResult.FileDownloadName = $"Payroll Summary-{DateTime.Now.ToString()}.xlsx";
                return fileStreamResult;
            }
        }
    }
}
