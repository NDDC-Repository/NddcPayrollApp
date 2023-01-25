using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Employee
{
    public class MyAnalysisDetailsModel
    {
        public int GradeLevelId { get; set; }
        public int JobTitleId { get; set; }
        public string Category { get; set; }
        public int DepartmentId { get; set; }
        public int PayPointId { get; set; }
        public int EmployeeId { get; set; }
    }
}
