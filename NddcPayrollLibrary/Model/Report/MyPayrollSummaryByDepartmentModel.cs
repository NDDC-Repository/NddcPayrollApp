﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class MyPayrollSummaryByDepartmentModel
    {
        public decimal SecretarialAllow { get; set; }
        public decimal CooporativeDed { get; set; }
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
        public decimal ShiftAllow { get; set; }
        public decimal Tax { get; set; }
        public decimal NHF { get; set; }
        public decimal Pension { get; set; }
        public decimal EmployerPension { get; set; }
        public decimal JSA { get; set; }
        public decimal SSA { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal MonthlyGross { get; set; }
        public int EmployeeCount { get; set; }
        public string DepartmentName { get; set; }
    }
}
