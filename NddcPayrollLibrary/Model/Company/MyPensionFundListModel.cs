using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Company
{
    public class MyPensionFundListModel
    {
        public int Id { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }

		private string myFullDescription;
		public string FullDescription
		{
			get 
			{
				myFullDescription = $"{Code} - {Description}";
				return myFullDescription; 
			}
		}

	}
}
