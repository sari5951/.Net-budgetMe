﻿using Logic.DTO;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Logic.DTO.TaskDTO;

namespace Logic.Services
{
    public interface ITaskService
    {
        List<TaskDTO> GetTasks(int CurrentUserId, SearchTaskDTO searchTask);

        // TaskDTO GetTask(int id);
        bool AddTask(TaskDTO task, int currentUserId);
        bool UpdateTask(TaskDTO task);
        bool DeleteTask(int id);



    }
    public class TaskService : ITaskService
    {
        private IDBService dbService;

        public TaskService(IDBService dbService)
        {
            this.dbService = dbService;
        }

        public List<TaskDTO> GetTasks(int CurrentUserId, SearchTaskDTO searchTask)//taskSearch
        {
            List<TaskDTO> tasks = new List<TaskDTO>();

            if (dbService.entities.Tasks.Any(x => x.UserId == CurrentUserId))
            {
                var query = dbService.entities.Tasks.Where(x => x.UserId == CurrentUserId);

                if(searchTask != null)
                {
                    query = query.Where(x => x.Description.Contains(searchTask.Description));


                }
                if (searchTask.CreateDate != default(DateTime))
                {
                    // Assuming CreateDate in database is also of type DateTime
                    // We'll compare the date part only, not the time
                    DateTime endDate = searchTask.CreateDate.AddDays(1); // end of the day of the search date

                    query = query.Where(x => x.CreateDate >= searchTask.CreateDate && x.CreateDate < endDate);
                }
                if (searchTask.Status.Name != "")
                {
                    query = query.Where(x => x.Status.Description.Contains(searchTask.Status.Name));
                }

                if (searchTask.Urgency.Name != "")
                {
                    query = query.Where(x => x.Urgency.Description.Contains(searchTask.Urgency.Name));
                }

                if (searchTask.DoDate != null)
                {
                    DateTime? endDate = searchTask.DoDate.Value.AddDays(1);
                    query = query.Where(x => x.DoDate >= searchTask.DoDate && x.DoDate < endDate);
                }

                if (!string.IsNullOrEmpty(searchTask.Comments))
                {
                    query = query.Where(x => x.Comment.Contains(searchTask.Comments));
                }



                tasks = query.Select(x => new TaskDTO()
                {
                    Id = x.Id,
                    Description = x.Description,
                    CreateDate = x.CreateDate,
                    Comments = x.Comment,

                    Status = new IdName
                    {
                        Id = x.Status.Id,
                        Name = x.Status.Description
                    },
                    Urgency = new IdName
                    {
                        Id = x.Urgency.Id,
                        Name = x.Urgency.Description
                    },
                    DoDate = x.DoDate,


                }).ToList();
            }
            return tasks;
        }

        //public TaskDTO GetTask(int id)
        //{
        //    var dbTask = dbService.entities.Tasks.FirstOrDefault(x => x.Id == id);
        //    if (dbTask != null)
        //    {
        //        var task = new TaskDTO()
        //        {
        //            Id = dbTask.Id,
        //            Description = dbTask.Description,
        //            Remarks = dbTask.Remarks,
        //            StatusId = dbTask.StatusId,
        //            UserId = dbTask.UserId,
        //            Date = dbTask.Date
        //        };
        //        return task;
        //    }
        //    return new TaskDTO();
        //}

        public bool AddTask(TaskDTO task, int currentUserId)
        {
            var newTask = new Task()
            {
                Description = task.Description,
                Comment = task.Comments,
                StatusId = task.Status.Id,
                UserId = currentUserId,
                CreateDate = DateTime.Now,
                UrgencyId = task.Urgency.Id,
                DoDate = task.DoDate
            };
            dbService.entities.Tasks.Add(newTask);
            dbService.Save();

            return true;

        }
        public bool UpdateTask(TaskDTO task)
        {
            var dbTask = dbService.entities.Tasks.FirstOrDefault(x => x.Id == task.Id);
            if (dbTask != null)
            {
                dbTask.Description = task.Description;
                dbTask.Comment = task.Comments;
                dbTask.StatusId = task.Status.Id;
                dbTask.DoDate = task.DoDate;
                dbTask.UrgencyId = task.Urgency.Id;

                dbService.Save();
                return true;
            }
            return false;
        }

        public bool DeleteTask(int id)
        {
            var dbTask = dbService.entities.Tasks.FirstOrDefault(x => x.Id == id);
            if (dbTask != null)
            {
                dbService.entities.Tasks.Remove(dbService.entities.Tasks.FirstOrDefault(x => x.Id == id));
                dbService.Save();
                return true;
            }
            return false;

        }
    }
}


