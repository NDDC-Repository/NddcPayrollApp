using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Reports;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using NddcPayrollLibrary.Model.Report;

namespace NddcPayroll.Web.Pages
{
    public class FreePageModel : PageModel
    {
        private readonly IReportsData repDb;

        public int MyProperty { get; set; }
        public List<MyPayRollListModel> PayrollList { get; set; }
        public FreePageModel(IReportsData repDb)
        {
            this.repDb = repDb;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            PayrollList = await Task.Run(() => repDb.GetPayrollListReport2Async());
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;

                //Create a workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];

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
                worksheet.Range["B3"].Text = "Current Period: 31/06/2023";
                worksheet.Range["B23"].CellStyle.Font.Bold = false;
                //worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
                worksheet.Range["B3"].CellStyle.Font.Size = 11;

                //Merge cells
                worksheet.Range["B3:I3"].Merge();

                //Enter text to the cell B4 and apply formatting.
                worksheet.Range["B4"].Text = $"Printed on: {DateTime.Now.ToString()} for Current Period 31/06/2023";
                worksheet.Range["B4"].CellStyle.Font.Bold = true;
                //worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
                worksheet.Range["B4"].CellStyle.Font.Size = 11;

                //Merge cells
                worksheet.Range["B4:I4"].Merge();
                //Apply alignment in the cell D1
                //worksheet.Range["D1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                //worksheet.Range["D1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignTop;

                worksheet.ImportData(PayrollList, 6, 1, true);
                worksheet.Range["A6:AC6"].AutofitColumns();
                worksheet.Range["A6:AC6"].CellStyle.Font.Bold = true;
                worksheet.Range["A6:AC6"].CellStyle.Font.Size = 11;

                int mCount = PayrollList.Count();

                worksheet[$"C{mCount + 8}"].Text = "Totals";
                worksheet.Range[$"C{mCount + 8}"].CellStyle.Font.Bold = true;

                worksheet.Range["A7"].FreezePanes();




                //Saving the Excel to the MemoryStream 
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                //Set the position as '0'.
                stream.Position = 0;

                //Download the Excel file in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");
                fileStreamResult.FileDownloadName = "CreateExcel.xlsx";
                return fileStreamResult;
            }
        }
    }
}
