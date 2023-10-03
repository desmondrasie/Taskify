using Taskify.Models;
using Taskify.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Taskify.Pages
{
    public partial class AllTasks : ComponentBase
    {
        [Inject]
        public ITaskService TaskService { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        public TaskItem newTask { get; set; } = new TaskItem();
        public List<TaskItem> tasks { get; set; } = new List<TaskItem>();
        public bool HasTasks => tasks.Any();
        protected override async Task OnInitializedAsync()
        {
            tasks = (await TaskService.GetPendingTasks()).ToList();
        }

        protected async Task HandleCreateTask()
        {
            if (!string.IsNullOrWhiteSpace(newTask.Description))
            {
                //await TaskService.AddTask(newTask,id);
                Snackbar.Add($"'{newTask.Description}' has been added.", Severity.Normal);
                newTask = new TaskItem();  // Reset for next entry
                tasks = (await TaskService.GetPendingTasks()).ToList();  // Refresh the list
            }
        }
        protected async Task HandleDeleteTask(TaskItem task)
        {
            if (task != null && task.Id != 0)
            {
                await TaskService.DeleteTask(task.Id);
                tasks = (await TaskService.GetPendingTasks()).ToList();  // Refresh the list
                
                Snackbar.Add($"'{task.Description}' has been deleted.", Severity.Error);
            }
        }
        protected async Task HandleEditTask(TaskItem task)
        {           
            await TaskService.EditTask(task);
        }
        protected async Task HandleCheckTask(TaskItem task)
        {
            await TaskService.CheckTask(task);
            tasks.Remove(task);                       
            Snackbar.Add($"'{task.Description}' has been completed! ", Severity.Success);
        }
        private EventCallback<bool> CreateCheckCallback(TaskItem task)
        {
            return EventCallback.Factory.Create<bool>(this,async isChecked =>
            {
                await HandleCheckTask(task);
            });
        }

    }
}
