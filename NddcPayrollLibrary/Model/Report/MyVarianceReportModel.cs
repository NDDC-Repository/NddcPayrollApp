using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class MyVarianceReportModel
    {
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }

        private string myFullName;

		public string FullName
		{
			get 
			{
				myFullName = $"{LastName} {FirstName} {OtherNames}";
				return myFullName; 
			}
		}
        public decimal TotalEarningsCurr { get; set; }
        public decimal TotalEarningsPP { get; set; }
        public decimal EarningsVariance { get; set; }
        public double EarningsVariancePerc { get; set; }
        public decimal TotalDedCurr { get; set; }
        public decimal TotalDedPP { get; set; }
        public decimal DedVariance { get; set; }
        public double DedVaraincePerc { get; set; }
        public decimal NetPayCurrent { get; set; }
        public decimal NetPayPP { get; set; }
        public decimal NetPayVariance { get; set; }
        public decimal NetPayVariancePerc { get; set; }
    }
}
