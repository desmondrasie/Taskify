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
        public TaskList NewList { get; set; } = new TaskList();
        public bool HasLists => MasterList.Any();
        public ICollection<TaskList> MasterList { get; set; } = new List<TaskList>();

        static private string ComputeLinkHref(int id)
        {
            return $"/list/{id}";
        }
 
        protected override async Task OnInitializedAsync()
        {
            MasterList = (await ListService.GetAllLists()).ToList();

        }
        protected async Task HandleCreateList()
        {
            if (!string.IsNullOrWhiteSpace(NewList.Name))
            {
                if(!MasterList.Any(list => list.Name == NewList.Name))
                {
                    await ListService.AddList(NewList);
                    Snackbar.Add($"'{NewList.Name}' has been created.", Severity.Normal);
                    NewList = new TaskList();  // Reset for next entry
                    MasterList = (await ListService.GetAllLists()).ToList();  // Refresh the list
                    StateHasChanged();
                }
                else
                {
                    Snackbar.Add($"'{NewList.Name}' already exists", Severity.Warning);
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
            MasterList = (await ListService.GetAllLists()).ToList();
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
                _ = (string)result.Data;
                await HandleDeleteList(list);
            }
        }

        protected async Task HandleEditListName(TaskList list, string newName)
        {
            if (MasterList.Any(l => l.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)) && !list.Name.Equals(newName, StringComparison.OrdinalIgnoreCase))
            {
                Snackbar.Add($"The list '{newName}' already exists. Please enter a different name.", Severity.Warning);
                return;
            }
            else if (string.IsNullOrWhiteSpace(newName))
            {
                //Snackbar.Add($"List name cannot be blank.", Severity.Warning);
                return;
            }
            list.Name = newName;
            await ListService.EditListName(list);
            MasterList = (await ListService.GetAllLists()).ToList();
        }
    }
}
