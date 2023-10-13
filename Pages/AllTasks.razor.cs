using ThoughtHarbour.Models;
using ThoughtHarbour.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ThoughtHarbour.Pages
{
    public partial class AllTasks : ComponentBase
    {
        [Inject]
        public ITaskService TaskService { get; set; } = null!;
        [Inject]
        public IListService ListService { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        private Dictionary<string, int> listNameToId = new();
        private string selectedListName = "Default"; 
        public TaskItem NewTask { get; set; } = new TaskItem();
        public TaskList CurrentList { get; set; } = null!;
        public List<TaskItem> AllPendingTasks { get; set; } = new List<TaskItem>();
        public bool HasTasks => AllPendingTasks.Any();
        public bool HasLists => MasterList.Any();

        private string originalDescription = null!;
        
        public ICollection<TaskList> MasterList { get; set; } = new List<TaskList>();
        [Parameter]
        public int ListId { get; set; }
        protected override async Task OnInitializedAsync()
        {            
            MasterList = (await ListService.GetAllLists()).ToList();
            AllPendingTasks = (await TaskService.GetPendingTasks()).ToList();
            listNameToId = MasterList.ToDictionary(list => list.Name, list => list.Id);
            //CurrentList = await ListService.GetListById(ListId);
        }

        private void StartEdit(TaskItem task)
        {
            originalDescription = task.Description;
        }
        protected async Task HandleCreateTask()
        {
            if (!string.IsNullOrWhiteSpace(NewTask.Description) && listNameToId.TryGetValue(selectedListName, out var selectedListId))
            {
                
                await TaskService.AddTask(NewTask,selectedListId);
                //Snackbar.Add($"'{newTask.Description}' has been added to '{selectedListName}'.", Severity.Normal);
                NewTask = new TaskItem();  // Reset for next entry
                AllPendingTasks = (await TaskService.GetPendingTasks()).ToList();  // Refresh the list
            }
            else if (string.IsNullOrWhiteSpace(NewTask.Description))
            {
                Snackbar.Add($"Task description cannot be empty.", Severity.Warning);
            }
            else
            {
                Snackbar.Add($"Please select a valid list.", Severity.Warning);
            }
            StateHasChanged();
        }
        protected async Task HandleDeleteTask(TaskItem task)
        {
            if (task != null && task.Id != 0)
            {
                await TaskService.DeleteTask(task.Id);
                AllPendingTasks = (await TaskService.GetPendingTasks()).ToList();  // Refresh the list
                
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

        // The rest of your code remains the same

        protected async Task HandleCheckTask(TaskItem task)
        {
            if (task == null || task.Id == 0) return;

            await TaskService.CheckTask(task); // Assume this updates the task in the backend

            if (task.IsChecked)
            {
                AllPendingTasks.Remove(task);
                Snackbar.Add($"'{task.Description}' has been completed! ", Severity.Success);
            }
            else
            {
                // Handle uncheck behavior, possibly adding it back to the list, etc.
                Snackbar.Add($"'{task.Description}' has been uncompleted! ", Severity.Info);
            }
            StateHasChanged();
        }

    }
}
