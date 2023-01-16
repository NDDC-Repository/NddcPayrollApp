using NddcPayrollLibrary.Model.Company;

namespace NddcPayrollLibrary.Data.Company
{
    public interface ICompanyData
    {
        void AddBank(BankModel Bank);
        void AddDepartment(DepartmentModel Department);
        void AddJobTitle(JobTitleModel JobTitle);
        void AddPensionAdmin(PensionAdminModel PensionAdmin);
        void AddGradeLevel(MyGradeLevelModel GradeLevel);
        List<BankModel> GetAllBanks();
        List<DepartmentModel> GetAllDepartments();
        List<JobTitleModel> GetAllJobTitles();
        List<PensionAdminModel> GetAllPensionAdmins();
        List<MyGradeLevelGridModel> GetAllGradeLevels();
        List<MyStatesModel> GetAllStates();
        List<MyPayPointModel> GetAllPayPoints();
        List<MyPensionFundListModel> GetAllPensionAdminsList();

    }
}