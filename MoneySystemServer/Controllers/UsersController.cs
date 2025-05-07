using Logic.DTO;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneySystemServer.Code;

namespace MoneySystemServer.Controllers
{
    public class UsersController : GlobalController
    {
        private IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }
        [IsActive]
        [IsPermission]
        [HttpPost]
        public GResult<List<UserDTO>> GetUsers(SearchUser SearchUser)
        {
            return Success(userService.GetUsers(UserId.Value, SearchUser));
        }

        [HttpGet("{id}")]
        public GResult<UserDTO> GetUser(int id)
        {

            return Success(userService.GetUser(id));
        }



        [AllowAnonymous]
        [HttpPost]
        public Result AddUser(UserGlobalDTO newUser)
        {
            int userId = 0;
            //var task = sessionService.GetCurrentUser();
            //UserDTO currentUser = null;
            //if (task != null)
            //{
            //    currentUser = task.Result;
            //}
            if (newUser != null)
            {
                var isEmailExist = userService.AddUser(newUser);
                if (isEmailExist)
                {
                    return Fail(message: "user with such email already exist");
                }
                else
                {
                    if (newUser.UserType == UserTypeDTO.SystemManager)
                    {
                        ChangeUser2Manager(newUser.Id);
                    }
                }
            }

            return Success();



        }
        
        [HttpPut]
        public Result UpdateUser(UserGlobalDTO user)
        {
            var isEmailExist = userService.UpdateUser(user);
            if (isEmailExist)
            {
                return Fail(message: "user with such email already exist");
            }
            else
            {
                if (user.UserType == UserTypeDTO.SystemManager)
                {
                    ChangeUser2Manager(user.Id);
                }
                return Success();
            }
        }
        [IsActive]
        [IsSystemManager]
        private void ChangeUser2Manager(int id)
        {
            userService.ChangeUser2Manager(id);
        }

        [IsActive]
        [IsPermission]
        [HttpDelete("{id}")]
        public Result DeleteUser(int id)
        {
            //AnsOption isSuccess = listService.DeleteItem(idName);
            //if (isSuccess == AnsOption.Yes)
            //{
            //    return new Result(true) { Message = "נמחק בהצלחה" };
            //}
            //else if (isSuccess == AnsOption.OtherOption)
            //{
            //    return new Result(false) { Message = " לא ניתן למחוק כיוון שנעשה שימוש בנתון זה במקום אחר במערכת " };
            //}
            //else
            //    return new Result(false) { Message = "ארעה שגיאה במהלך המחיקה ..." };
            var isUserExist = userService.DeleteUser(id);
            if (!isUserExist)
            {
                return Fail(message: "user not found");

            }
            return Success();
        }

        [IsPermission]
        [IsActive]
        [HttpGet]
        public GResult<List<IdName>> GetUserTypes()
        {
            //var task = sessionService.GetCurrentUser();
            //UserDTO currentUser = null;
            //if (task != null)
            //{
            //    currentUser = task.Result;
            //}
            return Success(userService.GetUserTypes(UserId.Value));
        }

        [HttpPut]
        public Result ChangeUserTypeOrLenderAndDelete(LenderParams lenderParams)
        {
            var succes = userService.ChangeUserTypeOrLenderAndDelete(lenderParams.oldLender, lenderParams.userType, lenderParams.newLender);
            return Success();
        }

       
        //[IsManagers]

    }
}
