using MudBlazor;
using Microsoft.AspNetCore.Components;

namespace ThoughtHarbour.Pages
{
    public partial class DeleteListDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;
        [Parameter] public string ListName { get; set; } = null!;

        private void Cancel() => MudDialog.Cancel();
        private void Delete() => MudDialog.Close(DialogResult.Ok("true"));
    }
}
