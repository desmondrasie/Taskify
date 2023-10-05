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
        [Inject]
        public IDialogService DialogService { get; set; } = null!;
        public TaskList newList { get; set; } = new TaskList();
        public bool HasLists => masterList.Any();
        public ICollection<TaskList> masterList { get; set; } = new List<TaskList>();

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
                if(!masterList.Any(list => list.Name == newList.Name))
                {
                    await ListService.AddList(newList);
                    Snackbar.Add($"'{newList.Name}' has been created.", Severity.Normal);
                    newList = new TaskList();  // Reset for next entry
                    masterList = (await ListService.GetAllLists()).ToList();  // Refresh the list
                    StateHasChanged();
                }
                else
                {
                    Snackbar.Add($"'{newList.Name}' already exists", Severity.Warning);
                }
            }
            else
            {
                Snackbar.Add($"List Name cannot be blank", Severity.Warning);
            }

        }
        protected async Task HandleDeleteList(TaskList list)
        {
            await ListService.DeleteList(list);
            Snackbar.Add($"'{list.Name}' has been deleted.", Severity.Error);
            masterList = (await ListService.GetAllLists()).ToList();
        }
         private async Task OpenDialogEdit(TaskList list)
        {
            var parameters = new DialogParameters { ["CurrentName"] = list.Name };
            var dialog = DialogService.Show<EditListDialog>("Edit List Name", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var newName = (string)result.Data;
                await HandleEditListName(list, newName);
            }
        }
        private async Task OpenDialogDelete(TaskList list)
        {
            var parameters = new DialogParameters { ["listName"] = list.Name };
            var dialog = DialogService.Show<DeleteListDialog>("Delete List", parameters);
            var result = await dialog.Result;

            if (!result.Canceled && (string)result.Data == "true")
            {
                var newName = (string)result.Data;
                await HandleDeleteList(list);
            }
        }

        protected async Task HandleEditListName(TaskList list, string newName)
        {
            list.Name = newName;
            await ListService.EditListName(list);
            masterList = (await ListService.GetAllLists()).ToList();
        }
    }
}
