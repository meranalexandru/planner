using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace planner
{
    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TaskItem>().Wait();
            _database.CreateTableAsync<Project>().Wait();
        }

        // TaskItem-related methods
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

        // Project-related methods
        public Task<List<Project>> GetProjectsAsync()
        {
            return _database.Table<Project>().ToListAsync();
        }

        public Task<int> SaveProjectAsync(Project project)
        {
            return _database.InsertAsync(project);
        }

        public Task<int> UpdateProjectAsync(Project project)
        {
            return _database.UpdateAsync(project);
        }

        public Task<int> DeleteProjectAsync(Project project)
        {
            return _database.DeleteAsync(project);
        }

        // Method to load tasks for a specific project
        public async Task LoadTasksForProjectAsync(Project project)
        {
            project.Tasks = await _database.Table<TaskItem>()
                                           .Where(t => t.ProjectId == project.ID)
                                           .ToListAsync();
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

        // Foreign key to link to a Project
        public int ProjectId { get; set; }
    }

    public class Project
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // List of tasks belonging to this project
        [Ignore]
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
