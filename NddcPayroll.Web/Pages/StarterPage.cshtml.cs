using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Reports;

namespace NddcPayroll.Web.Pages
{
    public class StarterPageModel : PageModel
    {
        private readonly IReportsData repDb;

        public StarterPageModel(IReportsData repDb)
        {
            this.repDb = repDb;
        }
        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            await Task.Run(() => repDb.UpdateEmployeesPayrollAsync());
        }
    }
}
