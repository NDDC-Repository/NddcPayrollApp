using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Report;
using Syncfusion.XlsIO;

namespace NddcPayroll.Web.Pages.Reports
{
    public class NHFReportSummaryModel : PageModel
    {
        private readonly ICompanyData compDb;
        private readonly IReportsData repDb;

        public NHFReportSummaryModel(ICompanyData compDb, IReportsData repDb)
        {
            this.compDb = compDb;
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

                worksheet.IsGridLinesVisible = true;
                worksheet.Workbook.StandardFont = "Arial";
                worksheet.Range["A1"].ColumnWidth = 2;

                worksheet.Range["B2"].Text = "National Housing Fund Report By State and Summary by States";
                worksheet.Range["B2"].CellStyle.Font.Bold = true;
                worksheet.Range["B2"].CellStyle.Font.Size = 14;
                worksheet.Range["B2:E2"].Merge();

                //General Header Styles
                IRange hRange;
                hRange = worksheet.Range["A4:G4"];
                hRange.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                hRange.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                hRange.CellStyle.Font.Size = 11;
                hRange.CellStyle.Font.Bold = true;

               
                //Collumn Defiiitions
                worksheet.Range["B4"].Text = "S/N";
                worksheet.Range["B4"].ColumnWidth = 5;
                worksheet.Range["B4"].BorderAround(ExcelLineStyle.Thin);

                worksheet.Range["C4"].Text = "Pay Point";
                worksheet.Range["C4"].ColumnWidth = 18;
                worksheet.Range["C4"].BorderAround(ExcelLineStyle.Thin);

                worksheet.Range["D4"].Text = "No. Of Employees";
                worksheet.Range["D4"].ColumnWidth = 12;
                worksheet.Range["D4"].WrapText=true;
                worksheet.Range["D4"].BorderAround(ExcelLineStyle.Thin);

                worksheet.Range["E4"].Text = "NHF Amount ";
                worksheet.Range["E4"].ColumnWidth = 45;
                worksheet.Range["E4"].BorderAround(ExcelLineStyle.Thin);

                //Populate Fields
                List<MyNHFReportModel> nhfSummary = new();
                nhfSummary = repDb.GetNHFReportSummary();
                int i = 5;

                foreach (var item in nhfSummary)
                {
                    worksheet.Range[$"B{i}"].Value = item.SrNo;
                    worksheet.Range[$"B{i}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range[$"B{i}"].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[$"C{i}"].Value = item.PayPoint;
                    worksheet.Range[$"C{i}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range[$"C{i}"].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[$"D{i}"].Value2 = item.EmpCount;
                    worksheet.Range[$"D{i}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range[$"D{i}"].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[$"E{i}"].Value2 = item.NHFAmount;
                    worksheet.Range[$"E{i}"].NumberFormat = "#,###.##";
                    worksheet.Range[$"E{i}"].BorderAround(ExcelLineStyle.Thin);
                    i = i + 1;
                }

                worksheet.Range[$"B5:E{i}"].BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[$"B{i}:E{i}"].CellStyle.Font.Bold = true;
                worksheet.Range[$"C{i}"].Text = "Totals";
                worksheet.Range[$"B{i}:C{i}"].Merge();
                worksheet.Range[$"B{i}:C{i}"].BorderAround(ExcelLineStyle.Thin);

                worksheet.Range[$"D{i}"].Formula = $"=SUM(D5:D{i-1})";
                worksheet.Range[$"D{i}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[$"D{i}"].BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[$"E{i}"].Formula = $"=SUM(E5:E{i-1})";
                worksheet.Range[$"E{i}"].NumberFormat = "#,###.##";

                worksheet.PageSetup.Orientation = ExcelPageOrientation.Landscape;
                worksheet.PageSetup.FitToPagesTall = 0;

                worksheet.PageSetup.LeftFooter = "&KFF0000 Dynamics 365 - Simple Payroll";
                worksheet.PageSetup.CenterFooter = $"&KFF0000 Printed On: &D at &T";
                worksheet.PageSetup.RightFooter = "&KFF0000 pages &P- of &N";

                //Generate other sheets
                List<MyPayPointModel> Paypoints = new List<MyPayPointModel>();
                Paypoints = compDb.GetAllPayPoints();

                IWorksheet newWorkSheet;
                foreach (var p in Paypoints)
                {
                    newWorkSheet = workbook.Worksheets.Create($"NHF - {p.Code}");

                    newWorkSheet.IsGridLinesVisible = true;
                    newWorkSheet.Workbook.StandardFont = "Arial";
                    newWorkSheet.Range["A1"].ColumnWidth = 2;

                    newWorkSheet.Range["B2"].Text = $"National Housing Fund Listing Report for - {p.PayPoint}";
                    newWorkSheet.Range["B2"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["B2"].CellStyle.Font.Size = 14;
                    newWorkSheet.Range["B2:H2"].Merge();

                    //General Header Styles
                    IRange hRange2;
                    hRange2 = newWorkSheet.Range["A4:H4"];
                    hRange2.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    hRange2.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    hRange2.CellStyle.Font.Size = 11;
                    hRange2.CellStyle.Font.Bold = true;


                    //Collumn Defiiitions
                    newWorkSheet.Range["B4"].Text = "S/N";
                    newWorkSheet.Range["B4"].ColumnWidth = 5;
                    newWorkSheet.Range["B4"].BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.Range["C4"].Text = "Point of Payment";
                    newWorkSheet.Range["C4"].ColumnWidth = 10;
                    newWorkSheet.Range["C4"].WrapText = true;
                    newWorkSheet.Range["C4"].BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.Range["D4"].Text = "Employee Code";
                    newWorkSheet.Range["D4"].ColumnWidth = 10;
                    newWorkSheet.Range["D4"].WrapText = true;
                    newWorkSheet.Range["D4"].BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.Range["E4"].Text = "NHF No ";
                    newWorkSheet.Range["E4"].ColumnWidth = 10;
                    newWorkSheet.Range["E4"].BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.Range["F4"].Text = "Surname ";
                    newWorkSheet.Range["F4"].ColumnWidth = 25;
                    newWorkSheet.Range["F4"].BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.Range["G4"].Text = "Full Names ";
                    newWorkSheet.Range["G4"].ColumnWidth = 25;
                    newWorkSheet.Range["G4"].BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.Range["H4"].Text = "NHF Amount ";
                    newWorkSheet.Range["H4"].ColumnWidth = 15;
                    newWorkSheet.Range["H4"].BorderAround(ExcelLineStyle.Thin);


                    List<MyNHFReportModel> nhfList = new();
                    nhfList = repDb.GetNHFReportByPaypoint(p.Code);
                    int g = 5;
                    foreach (var n in nhfList)
                    {
                        newWorkSheet.Range[$"B{g}"].Value = n.SrNo;
                        newWorkSheet.Range[$"C{g}"].Text = n.PayPoint;
                        newWorkSheet.Range[$"D{g}"].Text = n.EmployeeCode;
                        newWorkSheet.Range[$"E{g}"].Text = n.NHFNumber;
                        newWorkSheet.Range[$"F{g}"].Text = n.LastName;
                        newWorkSheet.Range[$"G{g}"].Text = $"{n.FirstNAme} {n.OtherNames}";
                        newWorkSheet.Range[$"H{g}"].Value2 = n.NHFAmount;
                        g = g + 1;
                    }

                    newWorkSheet.Range[$"B5:H{g}"].BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range[$"B{g}:H{g}"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range[$"C{g}"].Text = "Totals";
                    newWorkSheet.Range[$"B{g}:G{g}"].Merge();
                    newWorkSheet.Range[$"B{g}:G{g}"].BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.Range[$"H{g}"].Formula = $"=SUM(H5:H{g - 1})";
                    newWorkSheet.Range[$"H{g}"].BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range[$"H5:H{g}"].NumberFormat = "#,###.##";

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
                fileStreamResult.FileDownloadName = $"NHF-{DateTime.Now.ToString()}.xlsx";
                return fileStreamResult;
            }
        }
    }
}
