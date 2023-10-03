using Taskify.Models;
using System.Collections.Generic;
using System.Threading.Tasks; // This is necessary for the Task type.

namespace Taskify.Services;


public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllTasks();
    Task<IEnumerable<TaskItem>> GetPendingTasks();
    Task<IEnumerable<TaskItem>> GetPendingTasks(int TaskListId);
    Task<IEnumerable<TaskItem>> GetCompletedTasks();
    Task AddTask(TaskItem task, int TaskListId);
    Task EditTask(TaskItem task);
    Task DeleteTask(int id);
    Task CheckTask(TaskItem task);
    
}
