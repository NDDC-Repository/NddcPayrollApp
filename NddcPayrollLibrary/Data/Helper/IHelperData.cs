namespace NddcPayrollLibrary.Data.Helper
{
    public interface IHelperData
    {
        T GetAnyRecord<T, U>(string tableName, string returnFieldName, string parameterName, U ParamValue);
    }
}