using Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Logic.Services.MovingReportService;

namespace Logic.Services
{
    public interface IMovingReportsServies
    {
        ReturnMovingReport GetmovingReports(int current, SearchMovingReport search);

    }
    public class MovingReportService : IMovingReportsServies
    {



        private IDBService dbService;

        public MovingReportService(IDBService dbService)
        {
            this.dbService = dbService;
        }

        //public ReturnMovingReport GetmovingReports(int current, SearchMovingReport search)
        //{
        //    var MovingReports = new ReturnMovingReport();
        //    MovingReports.listMovingReports = new List<MovingReportDTO>();
        //    MovingReports.StartDate = new DateTime();

        //    if (search.all == true)
        //    {
        //        var oldMove = dbService.entities.Movings.Where(x => x.User2Area.UserId == current).OrderBy(x => x.Date).FirstOrDefault();
        //        if (oldMove != null)
        //        {
        //            search.startDate = oldMove.Date;
        //            MovingReports.StartDate = oldMove.Date;
        //        }
        //    }

        //    if (search != null)
        //    {
        //        DateTime firstDate = search.startDate;
        //        DateTime oldMonth = new DateTime(firstDate.Year, firstDate.Month, 1);
        //        DateTime lastMonth = DateTime.Now;
        //        DateTime finalMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);

        //        for (DateTime date = search.startDate; date < search.endDate; date = date.AddMonths(1))
        //        {
        //            int? sumOfExpenses = 0;
        //            int? sumOfRevenues = 0;

        //            var monings = dbService.entities.Movings.Where(x => x.User2Area.UserId == current).ToList();
        //            if (monings != null)
        //            {
        //                var expenses = monings.Where(l => l.User2Area.Type == 2 && l.Date.Month == date.Month && l.Date.Year == date.Year);
        //                var revenues = monings.Where(l => l.User2Area.Type == 1 && l.Date.Month == date.Month && l.Date.Year == date.Year);


        //                if (expenses != null)
        //                {
        //                    sumOfExpenses = expenses.Sum(l => l.Sum);
        //                }
        //                if (revenues != null)
        //                {
        //                    sumOfRevenues = revenues.Sum(l => l.Sum);
        //                }
        //
        //
        //  }
        public ReturnMovingReport GetmovingReports(int currentUserId, SearchMovingReport search)
        {
            var MovingReports = new ReturnMovingReport();

            if (search.all == true)
            {
                var oldMove = dbService.entities.Movings.Where(x => x.User2Area.UserId == currentUserId).OrderBy(x => x.Date).FirstOrDefault();
                if (oldMove != null)
                {
                    search.startDate = oldMove.Date;
                    MovingReports.StartDate = oldMove.Date;
                }
            }

            if (search != null)
            {
                for (DateTime date = search.startDate; date < search.endDate; date = date.AddMonths(1))
                {
                    int? sumOfExpenses = 0;
                    int? sumOfRevenues = 0;

                    if (dbService.entities.Movings != null)
                    {
                        sumOfExpenses = dbService.entities.Movings.Where(l => l.User2Area.UserId == currentUserId && l.User2Area.Type == 2 && l.Date.Month == date.Month && l.Date.Year == date.Year&&l.Sum!=null).Sum(l => l.Sum);
                        sumOfRevenues = dbService.entities.Movings.Where(l => l.User2Area.UserId == currentUserId && l.User2Area.Type == 1 && l.Date.Month == date.Month && l.Date.Year == date.Year && l.Sum != null).Sum(l => l.Sum);
                    }


                    MovingReports.listMovingReports.Add(new MovingReportDTO()
                    {
                        Month = date.Month.ToString(),
                        Year = date.Year.ToString(),
                        Expenses = sumOfExpenses,
                        Revenues = sumOfRevenues
                    });

                    // logic here

                }
            }

            return MovingReports;
        }


    }


}

