using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using SQLite;

namespace ToDoApp
{
    public partial class MainPage : ContentPage
    {
        private readonly ObservableCollection<TaskItem> _tasks;
        private readonly TaskRepository _taskRepository;

        public MainPage()
        {
            InitializeComponent();

            _taskRepository = new TaskRepository();
            _tasks = new ObservableCollection<TaskItem>();

            TaskListView.ItemsSource = _tasks;
            LoadTasksAsync();
        }

        private async void LoadTasksAsync()
        {
            var tasks = await _taskRepository.LoadTasksAsync();

            _tasks.Clear();
            foreach (var task in tasks)
            {
                _tasks.Add(task);
            }
        }

        private async void AddTaskButton_Clicked(object sender, EventArgs e)
        {
            var newTaskPage = new TaskDetailPage();
            newTaskPage.TaskSaved += async (s, task) =>
            {
                await _taskRepository.SaveTaskAsync(task);
                _tasks.Add(task);
            };
            await Navigation.PushAsync(newTaskPage);
        }

        private async void DeleteTaskButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is TaskItem task)
            {
                await _taskRepository.DeleteTaskAsync(task.Id);
                _tasks.Remove(task);
            }
        }
        private async void TaskCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.BindingContext is TaskItem task)
            {
                try
                {
                    task.IsCompleted = e.Value; 
                    await _taskRepository.SaveTaskAsync(task); 
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to update task: {ex.Message}", "OK");
                }
            }
        }

        private async void TaskView_Tapped(object sender, EventArgs e)
        {
            if (sender is ViewCell viewCell && viewCell.BindingContext is TaskItem task)
            {
                var editPage = new TaskEditPage(task);
                editPage.TaskUpdated += async (s, updatedTask) =>
                {
                    var index = _tasks.IndexOf(task);
                    if (index >= 0)
                    {
                        _tasks[index].Title = updatedTask.Title;
                        _tasks[index].Description = updatedTask.Description;
                        _tasks[index].IsCompleted = updatedTask.IsCompleted;
                        await _taskRepository.SaveTaskAsync(updatedTask);
                    }
                  
                };


                await Navigation.PushAsync(editPage);
                
            }
        }

    }
}
