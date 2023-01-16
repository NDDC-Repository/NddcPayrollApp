using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Enums
{
    public enum Categories
    {
      Permanent,
      Political,
      Sabatical,
      Temporary,
      Contract
    }

    public enum BenefitTypes
    {
        Housing,
        Furniture,
        Transportation,
        Meal,
        Utility,
        Domestic,
        Leave,
        Medical,
        Education,
        Driver,
        Vehicle,
        Security,
        Hazard
    }

    

    public enum GradeLevels
    {
        MD,
        EDFA,
        GL031,
        GL032,
        GL033,
        GL034,
        GL035,
        GL036,
        GL037,
        GL038,
        GL039
    }
}
