using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp;
public class TaskRepository
{
    private readonly SQLiteAsyncConnection _database;

    public TaskRepository()
    {
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TasksList.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<TaskItem>().Wait();
    }

    public Task<List<TaskItem>> LoadTasksAsync()
    {
        return _database.Table<TaskItem>().ToListAsync();
    }

    public Task SaveTaskAsync(TaskItem task)
    {
        
        if (task.Id != 0)
        {
            return _database.UpdateAsync(task);
        }
        return _database.InsertAsync(task);
    }

    public Task DeleteTaskAsync(int taskId)
    {
        return _database.DeleteAsync<TaskItem>(taskId);
    }
}