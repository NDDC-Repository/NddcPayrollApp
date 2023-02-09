using NddcPayrollLibrary.Model.DataManagement.DataMigration;

namespace NddcPayrollLibrary.Data.DataManagement
{
    public interface IDataMigration
    {
        List<MyEmployeeMigrationModel> MigrationEmployees { get; set; }

        void UpdateGradeLevel();
        void UpdateDepartment();
        void UpdateJobTitle();
    }
}