using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DTO
{
    public class MovingReportDTO
    {
        //אותיות גדולות בתחילת השם
        public string Month { get; set; }
        public string Year { get; set; }
        public double? Expenses { get; set; }//הוצאות
        public double? Revenues { get; set; }//הכנסות

    }
    public class SearchMovingReport
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool all { get; set; }

    }
    public class ReturnMovingReport
    {
        public List<MovingReportDTO> listMovingReports {  get; set; }
        public DateTime StartDate {  get; set; }

        public ReturnMovingReport()
        {
            listMovingReports = new List<MovingReportDTO>();
            StartDate = new DateTime();
        }
    }
}
