using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Company
{
    public class BankModel
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string Code { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
