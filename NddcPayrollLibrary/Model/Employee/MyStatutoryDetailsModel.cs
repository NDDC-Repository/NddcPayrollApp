using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Employee
{
    public class MyStatutoryDetailsModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int SID { get; set; }
        public string TaxStatus { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public DateTime TaxStartDate { get; set; }
        public string NHFNumber { get; set; }
        public bool NHFStatus { get; set; }
        public string NHIS { get; set; }
        public bool NHISStatus { get; set; }
        public string MedicalAidName { get; set; }
        public string MedicalAidNumber { get; set; }
        public int PensionFundId { get; set; }
        public string PensionFundNumber { get; set; }
        public bool NSITFSTatus { get; set; }
        public bool ITFStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
