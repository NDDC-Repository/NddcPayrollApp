using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Helper;
using NddcPayrollLibrary.Data.PayrollJournal;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Payroll;
using NddcPayrollLibrary.Model.Report;
using Org.BouncyCastle.Ocsp;
using Syncfusion.Blazor;
using Syncfusion.XlsIO;

namespace NddcPayroll.Web.Pages.Reports
{
    public class PayrollListingsReportModel : PageModel
    {
        private readonly IPayrollJournalData payJor;
        private readonly IReportsData repDb;
        private readonly IHelperData helpDb;

        [BindProperty]
        public int PayrollJournalTitleId { get; set; }
        public List<MyPayrollJournalTitleModel> JournalTitles { get; set; }
        public List<MyPayRollListModel> PayrollListings { get; set; }
        public List<PayrollListingModel2> PayrollListings2 { get; set; }

        public PayrollListingsReportModel(IPayrollJournalData payJor, IReportsData repDb, IHelperData helpDb)
        {
            this.payJor = payJor;
            this.repDb = repDb;
            this.helpDb = helpDb;
        }
        public void OnGet()
        {
            JournalTitles = payJor.GetPayrollJournalTitles();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            PayrollListings = await Task.Run(() => repDb.GetPayrollListReportById(PayrollJournalTitleId));
            DateTime currPeriod = helpDb.GetAnyRecord<DateTime, int>("PayrollJournalTitles", "CurrentPeriod", "Id", PayrollJournalTitleId);
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;

                //Create a workbook
                IWorkbook workbook = application.Workbooks.Create(0);
                IWorksheet worksheet = workbook.Worksheets.Create("Payroll Listing-1");
                IWorksheet worksheet2 = workbook.Worksheets.Create("Payroll Listing-2(Audit)");

                //IWorksheet worksheet = workbook.Worksheets[0];

                //Adding a picture
                //FileStream imageStream = new FileStream("AdventureCycles-Logo.png", FileMode.Open, FileAccess.Read);
                //IPictureShape shape = worksheet.Pictures.AddPicture(1, 1, imageStream, 20, 20);

                //Disable gridlines in the worksheet
                worksheet.IsGridLinesVisible = true;
                worksheet.Workbook.StandardFont = "Arial";

                //Enter text to the cell B1 and apply formatting.
                worksheet.Range["B1"].Text = "Payroll Listing Report";
                worksheet.Range["B1"].CellStyle.Font.Bold = true;
                //worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
                worksheet.Range["B1"].CellStyle.Font.Size = 16;

                //Merge cells
                worksheet.Range["B1:I1"].Merge();

                //Enter text to the cell B2 and apply formatting.
                worksheet.Range["B2"].Text = "001-NIGER DELTA DEVELOPMENT COMMISSION";
                worksheet.Range["B2"].CellStyle.Font.Bold = false;
                //worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
                worksheet.Range["B2"].CellStyle.Font.Size = 11;

                //Merge cells
                worksheet.Range["B2:I2"].Merge();

                //Enter text to the cell B3 and apply formatting.
                worksheet.Range["B3"].Text = $"Current Period: {currPeriod.ToShortDateString()}";
                worksheet.Range["B23"].CellStyle.Font.Bold = false;
                //worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
                worksheet.Range["B3"].CellStyle.Font.Size = 11;

                //Merge cells
                worksheet.Range["B3:I3"].Merge();

                //Enter text to the cell B4 and apply formatting.
                worksheet.Range["B4"].Text = $"Printed on: {DateTime.Now.ToString()} for Current Period {currPeriod.ToShortDateString()}";
                worksheet.Range["B4"].CellStyle.Font.Bold = true;
                //worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
                worksheet.Range["B4"].CellStyle.Font.Size = 11;

                //Merge cells
                worksheet.Range["B4:I4"].Merge();
                //Apply alignment in the cell D1
                //worksheet.Range["D1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                //worksheet.Range["D1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignTop;

                worksheet.ImportData(PayrollListings, 6, 1, true);
                worksheet.Range["A6:AI6"].AutofitColumns();
                worksheet.Range["A6:AI6"].CellStyle.Font.Bold = true;
                worksheet.Range["A6:AI6"].CellStyle.Font.Size = 11;

                int mCount = PayrollListings.Count();

                worksheet[$"B{mCount + 8}"].Text = "Totals";
                worksheet[$"B{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"H{mCount + 8}"].Formula = "=SUM(H7:H" + (mCount + 7) + ")";
                worksheet.Range[$"H{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"I{mCount + 8}"].Formula = "=SUM(I7:I" + (mCount + 7) + ")";
                worksheet.Range[$"I{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"J{mCount + 8}"].Formula = "=SUM(J7:J" + (mCount + 7) + ")";
                worksheet.Range[$"J{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"K{mCount + 8}"].Formula = "=SUM(K7:K" + (mCount + 7) + ")";
                worksheet.Range[$"K{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"L{mCount + 8}"].Formula = "=SUM(L7:L" + (mCount + 7) + ")";
                worksheet.Range[$"L{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"M{mCount + 8}"].Formula = "=SUM(M7:M" + (mCount + 7) + ")";
                worksheet.Range[$"M{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"N{mCount + 8}"].Formula = "=SUM(N7:N" + (mCount + 7) + ")";
                worksheet.Range[$"N{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"O{mCount + 8}"].Formula = "=SUM(O7:O" + (mCount + 7) + ")";
                worksheet.Range[$"O{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"P{mCount + 8}"].Formula = "=SUM(P7:P" + (mCount + 7) + ")";
                worksheet.Range[$"P{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"Q{mCount + 8}"].Formula = "=SUM(Q7:Q" + (mCount + 7) + ")";
                worksheet.Range[$"Q{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"R{mCount + 8}"].Formula = "=SUM(R7:R" + (mCount + 7) + ")";
                worksheet.Range[$"R{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"S{mCount + 8}"].Formula = "=SUM(S7:S" + (mCount + 7) + ")";
                worksheet.Range[$"S{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"T{mCount + 8}"].Formula = "=SUM(T7:T" + (mCount + 7) + ")";
                worksheet.Range[$"T{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"U{mCount + 8}"].Formula = "=SUM(U7:U" + (mCount + 7) + ")";
                worksheet.Range[$"U{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"V{mCount + 8}"].Formula = "=SUM(V7:V" + (mCount + 7) + ")";
                worksheet.Range[$"V{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"W{mCount + 8}"].Formula = "=SUM(W7:W" + (mCount + 7) + ")";
                worksheet.Range[$"W{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"X{mCount + 8}"].Formula = "=SUM(X7:X" + (mCount + 7) + ")";
                worksheet.Range[$"X{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"Y{mCount + 8}"].Formula = "=SUM(Y7:Y" + (mCount + 7) + ")";
                worksheet.Range[$"Y{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"Z{mCount + 8}"].Formula = "=SUM(Z7:Z" + (mCount + 7) + ")";
                worksheet.Range[$"Z{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"AA{mCount + 8}"].Formula = "=SUM(AA7:AA" + (mCount + 7) + ")";
                worksheet.Range[$"AA{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"AB{mCount + 8}"].Formula = "=SUM(AB7:AB" + (mCount + 7) + ")";
                worksheet.Range[$"AB{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"AC{mCount + 8}"].Formula = "=SUM(AC7:AC" + (mCount + 7) + ")";
                worksheet.Range[$"AC{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"AD{mCount + 8}"].Formula = "=SUM(AD7:AD" + (mCount + 7) + ")";
                worksheet.Range[$"AD{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"AE{mCount + 8}"].Formula = "=SUM(AE7:AE" + (mCount + 7) + ")";
                worksheet.Range[$"AE{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"AF{mCount + 8}"].Formula = "=SUM(AF7:AF" + (mCount + 7) + ")";
                worksheet.Range[$"AF{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"AG{mCount + 8}"].Formula = "=SUM(AG7:AG" + (mCount + 7) + ")";
                worksheet.Range[$"AG{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"AH{mCount + 8}"].Formula = "=SUM(AH7:AH" + (mCount + 7) + ")";
                worksheet.Range[$"AH{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range[$"AI{mCount + 8}"].Formula = "=SUM(AI7:AI" + (mCount + 7) + ")";
                worksheet.Range[$"AI{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Columns[7].NumberFormat = "#,###.##";
                worksheet.Columns[8].NumberFormat = "#,###.##";
                worksheet.Columns[9].NumberFormat = "#,###.##";
                worksheet.Columns[10].NumberFormat = "#,###.##";
                worksheet.Columns[11].NumberFormat = "#,###.##";
                worksheet.Columns[12].NumberFormat = "#,###.##";
                worksheet.Columns[13].NumberFormat = "#,###.##";
                worksheet.Columns[14].NumberFormat = "#,###.##";
                worksheet.Columns[15].NumberFormat = "#,###.##";
                worksheet.Columns[16].NumberFormat = "#,###.##";
                worksheet.Columns[17].NumberFormat = "#,###.##";
                worksheet.Columns[18].NumberFormat = "#,###.##";
                worksheet.Columns[19].NumberFormat= "#,###.##";
                worksheet.Columns[20].NumberFormat = "#,###.##";
                worksheet.Columns[21].NumberFormat = "#,###.##";
                worksheet.Columns[22].NumberFormat = "#,###.##";
                worksheet.Columns[23].NumberFormat = "#,###.##";
                worksheet.Columns[24].NumberFormat = "#,###.##";
                worksheet.Columns[25].NumberFormat = "#,###.##";
                worksheet.Columns[26].NumberFormat = "#,###.##";
                worksheet.Columns[27].NumberFormat = "#,###.##";
                worksheet.Columns[28].NumberFormat = "#,###.##";
                worksheet.Columns[29].NumberFormat = "#,###.##";
                worksheet.Columns[30].NumberFormat = "#,###.##";
                worksheet.Columns[31].NumberFormat= "#,###.##";
                worksheet.Columns[32].NumberFormat = "#,###.##";
                worksheet.Columns[33].NumberFormat = "#,###.##";
                worksheet.Columns[34].NumberFormat = "#,###.##";

                worksheet.Range["A7"].FreezePanes();










                //Start the second sheet

                List<PayrollListingModel2> Reports = new List<PayrollListingModel2>();
                foreach (var item in PayrollListings)
                {
                    PayrollListingModel2 payList = new();

                    payList.SrNo = item.SrNo;
                    payList.DepartmentName = item.DepartmentName;
                    payList.Category = item.Category;
                    payList.GradeLevel = item.GradeLevel;
                    payList.EmployeeCode = item.EmployeeCode;
                    payList.FirstName = item.FirstName;
                    payList.LastName = item.LastName;
                    payList.BasicSalary = item.BasicSalary;
                    payList.TotalEarnings = item.TotalEarnings;
                    payList.TotalDeductions = item.TotalDeductions;
                    payList.NetPay = item.NetPay;
                    payList.BankCode = item.BankCode;
                    payList.AccountNumber = item.AccountNumber;
                    payList.BankName = item.BankName;
                    
                    Reports.Add(payList);
                }


                //Disable gridlines in the worksheet
                worksheet2.IsGridLinesVisible = true;
                worksheet2.Workbook.StandardFont = "Arial";

                //Enter text to the cell B1 and apply formatting.
                worksheet2.Range["B1"].Text = "Payroll Listing Report";
                worksheet2.Range["B1"].CellStyle.Font.Bold = true;
                //worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
                worksheet2.Range["B1"].CellStyle.Font.Size = 16;

                //Merge cells
                worksheet2.Range["B1:I1"].Merge();

                //Enter text to the cell B2 and apply formatting.
                worksheet2.Range["B2"].Text = "001-NIGER DELTA DEVELOPMENT COMMISSION";
                worksheet2.Range["B2"].CellStyle.Font.Bold = false;
                //worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
                worksheet2.Range["B2"].CellStyle.Font.Size = 11;

                //Merge cells
                worksheet2.Range["B2:I2"].Merge();

                //Enter text to the cell B3 and apply formatting.
                worksheet2.Range["B3"].Text = $"Current Period: {currPeriod.ToShortDateString()}";
                worksheet2.Range["B23"].CellStyle.Font.Bold = false;
                //worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
                worksheet2.Range["B3"].CellStyle.Font.Size = 11;

                //Merge cells
                worksheet2.Range["B3:I3"].Merge();

                //Enter text to the cell B4 and apply formatting.
                worksheet2.Range["B4"].Text = $"Printed on: {DateTime.Now.ToString()} for Current Period {currPeriod.ToShortDateString()}";
                worksheet2.Range["B4"].CellStyle.Font.Bold = true;
                //worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
                worksheet2.Range["B4"].CellStyle.Font.Size = 11;

                //Merge cells
                worksheet2.Range["B4:I4"].Merge();
                //Apply alignment in the cell D1
                //worksheet.Range["D1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                //worksheet.Range["D1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignTop;

                worksheet2.ImportData(Reports, 6, 1, true);
                worksheet2.Range["A6:AI6"].AutofitColumns();
                worksheet2.Range["A6:AI6"].CellStyle.Font.Bold = true;
                worksheet2.Range["A6:AI6"].CellStyle.Font.Size = 11;

                worksheet2[$"B{mCount + 8}"].Text = "Totals";
                worksheet2[$"B{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet2.Range[$"H{mCount + 8}"].Formula = "=SUM(H7:H" + (mCount + 7) + ")";
                worksheet2.Range[$"H{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet2.Range[$"I{mCount + 8}"].Formula = "=SUM(I7:I" + (mCount + 7) + ")";
                worksheet2.Range[$"I{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet2.Range[$"J{mCount + 8}"].Formula = "=SUM(J7:J" + (mCount + 7) + ")";
                worksheet2.Range[$"J{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet2.Range[$"K{mCount + 8}"].Formula = "=SUM(K7:K" + (mCount + 7) + ")";
                worksheet2.Range[$"K{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet2.Columns[7].NumberFormat = "#,###.##";
                worksheet2.Columns[8].NumberFormat = "#,###.##";
                worksheet2.Columns[9].NumberFormat = "#,###.##";
                worksheet2.Columns[10].NumberFormat = "#,###.##";

                worksheet2.Range["A7"].FreezePanes();







                //Saving the Excel to the MemoryStream 
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                //Set the position as '0'.
                stream.Position = 0;

                //Download the Excel file in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");
                fileStreamResult.FileDownloadName = $"PayrollListing-{DateTime.Now.ToString()}.xlsx";
                return fileStreamResult;
            }
        }
    }
}
