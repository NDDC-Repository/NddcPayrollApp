using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NddcPayrollLibrary.Model.Payroll
{
    public class MyAllowanceModel
    {
        public int Housing { get; set; }
        public int Furniture { get; set; }
        public int Transportation { get; set; }
        public int Utility { get; set; }
        public int LeaveGrant { get; set; }
        public int Medical { get; set; }
        public int Education { get; set; }
        public int VehicleMaintenance { get; set; }
        public int Security { get; set; }
        public int Hazard { get; set; }
    }
}
