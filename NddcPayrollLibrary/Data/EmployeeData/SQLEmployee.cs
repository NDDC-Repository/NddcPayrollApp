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

        public void AddEmployee(EmployeeModel Employee)
        {
            db.SaveData("Insert Into Employees (EmployeeCode, Gender, MaritalStatus, firstName, LastName, OtherNames, MaidenName, SpouseName, Email, Phone, DateOfBirth, Address, City, SID, Passport, EmploymentStatus, DateEngaged, ContactName, ContactPhone) values(@EmployeeCode, @Gender, @MaritalStatus, @FirstName, @LastName, @OtherNames, @MaidenName, @SpouseName, @Email, @Phone, @DateOfBirth, @Address, @City, @SID, @Passport, @EmploymentStatus, @DateEngaged, @ContactName, @ContactPhone)", new { Employee.EmployeeCode, Employee.Gender, Employee.MaritalStatus, Employee.FirstName, Employee.LastName, Employee.OtherNames, Employee.MaidenName, Employee.SpouseName, Employee.Email, Employee.Phone, Employee.DateOfBirth, Employee.Address, Employee.City, Employee.SID, Employee.Passport, Employee.EmploymentStatus, Employee.DateEngaged, Employee.ContactName, Employee.ContactPhone }, connectionStringName, false);
        }

        public List<EmployeeGridModel> GetAllEmployees()
        {
            return db.LoadData<EmployeeGridModel, dynamic>("select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, EmployeeCode, Gender, FirstName, LastName, Email, Phone from Employees", new { }, connectionStringName, false).ToList();
        }

    }
}
