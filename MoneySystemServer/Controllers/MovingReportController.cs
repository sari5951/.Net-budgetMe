using Logic.DTO;
using Logic;
using Microsoft.AspNetCore.Mvc;
using MoneySystemServer.Code;
using MoneySystemServer.Controllers;
using static Logic.Services.MovingReportService;
using Logic.Services;

namespace Api.Controllers
{
    [IsActive]
    public class MovingReportController: GlobalController
    {


            private IMovingReportsServies MovingrepotsService;

            public MovingReportController(IMovingReportsServies ReportsService)
            {
                this.MovingrepotsService = ReportsService;
            }

            [HttpPost]
            public GResult<ReturnMovingReport> GetmovingReports(SearchMovingReport search)
            {
                return Success(MovingrepotsService.GetmovingReports(UserId.Value,search));
            }

    }
  

}
