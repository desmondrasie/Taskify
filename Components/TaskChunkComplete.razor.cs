using Microsoft.AspNetCore.Components;
using ThoughtHarbour.Models;
using ThoughtHarbour.Services;

namespace ThoughtHarbour.Components
{
    public partial class TaskChunkComplete : ComponentBase
    {
        [Inject]
        public IListService ListService { get; set; } = null!;
        [Parameter] public TaskItem Task { get; set; } = null!;
        //[Parameter] public string ListName { get; set; } = string.Empty;
        [Parameter] public EventCallback<TaskItem> OnDelete { get; set; }
        [Parameter] public EventCallback<TaskItem> OnEdit { get; set; }
        [Parameter] public EventCallback<TaskItem> OnStartEdit { get; set; }
        [Parameter] public EventCallback<bool> OnCheck { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadListName();
        }

        private string listName = string.Empty;

        private async Task LoadListName()
        {
            var list = await ListService.GetListById(Task.TaskListId);
            listName = list.Name;
        }
    }
}
