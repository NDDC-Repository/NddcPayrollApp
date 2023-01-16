﻿using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model;
using NddcPayrollLibrary.Model.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data.Company
{
    public class SQLCompany : ICompanyData
    {
        private const string connectionStringName = "SqlDb";
        private readonly ISqlDataAccess db;

        public SQLCompany(ISqlDataAccess db)
        {
            this.db = db;
        }

        public void AddBank(BankModel Bank)
        {
            db.SaveData("Insert Into Banks (Code, BankName, Branch, CreatedBy, DateCreated) values(@Code, @BankName, @Branch, @CreatedBy, @DateCreated)", new { Code = Bank.Code, BankName = Bank.BankName, Branch = Bank.Branch, CreatedBy = "Admin", DateCreated = DateTime.Now }, connectionStringName, false);
        }

        public List<BankModel> GetAllBanks()
        {
            return db.LoadData<BankModel, dynamic>("select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, Code, BankName, Branch from Banks", new { }, connectionStringName, false).ToList();
        }


        public void AddDepartment(DepartmentModel Department)
        {
            
            db.SaveData("Insert Into Departments (Code, DepartmentName, CreatedBy, DateCreated) values(@Code, @DepartmentName, @CreatedBy, @DateCreated)", new { Code = Department.Code, DepartmentName = Department.DepartmentName, CreatedBy = "Admin", DateCreated = DateTime.Now }, connectionStringName, false);
        }

        public List<DepartmentModel> GetAllDepartments()
        {
            return db.LoadData<DepartmentModel, dynamic>("select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, Code, DepartmentName from Departments", new { }, connectionStringName, false).ToList();
        }


        public void AddJobTitle(JobTitleModel JobTitle)
        {
            db.SaveData("Insert Into JobTitles (Code, Description, Abbreviation, CreatedBy, DateCreated) values(@Code, @Description, @Abbreviation, @CreatedBy, @DateCreated)", new { Code = JobTitle.Code, Description = JobTitle.Description, Abbreviation = JobTitle.Abbreviation, CreatedBy = "Admin", DateCreated = DateTime.Now }, connectionStringName, false);
        }

        public List<JobTitleModel> GetAllJobTitles()
        {
            return db.LoadData<JobTitleModel, dynamic>("select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, Code, Description, Abbreviation from JobTitles", new { }, connectionStringName, false).ToList();
        }


        public void AddPensionAdmin(PensionAdminModel PensionAdmin)
        {
            db.SaveData("Insert Into PensionAdministrators (Code, Description, CreatedBy, DateCreated) values(@Code, @Description, @CreatedBy, @DateCreated)", new { Code = PensionAdmin.Code, Description = PensionAdmin.Description, CreatedBy = "Admin", DateCreated = DateTime.Now }, connectionStringName, false);
        }

        public List<PensionAdminModel> GetAllPensionAdmins()
        {
            return db.LoadData<PensionAdminModel, dynamic>("select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, Code, Description from PensionAdministrators", new { }, connectionStringName, false).ToList();
        }

        public void AddGradeLevel(MyGradeLevelModel GradeLevel)
        {
            //GradeLevel.CreatedBy = "User";
            //GradeLevel.DateCreated = DateTime.Now;

            db.SaveData("Insert Into GradeLevel (GradeLevel, Description, Category, BasicSalary, CreatedBy, DateCreated) values(@GradeLevel, @Description, @Category, @BasicSalary, @CreatedBy, @DateCreated)", new { GradeLevel = GradeLevel.GradeLevel, Description = GradeLevel.Description, Category = GradeLevel.Category, BasicSalary = GradeLevel.BasicSalary, CreatedBy = "Admin", DateCreated = DateTime.Now }, connectionStringName, false) ;
        }

        public List<MyGradeLevelGridModel> GetAllGradeLevels()
        {
            return db.LoadData<MyGradeLevelGridModel, dynamic>("select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, GradeLevel, Description, Category, BasicSalary from GradeLevel", new { }, connectionStringName, false).ToList();
        }

        public List<MyStatesModel> GetAllStates()
        {
            return db.LoadData<MyStatesModel, dynamic>("select Id, Code, Description from States", new { }, connectionStringName, false).ToList();
        }
        public List<MyPayPointModel> GetAllPayPoints()
        {
            return db.LoadData<MyPayPointModel, dynamic>("select Id, PayPoint, Code from PayPoint", new { }, connectionStringName, false).ToList();
        }
        public List<MyPensionFundListModel> GetAllPensionAdminsList()
        {
            return db.LoadData<MyPensionFundListModel, dynamic>("select Id, Code, Description from PensionAdministrators", new { }, connectionStringName, false).ToList();
        }
    }
}
