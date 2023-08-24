using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using ToDoList.DAL.Interfaces;
using ToDoList.Domain.Entity;
using ToDoList.Domain.Enum;
using ToDoList.Domain.Extension;
using ToDoList.Domain.Response;
using ToDoList.Domain.ViewModels.Task;
using ToDoList.Service.Interfaces;

namespace ToDoList.Service.Implementations;

public class TaskService : ITaskService
{
    private readonly IBaseRepository<TaskEntity> _taskRepository;
    private ILogger<TaskService> _logger;

    public TaskService(IBaseRepository<TaskEntity> taskRepository,
        ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<IBaseResponse<TaskEntity>> Create(CreateTaskViewModel model)
    {
        try
        {
            _logger.LogInformation($"request for creating Task - {model.Name}");

            var task = await _taskRepository.GetAll()
                .Where(x => x.Created.Date == DateTime.Today)
                .FirstOrDefaultAsync(x => x.Name == model.Name);
            if (task != null)
            {
                return new BaseResponse<TaskEntity>()
                {
                    Description = "Task with this name already exist",
                    StatusCode = StatusCode.TaskIsHasAlready
                };
            }

            task = new TaskEntity()
            {
                Name = model.Name,
                Description = model.Description,
                IsDone = false,
                Priority = model.Priority,
                Created = DateTime.Now
            };
            await _taskRepository.Create(task);

            _logger.LogInformation($"Task created: {task.Name} {task.Created}");
            return new BaseResponse<TaskEntity>()
            {
                Description = "Task created",
                StatusCode = StatusCode.OK
            };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<TaskEntity>()
            {
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public  async Task<IBaseResponse<bool>> EndTask(long id)
    {
        try 
        {
            var task =  await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (task == null) 
            {
                return new BaseResponse<bool>()
                {
                    Description = "Task Not Found",
                    StatusCode = StatusCode.TaskNotFound
                };
            }

            task.IsDone = true;

            await _taskRepository.Update(task);
            
            return new BaseResponse<bool>()
            {
                Description = "Task Ended",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.EndTask]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTask()
    {
        try
        {
            var task = await _taskRepository.GetAll()
            .Select(x => new TaskViewModel()
            {
                Id = x.Id,
                Name =x.Name,
                Description = x.Description,
                IsDone = x.IsDone == true? "Ready":"Not Ready",
                Priority = x.Priority.GetDisplayName(),
                Created = x.Created.ToLongDateString()
            }).ToListAsync();
            
            return new BaseResponse<IEnumerable<TaskViewModel>>()
            {
                Data = task,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<IEnumerable<TaskViewModel>> ()
            {
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
}