using Logic.DTO;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using MoneySystemServer.Code;

namespace MoneySystemServer.Controllers
{
    [IsActive]
    public class OrganizationController : GlobalController
    {
        private IOrganizationService organzationService;

        public OrganizationController(IOrganizationService organzationService)
        {
            this.organzationService = organzationService;
        }

        [HttpGet("{id}")]
        [IsSystemManager]
        public GResult<OrganizationDTO> GetOrganizationById(int id)
        {
            return Success(organzationService.GetOrganizationById(id));
        }

        [HttpGet]
        [IsManagers]
        public GResult<OrganizationDTO> GetOrganization()
        {
            return Success(organzationService.GetCurrentOrganization(UserId.Value));
        }

        [HttpPost]
        [IsManagers]
        
        public Result AddOrganization(OrganizationDTO organizationDTO)
        {
            var isSuccess = organzationService.AddOrganization(organizationDTO, UserId.Value);
            if (isSuccess)
            {
                return Success();

            }
            return Fail();

        }

        [HttpPut]
        [IsManagers]

        public Result UpdateOrganization(OrganizationDTO organizationDTO) 
        {
            var isSuccess = organzationService.UpdateOrganization(organizationDTO);
            if (isSuccess)
            {
                return Success();
            }
            return Fail();
        }
    }
}
