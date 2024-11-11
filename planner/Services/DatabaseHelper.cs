using SQLite; // For SQLite functionality
using System.Collections.Generic; // For lists
using System.Threading.Tasks; // For asynchronous tasks

namespace planner
{
    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TaskItem>().Wait();
        }

        public Task<List<TaskItem>> GetTasksAsync()
        {
            return _database.Table<TaskItem>().ToListAsync();
        }

        public Task<int> SaveTaskAsync(TaskItem task)
        {
            return _database.InsertAsync(task);
        }

        public Task<int> UpdateTaskAsync(TaskItem task)
        {
            return _database.UpdateAsync(task);
        }

        public Task<int> DeleteTaskAsync(TaskItem task)
        {
            return _database.DeleteAsync(task);
        }
    }

    public class TaskItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string DueDate { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsContinuous { get; set; }
    }
}
