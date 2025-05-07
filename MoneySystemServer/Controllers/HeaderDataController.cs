using Logic;
using Logic.DTO;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneySystemServer.Code;
using MoneySystemServer.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Controllers
{

    public class HeaderDataController : GlobalController
    {
        private IHeaderDataService HeaderDataService;
        public HeaderDataController(IHeaderDataService HeaderDataService)
        {
            this.HeaderDataService = HeaderDataService;
        }

        [HttpGet]
        public GResult<HeaderDataDTO> GetHeaderData()
        {
            return Success(HeaderDataService.GetHeaderData(ManagerId.Value, UserId.Value));
        }

        [HttpPost]
        [IsManagers]
        public Result AddHeaderData([FromForm] HeaderDataDTO newHeaderData)
        {
            var request = Request;
            HeaderDataDTO newHeaderDataDTO= null;
            if (request.Form != null && request.Form.Files != null && request.Form.Files.Count > 0 && request.Form.Files[0] != null && request.Form.Files[0].Length > 0)
            {
                byte[] data = null;

                using (var ms = new MemoryStream())
                {
                    request.Form.Files[0].CopyTo(ms);
                    data = ms.ToArray();
                }


                newHeaderDataDTO = new HeaderDataDTO()
                {
                    Id = newHeaderData.Id,
                    Title = newHeaderData.Title,
                    Slogan = newHeaderData.Slogan,
                    ManagerId = UserId.Value,
                    ColorBackroundHeader = newHeaderData.ColorBackroundHeader,
                    ColorFont = newHeaderData.ColorFont,
                    ContentLogo = data,
                    FileName = request.Form.Files[0].FileName

                };

            }
            return Success(HeaderDataService.AddHeaderData(newHeaderDataDTO,UserId.Value));

        }

        [HttpPut]
        [IsManagers]
        public Result UpdateHeaderData([FromForm] HeaderDataDTO HeaderData)
        {

            var request = Request;
            HeaderDataDTO newHeaderDataDTO = null;
            if (request.Form != null && request.Form.Files != null && request.Form.Files.Count > 0 && request.Form.Files[0] != null && request.Form.Files[0].Length > 0)
            {
                byte[] data = null;

                using (var ms = new MemoryStream())
                {
                    request.Form.Files[0].CopyTo(ms);
                    data = ms.ToArray();
                }


                newHeaderDataDTO = new HeaderDataDTO()
                {
                    Id = HeaderData.Id,
                    Title = HeaderData.Title,
                    Slogan = HeaderData.Slogan,
                    ManagerId = UserId.Value,
                    ColorBackroundHeader = HeaderData.ColorBackroundHeader,
                    ColorFont = HeaderData.ColorFont,
                    ContentLogo = data,
                    FileName = request.Form.Files[0].FileName


                };

            }
            return Success(HeaderDataService.UpdateHeaderData(HeaderData,UserId.Value));
        }
    }

}


