using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Report;
using Syncfusion.Drawing;
using Syncfusion.XlsIO;


namespace NddcPayroll.Web.Pages.Reports
{
    public class PFASummaryReportModel : PageModel
    {
        private readonly IReportsData repDb;
        private readonly ICompanyData compDb;

        public List<PensionSummaryModel> PFASummaryList { get; set; }
        public List<PensionAdminModel> PensionAdmins { get; set; }
        

        public PFASummaryReportModel(IReportsData repDb, ICompanyData compDb)
        {
            this.repDb = repDb;
            this.compDb = compDb;
        }
        public void OnGet()
        {
        }
        //public void OnPost()
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

                //Enter text to the cell B1 and apply formatting.
                worksheet.Range["B2"].Text = "SUMMARY OF PENSION CONTRIBUTIONS";
                worksheet.Range["B2"].CellStyle.Font.Bold = true;
                worksheet.Range["B2"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                worksheet.Range["B2"].CellStyle.Font.Size = 16;

                //Merge cells
                worksheet.Range["B2:I2"].Merge();

                worksheet.Range["B3"].Text = "NIGER DELTA DEVELOPMENT COMMISSION";
                worksheet.Range["B3"].CellStyle.Font.Bold = false;
                worksheet.Range["B3"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                worksheet.Range["B3"].CellStyle.Font.Size = 14;

                //Merge cells
                worksheet.Range["B3:I3"].Merge();

                worksheet.Range["B5"].Text = "NO OF PFA FUNDS:";
                worksheet.Range["C5"].Value = "8";
                worksheet.Range["C5"].CellStyle.Font.Bold = true;
                worksheet.Range["C5"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                worksheet.Range["B5:C5"].CellStyle.Font.Size = 10;
                worksheet.Range["C5"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                IRange range;
                range = worksheet.Range["C5"];
                range.BorderAround(ExcelLineStyle.Medium);

                worksheet.Range["B6"].Text = "TOTAL CONTRIBUTION:";
                worksheet.Range["C6"].CellStyle.Font.Bold = true;
                worksheet.Range["C6"].Value = "330,734,959.82";
                worksheet.Range["C6"].NumberFormat = "#,###.##";
                worksheet.Range["C6"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                worksheet.Range["B6:C6"].CellStyle.Font.Size = 10;
                worksheet.Range["C6"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                range = worksheet.Range["C6"];
                range.BorderAround(ExcelLineStyle.Medium);

                worksheet.Range["B8"].Text = "PFA CODE";
                worksheet.Range["B8"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["B8"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                worksheet.Range["B8"].CellStyle.Font.Size = 8;
                worksheet.Range["B8"].ColumnWidth = 19;
                range = worksheet.Range["B8:B9"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["B8"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["B8:B9"].Merge();

                worksheet.Range["C8"].Text = "PFA NAME";
                worksheet.Range["C8"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["C8"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                worksheet.Range["C8"].CellStyle.Font.Size = 8;
                worksheet.Range["C8"].ColumnWidth = 34;
                range = worksheet.Range["C8:C9"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["C8"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["C8:C9"].Merge();

                worksheet.Range["D8"].Text = "NO. OF EMPLOYEES";
                worksheet.Range["D8"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["D8"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                worksheet.Range["D8"].CellStyle.Font.Size = 8;
                worksheet.Range["D8"].ColumnWidth = 25;
                range = worksheet.Range["D8:D9"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["D8"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["D8:D9"].Merge();

                worksheet.Range["E8"].Text = "ARREARS";
                worksheet.Range["E8"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["E8"].CellStyle.Font.Bold = true;
                worksheet.Range["E8"].CellStyle.Font.Size = 10;
                worksheet.Range["E8"].ColumnWidth = 30;
                range = worksheet.Range["E8:F8"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["E8"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["E8:F8"].Merge();
                worksheet.Range["E9"].Text = "EMPLOYER";
                worksheet.Range["E9"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["E9"].ColumnWidth = 15;
                range = worksheet.Range["E9"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["E9"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["F9"].Text = "EMPLOYEE";
                worksheet.Range["F9"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["F9"].ColumnWidth = 15;
                range = worksheet.Range["F9"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["F9"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                worksheet.Range["G8"].Text = "CURRENT";
                worksheet.Range["G8"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["G8"].CellStyle.Font.Bold = true;
                worksheet.Range["G8"].CellStyle.Font.Size = 10;
                worksheet.Range["G8"].ColumnWidth = 70;
                range = worksheet.Range["G8:J8"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["G8"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["G8:J8"].Merge();
                worksheet.Range["G9"].Text = "EMPLOYER";
                worksheet.Range["G9"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["G9"].ColumnWidth = 15;
                range = worksheet.Range["G9"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["G9"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["H9"].Text = "EMPLOYEE";
                worksheet.Range["H9"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["H9"].ColumnWidth = 15;
                range = worksheet.Range["H9"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["H9"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["I9"].Text = "VOLUNTARY EMPLOYER";
                worksheet.Range["I9"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["I9"].ColumnWidth = 18;
                range = worksheet.Range["I9"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["I9"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["J9"].Text = "VOLUNTARY EMPLOYEE";
                worksheet.Range["J9"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["J9"].ColumnWidth = 18;
                range = worksheet.Range["J9"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["J9"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                worksheet.Range["K8"].Text = "TOTAL";
                worksheet.Range["K8"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["K8"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                worksheet.Range["K8"].CellStyle.Font.Size = 8;
                worksheet.Range["K8"].ColumnWidth = 15;
                range = worksheet.Range["K8:K9"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range["K8"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["k8:K9"].Merge();

                PFASummaryList = await Task.Run(() => repDb.GetPFASummary());
                int i = 10;
                
                foreach (var item in PFASummaryList)
                {
                    worksheet.Range[$"B{i}"].Value = item.PFACode;
                    worksheet.Range[$"C{i}"].Value = item.PFAName;
                    worksheet.Range[$"D{i}"].Value2 = item.EmployeeCount;
                    worksheet.Range[$"E{i}"].Value2 = 0.00;
                    worksheet.Range[$"F{i}"].Value2 = 0.00;
                    worksheet.Range[$"G{i}"].Value2 = item.EmployerPension;
                    worksheet.Range[$"H{i}"].Value2 = item.EmployeePension;
                    worksheet.Range[$"I{i}"].Value2 = 0.00;
                    worksheet.Range[$"J{i}"].Value2 = 0.00;
                    worksheet.Range[$"K{i}"].Value2 = item.Total;
                    i =i+1;
                }

                int r = i - 1;
                worksheet.Range[$"B{i}"].Text = "GRAND TOTALS";
                worksheet.Range[$"D{i}"].Formula = $"=SUM(D10:D{r})";
                worksheet.Range[$"E{i}"].Formula = $"=SUM(E10:E{r})";
                worksheet.Range[$"F{i}"].Formula = $"=SUM(F10:F{r})";
                worksheet.Range[$"G{i}"].Formula = $"=SUM(G10:G{r})";
                worksheet.Range[$"H{i}"].Formula = $"=SUM(H10:H{r})";
                worksheet.Range[$"I{i}"].Formula = $"=SUM(I10:I{r})";
                worksheet.Range[$"J{i}"].Formula = $"=SUM(J10:J{r})";
                worksheet.Range[$"K{i}"].Formula = $"=SUM(K10:K{r})";
                //worksheet.Range[$"M{mCount + 8}"].Formula = "=SUM(M7:M" + (mCount + 7) + ")";


                //PFASummaryList = await Task.Run(() => repDb.GetPFASummary());
                //worksheet.ImportData(PFASummaryList, 10, 2, false);
                worksheet.Range[$"D10:K{i}"].NumberFormat = "#,###.##";
                worksheet.Range[$"B8:K{r}"].CellStyle.Font.Size = 8;
                worksheet.Range[$"B{i}:K{i}"].CellStyle.Font.Bold = true;
                worksheet.Range[$"B{i}:K{i}"].CellStyle.Font.Size = 10;
                worksheet.Range[$"E{i}:K{i}"].NumberFormat = "#,###.##";
                range = worksheet.Range[$"B10:K{i}"];
                range.BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[$"B{i}:K{i}"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                worksheet.Range["C6"].Value = $"=K{i}";
                worksheet.Range["C6"].NumberFormat = "#,###.##";
                worksheet.Range["C5"].Value = $"=COUNT(D10:D{r})";

                worksheet.Range[$"K10:K{r}"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                worksheet.TabColor = ExcelKnownColors.Green;

                worksheet.Range["E8"].CellStyle.Font.Size = 9;
                worksheet.Range["G8"].CellStyle.Font.Size = 9;





                //Create other sheets

                PensionAdmins = compDb.GetAllPensionAdmins();

                IWorksheet newWorkSheet;
                foreach (var item in PensionAdmins)
                {
                    newWorkSheet = workbook.Worksheets.Create($"PFA - {item.Code}");

                    newWorkSheet.IsGridLinesVisible = false;
                    newWorkSheet.Workbook.StandardFont = "Arial";
                    newWorkSheet.Range["A1"].ColumnWidth = 2;
                    newWorkSheet.Range["L1"].ColumnWidth = 5;

                    //Enter text to the cell B1 and apply formatting.
                    newWorkSheet.Range["B2"].Text = "SCHEDULE OF PENSION CONTRIBUTIONS";
                    newWorkSheet.Range["B2"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["B2"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                    newWorkSheet.Range["B2"].CellStyle.Font.Size = 16;
                    //Merge cells
                    newWorkSheet.Range["B2:I2"].Merge();

                    newWorkSheet.Range["B3"].Text = "NIGER DELTA DEVELOPMENT COMMISSION";
                    newWorkSheet.Range["B3"].CellStyle.Font.Bold = false;
                    newWorkSheet.Range["B3"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                    newWorkSheet.Range["B3"].CellStyle.Font.Size = 14;
                    //Merge cells
                    newWorkSheet.Range["B3:I3"].Merge();

                    newWorkSheet.Range["B5"].Text = "PFA NAME:";
                    newWorkSheet.Range["C5"].Text = item.Description;
                    newWorkSheet.Range["C5"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["B5:C5"].CellStyle.Font.Size = 10;

                    newWorkSheet.Range["B6"].Text = "PFA CODE:";
                    newWorkSheet.Range["C6"].Text = item.Code;
                    newWorkSheet.Range["C6"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["B6:C6"].CellStyle.Font.Size = 10;

                    newWorkSheet.Range["B8"].Text = "PFA TOTAL:";
                    newWorkSheet.Range["C8"].Value = "330,734,959.82";
                    newWorkSheet.Range["C8"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["C8"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                    newWorkSheet.Range["B8:C8"].CellStyle.Font.Size = 10;
                    newWorkSheet.Range["C8"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    IRange range2;
                    range2 = newWorkSheet.Range["C8"];
                    range2.BorderAround(ExcelLineStyle.Medium);

                    newWorkSheet.Range["B9"].Text = "PFA EMPLOYEES:";
                    newWorkSheet.Range["C9"].Value = "8";
                    newWorkSheet.Range["C9"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["C9"].NumberFormat = "#,###.##";
                    newWorkSheet.Range["C9"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                    newWorkSheet.Range["B9:C9"].CellStyle.Font.Size = 10;
                    newWorkSheet.Range["C9"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    range2 = newWorkSheet.Range["C9"];
                    range2.BorderAround(ExcelLineStyle.Medium);

                    newWorkSheet.Range["K2"].Text = DateTime.Now.ToString("MMMM yyyy");
                    newWorkSheet.Range["K2"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["K2"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K2"].CellStyle.Font.RGBColor = Color.FromArgb(0, 127, 100);
                    newWorkSheet.Range["K2"].CellStyle.Font.Size = 16;

                    newWorkSheet.Range["J5"].Text = "MODE OF PAYMENT:";
                    newWorkSheet.Range["K5"].Text = "Cheque";
                    newWorkSheet.Range["J5"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K5"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K5"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["J5:K5"].CellStyle.Font.Size = 10;

                    newWorkSheet.Range["J6"].Text = "RECEIVING BANK:";
                    newWorkSheet.Range["K6"].Text = "USER INPUT";
                    newWorkSheet.Range["J6"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K6"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K6"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["J6:K6"].CellStyle.Font.Size = 10;

                    newWorkSheet.Range["J7"].Text = "RECEIVING BANK ACC:";
                    newWorkSheet.Range["K7"].Text = "USER INPUT";
                    newWorkSheet.Range["J7"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K7"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K7"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["J7:K7"].CellStyle.Font.Size = 10;

                    newWorkSheet.Range["J8"].Text = "ACCOUNT NAME:";
                    newWorkSheet.Range["K8"].Text = "IENT COMMISSION";
                    newWorkSheet.Range["J8"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K8"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K8"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["J8:K8"].CellStyle.Font.Size = 10;

                    newWorkSheet.Range["J9"].Text = "INSTRUMENT NO:";
                    newWorkSheet.Range["K9"].Text = "Cheque";
                    newWorkSheet.Range["J9"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K9"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                    newWorkSheet.Range["K9"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["J9:K9"].CellStyle.Font.Size = 10;

                    newWorkSheet.Range["B11"].Text = "CODE";
                    newWorkSheet.Range["B11"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["B11"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    newWorkSheet.Range["B11"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["B11"].ColumnWidth = 19;
                    range2 = newWorkSheet.Range["B11:B13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["B11"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["B11:B13"].Merge();

                    newWorkSheet.Range["C11"].Text = "EMPLOYEE NAME";
                    newWorkSheet.Range["C11"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["C11"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    newWorkSheet.Range["C11"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["C11"].ColumnWidth = 34;
                    range2 = newWorkSheet.Range["C11:C13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["C11"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["C11:C13"].Merge();

                    newWorkSheet.Range["D11"].Text = "RSA PIN NO.";
                    newWorkSheet.Range["D11"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["D11"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    newWorkSheet.Range["D11"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["D11"].ColumnWidth = 25;
                    range2 = newWorkSheet.Range["D11:D13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["D11"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["D11:D13"].Merge();

                    newWorkSheet.Range["E11"].Text = "ARREARS";
                    newWorkSheet.Range["E11"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["E11"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["E11"].CellStyle.Font.Size = 10;
                    newWorkSheet.Range["E11"].ColumnWidth = 30;
                    range2 = newWorkSheet.Range["E11:F13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["E11"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["E11:F11"].Merge();
                    newWorkSheet.Range["E12"].Text = "";
                    newWorkSheet.Range["E12"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["E12"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["E12"].ColumnWidth = 30;
                    range2 = newWorkSheet.Range["E12:F12"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["E12:F12"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["E12:F12"].Merge();
                    newWorkSheet.Range["E13"].Text = "EMPLOYER";
                    newWorkSheet.Range["E13"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["E13"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["E13"].ColumnWidth = 15;
                    range2 = newWorkSheet.Range["E13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["E13"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["F13"].Text = "EMPLOYEE";
                    newWorkSheet.Range["F13"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["F13"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["F13"].ColumnWidth = 15;
                    range2 = newWorkSheet.Range["F13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["F13"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                    newWorkSheet.Range["G11"].Text = "CURRENT CONTRIBUTIONS";
                    newWorkSheet.Range["G11"].CellStyle.Font.Bold = true;
                    newWorkSheet.Range["G11"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["G11"].CellStyle.Font.Size = 10;
                    newWorkSheet.Range["G11"].ColumnWidth = 70;
                    range2 = newWorkSheet.Range["G11:J11"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["G11"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["G11:J11"].Merge();
                    newWorkSheet.Range["G12"].Text = DateTime.Now.ToString("MMMM yyyy");
                    newWorkSheet.Range["G12"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["G12"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["G12"].ColumnWidth = 30;
                    range2 = newWorkSheet.Range["G12:J12"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["G12:J12"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["G12:J12"].Merge();
                    newWorkSheet.Range["G13"].Text = "EMPLOYER";
                    newWorkSheet.Range["G13"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["G13"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["G13"].ColumnWidth = 15;
                    range2 = newWorkSheet.Range["G13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["G13"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["H13"].Text = "EMPLOYEE";
                    newWorkSheet.Range["H13"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["H13"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["H13"].ColumnWidth = 15;
                    range2 = newWorkSheet.Range["H13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["H13"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["I13"].Text = "VOLUNTARY EMPLOYER";
                    newWorkSheet.Range["I13"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["I13"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["I13"].ColumnWidth = 18;
                    range2 = newWorkSheet.Range["I13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["I13"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["J13"].Text = "VOLUNTARY EMPLOYEE";
                    newWorkSheet.Range["J13"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["J13"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["J13"].ColumnWidth = 18;
                    range2 = newWorkSheet.Range["J13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["J13"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                    newWorkSheet.Range["K11"].Text = "TOTAL CONTRIBUTION";
                    newWorkSheet.Range["K11"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    newWorkSheet.Range["K11"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    newWorkSheet.Range["K11"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range["K11"].ColumnWidth = 15;
                    range2 = newWorkSheet.Range["K11:K13"];
                    range2.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range["K11"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range["k11:K13"].Merge();

                    List<PensionSummaryModel> EmpPensionList = new();
                    EmpPensionList = repDb.GetPFASummaryForEmployees(item.Id);
                    int j = 14;
                    foreach (var e in EmpPensionList)
                    {
                        newWorkSheet.Range[$"B{j}"].Value = e.EmployeeCode;
                        newWorkSheet.Range[$"C{j}"].Value = e.EmployeeName;
                        newWorkSheet.Range[$"D{j}"].Value2 = e.RSAPin;
                        newWorkSheet.Range[$"E{j}"].Value2 = 0.00;
                        newWorkSheet.Range[$"F{j}"].Value2 = 0.00;
                        newWorkSheet.Range[$"G{j}"].Value2 = e.EmployerPension;
                        newWorkSheet.Range[$"H{j}"].Value2 = e.EmployeePension;
                        newWorkSheet.Range[$"I{j}"].Value2 = 0.00;
                        newWorkSheet.Range[$"J{j}"].Value2 =e.VoluntaryPension;
                        newWorkSheet.Range[$"K{j}"].Value2 = e.Total;
                        j = j + 1;
                    }
                    //j = j - 1;
                    int k = j + 1;
                    newWorkSheet.Range[$"B14:K{j}"].CellStyle.Font.Size = 8;
                    newWorkSheet.Range[$"B14:B{j}"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                    newWorkSheet.Range[$"E14:K{k}"].NumberFormat = "#,###.##";
                    newWorkSheet.Range[$"B{k}:K{k}"].CellStyle.Font.Bold = true;
                    range = newWorkSheet.Range[$"B{k}:K{k}"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    newWorkSheet.Range[$"B{k}:K{k}"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    newWorkSheet.Range[$"K14:K{k}"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                    newWorkSheet.Range[$"B{k}"].Text = "TOTAL PENSION CONTRIBUTION FOR PFA";
                    newWorkSheet.Range[$"E{k}"].Formula = $"=SUM(E14:E{j})";
                    newWorkSheet.Range[$"F{k}"].Formula = $"=SUM(F14:F{j})";
                    newWorkSheet.Range[$"G{k}"].Formula = $"=SUM(G14:G{j})";
                    newWorkSheet.Range[$"H{k}"].Formula = $"=SUM(H14:H{j})";
                    newWorkSheet.Range[$"I{k}"].Formula = $"=SUM(I14:I{j})";
                    newWorkSheet.Range[$"J{k}"].Formula = $"=SUM(J14:J{j})";
                    newWorkSheet.Range[$"K{k}"].Formula = $"=SUM(K14:K{j})";

                    newWorkSheet.Range["C8"].Value = $"=K{k}";
                    newWorkSheet.Range["C8"].NumberFormat = "#,###.##";
                    newWorkSheet.Range["C9"].Value = $"=COUNT(B14:B{j-1})";
                    newWorkSheet.Range[$"D13:D{k}"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    newWorkSheet.Range[$"F13:F{k}"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    newWorkSheet.Range[$"J13:J{k}"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    range = newWorkSheet.Range[$"B14:K{k}"];
                    range.BorderAround(ExcelLineStyle.Thin);

                    newWorkSheet.TabColor = ExcelKnownColors.Green;
                }


                ////Encrypt the workbook with password
                //workbook.PasswordToOpen = "password";

                ////Set the password to modify the workbook
                //workbook.SetWriteProtectionPassword("modify_password");

                ////Set the workbook as read-only
                //workbook.ReadOnlyRecommended = true;



                //Saving the Excel to the MemoryStream 
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                //Set the position as '0'.
                stream.Position = 0;
                
                //Download the Excel file in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");
                fileStreamResult.FileDownloadName = $"PENCOM-{DateTime.Now.ToString()}.xlsx";
                return fileStreamResult;
            }
        }
    }
}
