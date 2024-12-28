using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp;
public partial class TaskDetailPage : ContentPage
{
    public event EventHandler<TaskItem> TaskSaved;

    public TaskDetailPage()
    {
        InitializeComponent();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert("Validation Error", "Task title cannot be empty.", "OK");
            return;
        }
        var task = new TaskItem
        {
            Title = TitleEntry.Text,
            Description = DescriptionEditor.Text,
            IsCompleted = false
        };

        TaskSaved.Invoke(this, task);
        Navigation.PopAsync();
    }
}