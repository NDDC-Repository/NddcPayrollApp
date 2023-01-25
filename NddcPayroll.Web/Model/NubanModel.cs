namespace NddcPayroll.Web.Model
{

    //public class NubanModel
    //{
    //    public string Bank_name { get; set; }
    //    public string Account_name { get; set; }
    //    public string Account_number { get; set; }
    //    public string Bank_code { get; set; }
    //    public string Requests { get; set; }
    //    public string Execution_time { get; set; }
    //}


    //public class NubanModel
    //{
    //    public NubanDetails[] Property1 { get; set; }
    //}

    //public class NubanDetails
    //{
    //    public string bank_name { get; set; }
    //    public string account_name { get; set; }
    //    public string account_number { get; set; }
    //    public string bank_code { get; set; }
    //}


    public class NubanModel
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
        public string Bank_name { get; set; }
        public string bank_code { get; set; }
        public string requests { get; set; }
        public string execution_time { get; set; }
        public string status { get; set; }
    }

}
