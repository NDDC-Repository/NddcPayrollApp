using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data.EmployeeData
{
    public class SQLEmployee : IEmployeeData
    {
        private readonly ISqlDataAccess db;
        private const string connectionStringName = "SqlDb";

        public SQLEmployee(ISqlDataAccess db)
        {
            this.db = db;
        }

        public int AddEmployee(EmployeeModel Employee)
        {
            Employee.CreatedBy = "Admin";
            Employee.DateCreated = DateTime.Now;

            db.SaveData("Insert Into Employees (EmployeeCode, Gender, MaritalStatus, firstName, LastName, OtherNames, MaidenName," +
                " SpouseName, Email, Phone, DateOfBirth, Address, City, SID, Passport, EmploymentStatus, DateEngaged, ContactName, ContactPhone, CreatedBy, DateCreated) " +
                "values(@EmployeeCode, @Gender, @MaritalStatus, @FirstName, @LastName, @OtherNames, @MaidenName, @SpouseName, @Email, @Phone, " +
                "@DateOfBirth, @Address, @City, @SID, @Passport, @EmploymentStatus, @DateEngaged, @ContactName, @ContactPhone, @CreatedBy, @DateCreated)", 
                new { Employee.EmployeeCode, Employee.Gender, Employee.MaritalStatus, Employee.FirstName, Employee.LastName, Employee.OtherNames, 
                    Employee.MaidenName, Employee.SpouseName, Employee.Email, Employee.Phone, Employee.DateOfBirth, Employee.Address, Employee.City, 
                    Employee.SID, Employee.Passport, Employee.EmploymentStatus, Employee.DateEngaged, Employee.ContactName, Employee.ContactPhone, Employee.CreatedBy, Employee.DateCreated }, 
                connectionStringName, false);

            int Id = db.LoadData<int, dynamic>("select Id from Employees where EmployeeCode = @EmployeeCode",
                new { Employee.EmployeeCode }, connectionStringName, false).First();

            return Id;
        }
        public List<EmployeeGridModel> GetAllEmployees()
        {
            string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id DESC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.Gender, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Phone, Employees.Category, GradeLevel.GradeLevel, Departments.DepartmentName, JobTitles.Description FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id ORDER BY Employees.Id DESC";
            return db.LoadData<EmployeeGridModel, dynamic>(SQL, new { }, connectionStringName, false).ToList();
        }

        public EmployeeModel GetEmployeeDetails(int EmpId)
        {
            string SQL = "SELECT EmployeeCode, Gender, MaritalStatus, FirstName, LastName, OtherNames, SpouseName, Email, Phone, DateOfBirth, Address, City, SID, Passport, EmploymentStatus, DateEngaged, ContactName, ContactPhone from Employees Where Id = @Id";
            return db.LoadData<EmployeeModel, dynamic>(SQL, new { Id = EmpId }, connectionStringName, false).First();
        }

        public void AddStatutoryDetails(MyStatutoryDetailsModel Employee)
        {
            Employee.DateCreated= DateTime.Now;

            string SQL = "update Employees set StateId=@SID, TaxStatus=@TaxStatus, TaxOffice=@TaxOffice, TaxNumber=@TaxNumber, TaxStartDate=@TaxStartDate, NHFNumber=@NHFNumber, NHFStatus=@NHFStatus, " +
                "NHIS=@NHIS, NHISStatus=@NHISStatus, MedicalAidName=@MedicalAidName, MedicalAidNumber=@MedicalAidNumber, PensionFundId=@PensionFundId, PensionFundNumber=@PensionFundNumber, NSITFStatus=@NSITFStatus, ITFStatus=@ITFStatus where Id = @EmployeeId ";

            //db.SaveData("Insert Into StatutoryDetails (EmployeeId, SID, TaxStatus, TaxOffice, TaxNumber, TaxStartDate, NHFNumber, NHFStatus, " +
            //    "NHIS, NHISStatus, MedicalAidName, MedicalAidNumber, PensionFundId, PensionFundNumber, NSITFStatus, ITFStatus, CreatedBy, DateCreated) " +
            //    "values(@EmployeeId, @SID, @TaxStatus, @TaxOffice, @TaxNumber, @TaxStartDate, @NHFNumber, @NHFStatus, " +
            //    "@NHIS, @NHISStatus, @MedicalAidName, @MedicalAidNumber, @PensionFundId, @PensionFundNumber, @NSITFStatus, @ITFStatus, @CreatedBy, @DateCreated)",
            db.SaveData(SQL,
               new
                {
                    Employee.EmployeeId,
                    Employee.SID,
                    Employee.TaxStatus,
                    Employee.TaxOffice,
                    Employee.TaxNumber,
                    Employee.TaxStartDate,
                    Employee.NHFNumber,
                    Employee.NHFStatus,
                    Employee.NHIS,
                    Employee.NHISStatus,
                    Employee.MedicalAidName,
                    Employee.MedicalAidNumber,
                    Employee.PensionFundId,
                    Employee.PensionFundNumber,
                    Employee.NSITFSTatus,
                    Employee.ITFStatus
                },
                connectionStringName, false);

            
        }

        public void AddAnalysisDetails(MyAnalysisDetailsModel AnalysisDetails)
        {

            string SQL = "update Employees Set GradeLevelId = @GradeLevelId, JobTitleId = @JobTitleId, Category = @Category, DepartmentId = @DepartmentId, PayPointId = @PayPointId where Id = @EmployeeId";

            db.SaveData(SQL,
               new
               {
                   AnalysisDetails.GradeLevelId,
                   AnalysisDetails.JobTitleId,
                   AnalysisDetails.Category,
                   AnalysisDetails.DepartmentId,
                   AnalysisDetails.PayPointId,
                   AnalysisDetails.EmployeeId
               },
                connectionStringName, false);


        }

    }
}
