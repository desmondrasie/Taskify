using ThoughtHarbour.Models;
using ThoughtHarbour.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ThoughtHarbour.Pages
{
    public partial class TaskDisplay : ComponentBase
    {
        [Inject]
        public ITaskService TaskService { get; set; } = null!;
        [Inject]
        public IListService ListService { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        public TaskItem NewTask { get; set; } = new();
          
        public List<TaskItem> PendingTasks { get; set; } = new List<TaskItem>();
        public bool HasTasks => PendingTasks.Any();
        [Parameter]
        public int ListId { get; set; }
        public TaskList CurrentList { get; set; } = null!;

        private string originalDescription = null!;

        private string selectedSort = "Creation Date"; // Default sort option.
        private List<TaskItem> SortedTasks { get; set; } = new List<TaskItem>();



        protected override void OnParametersSet()
        {
            // Re-sort tasks whenever parameters are updated.
            SortTasks(selectedSort);
        }

        // <--- Start of Methods --->
        protected override async Task OnInitializedAsync()
        {
            CurrentList = await ListService.GetListById(ListId);
            PendingTasks = (await TaskService.GetPendingTasks(CurrentList.Id)).ToList();
            SortTasks(selectedSort);
        }
        public void SortTasks(string sortBy)
        {
            selectedSort = sortBy;

            switch (sortBy)
            {
                case "Due Date":
                    SortedTasks = PendingTasks
                        .OrderBy(task => task.DueDetails?.DueDate ?? DateTime.MaxValue)
                        .ThenBy(task => task.DueDetails?.DueTime ?? DateTime.MaxValue.TimeOfDay)
                        .ToList();
                    break;
                case "Creation Date":
                    SortedTasks = PendingTasks.OrderBy(task => task.Id).ToList();
                    break;
                // Add more cases here as per new sort options.
                
            }
            Console.WriteLine($"Sorting by: {sortBy}");
            StateHasChanged();
        }

        private void SortTasksChanged(string newSortValue)
        {
            selectedSort = newSortValue;
            SortTasks(selectedSort);
        }


        private void StartEdit(TaskItem task)
        {
            originalDescription = task.Description;
        }

        protected async Task HandleCreateTask()
        {
            if (!string.IsNullOrWhiteSpace(NewTask.Description))
            {
                await TaskService.AddTask(NewTask,CurrentList.Id);
                //Snackbar.Add($"'{newTask.Description}' has been added.", Severity.Normal);
                NewTask = new TaskItem();  // Reset for next entry
                PendingTasks = (await TaskService.GetPendingTasks(CurrentList.Id)).ToList();  // Refresh the list
                SortTasks(selectedSort);
            }
        }
        protected async Task HandleDeleteTask(TaskItem task)
        {
            if (task != null && task.Id != 0)
            {
                await TaskService.DeleteTask(task.Id);
                PendingTasks = (await TaskService.GetPendingTasks(CurrentList.Id)).ToList();  // Refresh the list
                SortTasks(selectedSort);
                //Snackbar.Add($"'{task.Description}' has been deleted.", Severity.Error);
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
            PendingTasks.Remove(task);
            SortTasks(selectedSort);
            Snackbar.Add($"'{task.Description}' has been completed! ", Severity.Success);
        }
        protected async Task HandleEditDueDate(TaskItem task)
        {
            await TaskService.EditDueDate(task);
        }
    }
}
