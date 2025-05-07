using Logic.DTO;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneySystemServer.Code;
using System.ServiceModel.Channels;

namespace MoneySystemServer.Controllers
{
    public class ListController : GlobalController
    {

        private IListService listService;

        public ListController(IListService listService)
        {
            this.listService = listService;
        }
        [HttpGet]
        [IsManagers]
        public GResult<ListsDTO> GetAllLists()
        {
            return Success(listService.GetAllLists());
        }
        [IsPermission]
        [HttpPost]
        public GResult<List<IdName>> GetList(IdNameDB item)
        {
            return Success(listService.GetList(item));
        }

        [HttpPost]
        [IsManagers]
        public AnsOption AddItem(IdNameDB idName)
        {
            var isSuccess = listService.AddItem(idName);
            if (isSuccess == AnsOption.Yes)
            {
                return AnsOption.Yes;
            }
            else
                if (isSuccess == AnsOption.No)
            {
                return AnsOption.No;
            }
            else { return AnsOption.OtherOption; }
        }

        [HttpPost]
        [IsManagers]
        public AnsOption DeleteItem(IdNameDB idName)
        {
            AnsOption ans = listService.DeleteItem(idName);
            return ans;
        }

        [HttpPut]
        [IsManagers]
        public Result UpdateItem(IdNameDB idName)
        {
            var isSuccess = listService.UpdateItem(idName);
            if (isSuccess)
            {
                return Success();
            }
            return Fail(message: "אין אפשרות לעדכן תחומים עם תחום זה");
        }
    }
}
