using Taskify.Models;
using Taskify.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;


namespace Taskify.Pages
{
    public partial class MyLists : ComponentBase
    {
        [Inject]
        public IListService ListService { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        public TaskList newList { get; set; } = new TaskList();
        public List<TaskList> masterList { get; set; } = new List<TaskList>();

        private string ComputeLinkHref(int id)
        {
            return $"/list/{id}";
        }

        protected override async Task OnInitializedAsync()
        {
            masterList = (await ListService.GetAllLists()).ToList();

        }
        protected async Task HandleCreateList()
        {
            if (!string.IsNullOrWhiteSpace(newList.Name))
            {
                await ListService.AddList(newList);
                Snackbar.Add($"'{newList.Name}' has been created.", Severity.Normal);
                newList = new TaskList();  // Reset for next entry
                masterList = (await ListService.GetAllLists()).ToList();  // Refresh the list
                StateHasChanged();
            }
        }
        protected async Task HandleDeleteList(TaskList list)
        {
            await ListService.DeleteList(list);
            Snackbar.Add($"'{list.Name}' has been deleted.", Severity.Error);
            masterList = (await ListService.GetAllLists()).ToList();
        }
    }
}
