using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using BlogPlatform.Clients.AdminPanel.Components.Dialogs;
using BlogPlatform.Clients.AdminPanel.Contracts;
using MudBlazor;

namespace BlogPlatform.Clients.AdminPanel.Pages.Categories;

public partial class Index : ComponentBase
{
    [Inject] private HttpClient HttpClient { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private IDialogService DialogService { get; set; } = null!;

    private List<CategoryDto>? _categories;

    protected override async Task OnInitializedAsync()
    {
        _categories = await HttpClient.GetFromJsonAsync<List<CategoryDto>>("api/v1/Categories");
    }

    private async Task HandleClickAddItemButton()
    {
        var category = new CategoryDto();
        
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            ClassBackground = "my-custom-class",
            MaxWidth = MaxWidth.Large,
            FullWidth = true
        };
        var parameters = new DialogParameters { ["Category"] = category, };
        var dialog = await DialogService.ShowAsync<EditFormModal>(
            title: "Добавить новую категорию",
            options: options,
            parameters: parameters);
        var result = await dialog.Result;


        if (!result.Canceled)
        {
            var response = await HttpClient.PostAsJsonAsync($"api/v1/Categories/", category);
            if (response.IsSuccessStatusCode)
            {
                await OnInitializedAsync();
            }
        }
    }

    private async Task HandleClickEditItemButton(CategoryDto category)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            ClassBackground = "my-custom-class",
            MaxWidth = MaxWidth.Large,
            FullWidth = true
        };
        var parameters = new DialogParameters { ["Category"] = category, };
        var dialog = await DialogService.ShowAsync<EditFormModal>(
            title: "Редактировать категория",
            options: options,
            parameters: parameters);
        var result = await dialog.Result;


        if (!result.Canceled)
        {
            var response = await HttpClient.PutAsJsonAsync($"api/v1/Categories/{category.Id}", category);
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
            var response = await HttpClient.DeleteAsync($"api/v1/Categories/{id}");
            if (response.IsSuccessStatusCode)
            {
                await OnInitializedAsync();
            }
        }
    }
}