using Logic.DTO;
using Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Logic.Services
{
    public interface IReportsServies
    {
        //List<HistoryDTO> GetHistory(int current);
        List<TitherDTO> GetTithers(int userId, SearchTither searchTither);
        List<DebtReportDTO> GetDebtsRports(int currentUserId);

    }

    public class ReportsService : IReportsServies
    {
        private IDBService dbService;

        public ReportsService(IDBService dbService)
        {
            this.dbService = dbService;
        }



        public List<TitherDTO> GetTithers(int userId, SearchTither searchTither)
        {
            var tithers = new List<TitherDTO>();

            var oldMove = dbService.entities.Movings
                .Where(x => x.User2Area.UserId == userId && x.User2Area.IsMaaser == true)
                .OrderBy(x => x.Date)
                .FirstOrDefault();

            if (oldMove != null)
            {
                DateTime firstDate = oldMove.Date;
                DateTime oldMonth = new DateTime(firstDate.Year, firstDate.Month, 1);
                DateTime lastMonth = DateTime.Now;

                Console.WriteLine($"Received startDate: {searchTither.startDate}");
                Console.WriteLine($"Received endDate: {searchTither.endDate}");

        
                oldMonth = new DateTime(Math.Max(oldMonth.Year, searchTither.startDate.Year), Math.Max(oldMonth.Month, searchTither.startDate.Month), 1);
                lastMonth = new DateTime(Math.Min(lastMonth.Year, searchTither.endDate.Year), Math.Min(lastMonth.Month, searchTither.endDate.Month), 1);

                Console.WriteLine($"Filtered oldMonth: {oldMonth}");
                Console.WriteLine($"Filtered lastMonth: {lastMonth}");

                for (DateTime date = oldMonth; date <= lastMonth; date = date.AddMonths(1))
                {
                    var sumExpenses = dbService.entities.Movings
                        .Where(x => x.User2Area.UserId == userId && x.User2Area.Type == 2 && x.User2Area.IsMaaser == true && x.Date.Year == date.Year && x.Date.Month == date.Month)
                        .Sum(x => x.Sum);

                    var sumRevenues = dbService.entities.Movings
                        .Where(x => x.User2Area.UserId == userId && x.User2Area.Type == 1 && x.User2Area.IsMaaser == true && x.Date.Year == date.Year && x.Date.Month == date.Month)
                        .Sum(x => x.Sum);

                    tithers.Add(new TitherDTO()
                    {
                        Month = date.Month.ToString(),
                        Year = date.Year.ToString(),
                        Expenses = sumExpenses,
                        Revenues = sumRevenues
                    });
                }
            }

            return tithers;
        }

        public List<DebtReportDTO> GetDebtsRports(int currentUserId)
        {
            List<DebtReportDTO> list = new List<DebtReportDTO>();
            List<Debt> debts = dbService.entities.Debts.Where(x => x.UserId == currentUserId).ToList();
            foreach (var debt in debts)
            {
                int sum = 0;
                sum = dbService.entities.Movings.Where(x => x.User2Area.UserId == currentUserId && x.User2Area.Id == debt.AreaId).Sum(x => x.Sum);

                list.Add(new DebtReportDTO()
                {
                    Description = debt.Description,
                    Urgency = new IdName()
                    {
                        Id = debt.UrgencyId,
                        Name = debt.Urgency.Description
                    },
                    Sum = debt.Sum,
                    balance = (debt.Sum- sum)
                });
            }
            return list;
        }
    }
}

