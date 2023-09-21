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
using Org.BouncyCastle.Ocsp;
using Syncfusion.XlsIO;
using Syncfusion.Blazor.Charts.Chart.Internal;

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
        public IActionResult OnPost()
        {
            //PaySlips = empDb.GetEmployeePaySlipByDept(DepartmentId, PayrollJournalTitleId);

            //JournalTitles = payJoDb.GetPayrollJournalTitles();
            //Departments = compDb.GetAllDepartments();

            //return RedirectToAction($"/payslipdept", new { departmentId = DepartmentId, payrollJournalTitleId = PayrollJournalTitleId });

            return RedirectToPage($"/payslipdept", new { handler = "ReportFromPartial", departmentId = DepartmentId, payrollJournalTitleId = PayrollJournalTitleId });
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
        public async Task<IActionResult> OnPostExcelExportAsync()
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;

                //Create a workbook
                IWorkbook workbook = application.Workbooks.Create(0);
                IWorksheet worksheet = workbook.Worksheets.Create("SUMMARY");

                worksheet.IsGridLinesVisible = false;
                worksheet.Workbook.StandardFont = "Courier New";
                worksheet.Workbook.StandardFontSize = 10;
                worksheet.Zoom = 100;

                PaySlips = await Task.Run(() => empDb.GetEmployeePaySlipByDept(DepartmentId, PayrollJournalTitleId));
                int i = 2;
                foreach (var p in PaySlips)
                {
                    worksheet.Range[$"A{i}"].Text = "Co. Name";
                    worksheet.Range[$"A{i}"].ColumnWidth = 12;
                    worksheet.Range[$"A{i}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i}"].Text = "NIGER DELTA DEVELOPMENT COMM";
                    worksheet.Range[$"B{i}"].ColumnWidth = 33;
                    worksheet.Range[$"C{i}"].Text = "Co. Address";
                    worksheet.Range[$"C{i}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"C{i}"].ColumnWidth = 12;
                    worksheet.Range[$"D{i}"].Text = "Eastern Byepass, PHC";
                    worksheet.Range[$"D{i}"].ColumnWidth = 30;
                    worksheet.Range[$"E{i}"].Text = "Payment Dt";
                    worksheet.Range[$"E{i}"].ColumnWidth = 12;
                    worksheet.Range[$"E{i}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"F{i}"].Text = p.DateOfPayment;
                    worksheet.Range[$"F{i}"].ColumnWidth = 10;

                    worksheet.Range[$"A{i + 1}"].Text = "Emp Code";
                    worksheet.Range[$"A{i + 1}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 1}"].Text = p.EmployeeCode;
                    worksheet.Range[$"C{i + 1}"].Text = "Department";
                    worksheet.Range[$"C{i + 1}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"D{i + 1}"].Text = p.DepartmentName;
                    worksheet.Range[$"D{i + 1}"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                    worksheet.Range[$"E{i + 1}"].Text = "Date Engaged";
                    worksheet.Range[$"E{i + 1}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"F{i + 1}"].Value2 = p.DateEngaged;

                    worksheet.Range[$"A{i + 2}"].Text = "Emp Name";
                    worksheet.Range[$"A{i + 2}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 2}"].Text = p.FirstName + " " + p.LastName;
                    worksheet.Range[$"C{i + 2}"].Text = "Account No";
                    worksheet.Range[$"C{i + 2}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"D{i + 2}"].Text = p.AccountNumber;
                    worksheet.Range[$"E{i + 2}"].Text = "Branch Code";
                    worksheet.Range[$"E{i + 2}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"F{i + 2}"].Text = p.BankCode;

                    worksheet.Range[$"A{i + 3}"].Text = "Emp Address";
                    worksheet.Range[$"A{i + 3}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"B{i + 3}"].Text = "Eastern Bypass, PHC";
                    worksheet.Range[$"C{i + 3}"].Text = "Job Title";
                    worksheet.Range[$"C{i + 3}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"D{i + 3}"].Text = p.JobTitle;
                    worksheet.Range[$"E{i + 3}"].Text = "Job Grade";
                    worksheet.Range[$"E{i + 3}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"F{i + 3}"].Text = p.JobGrade;

                    worksheet.Range[$"A{i}:F{i + 3}"].BorderAround(ExcelLineStyle.Thin);

                    //Earnings Section
                    worksheet.Range[$"A{i + 4}"].Text = "EARNINGS";
                    worksheet.Range[$"A{i + 4}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"A{i + 4}:C{i + 4}"].Merge();
                    worksheet.Range[$"A{i + 4}"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

                    worksheet.Range[$"A{i + 5}"].Text = "Description";
                    worksheet.Range[$"A{i + 5}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"A{i + 5}:B{i + 5}"].Merge();
                    worksheet.Range[$"C{i + 5}"].Text = "Amount";
                    worksheet.Range[$"C{i + 5}"].CellStyle.Font.Bold = true;

                    worksheet.Range[$"A{i + 6}"].Text = "Basic Salary";
                    worksheet.Range[$"A{i + 6}:B{i + 6}"].Merge();
                    worksheet.Range[$"C{i + 6}"].Value2 = p.BasicSalary;

                    worksheet.Range[$"A{i + 7}"].Text = "Housing Allowance";
                    worksheet.Range[$"A{i + 7}:B{i + 7}"].Merge();
                    worksheet.Range[$"C{i + 7}"].Value2 = p.HousingAllow;

                    worksheet.Range[$"A{i + 8}"].Text = "Furniture Allowance";
                    worksheet.Range[$"A{i + 8}:B{i + 8}"].Merge();
                    worksheet.Range[$"C{i + 8}"].Value2 = p.FurnitureAllow;

                    worksheet.Range[$"A{i + 9}"].Text = "Transport Allowance";
                    worksheet.Range[$"A{i + 9}:B{i + 9}"].Merge();
                    worksheet.Range[$"C{i + 9}"].Value2 = p.TransportAllow;

                    worksheet.Range[$"A{i + 10}"].Text = "Meal Allowance";
                    worksheet.Range[$"A{i + 10}:B{i + 10}"].Merge();
                    worksheet.Range[$"C{i + 10}"].Value2 = p.MealAllow;

                    worksheet.Range[$"A{i + 11}"].Text = "Utility Allowance";
                    worksheet.Range[$"A{i + 11}:B{i + 11}"].Merge();
                    worksheet.Range[$"C{i + 11}"].Value2 = p.UtilityAllow;

                    worksheet.Range[$"A{i + 12}"].Text = "Security Allowance";
                    worksheet.Range[$"A{i + 12}:B{i + 12}"].Merge();
                    worksheet.Range[$"C{i + 12}"].Value2 = p.SecurityAllow;

                    worksheet.Range[$"A{i + 13}"].Text = "Domestic Serv Allowance";
                    worksheet.Range[$"A{i + 13}:B{i + 13}"].Merge();
                    worksheet.Range[$"C{i + 13}"].Value2 = p.DomesticServantAllow;

                    worksheet.Range[$"A{i + 14}"].Text = "Driver Allowance";
                    worksheet.Range[$"A{i + 14}:B{i + 14}"].Merge();
                    worksheet.Range[$"C{i + 14}"].Value2 = p.DriverAllow;

                    worksheet.Range[$"A{i + 15}"].Text = "Education Allowance";
                    worksheet.Range[$"A{i + 15}:B{i + 15}"].Merge();
                    worksheet.Range[$"C{i + 15}"].Value2 = p.EducationAllow;

                    worksheet.Range[$"A{i + 16}"].Text = "Medical Allowance";
                    worksheet.Range[$"A{i + 16}:B{i + 16}"].Merge();
                    worksheet.Range[$"C{i + 16}"].Value2 = p.MedicalAllow;

                    worksheet.Range[$"A{i + 17}"].Text = "Vehicle Maintenance";
                    worksheet.Range[$"A{i + 17}:B{i + 17}"].Merge();
                    worksheet.Range[$"C{i + 17}"].Value2 = p.VehicleMaintenanceAllow;

                    worksheet.Range[$"A{i + 18}"].Text = "Hazard Allowance";
                    worksheet.Range[$"A{i + 18}:B{i + 18}"].Merge();
                    worksheet.Range[$"C{i + 18}"].Value2 = p.HazardAllow;

                    worksheet.Range[$"A{i + 19}"].Text = "Shift Allowance";
                    worksheet.Range[$"A{i + 19}:B{i + 19}"].Merge();
                    worksheet.Range[$"C{i + 19}"].Value2 = p.ShiftAllow;

                    worksheet.Range[$"A{i + 20}"].Text = "Uniform Allowance";
                    worksheet.Range[$"A{i + 20}:B{i + 20}"].Merge();
                    worksheet.Range[$"C{i + 20}"].Value2 = p.UniformAllow;

                    worksheet.Range[$"A{i + 21}"].Text = "Secretarial Allowance";
                    worksheet.Range[$"A{i + 21}:B{i + 21}"].Merge();
                    worksheet.Range[$"C{i + 21}"].Value2 = p.SecretarialAllow;

                    worksheet.Range[$"A{i + 22}"].Text = "Acting Allowance";
                    worksheet.Range[$"A{i + 22}:B{i + 22}"].Merge();
                    worksheet.Range[$"C{i + 22}"].Value2 = p.ActingAllow;

                    worksheet.Range[$"A{i + 23}"].Text = "Entartainmant Allowance";
                    worksheet.Range[$"A{i + 23}:B{i + 23}"].Merge();
                    worksheet.Range[$"C{i + 23}"].Value2 = p.EntertainmentAllow;

                    worksheet.Range[$"A{i + 24}"].Text = "Newspaper Allowance";
                    worksheet.Range[$"A{i + 24}:B{i + 24}"].Merge();
                    worksheet.Range[$"C{i + 24}"].Value2 = p.NewspaperAllow;

                    worksheet.Range[$"A{i + 25}"].Text = "Total Earnings";
                    worksheet.Range[$"A{i + 25}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"A{i + 25}:B{i + 25}"].Merge();
                    worksheet.Range[$"C{i + 25}"].Value2 = p.TotalEarnings;
                    worksheet.Range[$"C{i + 25}"].CellStyle.Font.Bold = true;

                    worksheet.Range[$"A{i + 4}:C{i + 25}"].BorderAround(ExcelLineStyle.Thin);

                    FileStream imageStream = new FileStream("wwwroot/images/dynamicsimage.png", FileMode.Open, FileAccess.Read);
                    IPictureShape shape = worksheet.Pictures.AddPicture(i + 26, 1, imageStream, 20, 20);
                    worksheet.Range[$"A{i + 26}"].RowHeight = 25;
                    worksheet.Range[$"A{i + 26}:C{i + 26}"].Merge();
                    worksheet.Range[$"A{i + 26}:C{i + 26}"].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[$"A{i + 26}"].VerticalAlignment = ExcelVAlign.VAlignCenter;

                    //Deductions
                    worksheet.Range[$"D{i + 4}"].Text = "DEDUCTIONS";
                    worksheet.Range[$"D{i + 4}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"D{i + 4}:F{i + 4}"].Merge();
                    worksheet.Range[$"D{i + 4}"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

                    worksheet.Range[$"D{i + 5}"].Text = "Description";
                    worksheet.Range[$"D{i + 5}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"E{i + 5}"].Text = "Amount";
                    worksheet.Range[$"E{i + 5}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"F{i + 5}"].Text = "Opening";
                    worksheet.Range[$"F{i + 5}"].CellStyle.Font.Bold = true;

                    worksheet.Range[$"D{i + 6}"].Text = "Tax";
                    worksheet.Range[$"E{i + 6}"].Value2 = p.Tax;

                    worksheet.Range[$"D{i + 7}"].Text = "Nat. Housing Fund";
                    worksheet.Range[$"E{i + 7}"].Value2 = p.NHF;

                    worksheet.Range[$"D{i + 8}"].Text = "Pension";
                    worksheet.Range[$"E{i + 8}"].Value2 = p.Pension;

                    worksheet.Range[$"D{i + 9}"].Text = "Cooperative";
                    worksheet.Range[$"E{i + 9}"].Value2 = p.CooperativeDed;

                    worksheet.Range[$"D{i + 10}"].Text = "SSA";
                    worksheet.Range[$"E{i + 10}"].Value2 = p.SSA;

                    worksheet.Range[$"D{i + 11}"].Text = "JSA";
                    worksheet.Range[$"E{i + 11}"].Value2 = p.JSA;

                    worksheet.Range[$"D{i + 11}"].Text = "Voluntary Pension";
                    worksheet.Range[$"E{i + 11}"].Value2 = p.VoluntaryPension;

                    worksheet.Range[$"D{i + 25}"].Text = "Total Deductions";
                    worksheet.Range[$"E{i + 25}"].Value2 = p.TotalDeductions;
                    worksheet.Range[$"D{i + 25}:E{i + 25}"].CellStyle.Font.Bold = true;

                    worksheet.Range[$"D{i + 4}:F{i + 26}"].BorderAround(ExcelLineStyle.Thin);

                    worksheet.Range[$"D{i + 26}"].Text = "NET PAY";
                    worksheet.Range[$"D{i + 26}:F{i + 26}"].CellStyle.Font.Bold = true;
                    worksheet.Range[$"D{i + 26}:F{i + 26}"].CellStyle.Font.Size = 18;
                    worksheet.Range[$"E{i + 26}"].Value2 = p.NetPay;
                    worksheet.Range[$"E{i + 26}:F{i + 26}"].Merge();
                    worksheet.Range[$"D{i + 26}:F{i + 26}"].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[$"E{i + 26}"].HorizontalAlignment = ExcelHAlign.HAlignRight;

                    worksheet.Range[$"A{i + 1}:F{i + 22}"].RowHeight = 15;
                    worksheet.Range[$"A{i + 22}"].RowHeight = 25;
                    worksheet.Range[$"C{i + 6}:C{i + 22}"].NumberFormat = "##,##.##";
                    worksheet.Range[$"E{i + 6}:E{i + 22}"].NumberFormat = "##,##.##";
                    worksheet.Range[$"E{i + 22}"].NumberFormat = "##,##.##";
                    worksheet.Range[$"C{i + 6}:C{i + 22}"].HorizontalAlignment = ExcelHAlign.HAlignLeft;



                    i = i + 28;
                }


                worksheet.PageSetup.Orientation = ExcelPageOrientation.Portrait;
                worksheet.PageSetup.FitToPagesTall = 0;

                //Saving the Excel to the MemoryStream 
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                //Set the position as '0'.
                stream.Position = 0;

                //Download the Excel file in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");
                fileStreamResult.FileDownloadName = $"PayslipsByDept-{DateTime.Now.ToString()}.xlsx";
                return fileStreamResult;
            }
               
        }

    }
}
