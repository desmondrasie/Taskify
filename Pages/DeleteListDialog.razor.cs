using MudBlazor;
using Microsoft.AspNetCore.Components;

namespace Taskify.Pages
{
    public partial class DeleteListDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;
        [Parameter] public string listName { get; set; } = null!;

        private void Cancel() => MudDialog.Cancel();
        private void Delete() => MudDialog.Close(DialogResult.Ok("true"));
    }
}
