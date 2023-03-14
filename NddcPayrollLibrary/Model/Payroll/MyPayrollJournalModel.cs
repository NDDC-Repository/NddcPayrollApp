using NddcPayrollLibrary.Model.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Payroll
{
    public class MyPayrollJournalModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string MaidenName { get; set; }
        public string SpouseName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int SID { get; set; }
        public MyStatesModel State { get; set; }
        public string EmploymentStatus { get; set; }
        public DateTime DateEngaged { get; set; }
        public string JobTitle { get; set; }
        public string TaxStateProvince { get; set; }
        public string TaxStatus { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public DateTime TaxStartDate { get; set; }
        public string NHFNumber { get; set; }
        public bool NHFStatus { get; set; }
        public string NHIS { get; set; }
        public bool NHISStatus { get; set; }
        public string MedicalAidName { get; set; }
        public int PensionFundId { get; set; }
        public string PensionFundNumber { get; set; }
        public bool NSITFStatus { get; set; }
        public bool ITFStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string PayInfo1 { get; set; }
        public string PayInfo2 { get; set; }
        public string SortCode { get; set; }
        public int GradeLevelId { get; set; }
        public int JobTitleId { get; set; }
        public string Category { get; set; }
        public int DepartmentId { get; set; }
        public string PayPoint { get; set; }
        public decimal Insurance { get; set; }
        public decimal SecretarialAllow { get; set; }
        public decimal CooperativeDed { get; set; }
        public decimal VoluntaryPension { get; set; }
        public decimal TransportAllow { get; set; }
        public decimal HousingAllow { get; set; }
        public decimal FurnitureAllow { get; set; }
        public decimal MealAllow { get; set; }
        public decimal UtilityAllow { get; set; }
        public decimal EducationAllow { get; set; }
        public decimal SecurityAllow { get; set; }
        public decimal DomesticServantAllow { get; set; }
        public decimal MedicalAllow { get; set; }
        public decimal DriverAllow { get; set; }
        public decimal VehicleAllow { get; set; }
        public decimal HazardAllow { get; set; }
        public decimal Tax { get; set; }
        public decimal NHF { get; set; }
        public decimal Pension { get; set; }
        public decimal JSA { get; set; }
        public decimal SSA { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal MonthlyGross { get; set; }
        public decimal LeaveAllow { get; set; }
        public decimal ActingAllow { get; set; }
        public decimal ShiftAllow { get; set; }
        public decimal UniformAllow { get; set; }
        public string TaxCalc { get; set; }
        public decimal Arreas { get; set; }
        public decimal EmployerPension { get; set; }
        public decimal TaxAdjustment { get; set; }
        public int PayrollJournalTitleID { get; set; }
        public string JournalName { get; set; }
        public int Month { get; set; }
        public string MonthYear { get; set; }
        public DateTime CurrentPeriod { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
