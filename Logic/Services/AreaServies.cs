﻿using Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IAreaServies
    {
        List<AreaDTO> GetAreas(int CurrentUserId, int? type);

        bool AddArea(AreaDTO area, int CurrentUserId);

        bool UpdateArea(AreaDTO area, int CurrentUserId);

        bool DeleteArea(int id);

        List<IdName> GetAreaList(int userId, int type);
    }
    public class AreaServies : IAreaServies
    {
        private IDBService dbService;

        public AreaServies(IDBService dbService)
        {
            this.dbService = dbService;
        }

        public List<AreaDTO> GetAreas(int CurrentUserId, int? type)
        {
            List<AreaDTO> list = new List<AreaDTO>();

            if (dbService.entities.User2Areas.Any(x => x.UserId == CurrentUserId))
            {
                var query = dbService.entities.User2Areas.Where(x => x.UserId == CurrentUserId).ToList();
                if (type > 0)
                {
                    query = query.Where(x => x.UserId == CurrentUserId && x.Type == (int)type).ToList();
                }
                if (!query.Any(x => x.Index > 0))
                {
                    SortByName(CurrentUserId, type);
                }
                
                query = query.OrderBy(x => x.Index).ToList();
                list = query.Select(x => new AreaDTO()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Type = x.Type,
                    Sum = x.Sum,
                    IsMaaser = x.IsMaaser,
                    Index = x.Index,
                    IsActive = x.IsActive,
                    IsConnected= dbService.entities.Debts.Any(y => y.UserId == CurrentUserId&& x.Id == y.AreaId)
                }).ToList();
            }
            return list;
        }

        public List<IdName> GetAreaList(int userId, int type)
        {
            List<IdName> list = new List<IdName>();
            list = dbService.entities.Areas.Where(x => x.Type == type).Select(x => new IdName()
            {
                Name = x.Description,
                Id = x.Id
            }).ToList();

            return list;
        }

        public bool AddArea(AreaDTO user2Area, int CurrentUserId)
        {
            //if (dbService.entities.User2Area.Any(x => x.UserId == CurrentUserId && x.Type == user2Subject.Type && x.Subject.Id != user2Subject.Id && x.Subject.Description == user2Subject.Description.Name))
            //{
            //    return false;
            //}
            //var subject = dbService.entities.User2Area.FirstOrDefault(x => x.Id == user2Subject.Description.Id);
            //if (user2Subject.Description.Id == 0)
            //{
            //    var newSubject = new Subject()
            //    {
            //        Description = user2Subject.Description.Name,
            //        Type = user2Subject.Type,
            //        IsGlobal = false
            //    };
            //    dbService.entities.Subjects.Add(newSubject);
            //    dbService.Save();
            //    user2Subject.Description.Id = newSubject.Id;
            //}
            var newUser2Subject = new User2Area()
            {
                UserId = CurrentUserId,
                Sum = user2Area.Sum,
                IsMaaser = user2Area.IsMaaser,
                IsActive = user2Area.IsActive,
                Description = user2Area.Description,
                Index = user2Area.Index,
                Type = user2Area.Type
            };
            dbService.entities.User2Areas.Add(newUser2Subject);


            if (user2Area.IndexOn == true)
            {
                var dbUser = dbService.entities.Users.FirstOrDefault(x => x.Id == CurrentUserId);
                dbUser.AreaIndexOn = true;
            }
            dbService.Save();
            var dbIndexOn = dbService.entities.Users.FirstOrDefault(x => x.Id == CurrentUserId).AreaIndexOn;
            if (user2Area.IndexOn != true && dbIndexOn != true)
            {
                SortByName(CurrentUserId, newUser2Subject.Type);
            }
            return true;
        }

        public bool UpdateArea(AreaDTO area, int CurrentUserId)
        {
            if (dbService.entities.User2Areas.Any(x => x.UserId == CurrentUserId && x.Type == area.Type && x.Id != area.Id && x.Description == area.Description))
            {
                return false;
            }
            var dbUser2Area = dbService.entities.User2Areas.FirstOrDefault(x => x.Id == area.Id);
            //var subject = dbService.entities.Subjects.FirstOrDefault(x => x.Id == user2Subject.Description.Id);
            //if (user2Subject.Description.Id == 0)
            //{
            //    var newSubject = new Subject()
            //    {
            //        Description = user2Subject.Description.Name,
            //        Type = user2Subject.Type,
            //        IsGlobal = false
            //    };
            //    dbService.entities.Subjects.Add(newSubject);
            //    dbService.Save();
            //    user2Subject.Description.Id = newSubject.Id;
            //}
            if (dbUser2Area != null)
            {
                //if (dbUser2Sub.Subject.IsGlobal != true)
                //{
                //    dbUser2Sub.Subject.Description = user2Subject.Description.Name;
                //}
                //dbUser2Area.SubjectId = user2Subject.Description.Id;
                //dbUser2Area.Global = user2Subject.Global;
                dbUser2Area.Index = area.Index;
                dbUser2Area.IsActive = area.IsActive;
                dbUser2Area.IsMaaser = area.IsMaaser;
                dbUser2Area.Description = area.Description;
                dbUser2Area.Sum = area.Sum;
                if (area.IndexOn == true)
                {
                    var dbUser = dbService.entities.Users.FirstOrDefault(x => x.Id == CurrentUserId);
                    dbUser.AreaIndexOn = true;
                }
                dbService.Save();
                var dbIndexOn = dbService.entities.Users.FirstOrDefault(x => x.Id == CurrentUserId).AreaIndexOn;
                if (area.IndexOn != true && dbIndexOn != true)
                {
                    SortByName(CurrentUserId, area.Type);
                }
                return true;
            }
            return false;
        }

        public bool DeleteArea(int id)
        {
            var dbArea = dbService.entities.Areas.FirstOrDefault(x => x.Id == id);

            if (dbArea != null)
            {
                dbService.entities.Areas.Remove(dbService.entities.Areas.FirstOrDefault(x => x.Id == id));
                dbService.Save();
                return true;
            }
            else { 
                return false;
             }
        }

        public void SortByName(int CurrentUserId, int? type)
        {
            var list = dbService.entities.User2Areas.Where(x => x.UserId == CurrentUserId && x.Type == type).ToList();
            list = list.OrderBy(x => x.Description).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Index = i + 1;
            }
            dbService.Save();
        }

    }
}
