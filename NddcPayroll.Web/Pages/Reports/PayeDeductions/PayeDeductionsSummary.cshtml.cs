using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Report;
using Syncfusion.Blazor.Grids;
using Syncfusion.Drawing;
using Syncfusion.XlsIO;

namespace NddcPayroll.Web.Pages.Reports.PayeDeductions
{
    public class PayeDeductionsSummaryModel : PageModel
    {
        private readonly IReportsData repDb;

        public PayeDeductionsSummaryModel(IReportsData repDb)
        {
            this.repDb = repDb;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;


                //Create a workbook
                IWorkbook workbook = application.Workbooks.Create(0);
                IWorksheet worksheet = workbook.Worksheets.Create("SUMMARY");
                //IWorksheet worksheet2 = workbook.Worksheets.Create("Payroll Listing-2(Audit)");

                worksheet.IsGridLinesVisible = false;
                worksheet.Workbook.StandardFont = "Arial";
                worksheet.Range["A1"].ColumnWidth = 2;
                worksheet.Zoom = 80;

                //Enter text to the cell B1 and apply formatting.
                worksheet.Range["B2"].Text = "SUMMARY OF PAYE TAX DEDUCTIONS";
                worksheet.Range["B2"].CellStyle.Font.Bold = true;
                worksheet.Range["B2"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                worksheet.Range["B2"].CellStyle.Font.Size = 16;

                //Merge cells
                //worksheet.Range["B2:D2"].Merge();

                worksheet.Range["B3"].Text = "NIGER DELTA DEVELOPMENT COMMISSION";
                worksheet.Range["B3"].CellStyle.Font.Bold = false;
                worksheet.Range["B3"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                worksheet.Range["B3"].CellStyle.Font.Size = 14;

                //Merge cells
                //worksheet.Range["B3:D3"].Merge();

                worksheet.Range["B5"].Text = "TOTAL LOCATIONS:";
                worksheet.Range["B5"].ColumnWidth = 20;
                worksheet.Range["C5"].ColumnWidth = 15;
                worksheet.Range["B5:C5"].CellStyle.Font.Bold = true;
                worksheet.Range["C5"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                worksheet.Range["B5:C5"].CellStyle.Font.Size = 10;
                worksheet.Range["C5"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["B5:F5"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                worksheet.Range["F5"].RowHeight = 20;
                IRange range;
                range = worksheet.Range["C5"];
                range.BorderAround(ExcelLineStyle.Thin);

                worksheet.Range["F2"].Text = DateTime.Now.ToString("MMMM yyyy"); ;
                worksheet.Range["F2"].CellStyle.Font.Bold = true;
                worksheet.Range["F2"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                worksheet.Range["F2"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                worksheet.Range["F2"].CellStyle.Font.Size = 16;

                worksheet.Range["E5"].Text = "TOTAL TAX:";
                worksheet.Range["E5:F5"].CellStyle.Font.Bold = true;
                worksheet.Range["E5"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                worksheet.Range["F5"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                worksheet.Range["F5"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                worksheet.Range["E5:F5"].CellStyle.Font.Size = 10;
                worksheet.Range["F5"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["F5"].BorderAround(ExcelLineStyle.Thin);

                IRange myRange;
                myRange = worksheet.Range["B7:F7"];
                myRange.CellStyle.Font.Bold = true;
                myRange.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                myRange.CellStyle.Font.Size = 10;
                myRange.BorderAround(ExcelLineStyle.Thin);
                myRange.BorderInside(ExcelLineStyle.Thin);
                //myRange.CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                myRange.CellStyle.Color = Color.FromArgb(224, 225, 221);
                myRange.RowHeight = 30;
                myRange.VerticalAlignment = ExcelVAlign.VAlignCenter;

                worksheet.Range["B7:C7"].Merge();
                worksheet.Range["B7"].Text = "LOCATION";
                worksheet.Range["D7"].Text = "NO OF EMPLOYEES";
                worksheet.Range["D7"].ColumnWidth = 30;
                worksheet.Range["E7"].Text = "GROSS INCOME";
                worksheet.Range["E7"].ColumnWidth = 33;
                worksheet.Range["F7"].Text = "TAX FOR LOCATION";
                worksheet.Range["F7"].ColumnWidth = 28;

                List<MyPayeReportSummaryModel> PayeSummaryList = new();
                PayeSummaryList = await repDb.GetPayeSummaryReportAsync();

                int i = 8;
                foreach (var p in PayeSummaryList)
                {
                    worksheet.Range[$"B{i}"].Text = p.Location;
                    worksheet.Range[$"B{i}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range[$"D{i}"].Value2 = p.EmployeeCount;
                    worksheet.Range[$"D{i}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range[$"E{i}"].Value2 = p.GrossIncome;
                    worksheet.Range[$"E{i}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"E{i}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range[$"E{i}"].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[$"F{i}"].Value2 = p.Tax;
                    worksheet.Range[$"F{i}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"F{i}"].BorderAround(ExcelLineStyle.Thin);
                    i = i + 1;
                }

                worksheet.Range[$"B8:F{i}"].BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[$"B{i}:F{i}"].CellStyle.Font.Bold = true;
                worksheet.Range[$"B{i}:F{i}"].CellStyle.Color = Color.FromArgb(224, 225, 221);
                worksheet.Range[$"B{i}"].Text = "TOTAL TAX REMITED";
                worksheet.Range[$"B{i}:C{i}"].Merge();
                worksheet.Range[$"B{i}:C{i}"].BorderAround(ExcelLineStyle.Thin);

                worksheet.Range[$"D{i}"].Formula = $"=SUM(D8:D{i - 1})";
                worksheet.Range[$"D{i}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[$"D{i}"].BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[$"E{i}"].Formula = $"=SUM(E8:E{i - 1})";
                worksheet.Range[$"E{i}"].NumberFormat = "#,###.##";
                worksheet.Range[$"E{i}"].BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[$"F{i}"].Formula = $"=SUM(F8:E{i - 1})";
                worksheet.Range[$"F{i}"].NumberFormat = "#,###.##";
                worksheet.Range[$"F{i}"].BorderAround(ExcelLineStyle.Thin);
                
                worksheet.Range["C5"].Value = $"=COUNT(D8:D{i - 1})";
                worksheet.Range["F5"].Value = $"=(F{i})";
                worksheet.Range["F5"].NumberFormat = "#,###.##";
                worksheet.Range[$"E8:F{i}"].CellStyle.Color = Color.FromArgb(224, 225, 221);

                worksheet.PageSetup.Orientation = ExcelPageOrientation.Landscape;
                worksheet.PageSetup.FitToPagesTall = 0;

                worksheet.PageSetup.LeftFooter = "&KFF0000 Dynamics 365 - Simple Payroll";
                worksheet.PageSetup.CenterFooter = $"&KFF0000 Printed On: &D at &T";
                worksheet.PageSetup.RightFooter = "&KFF0000 pages &P- of &N";

                //Create other sheets
                IWorksheet newWorkSheet;
                foreach (var p in PayeSummaryList)
                {
                    newWorkSheet = workbook.Worksheets.Create($"PAYE - {p.Location}");

                    newWorkSheet.IsGridLinesVisible = false;
                    newWorkSheet.Workbook.StandardFont = "Arial";
                    newWorkSheet.Range["A1"].ColumnWidth = 2;
                    newWorkSheet.Zoom = 80;

                    //Enter text to the cell B1 and apply formatting.
                    newWorkSheet.Range["B2"].Text = "SUMMARY OF PAYE TAX DEDUCTIONS";
                    newWorkSheet.Range["B2"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["B2"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                    newWorkSheet.Range["B2"].CellStyle.Font.Size = 16;

                    //Merge cells
                    //worksheet.Range["B2:D2"].Merge();

                    newWorkSheet.Range["B3"].Text = "NIGER DELTA DEVELOPMENT COMMISSION";
                    newWorkSheet.Range["B3"].CellStyle.Font.Bold = false;
                    newWorkSheet.Range["B3"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                    newWorkSheet.Range["B3"].CellStyle.Font.Size = 14;

                    //Merge cells
                    //worksheet.Range["B3:D3"].Merge();

                    newWorkSheet.Range["B5"].Text = "BENEFICIARY:";
                    newWorkSheet.Range["B5:C5"].Merge();
                    newWorkSheet.Range["B5:C5"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["D5"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                    newWorkSheet.Range["B5:C5"].CellStyle.Font.Size = 10;
                    newWorkSheet.Range["D5"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["B5:F5"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    newWorkSheet.Range["F5"].RowHeight = 20;
                    //newWorkSheet.Range["D5"].BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.Range["H2"].Text = DateTime.Now.ToString("MMMM yyyy"); ;
                    newWorkSheet.Range["H2"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["H2"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["H2"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                    newWorkSheet.Range["H2"].CellStyle.Font.Size = 16;

                    newWorkSheet.Range["G5"].Text = "NUMBER OF EMPLOYEES:";
                    newWorkSheet.Range["G5:H5"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["G5"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["H5"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                    newWorkSheet.Range["G5:H5"].CellStyle.Font.Size = 10;
                    newWorkSheet.Range["G5"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["H5"].BorderAround(ExcelLineStyle.Thin);

                    IRange myRange2;
                    myRange2 = newWorkSheet.Range["B7:H7"];
                    myRange2.CellStyle.Font.Bold = true;
                    myRange2.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    myRange2.CellStyle.Font.Size = 10;
                    myRange2.BorderAround(ExcelLineStyle.Thin);
                    myRange2.BorderInside(ExcelLineStyle.Thin);
                    //myRange.CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    myRange2.CellStyle.Color = Color.FromArgb(224, 225, 221);
                    myRange2.RowHeight = 30;
                    myRange2.VerticalAlignment = ExcelVAlign.VAlignCenter;

                    newWorkSheet.Range["B7"].Text = "S/N";
                    newWorkSheet.Range["B7"].ColumnWidth = 5;
                    newWorkSheet.Range["C7"].Text = "STAFF TIN NUMBER";
                    newWorkSheet.Range["C7"].WrapText = true;
                    newWorkSheet.Range["C7"].ColumnWidth = 15;
                    newWorkSheet.Range["D7"].Text = "STAFF ID NUMBER";
                    newWorkSheet.Range["D7"].ColumnWidth = 17;
                    newWorkSheet.Range["E7"].Text = "SURNAME";
                    newWorkSheet.Range["E7"].ColumnWidth = 35;
                    newWorkSheet.Range["F7"].Text = "OTHER NAMES";
                    newWorkSheet.Range["F7"].ColumnWidth = 35;
                    newWorkSheet.Range["G7"].Text = "MONTHLY GROSS INCOME";
                    newWorkSheet.Range["G7"].WrapText = true;
                    newWorkSheet.Range["G7"].ColumnWidth = 17;
                    newWorkSheet.Range["H7"].Text = "MONTHLY TAX DEDUCTED";
                    newWorkSheet.Range["H7"].WrapText = true;
                    newWorkSheet.Range["H7"].ColumnWidth = 17;

                    List<MyStaffPayeDeductionsModel> StaffPayeList = new();
                    StaffPayeList = await repDb.GetStaffPayeDeductionByLocationAsync(p.Location);

                    int v = 8;
                    foreach (var s in StaffPayeList)
                    {
                        newWorkSheet.Range[$"B{v}"].Value2 = s.SrNo;
                        newWorkSheet.Range[$"B{v}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        newWorkSheet.Range[$"D{v}"].Text = s.EmployeeCode;
                        newWorkSheet.Range[$"D{v}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        newWorkSheet.Range[$"E{v}"].Value2 = s.LastName;
                        //newWorkSheet.Range[$"E{v}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        newWorkSheet.Range[$"F{v}"].Value2 = s.FirstName;
                        //newWorkSheet.Range[$"F{v}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        newWorkSheet.Range[$"G{v}"].Value2 = s.TotalEarnings;
                        newWorkSheet.Range[$"G{v}"].NumberFormat = "#,###.##";
                        newWorkSheet.Range[$"G{v}"].BorderAround(ExcelLineStyle.Thin);
                        newWorkSheet.Range[$"H{v}"].Value2 = s.Tax;
                        newWorkSheet.Range[$"H{v}"].NumberFormat = "#,###.##";
                        newWorkSheet.Range[$"H{v}"].BorderAround(ExcelLineStyle.Thin);
                        v = v + 1;
                    }

                    newWorkSheet.Range[$"B8:H{v}"].BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range[$"B{v}:H{v}"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range[$"B{v}:H{v}"].CellStyle.Color = Color.FromArgb(224, 225, 221);
                    newWorkSheet.Range[$"B{v}"].Text = "TOTAL TAX REMITED";
                    newWorkSheet.Range[$"B{v}:F{v}"].Merge();
                    newWorkSheet.Range[$"B{v}:F{v}"].BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.Range[$"G{v}"].Formula = $"=SUM(G8:G{v - 1})";
                    //newWorkSheet.Range[$"G{v}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range[$"G{v}"].BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range[$"G{v}"].NumberFormat = "#,###.##";
                    newWorkSheet.Range[$"H{v}"].Formula = $"=SUM(H8:H{v - 1})";
                    newWorkSheet.Range[$"H{v}"].NumberFormat = "#,###.##";
                    newWorkSheet.Range[$"H{v}"].BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.Range["H5"].Value = $"=COUNT(B8:B{v - 1})";
                    newWorkSheet.Range["D5"].Text = p.Location;
                    newWorkSheet.Range[$"G8:H{v}"].CellStyle.Color = Color.FromArgb(224, 225, 221);

                    newWorkSheet.PageSetup.Orientation = ExcelPageOrientation.Landscape;
                    newWorkSheet.PageSetup.FitToPagesTall = 0;

                    newWorkSheet.PageSetup.LeftFooter = "&KFF0000 Dynamics 365 - Simple Payroll";
                    newWorkSheet.PageSetup.CenterFooter = $"&KFF0000 Printed On: &D at &T";
                    newWorkSheet.PageSetup.RightFooter = "&KFF0000 pages &P- of &N";

                }



                    //Saving the Excel to the MemoryStream 
                    MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                //Set the position as '0'.
                stream.Position = 0;

                //Download the Excel file in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");
                fileStreamResult.FileDownloadName = $"PAYE-{DateTime.Now.ToString()}.xlsx";
                return fileStreamResult;
            }
        }
    }
}
