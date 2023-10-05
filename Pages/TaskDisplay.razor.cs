using Taskify.Models;
using Taskify.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Taskify.Pages
{
    public partial class TaskDisplay : ComponentBase
    {
        [Inject]
        public ITaskService TaskService { get; set; } = null!;
        [Inject]
        public IListService ListService { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        public TaskItem newTask { get; set; } = new TaskItem();
        public List<TaskItem> tasks { get; set; } = new List<TaskItem>();
        public bool HasTasks => tasks.Any();
        [Parameter]
        public int ListId { get; set; }
        public TaskList CurrentList { get; set; } = null!;

        private string originalDescription = null!;
        protected override async Task OnInitializedAsync()
        {
            CurrentList = await ListService.GetListById(ListId);
            tasks = (await TaskService.GetPendingTasks(CurrentList.Id)).ToList();
            
        }
        private void StartEdit(TaskItem task)
        {
            originalDescription = task.Description;
        }

        protected async Task HandleCreateTask()
        {
            if (!string.IsNullOrWhiteSpace(newTask.Description))
            {
                await TaskService.AddTask(newTask,CurrentList.Id);
                //Snackbar.Add($"'{newTask.Description}' has been added.", Severity.Normal);
                newTask = new TaskItem();  // Reset for next entry
                tasks = (await TaskService.GetPendingTasks(CurrentList.Id)).ToList();  // Refresh the list
            }
        }
        protected async Task HandleDeleteTask(TaskItem task)
        {
            if (task != null && task.Id != 0)
            {
                await TaskService.DeleteTask(task.Id);
                tasks = (await TaskService.GetPendingTasks(CurrentList.Id)).ToList();  // Refresh the list

                Snackbar.Add($"'{task.Description}' has been deleted.", Severity.Error);
            }
        }
        protected async Task HandleEditTask(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Description))
            {
                //Snackbar.Add($"Task name cannot be blank.", Severity.Warning);
                task.Description = originalDescription;  // Revert to original description
                StateHasChanged();  // Request UI update
                return;
            }

            // Continue with saving the task if the edited description is valid
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
            return EventCallback.Factory.Create<bool>(this, async isChecked =>
            {
                await HandleCheckTask(task);
            });
        }
    }
}
