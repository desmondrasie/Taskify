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
        public IListService ListService { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        private Dictionary<string, int> listNameToId = new Dictionary<string, int>();
        private string selectedListName = null!;
        public TaskItem newTask { get; set; } = new TaskItem();
        public TaskList CurrentList { get; set; } = null!;
        public List<TaskItem> tasks { get; set; } = new List<TaskItem>();
        public bool HasTasks => tasks.Any();
        public bool HasLists => masterList.Any();

        private string originalDescription = null!;
        
        public ICollection<TaskList> masterList { get; set; } = new List<TaskList>();
        [Parameter]
        public int ListId { get; set; }
        protected override async Task OnInitializedAsync()
        {
            
            masterList = (await ListService.GetAllLists()).ToList();
            tasks = (await TaskService.GetPendingTasks()).ToList();
            listNameToId = masterList.ToDictionary(list => list.Name, list => list.Id);
            //CurrentList = await ListService.GetListById(ListId);
        }

        private void StartEdit(TaskItem task)
        {
            originalDescription = task.Description;
        }
        protected async Task HandleCreateTask()
        {
            if (!string.IsNullOrWhiteSpace(newTask.Description) && listNameToId.TryGetValue(selectedListName, out var selectedListId))
            {
                
                await TaskService.AddTask(newTask,selectedListId);
                //Snackbar.Add($"'{newTask.Description}' has been added to '{selectedListName}'.", Severity.Normal);
                newTask = new TaskItem();  // Reset for next entry
                tasks = (await TaskService.GetPendingTasks()).ToList();  // Refresh the list
            }
            else if (string.IsNullOrWhiteSpace(newTask.Description))
            {
                Snackbar.Add($"Task description cannot be empty.", Severity.Error);
            }
            else
            {
                Snackbar.Add($"Please select a valid list.", Severity.Error);
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
            StateHasChanged();
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
