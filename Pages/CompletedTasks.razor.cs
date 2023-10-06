using Taskify.Models;
using Taskify.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace Taskify.Pages
{
    public partial class CompletedTasks : ComponentBase
    {

        [Inject]
        public ITaskService TaskService { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        // VARIABLES //
        public TaskItem NewTask { get; set; } = new TaskItem();
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public bool HasTasks => Tasks.Any();

        // METHODS //
        protected override async Task OnInitializedAsync()
        {
            Tasks = (await TaskService.GetCompletedTasks()).ToList();
        }

        protected async Task HandleDeleteTask(TaskItem task)
        {
            if (task != null && task.Id != 0)
            {
                await TaskService.DeleteTask(task.Id);
                Tasks = (await TaskService.GetCompletedTasks()).ToList();  // Refresh the list
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
            Tasks.Remove(task);
            Snackbar.Add($"'{task.Description}' has been added back to list. ", Severity.Info);
        }
        protected async Task HandleDeleteAll()
        {
            foreach (var task in Tasks)
            {
                await TaskService.DeleteTask(task.Id);
            }
            Tasks = (await TaskService.GetCompletedTasks()).ToList();
            Snackbar.Add("All completed tasks have been cleared.", Severity.Error);
        }

    }

}
