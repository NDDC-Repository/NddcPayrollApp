﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Payroll
{
    public class MyPayrollJournalTitleModel
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "The Batch Name is Required")]
        public string JournalName { get; set; }
        public int Month { get; set; }
        public string MonthYear { get; set; }
        public DateTime CurrentPeriod { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        private string myFullDesc;
        public string FullDescription
        {
            get
            {
                myFullDesc = $"{JournalName} {MonthYear}";
                return myFullDesc;
            }

        }
    }
}
