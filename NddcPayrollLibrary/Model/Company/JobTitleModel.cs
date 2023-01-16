using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Company
{
    public class JobTitleModel
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
    }
}
