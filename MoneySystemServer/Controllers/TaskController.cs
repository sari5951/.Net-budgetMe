using Logic.DTO;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneySystemServer.Code;
using static Logic.DTO.TaskDTO;

namespace MoneySystemServer.Controllers
{
    [IsActive]
    public class TaskController : GlobalController
    {
        private ITaskService TaskService;
        public TaskController(ITaskService taskService)
        {
            this.TaskService = taskService;
        }

        [HttpPost]
        public GResult<List<TaskDTO>> GetTasks(SearchTaskDTO searchTask)
        {
            return Success(TaskService.GetTasks(UserId.Value, searchTask));

        }

        [HttpPost]
        public Result AddTask(TaskDTO task)
        {
            TaskService.AddTask(task, UserId.Value);
            return Success();
        }
        [HttpPut]
        public Result UpdateTask(TaskDTO task)
        {
            TaskService.UpdateTask(task);
            return Success();
        }

        [HttpDelete("{Id}")]
        public Result DeleteDebt(int Id)
        {
            var isTaskExist = TaskService.DeleteTask(Id);
            if (!isTaskExist)
                return Fail();
            return Success();
        }

    }
}
