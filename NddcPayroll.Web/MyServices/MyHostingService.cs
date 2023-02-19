using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace NddcPayroll.Web.MyServices
{
    public class MyHostingService
    {
        private IWebHostEnvironment _hostEnvironment;
        public string GetPath(string filename)
        {
            string path = Path.Combine(_hostEnvironment.WebRootPath, filename);
            return path;
        }
    }
}
