using Logic.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IUserService
    {
        List<UserDTO> GetUsers(int currentUserId, SearchUser SearchUser);
        List<IdName> GetUserTypes(int currentUserId);
        UserDTO GetUser(int id);
        bool AddUser(UserGlobalDTO user);
        bool UpdateUser(UserGlobalDTO user);
        bool DeleteUser(int id);
        void ChangeUser2Manager(int id);
        bool ChangeUserTypeOrLenderAndDelete(int oldLender, UserTypeDTO userType, int? newLender);
        int? GetManagerId(int id);
    }

    public class UserService : IUserService
    {
        private IDBService dbService;

        public UserService(IDBService dbService)
        {
            this.dbService = dbService;
        }
        public List<UserDTO> GetUsers(int currentUserId, SearchUser searchUser)
        {
            var users = dbService.entities.Users.ToList();

            var currentUser = dbService.entities.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.UserTypeId == (int)UserTypeDTO.LendersManager)
            {
                var lenders = users.Where(x => x.ManagerId == currentUser.Id).ToList();
                var lenderUserIds = lenders.Select(l => l.Id).ToList();

                users = users.Where(u => lenderUserIds.Contains(u.LenderId.GetValueOrDefault()) || lenders.Any(l => l.Id == u.Id)).ToList();
            }
            else if (currentUser.UserTypeId == (int)UserTypeDTO.Lender)
            {
                users = users.Where(x => x.LenderId == currentUser.Id).ToList();
            }


            if (searchUser.UserType.Name != "" && searchUser.UserType.Id > 0)
            {
                users = users.Where(x => x.UserTypeId == searchUser.UserType.Id).ToList();
            }

            else if (searchUser.GuidersManager.Name != "" && searchUser.GuidersManager.Id > 0)
            {
                users = users.Where(x => x.ManagerId == searchUser.GuidersManager.Id).ToList();
            }

            else if (searchUser.Lender.Name != "" && searchUser.Lender.Id > 0)
            {
                users = users.Where(x => x.LenderId == searchUser.Lender.Id).ToList();
            }





            var list = users.Select(x => new UserDTO()
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Phone = x.Phone,
                Password = "********",
                UserType = (UserTypeDTO)x.UserTypeId,
                IsActive = x.IsActive,
                IsYearlyPay = x.IsYearlyPay,
                RegisterDate = x.RegisterDate,
                PayDate = x.PayDate,
                UserKind=x.UserKind,
                OrganizationId = x.OrganizationId,
                
                City = new IdName()
                {
                    Id = x.CityId != null ? (int)x.CityId : 0,
                    Name = x.CityId != null ? x.City.Name : "",

                }

            }).ToList();

            return list;
        }

        public UserDTO GetUser(int id)
        {
            var dbUser = dbService.entities.Users.FirstOrDefault(x => x.Id == id);
            if (dbUser != null)
            {
                var user = new UserDTO()
                {
                    Id = dbUser.Id,
                    Email = dbUser.Email,
                    FirstName = dbUser.FirstName,
                    LastName = dbUser.LastName,
                    Phone = dbUser.Phone,
                    Password = dbUser.Password,
                    UserType = (UserTypeDTO)dbUser.UserTypeId,
                    IsActive = dbUser.IsActive,
                    IsYearlyPay = dbUser.IsYearlyPay,
                    RegisterDate = dbUser.RegisterDate,
                    PayDate = dbUser.PayDate,
                    UserKind = dbUser.UserKind,
                    OrganizationId = dbUser.OrganizationId,

                    Lender = new IdName()
                    {
                        Id = dbUser.LenderId != null ? (int)dbUser.LenderId : 0,
                        Name = dbUser.LenderId != null ? dbUser.Lender.FirstName + " " + dbUser.Lender.LastName : ""
                    }
                    ,
                    Manager = new IdName()
                    {
                        Id = dbUser.ManagerId > 0 ? (int)dbUser.ManagerId : 0,
                        Name = dbUser.ManagerId > 0 ? dbUser.Manager.FirstName + " " + dbUser.Manager.LastName : ""
                    },

                };
                if (dbUser.City != null)
                {
                    user.City = new IdName()
                    {
                        Id = (int)dbUser.CityId,
                        Name = dbUser.City.Name
                    };
                }

                return user;

            }
            return new UserDTO();
        }


        public bool AddUser(UserGlobalDTO newUser)
        {
            bool isExist = dbService.entities.Users.Any(x => x.Email == newUser.Email);
            if (!isExist)
            {
                var dbNewUser = new User()
                {
                    Email = newUser.Email,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Phone = newUser.Phone,
                    Password = newUser.Password,
                    UserTypeId = (int)newUser.UserType,
                    IsActive = newUser.IsActive,
                    IsYearlyPay = newUser.IsYearlyPay,
                    RegisterDate = newUser.RegisterDate,
                    PayDate = newUser.PayDate,
                    UserKind = newUser.UserKind,

                };
                if (newUser.City.Id > 0)
                {
                    dbNewUser.CityId = newUser.City.Id;
                }
                if (newUser.Lender.Id > 0)
                {
                    dbNewUser.LenderId = newUser.Lender.Id;
                }
                if (newUser.Manager.Id > 0)
                {
                    dbNewUser.ManagerId = newUser.Manager.Id;
                }


                var currentUser = dbService.entities.Users.FirstOrDefault(x => x.Id == newUser.Id);
                if (currentUser != null)
                {
                    //להוסיף בדיקה שאם את ה מנהל מלווים והמשתמש החדש שרוצים להוסיף הוא מלווה אז שיהיה לו מלווה אידי

                    //if (currentUser.UserType.Id == 5)//האם אתה מנהל מלווים
                    //{
                    //    dbuser.UserTypeId = 2;
                    //    dbuser.ManagerId = currentUser.Id;
                    //}
                    if (currentUser.UserTypeId == (int)UserTypeDTO.Lender)//האם אתה מלווה
                    {
                        dbNewUser.UserTypeId = (int)UserTypeDTO.Attendance;
                        dbNewUser.LenderId = currentUser.Id;
                    }
                }
                else
                {
                    if (currentUser == null)
                    {
                        dbNewUser.UserTypeId = (int)UserTypeDTO.Regular;
                    }
                }
               
               
                dbService.entities.Users.Add(dbNewUser);
                dbService.Save();
                return false;
            }
            return true;
        }

        public bool UpdateUser(UserGlobalDTO user)
        {
            var dbUser = dbService.entities.Users.FirstOrDefault(x => x.Id == user.Id);
            if (dbUser != null)
            {
                if (dbService.entities.Users.Any(x => x.Id != user.Id && x.Email == user.Email))
                {
                    return true;
                }
                else
                {
                    dbUser.Email = user.Email;
                    dbUser.FirstName = user.FirstName;
                    dbUser.LastName = user.LastName;
                    dbUser.Phone = user.Phone;
                    dbUser.Password = user.Password;



                    if (user.UserType != (UserTypeDTO)dbUser.UserTypeId)
                    {
                        if (dbUser.UserTypeId == (int)UserTypeDTO.Lender)
                        {

                            var change = dbService.entities.Users.Where(x => x.ManagerId == user.Id).ToList();
                            change.ForEach(x => x.ManagerId = null);
                        }
                        else if (dbUser.UserTypeId == (int)UserTypeDTO.Attendance)
                        {
                            var change = dbService.entities.Users.Where(x => x.LenderId == user.Id).ToList();
                            change.ForEach(x => x.LenderId = null);
                        }
                    }

                    dbUser.UserTypeId = (int)user.UserType;
                    dbUser.IsActive = user.IsActive;
                    dbUser.IsYearlyPay = user.IsYearlyPay;
                    dbUser.RegisterDate = user.RegisterDate;
                    dbUser.PayDate = user.PayDate;

                    //if (user.City.Id > 0)
                    //{
                    //    dbUser.CityId = user.City.Id;
                    //}
                    if (user.Lender.Id > 0)
                    {
                        dbUser.LenderId = user.Lender.Id;
                    }
                    if (user.Lender == null)
                    {
                        dbUser.LenderId = null;
                    }
                    if (user.UserType != UserTypeDTO.Attendance)
                    {
                        dbUser.LenderId = null;
                    }
                    if (user.Manager == null || user.UserType != UserTypeDTO.Lender)
                    {
                        dbUser.ManagerId = null;
                    }
                    else if (user.Manager.Id > 0)
                    {
                        dbUser.ManagerId = user.Manager.Id;
                    }


                    dbService.Save();
                    return false;
                }
            }
            return false;

        }

        public bool DeleteUser(int id)
        {
            var dbUser = dbService.entities.Users.FirstOrDefault(x => x.Id == id);
            if (dbUser != null)
            {
                dbUser.CityId = null;
            }

            if (dbUser != null)
            {

                dbUser.IsActive = false;
                dbService.Save();
                return true;
            }
            return false;

        }


        public void ChangeUser2Manager(int id)
        {
            var dbUser = dbService.entities.Users.FirstOrDefault(x => x.Id == id);
            if (dbUser != null)
            {
                dbUser.UserTypeId = (int)UserTypeDTO.SystemManager;
                dbService.Save();
            }
        }

        public List<IdName> GetUserTypes(int currentUserId)
        {
            var query = dbService.entities.UserTypes.ToList();
            var currentUser = dbService.entities.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser != null)
            {
                if (currentUser.UserType.Id != (int)UserTypeDTO.SystemManager)
                {
                    query = query.Where(x => x.Id != 1).ToList();
                }
            }
            var userTypes = query.Select(x => new IdName()
            {
                Id = x.Id,
                Name = x.Description
            }).ToList();
            return userTypes;
        }



        public bool ChangeUserTypeOrLenderAndDelete(int oldLender, UserTypeDTO userType, int? newLender)
        {

            var usersUnderLender = dbService.entities.Users.Where(x => x.LenderId == oldLender).ToList();
            foreach (var u in usersUnderLender)
            {
                u.LenderId = newLender;
                u.UserTypeId = (int)userType;
            }

            dbService.Save();
            this.DeleteUser(oldLender);
            return true;
        }

        public int? GetManagerId(int id)
        {
            
            var currentUser = dbService.entities.Users.FirstOrDefault(x => x.Id == id);
            if (currentUser != null)
            {
                if (currentUser.UserTypeId == (int)UserTypeDTO.LendersManager ||
                   currentUser.UserTypeId == (int)UserTypeDTO.SystemManager)
                {
                    return id;
                }
                else if (currentUser.UserTypeId ==(int)UserTypeDTO.Lender)
                {
                    var managerId = currentUser.ManagerId;
                    if(managerId != null)
                    {
                        return managerId;
                    }
                }
                else if(currentUser.UserTypeId == (int)UserTypeDTO.UnderLander)
                {
                    var managerId = currentUser.Lender?.ManagerId;
                    if(managerId!=null)
                    {
                        return managerId;
                    }
                }
                else
                {
                    var systemManager = dbService.entities.Users.FirstOrDefault(u => u.UserTypeId == (int)UserTypeDTO.SystemManager);
                    if (systemManager != null)
                    {
                        return systemManager.Id;
                    }
                }
               
            }
            return null;

        }


    }
}
