using Logic.DTO;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoneySystemServer.Code;
using static Logic.DTO.TitherDTO;

namespace MoneySystemServer.Controllers
{
    //public class RepotsController 
    //{
    //    private IReportsServies repotsService;

    //    public RepotsController(IReportsServies ReportsService)
    //    {
    //        this.repotsService = ReportsService;
    //    }

    //    [HttpGet]
    //    public GResult<List<HistoryDTO>> GetHistory()
    //    {
    //        return Success(repotsService.GetHistory(UserId.Value));
    //    }

    //    [HttpGet]
    //    public GResult<List<TitherDTO>> GetTithers()
    //    {
    //        return Success(repotsService.GetTithers(UserId.Value));
    //    }

    //}
    [IsActive]
    public class RepotsController : GlobalController
    {
        private IReportsServies repotsService;

        public RepotsController(IReportsServies ReportsService)
        {
            this.repotsService = ReportsService;
        }

        //[HttpGet]
        //public GResult<List<HistoryDTO>> GetHistory()
        //{
        //    return Success(repotsService.GetHistory(UserId.Value));
        //}
        [HttpPost]
        public GResult<List<TitherDTO>> GetTithers(SearchTither searchTither)
        {
            return Success(repotsService.GetTithers(UserId.Value,searchTither));
        }
        [HttpGet]
        public GResult<List<DebtReportDTO>> GetDebtsRports()
        {
            return Success(repotsService.GetDebtsRports(UserId.Value));
        }

    }
}
