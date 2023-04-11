using NddcPayrollLibrary.Data.Calculations.Allowance;
using NddcPayrollLibrary.Data.Calculations.Deductions;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NddcPayrollLibrary.Data.Reports
{
    public class SQLReports : IReportsData
    {
        private const string connectionStringName = "SqlDb";
        private readonly ISqlDataAccess db;
        private readonly IPayrollData payDb;
        private readonly IAllowanceData allowDb;
        private readonly IDeductionData dedDb;
        private readonly IEmployeeData empDb;

        public SQLReports(ISqlDataAccess db, IPayrollData payDb, IAllowanceData allowDb, IDeductionData dedDb, IEmployeeData empDb)
        {
            this.db = db;
            this.payDb = payDb;
            this.allowDb = allowDb;
            this.dedDb = dedDb;
            this.empDb = empDb;
        }

        public List<MyPayRollListModel> GetPayrollListReport()
        {
            
            //MyPayRollListModel reportModel;
            List<MyPayRollListModel> Reports = new List<MyPayRollListModel>();
            string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Category, GradeLevel.GradeLevel, Departments.DepartmentName FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id ORDER BY Employees.Id ASC";
            
            Reports = db.LoadData<MyPayRollListModel, dynamic>(SQL, new { }, connectionStringName, false).ToList();
            foreach (var item in Reports)
            {
                item.BasicSalary = payDb.GetBasicSalary(item.Id);
                //item.TransportAllowance = allowDb.GetTransportAllowance(item.Id);
                item.TransportAllowance = (50M / 100M) * item.BasicSalary;
                item.HousingAllowance = allowDb.GetHousingAllowance(item.Id);
                item.FurnitureAllowance = allowDb.GetFurnitureAllowance(item.Id);
                item.MealAllowance = allowDb.GetMealAllowance(item.Id);
                item.UtilityAllowance = allowDb.GetUtilityAllowance(item.Id);
                item.EducationAllowance = allowDb.GetEducationAllowance(item.Id);
                //item.EducationAllowance = (20M/100M) * item.BasicSalary;
                item.SecurityAllowance = allowDb.GetSecurityAllowance(item.Id);
                item.DomesticServantAllowance = allowDb.GetDomesticServantAllowance(item.Id);
                item.MedicalAllowance = allowDb.GetMedicalAllowance(item.Id);
                //item.MedicalAllowance = (15M/100M) * item.BasicSalary;
                item.VehicleMaintenanceAllowance = allowDb.GetVehicleMaintenanceAllowance(item.Id);
                item.HazardAllowance = allowDb.GetHazardAllowance(item.Id);
                //item.HazardAllowance = (20M/100M) * item.BasicSalary;
                item.DriversAllowance = allowDb.GetDriverAllowance(item.Id);

                item.TaxDeduction = dedDb.GetPAYEAmount(item.Id);
                item.NHFDeduction = dedDb.GetNHFAmount(item.Id);
                item.PensionDeduction = dedDb.GetPensionAmount(item.Id);
                item.JSADeduction = dedDb.GetJSA(item.Id);
                item.SSADeduction = dedDb.GetSSA(item.Id);
                item.TotalEarnings = payDb.GetMonthlyGross(item.Id);
                item.TotalDeductions = dedDb.GetTotalDeductions(item.Id);
                item.NetPay = (item.TotalEarnings) - (item.TotalDeductions);
            }

            return Reports;
        }

        public async  Task<List<MyPayRollListModel>> GetPayrollListReportAsync()
        {

            //MyPayRollListModel reportModel;
            List<MyPayRollListModel> Reports = new List<MyPayRollListModel>();
            List<Task<decimal>> tasks = new List<Task<decimal>>();

            string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Category, GradeLevel.GradeLevel, Departments.DepartmentName FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id WHERE Employees.Archive = 0 ORDER BY Employees.Id ASC";

            Reports = db.LoadData<MyPayRollListModel, dynamic>(SQL, new { }, connectionStringName, false).ToList();
                foreach (var item in Reports)
                {
                //item.BasicSalary = payDb.GetBasicSalary(item.Id);
                tasks.Add(Task.Run(() => item.TotalEarnings = payDb.GetMonthlyGross(item.Id)));
                tasks.Add(Task.Run(() => item.TotalDeductions = dedDb.GetTotalDeductions(item.Id)));
                tasks.Add(Task.Run(() => item.BasicSalary = payDb.GetBasicSalary(item.Id)));
                tasks.Add(Task.Run(() => item.TransportAllowance = allowDb.GetTransportAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.HousingAllowance = allowDb.GetHousingAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.FurnitureAllowance = allowDb.GetFurnitureAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.MealAllowance = allowDb.GetMealAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.UtilityAllowance = allowDb.GetUtilityAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.EducationAllowance = allowDb.GetEducationAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.SecurityAllowance = allowDb.GetSecurityAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.DomesticServantAllowance = allowDb.GetDomesticServantAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.MedicalAllowance = allowDb.GetMedicalAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.VehicleMaintenanceAllowance = allowDb.GetVehicleMaintenanceAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.HazardAllowance = allowDb.GetHazardAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.DriversAllowance = allowDb.GetDriverAllowance(item.Id)));
                tasks.Add(Task.Run(() => item.TaxDeduction = dedDb.GetPAYEAmount(item.Id)));
                tasks.Add(Task.Run(() => item.NHFDeduction = dedDb.GetNHFAmount(item.Id)));
                tasks.Add(Task.Run(() => item.PensionDeduction = dedDb.GetPensionAmount(item.Id)));
                tasks.Add(Task.Run(() => item.JSADeduction = dedDb.GetJSA(item.Id)));
                tasks.Add(Task.Run(() => item.SSADeduction = dedDb.GetSSA(item.Id)));
               
                tasks.Add(Task.Run(() => item.NetPay = payDb.GetMonthlyGross(item.Id) - dedDb.GetTotalDeductions(item.Id)));
                

                ////item.TransportAllowance = allowDb.GetTransportAllowance(item.Id);
                //item.TransportAllowance = (50M / 100M) * item.BasicSalary;
                //item.HousingAllowance = allowDb.GetHousingAllowance(item.Id);
                //item.FurnitureAllowance = allowDb.GetFurnitureAllowance(item.Id);
                //item.MealAllowance = allowDb.GetMealAllowance(item.Id);
                //item.UtilityAllowance = allowDb.GetUtilityAllowance(item.Id);
                //item.EducationAllowance = allowDb.GetEducationAllowance(item.Id);
                ////item.EducationAllowance = (20M/100M) * item.BasicSalary;
                //item.SecurityAllowance = allowDb.GetSecurityAllowance(item.Id);
                //item.DomesticServantAllowance = allowDb.GetDomesticServantAllowance(item.Id);
                //item.MedicalAllowance = allowDb.GetMedicalAllowance(item.Id);
                ////item.MedicalAllowance = (15M/100M) * item.BasicSalary;
                //item.VehicleMaintenanceAllowance = allowDb.GetVehicleMaintenanceAllowance(item.Id);
                //item.HazardAllowance = allowDb.GetHazardAllowance(item.Id);
                ////item.HazardAllowance = (20M/100M) * item.BasicSalary;
                //item.DriversAllowance = allowDb.GetDriverAllowance(item.Id);

                //item.TaxDeduction = dedDb.GetPAYEAmount(item.Id);
                //item.NHFDeduction = dedDb.GetNHFAmount(item.Id);
                //item.PensionDeduction = dedDb.GetPensionAmount(item.Id);
                //item.JSADeduction = dedDb.GetJSA(item.Id);
                //item.SSADeduction = dedDb.GetSSA(item.Id);
                //item.TotalEarnings = payDb.GetMonthlyGross(item.Id);
                //item.TotalDeductions = dedDb.GetTotalDeductions(item.Id);
                //item.NetPay = (item.TotalEarnings) - (item.TotalDeductions);
                await Task.WhenAll(tasks);
            }

                
            return Reports;
        }

        public async Task<List<MyPayRollListModel>> GetPayrollListReport2Async()
        {

            //MyPayRollListModel reportModel;
            List<MyPayRollListModel> Reports = new List<MyPayRollListModel>();
            //List<Task<decimal>> tasks = new List<Task<decimal>>();

            string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id DESC) As SrNo, Employees.Id, Employees.BasicSalary, Employees.EmployeeCode, Employees.FirstName, Employees.SecretarialAllow, Employees.CooperativeDed, Employees.VoluntaryPension, Employees.EntertainmentAllow, Employees.NewspaperAllow, Employees.Arreas, " +
                "Employees.LastName, Employees.Email, Employees.Category, (TransportAllow) As TransportAllowance, (HousingAllow) As HousingAllowance, (FurnitureAllow) As FurnitureAllowance, (MealAllow) As MealAllowance, (UtilityAllow) As UtilityAllowance, " +
                "(EducationAllow) As EducationAllowance, (DomesticServantAllow) As DomesticServantAllowance, (DriverAllow) As DriversAllowance, (VehicleAllow) As VehicleMaintenanceAllowance, (HazardAllow) As HazardAllowance, (Tax) As TaxDeduction, (NHF) As NHFDeduction, (JSA) As JSADeduction, (SSA) As SSADeduction, TotalEarnings, TotalDeductions, " +
                "NetPay, (Pension) As PensionDeduction, (MedicalAllow) As MedicalAllowance, (SecurityAllow) As SecurityAllowance, GradeLevel.GradeLevel, Departments.DepartmentName, Employees.EntertainmentAllow, Employees.NewspaperAllow, Employees.LeaveAllow FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = " +
                "GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = " +
                "JobTitles.Id WHERE Employees.Archived = 0 ORDER BY Employees.Id DESC";

            await (Task.Run(() => Reports = db.LoadData<MyPayRollListModel, dynamic>(SQL, new { }, connectionStringName, false).ToList()));
            //foreach (var item in Reports)
            //{
            //    //item.BasicSalary = payDb.GetBasicSalary(item.Id);
            //    tasks.Add(Task.Run(() => item.TotalEarnings = (item.Id)));
            //    tasks.Add(Task.Run(() => item.TotalDeductions = (item.Id)));
            //    tasks.Add(Task.Run(() => item.BasicSalary = (item.Id)));
            //    tasks.Add(Task.Run(() => item.TransportAllowance = (item.Id)));
            //    tasks.Add(Task.Run(() => item.HousingAllowance = (item.Id)));
            //    tasks.Add(Task.Run(() => item.FurnitureAllowance = allowDb.GetFurnitureAllowance(item.Id)));
            //    tasks.Add(Task.Run(() => item.MealAllowance = allowDb.GetMealAllowance(item.Id)));
            //    tasks.Add(Task.Run(() => item.UtilityAllowance = allowDb.GetUtilityAllowance(item.Id)));
            //    tasks.Add(Task.Run(() => item.EducationAllowance = allowDb.GetEducationAllowance(item.Id)));
            //    tasks.Add(Task.Run(() => item.SecurityAllowance = allowDb.GetSecurityAllowance(item.Id)));
            //    tasks.Add(Task.Run(() => item.DomesticServantAllowance = allowDb.GetDomesticServantAllowance(item.Id)));
            //    tasks.Add(Task.Run(() => item.MedicalAllowance = allowDb.GetMedicalAllowance(item.Id)));
            //    tasks.Add(Task.Run(() => item.VehicleMaintenanceAllowance = allowDb.GetVehicleMaintenanceAllowance(item.Id)));
            //    tasks.Add(Task.Run(() => item.HazardAllowance = allowDb.GetHazardAllowance(item.Id)));
            //    tasks.Add(Task.Run(() => item.DriversAllowance = allowDb.GetDriverAllowance(item.Id)));
            //    tasks.Add(Task.Run(() => item.TaxDeduction = dedDb.GetPAYEAmount(item.Id)));
            //    tasks.Add(Task.Run(() => item.NHFDeduction = dedDb.GetNHFAmount(item.Id)));
            //    tasks.Add(Task.Run(() => item.PensionDeduction = dedDb.GetPensionAmount(item.Id)));
            //    tasks.Add(Task.Run(() => item.JSADeduction = dedDb.GetJSA(item.Id)));
            //    tasks.Add(Task.Run(() => item.SSADeduction = dedDb.GetSSA(item.Id)));

            //    tasks.Add(Task.Run(() => item.NetPay = payDb.GetMonthlyGross(item.Id) - dedDb.GetTotalDeductions(item.Id)));

            //    await Task.WhenAll(tasks);
            //}


            return Reports;
        }

        public async Task<List<MyPayRollListModel>> GetPayrollListSummaryReportAsync(int payrollJournalTitleId)
        {

            //MyPayRollListModel reportModel;
            List<MyPayRollListModel> Reports = new List<MyPayRollListModel>();
            //List<Task<decimal>> tasks = new List<Task<decimal>>();

            //string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY PayrollJournals.Id DESC) As SrNo, PayrollJournals.Id, PayrollJournals.EmployeeCode, PayrollJournals.FirstName, " +
            //    "PayrollJournals.LastName, PayrollJournals.Category, " +
            //    "TotalEarnings, TotalDeductions, BankCode, AccountName, AccountNumber, " +
            //    "NetPay, GradeLevel.GradeLevel, GradeLevel.BasicSalary, Departments.DepartmentName, Banks.BankName FROM PayrollJournals LEFT JOIN GradeLevel ON PayrollJournals.GradeLevelId = " +
            //    "GradeLevel.Id LEFT JOIN Departments ON PayrollJournals.DepartmentId = Departments.Id LEFT JOIN JobTitles ON PayrollJournals.JobTitleId = " +
            //    "JobTitles.Id LEFT JOIN Banks ON Banks.Code = PayrollJournals.BankCode WHERE PayrollJournalTitleId = @PayrollJournalTitleId ORDER BY PayrollJournals.Id DESC";

            string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) As SrNo, Id, EmployeeCode, FirstName, " +
                "LastName, Category, " +
                "TotalEarnings, TotalDeductions, BankCode, AccountName, AccountNumber, " +
                "NetPay, GradeLevel, BasicSalary, DepartmentName, BankName FROM PayrollJournals WHERE PayrollJournalTitleId = @PayrollJournalTitleId ORDER BY PayrollJournals.Id DESC";

            await (Task.Run(() => Reports = db.LoadData<MyPayRollListModel, dynamic>(SQL, new { PayrollJournalTitleId = payrollJournalTitleId }, connectionStringName, false).ToList()));
           

            return Reports;
        }

        public List<MyPayRollListModel> GetPayrollListReportById(int payrollJournalTitleId)
        {

            //MyPayRollListModel reportModel;
            List<MyPayRollListModel> Reports = new List<MyPayRollListModel>();
            //List<Task<decimal>> tasks = new List<Task<decimal>>();

            string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY PayrollJournals.Id DESC) As SrNo, PayrollJournals.Id, PayrollJournals.EmployeeCode, PayrollJournals.FirstName, " +
                "PayrollJournals.LastName, PayrollJournals.Email, PayrollJournals.Category, (TransportAllow) As TransportAllowance, (HousingAllow) As HousingAllowance, (FurnitureAllow) As FurnitureAllowance, (MealAllow) As MealAllowance, (UtilityAllow) As UtilityAllowance, " +
                "(EducationAllow) As EducationAllowance, (DomesticServantAllow) As DomesticServantAllowance, (DriverAllow) As DriversAllowance, (VehicleAllow) As VehicleMaintenanceAllowance, (HazardAllow) As HazardAllowance, (Tax) As TaxDeduction, (NHF) As NHFDeduction, (JSA) As JSADeduction, (SSA) As SSADeduction, TotalEarnings, TotalDeductions, " +
                "NetPay, (Pension) As PensionDeduction, (MedicalAllow) As MedicalAllowance, (SecurityAllow) As SecurityAllowance, GradeLevel.GradeLevel, GradeLevel.BasicSalary, Departments.DepartmentName, Arreas, LeaveAllow, EntertainmentAllow, NewspaperAllow, SecretarialAllow, CooperativeDed, VoluntaryPension FROM PayrollJournals LEFT JOIN GradeLevel ON PayrollJournals.GradeLevelId = " +
                "GradeLevel.Id LEFT JOIN Departments ON PayrollJournals.DepartmentId = Departments.Id LEFT JOIN JobTitles ON PayrollJournals.JobTitleId = " +
                "JobTitles.Id Where PayrollJournals.PayrollJournalTitleId = @PayrollJournalTitleId ORDER BY PayrollJournals.Id DESC";

            Reports = db.LoadData<MyPayRollListModel, dynamic>(SQL, new { PayrollJournalTitleId = payrollJournalTitleId }, connectionStringName, false).ToList();
           
            return Reports;
        }

        public async Task<List<MyPayeReportSummaryModel>> GetPayeSummaryReportAsync()
        {

            //MyPayRollListModel reportModel;
            List<MyPayeReportSummaryModel> PayeSummaryList = new List<MyPayeReportSummaryModel>();
            //List<Task<decimal>> tasks = new List<Task<decimal>>();

            string SQL = "Select (TaxStateProvince) As Location,Count(EmployeeCode) As EmployeeCount, Sum(TotalEarnings) As GrossIncome, Sum(Tax) As Tax From Employees Where Archived = 0 Group By TaxStateProvince";

            await (Task.Run(() => PayeSummaryList = db.LoadData<MyPayeReportSummaryModel, dynamic>(SQL, new { }, connectionStringName, false).ToList()));
            
            return PayeSummaryList;
        }

        public async Task<List<MyPayrollSummaryByDepartmentModel>> GetPayrollSummaryByDeptReportAsync()
        {

            //MyPayRollListModel reportModel;
            List<MyPayrollSummaryByDepartmentModel> PayrollSummaryList = new List<MyPayrollSummaryByDepartmentModel>();
            //List<Task<decimal>> tasks = new List<Task<decimal>>();

            string SQL = "SELECT Sum(Employees.SecretarialAllow) As SecretarialAllow, Sum(Employees.CooperativeDed) As CooporativeDed, " +
                "Sum(Employees.VoluntaryPension) As VoluntaryPension, Sum(Employees.TransportAllow) As TransportAllow, " +
                "Sum(Employees.HousingAllow) As HousingAllow, Count(Employees.Id) As EmployeeCount, Sum(Employees.FurnitureAllow) As FurnitureAllow, " +
                "Sum(Employees.MealAllow) As MealAllow, Sum(Employees.UtilityAllow) As UtilityAllow, " +
                "Sum(Employees.EducationAllow) As EducationAllow, Sum(Employees.SecurityAllow) As SecurityAllow, " +
                "Sum(Employees.MedicalAllow) As MedicalAllow, Sum(Employees.DomesticServantAllow) As DomesticServantAllow, " +
                "Sum(Employees.DriverAllow) As DriverAllow, Sum(Employees.VehicleAllow) As VehicleAllow, " +
                "Sum(Employees.HazardAllow) As HazardAllow, Sum(Employees.Tax) As Tax, Sum(Employees.NHF) As NHF, " +
                "Sum(Employees.JSA) As JSA, Sum(Employees.SSA) As SSA, Sum(Employees.TotalEarnings) As TotalEarnings, " +
                "Sum(Employees.NetPay) As NetPay, Sum(Employees.BasicSalary) As BasicSalary, " +
                "Sum(Employees.MonthlyGross) As MonthlyGross, Sum(Employees.Pension) As Pension, DepartmentId, " +
                "Departments.DepartmentName FROM  Employees LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id " +
                " Where Employees.Archived = 0 Group By DepartmentId, Departments.DepartmentName";

            await (Task.Run(() => PayrollSummaryList = db.LoadData<MyPayrollSummaryByDepartmentModel, dynamic>(SQL, new { }, connectionStringName, false).ToList()));

            foreach (var item in PayrollSummaryList)
            {
                item.TotalEarnings = (item.BasicSalary + item.TransportAllow + item.MealAllow + item.UtilityAllow + item.EducationAllow + item.SecurityAllow +
                    item.DomesticServantAllow + item.MedicalAllow + item.DriverAllow + item.VehicleAllow + item.HazardAllow + item.SecretarialAllow);
                item.TotalDeductions = (item.Tax + item.NHF + item.Pension + item.JSA + item.CooporativeDed + item.SSA);
                item.NetPay = item.TotalEarnings - item.TotalDeductions;
            }

            return PayrollSummaryList;
        }

        public async Task<List<MyPayrollSummaryByDepartmentModel>> GetPayrollTotalsReportAsync()
        {

            //MyPayRollListModel reportModel;
            List<MyPayrollSummaryByDepartmentModel> PayrollSummaryList = new List<MyPayrollSummaryByDepartmentModel>();
            //List<Task<decimal>> tasks = new List<Task<decimal>>();

            string SQL = "SELECT Sum(Employees.SecretarialAllow) As SecretarialAllow, Sum(Employees.CooperativeDed) As CooporativeDed, " +
                "Sum(Employees.VoluntaryPension) As VoluntaryPension, Sum(Employees.TransportAllow) As TransportAllow, " +
                "Sum(Employees.HousingAllow) As HousingAllow, Count(Employees.Id) As EmployeeCount, Sum(Employees.FurnitureAllow) As FurnitureAllow, " +
                "Sum(Employees.MealAllow) As MealAllow, Sum(Employees.UtilityAllow) As UtilityAllow, " +
                "Sum(Employees.EducationAllow) As EducationAllow, Sum(Employees.SecurityAllow) As SecurityAllow, " +
                "Sum(Employees.MedicalAllow) As MedicalAllow, Sum(Employees.DomesticServantAllow) As DomesticServantAllow, " +
                "Sum(Employees.DriverAllow) As DriverAllow, Sum(Employees.VehicleAllow) As VehicleAllow, " +
                "Sum(Employees.HazardAllow) As HazardAllow, Sum(Employees.Tax) As Tax, Sum(Employees.NHF) As NHF, " +
                "Sum(Employees.JSA) As JSA, Sum(Employees.SSA) As SSA, Sum(Employees.TotalEarnings) As TotalEarnings, " +
                "Sum(Employees.NetPay) As NetPay, Sum(Employees.BasicSalary) As BasicSalary, " +
                "Sum(Employees.MonthlyGross) As MonthlyGross, Sum(Employees.Pension) As Pension FROM  Employees" +
                " Where Employees.Archived = 0";

            await (Task.Run(() => PayrollSummaryList = db.LoadData<MyPayrollSummaryByDepartmentModel, dynamic>(SQL, new { }, connectionStringName, false).ToList()));

            foreach (var item in PayrollSummaryList)
            {
                item.TotalEarnings = (item.BasicSalary + item.TransportAllow + item.MealAllow + item.UtilityAllow + item.EducationAllow + item.SecurityAllow +
                    item.DomesticServantAllow + item.MedicalAllow + item.DriverAllow + item.VehicleAllow + item.HazardAllow + item.SecretarialAllow);
                item.TotalDeductions = (item.Tax + item.NHF + item.Pension + item.JSA + item.CooporativeDed + item.SSA);
                item.NetPay = item.TotalEarnings - item.TotalDeductions;
            }

            return PayrollSummaryList;
        }

        public async Task<List<MyStaffPayeDeductionsModel>> GetStaffPayeDeductionByLocationAsync(string staffStateProvince)
        {

            //MyPayRollListModel reportModel;
            List<MyStaffPayeDeductionsModel> StaffPayeList = new List<MyStaffPayeDeductionsModel>();
            //List<Task<decimal>> tasks = new List<Task<decimal>>();

            string SQL = "select ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, EmployeeCode, LastName, FirstName, TotalEarnings, Tax From Employees Where TaxStateProvince = @TaxStateProvince And Archived = 0";

            await (Task.Run(() => StaffPayeList = db.LoadData<MyStaffPayeDeductionsModel, dynamic>(SQL, new { TaxStateProvince = staffStateProvince }, connectionStringName, false).ToList()));

            return StaffPayeList;
        }

        public async Task UpdateEmployeesPayrollAsync()
        {

            //MyPayRollListModel reportModel;
            List<EmployeeModel> Employees = new List<EmployeeModel>();
            List<Task<decimal>> tasks = new List<Task<decimal>>();

            string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Category, GradeLevel.GradeLevel, Departments.DepartmentName FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id Where TaxCalc = 'Automatic' ORDER BY Employees.Id ASC";

            Employees = db.LoadData<EmployeeModel, dynamic>(SQL, new { }, connectionStringName, false).ToList();
            foreach (var item in Employees)
            {
                EmployeeModel employee = new EmployeeModel();

                //item.BasicSalary = payDb.GetBasicSalary(item.Id);
                tasks.Add(Task.Run(() => employee.TotalEarnings = payDb.GetMonthlyGross(item.Id)));
                //tasks.Add(Task.Run(() => employee.EmployeeCode = item.EmployeeCode));
                employee.EmployeeCode = item.EmployeeCode;
                tasks.Add(Task.Run(() => employee.TotalDeductions = dedDb.GetTotalDeductions(item.Id)));
                tasks.Add(Task.Run(() => employee.BasicSalary = payDb.GetBasicSalary(item.Id)));
                tasks.Add(Task.Run(() => employee.TransportAllow = allowDb.GetTransportAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.HousingAllow = allowDb.GetHousingAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.FurnitureAllow = allowDb.GetFurnitureAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.MealAllow = allowDb.GetMealAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.UtilityAllow = allowDb.GetUtilityAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.EducationAllow = allowDb.GetEducationAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.SecurityAllow = allowDb.GetSecurityAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.DomesticServantAllow = allowDb.GetDomesticServantAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.MedicalAllow = allowDb.GetMedicalAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.VehicleAllow = allowDb.GetVehicleMaintenanceAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.HazardAllow = allowDb.GetHazardAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.DriverAllow = allowDb.GetDriverAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.EntertainmentAllow = allowDb.GetEntertainmentAllow(item.Id)));
                tasks.Add(Task.Run(() => employee.NewspaperAllow = allowDb.GetNewspaperAllow(item.Id)));
                tasks.Add(Task.Run(() => employee.Tax = dedDb.GetPAYEAmount(item.Id)));
                tasks.Add(Task.Run(() => employee.NHF = dedDb.GetNHFAmount(item.Id)));
                tasks.Add(Task.Run(() => employee.Pension = dedDb.GetPensionAmount(item.Id)));
                tasks.Add(Task.Run(() => employee.JSA = dedDb.GetJSA(item.Id)));
                tasks.Add(Task.Run(() => employee.SSA = dedDb.GetSSA(item.Id)));
                tasks.Add(Task.Run(() => employee.SSA = dedDb.GetSSA(item.Id)));
                tasks.Add(Task.Run(() => employee.EmployerPension = dedDb.GetEmployerPensionAmount(item.Id)));
                //tasks.Add(Task.Run(() => employee.MonthlyGross = dedDb.GetSSA(item.Id)));
                tasks.Add(Task.Run(() => employee.NetPay = payDb.GetMonthlyGross(item.Id) - dedDb.GetTotalDeductions(item.Id)));

               

                await Task.WhenAll(tasks);

                empDb.UpdateEmployeePayroll(employee);
            }


            //return Reports;
        }

        public async Task UpdateEmployeesPayrollByGradeLevelAsync(int gradeLevelId)
        {

            //MyPayRollListModel reportModel;
            List<EmployeeModel> Employees = new List<EmployeeModel>();
            List<Task<decimal>> tasks = new List<Task<decimal>>();

            string SQL = "SELECT Id, EmployeeCode From Employees Where GradeLevelId = @GradeLevelId And TaxCalc = 'Automatic'";

            Employees = db.LoadData<EmployeeModel, dynamic>(SQL, new { gradeLevelId }, connectionStringName, false).ToList();
            foreach (var item in Employees)
            {
                EmployeeModel employee = new EmployeeModel();

                //item.BasicSalary = payDb.GetBasicSalary(item.Id);
                tasks.Add(Task.Run(() => employee.TotalEarnings = payDb.GetMonthlyGross(item.Id)));
                //tasks.Add(Task.Run(() => employee.EmployeeCode = item.EmployeeCode));
                employee.EmployeeCode = item.EmployeeCode;
                tasks.Add(Task.Run(() => employee.TotalDeductions = dedDb.GetTotalDeductions(item.Id)));
                tasks.Add(Task.Run(() => employee.BasicSalary = payDb.GetBasicSalary(item.Id)));
                tasks.Add(Task.Run(() => employee.TransportAllow = allowDb.GetTransportAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.HousingAllow = allowDb.GetHousingAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.FurnitureAllow = allowDb.GetFurnitureAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.MealAllow = allowDb.GetMealAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.UtilityAllow = allowDb.GetUtilityAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.EducationAllow = allowDb.GetEducationAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.SecurityAllow = allowDb.GetSecurityAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.DomesticServantAllow = allowDb.GetDomesticServantAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.MedicalAllow = allowDb.GetMedicalAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.VehicleAllow = allowDb.GetVehicleMaintenanceAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.HazardAllow = allowDb.GetHazardAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.DriverAllow = allowDb.GetDriverAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.EntertainmentAllow = allowDb.GetEntertainmentAllow(item.Id)));
                tasks.Add(Task.Run(() => employee.NewspaperAllow = allowDb.GetNewspaperAllow(item.Id)));
                tasks.Add(Task.Run(() => employee.Tax = dedDb.GetPAYEAmount(item.Id)));
                tasks.Add(Task.Run(() => employee.NHF = dedDb.GetNHFAmount(item.Id)));
                tasks.Add(Task.Run(() => employee.Pension = dedDb.GetPensionAmount(item.Id)));
                tasks.Add(Task.Run(() => employee.JSA = dedDb.GetJSA(item.Id)));
                tasks.Add(Task.Run(() => employee.SSA = dedDb.GetSSA(item.Id)));
                tasks.Add(Task.Run(() => employee.EmployerPension = dedDb.GetEmployerPensionAmount(item.Id)));
                //tasks.Add(Task.Run(() => employee.MonthlyGross = dedDb.GetSSA(item.Id)));
                tasks.Add(Task.Run(() => employee.NetPay = payDb.GetMonthlyGross(item.Id) - dedDb.GetTotalDeductions(item.Id)));



                await Task.WhenAll(tasks);

                //not async

                ////item.BasicSalary = payDb.GetBasicSalary(item.Id);
                //employee.TotalEarnings = payDb.GetMonthlyGross(item.Id);
                ////tasks.Add(Task.Run(() => employee.EmployeeCode = item.EmployeeCode));
                //employee.EmployeeCode = item.EmployeeCode;
                //employee.TotalDeductions = dedDb.GetTotalDeductions(item.Id);
                //employee.BasicSalary = payDb.GetBasicSalary(item.Id);
                //employee.TransportAllow = allowDb.GetTransportAllowance(item.Id);
                //employee.HousingAllow = allowDb.GetHousingAllowance(item.Id);
                //employee.FurnitureAllow = allowDb.GetFurnitureAllowance(item.Id);
                //employee.MealAllow = allowDb.GetMealAllowance(item.Id);
                //employee.UtilityAllow = allowDb.GetUtilityAllowance(item.Id);
                //employee.EducationAllow = allowDb.GetEducationAllowance(item.Id);
                //employee.SecurityAllow = allowDb.GetSecurityAllowance(item.Id);
                //employee.DomesticServantAllow = allowDb.GetDomesticServantAllowance(item.Id);
                //employee.MedicalAllow = allowDb.GetMedicalAllowance(item.Id);
                //employee.VehicleAllow = allowDb.GetVehicleMaintenanceAllowance(item.Id);
                //employee.HazardAllow = allowDb.GetHazardAllowance(item.Id);
                //employee.DriverAllow = allowDb.GetDriverAllowance(item.Id);
                //employee.Tax = dedDb.GetPAYEAmount(item.Id);
                //employee.NHF = dedDb.GetNHFAmount(item.Id);
                //employee.Pension = dedDb.GetPensionAmount(item.Id);
                //employee.JSA = dedDb.GetJSA(item.Id);
                //employee.SSA = dedDb.GetSSA(item.Id);
                ////tasks.Add(Task.Run(() => employee.MonthlyGross = dedDb.GetSSA(item.Id)));


                empDb.UpdateEmployeePayroll(employee);
            }


            //return Reports;
        }

        public async Task UpdateEmployeesPayrollByEmpIdAsync(int empId)
        {

            //MyPayRollListModel reportModel;
            List<EmployeeModel> Employees = new List<EmployeeModel>();
            List<Task<decimal>> tasks = new List<Task<decimal>>();

            string SQL = "SELECT Id, EmployeeCode From Employees Where Id = @Id And TaxCalc = 'Automatic'";

            Employees = db.LoadData<EmployeeModel, dynamic>(SQL, new { @Id = empId }, connectionStringName, false).ToList();
            foreach (var item in Employees)
            {
                EmployeeModel employee = new EmployeeModel();

                //item.BasicSalary = payDb.GetBasicSalary(item.Id);
                tasks.Add(Task.Run(() => employee.TotalEarnings = payDb.GetMonthlyGross(item.Id)));
                //tasks.Add(Task.Run(() => employee.EmployeeCode = item.EmployeeCode));
                employee.EmployeeCode = item.EmployeeCode;
                tasks.Add(Task.Run(() => employee.TotalDeductions = dedDb.GetTotalDeductions(item.Id)));
                tasks.Add(Task.Run(() => employee.BasicSalary = payDb.GetBasicSalary(item.Id)));
                tasks.Add(Task.Run(() => employee.TransportAllow = allowDb.GetTransportAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.HousingAllow = allowDb.GetHousingAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.FurnitureAllow = allowDb.GetFurnitureAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.MealAllow = allowDb.GetMealAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.UtilityAllow = allowDb.GetUtilityAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.EducationAllow = allowDb.GetEducationAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.SecurityAllow = allowDb.GetSecurityAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.DomesticServantAllow = allowDb.GetDomesticServantAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.MedicalAllow = allowDb.GetMedicalAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.VehicleAllow = allowDb.GetVehicleMaintenanceAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.HazardAllow = allowDb.GetHazardAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.DriverAllow = allowDb.GetDriverAllowance(item.Id)));
                tasks.Add(Task.Run(() => employee.EntertainmentAllow = allowDb.GetEntertainmentAllow(item.Id)));
                tasks.Add(Task.Run(() => employee.NewspaperAllow = allowDb.GetNewspaperAllow(item.Id)));
                tasks.Add(Task.Run(() => employee.Tax = dedDb.GetPAYEAmount(item.Id)));
                tasks.Add(Task.Run(() => employee.NHF = dedDb.GetNHFAmount(item.Id)));
                tasks.Add(Task.Run(() => employee.Pension = dedDb.GetPensionAmount(item.Id)));
                tasks.Add(Task.Run(() => employee.JSA = dedDb.GetJSA(item.Id)));
                tasks.Add(Task.Run(() => employee.SSA = dedDb.GetSSA(item.Id)));
                tasks.Add(Task.Run(() => employee.EmployerPension = dedDb.GetEmployerPensionAmount(item.Id)));
                //tasks.Add(Task.Run(() => employee.MonthlyGross = dedDb.GetSSA(item.Id)));
                tasks.Add(Task.Run(() => employee.NetPay = payDb.GetMonthlyGross(item.Id) - dedDb.GetTotalDeductions(item.Id)));

                await Task.WhenAll(tasks);

                empDb.UpdateEmployeePayroll(employee);
            }


            //return Reports;
        }
        public async Task<List<MyRemitaUploadModel>> GetRemitaReportAsync()
        {

            //MyPayRollListModel reportModel;
            List<MyRemitaUploadModel> Reports = new List<MyRemitaUploadModel>();
            List<Task<decimal>> tasks = new List<Task<decimal>>();

            //string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, Employees.Id, Employees.EmployeeCode, (Employees.FirstName + ' ' + Employees.LastName) As EmployeeName, Employees.BankCode, Employees.AccountNumber FROM Employees ORDER BY Employees.Id ASC";
            string SQL2 = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, Employees.Id, Employees.EmployeeCode, (Employees.FirstName + ' ' + Employees.LastName) As EmployeeName, Employees.BankCode, Employees.AccountNumber, (Employees.NetPay) As PayableAmount FROM Employees Where Archived = 0 ORDER BY Employees.Id ASC";

            Reports = db.LoadData<MyRemitaUploadModel, dynamic>(SQL2, new { }, connectionStringName, false).ToList();
            //foreach (var item in Reports)
            //{
                
            //    tasks.Add(Task.Run(() => item.PayableAmount = payDb.GetMonthlyGross(item.Id) - dedDb.GetTotalDeductions(item.Id)));
            //    //tasks.Add(Task.Run(() => item.AccountType = "20"));



            //    await Task.WhenAll(tasks);
            //}


            return Reports;
        }

        public async Task<List<MyNHFReportModel>> GetNHFReportAsync()
        {

            //MyPayRollListModel reportModel;
            List<MyNHFReportModel> Reports = new List<MyNHFReportModel>();
            List<Task<decimal>> tasks = new List<Task<decimal>>();

            //string SQL2 = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.FirstName, Employees.LastName, Employees.NHFNumber, Employees.PayPoint, GradeLevel.BasicSalary FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id ORDER BY Employees.Id ASC";
            //string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.FirstName, Employees.LastName, Employees.NHFNumber, Employees.AccountNumber FROM Employees ORDER BY Employees.Id ASC";
            string SQL3 = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.FirstName, Employees.LastName, Employees.NHFNumber, Employees.PayPoint, (Employees.NHF) As NHFAmount FROM Employees WHERE Archived = 0 ORDER BY Employees.Id ASC";

            Reports = db.LoadData<MyNHFReportModel, dynamic>(SQL3, new { }, connectionStringName, false).ToList();
            //foreach (var item in Reports)
            //{

            //    tasks.Add(Task.Run(() => item.NHFAmount = 2.5M / 100M * item.BasicSalary));
            //    //tasks.Add(Task.Run(() => item.AccountType = "20"));



            //    await Task.WhenAll(tasks);
            //}


            return Reports;
        }

        public List<MyPayPointChartModel> GetPayPointData()
        {
            string sql = "select sum(TotalEarnings) As TotalEarnings, TaxStateProvince From Employees Group By TaxStateProvince";
            return db.LoadData<MyPayPointChartModel, dynamic>(sql, new { }, connectionStringName, false).ToList();
        }
        public List<MyEarningsByDeptChartModel> GetEarningsByDeptData()
        {
            string sql = "select sum(Employees.TotalEarnings) As Amount, Departments.DepartmentName From Employees INNER JOIN Departments ON Employees.DepartmentId = Departments.Id Group By Departments.DepartmentName";
            return db.LoadData<MyEarningsByDeptChartModel, dynamic>(sql, new { }, connectionStringName, false).ToList();
        }

    }
}
