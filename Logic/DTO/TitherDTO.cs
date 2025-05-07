using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DTO
{
    public class TitherDTO
    {

        public string Month { get; set; }
        public string Year { get; set; }
      
        public int Expenses { get; set; }//int
        public int Revenues { get; set; }//int
    
    
        



    }
    public class SearchTither
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }



    }
}
