using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp;

public partial class TaskEditPage : ContentPage
{
    private TaskItem _task;
    //private readonly TaskRepository _taskRepository;

    public event EventHandler<TaskItem> TaskUpdated;

    public TaskEditPage(TaskItem task)
    {
        InitializeComponent();
        _task = task;
        //_taskRepository = new TaskRepository();

        TitleEntry.Text = _task.Title;
        DescriptionEditor.Text = _task.Description;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert("Validation Error", "Task title cannot be empty.", "OK");
            return;
        }

        _task.Title = TitleEntry.Text;
        _task.Description = DescriptionEditor.Text;
   
        //await _taskRepository.SaveTaskAsync(_task);
        TaskUpdated.Invoke(this, _task);
        await Navigation.PopAsync();
    }
}
