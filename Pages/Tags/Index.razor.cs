using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using BlogPlatform.Clients.AdminPanel.Components.Dialogs;
using BlogPlatform.Clients.AdminPanel.Contracts;
using MudBlazor;

namespace BlogPlatform.Clients.AdminPanel.Pages.Tags;

public partial class Index : ComponentBase
{
    [Inject] private HttpClient HttpClient { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;

    private List<TagDto>? _tags;

    protected override async Task OnInitializedAsync()
    {
        _tags = await HttpClient.GetFromJsonAsync<List<TagDto>>("api/v1/Tags");
    }

    private async Task HandleClickAddItemButton()
    {
        var tag = new TagDto();
        
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            ClassBackground = "my-custom-class",
            MaxWidth = MaxWidth.Large,
            FullWidth = true
        };
        var parameters = new DialogParameters
        {
            ["Tag"] = tag,
            ["SubmitBtnText"] = "Добавить",
        };
        var dialog = await DialogService.ShowAsync<TagEditFormModal>(
            title: "Добавить новый тег",
            options: options,
            parameters: parameters);
        var result = await dialog.Result;


        if (!result.Canceled)
        {
            var response = await HttpClient.PostAsJsonAsync($"api/v1/Tags/", tag);
            if (response.IsSuccessStatusCode)
            {
                await OnInitializedAsync();
            }
        }
    }

    private async Task HandleClickEditItemButton(TagDto tag)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            ClassBackground = "my-custom-class",
            MaxWidth = MaxWidth.Large,
            FullWidth = true
        };
        var parameters = new DialogParameters
        {
            ["Tag"] = tag,
            ["SubmitBtnText"] = "Сохранить",
        };
        var dialog = await DialogService.ShowAsync<TagEditFormModal>(
            title: "Редактировать тег",
            options: options,
            parameters: parameters);
        var result = await dialog.Result;


        if (!result.Canceled)
        {
            var response = await HttpClient.PutAsJsonAsync($"api/v1/Tags/{tag.Id}", tag);
            if (response.IsSuccessStatusCode)
            {
                await OnInitializedAsync();
            }
        }
    }


    private async Task HandleClickRemoveItemButton(Guid id)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.TopCenter,
            ClassBackground = "my-custom-class"
        };
        var dialog = await DialogService.ShowAsync<ConfirmationModal>("Подвердите действия", options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var response = await HttpClient.DeleteAsync($"api/v1/Tags/{id}");
            if (response.IsSuccessStatusCode)
            {
                await OnInitializedAsync();
            }
        }
    }
}