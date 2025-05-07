using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logic.DTO
{
    public class DebtDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Payments { get; set; }
        public IdName Urgency { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public int Sum { get; set; }
        public IdName AreaId { get; set; }

        //כרגע אין צורך ב UserId 
        //האדם שנכנס הוא בעל החוב, בהמשך יהיה גם מלווה שיוכל להכנס
    }

    public class SearchDebtDTO
    {
        public bool? IsActive { get; set; } 

        public IdName Urgency { get; set; }

        public string Description { get; set; }

        public int Payments { get; set; }

        public int Sum { get; set; }
    }

    public class DebtReportDTO
    {
        public string? Description { get; set; }
        public IdName Urgency { get; set; }
        public int Sum { get; set; }
        public int balance { get; set; }

    }

}
