using ToDoList.Domain.Entity;
using ToDoList.Domain.Response;
using ToDoList.Domain.ViewModels.Task;

namespace ToDoList.Service.Interfaces;

public interface ITaskService
{
    Task<IBaseResponse<TaskEntity>> Create(CreateTaskViewModel model);
    Task<IBaseResponse<bool>> EndTask(long id);
    Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTask();
}