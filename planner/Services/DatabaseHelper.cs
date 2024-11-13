using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        public async Task<int> SaveTaskAsync(TaskItem task)
        {
            if (task.ID == 0)
            {
                return await _database.InsertAsync(task);
            }
            return await _database.UpdateAsync(task);
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

        public async Task<int> SaveProjectAsync(Project project)
        {
            if (project.ID == 0)
            {
                return await _database.InsertAsync(project);
            }
            return await _database.UpdateAsync(project);
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
                                           .OrderBy(t => t.Priority)
                                           .ToListAsync();
        }

        public Task<List<TaskItem>> GetPendingTasksAsync()
        {
            return _database.Table<TaskItem>().Where(t => !t.IsDone).ToListAsync();
        }

        public async Task<List<TaskItem>> GetTodayPendingTasksAsync()
        {
            var today = DateTime.Today;
            var tasks = await _database.Table<TaskItem>()
                                        .Where(t => !t.IsDone)
                                        .ToListAsync();

            // Filter for tasks due today
            return tasks.Where(t => t.DueDate.Date == today).ToList();
        }

        public Task<int> MarkTaskAsDoneAsync(TaskItem task)
        {
            task.IsDone = true;
            return _database.UpdateAsync(task);
        }

        public Task<List<TaskItem>> GetTasksForProjectAsync(int projectId)
        {
            return _database.Table<TaskItem>()
                            .Where(t => t.ProjectId == projectId)
                            .OrderBy(t => t.Priority)
                            .ToListAsync();
        }

        public async Task<Dictionary<DateTime, int>> GetTasksByDueDateAsync()
        {
            var today = DateTime.Today;
            var nextWeek = today.AddDays(6);

            var tasks = await _database.Table<TaskItem>()
                                        .Where(t => t.DueDate >= today && t.DueDate <= nextWeek)
                                        .ToListAsync();

            var tasksByDate = new Dictionary<DateTime, int>();
            foreach (var task in tasks)
            {
                var date = task.DueDate.Date;
                if (tasksByDate.ContainsKey(date))
                {
                    tasksByDate[date]++;
                }
                else
                {
                    tasksByDate[date] = 1;
                }
            }
            return tasksByDate;
        }

    }

    public class TaskItem : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get => id;
            set { SetProperty(ref id, value); }
        }

        private string name;
        public string Name
        {
            get => name;
            set { SetProperty(ref name, value); }
        }

        private DateTime dueDate;
        public DateTime DueDate
        {
            get => dueDate;
            set { SetProperty(ref dueDate, value); }
        }

        private string description;
        public string Description
        {
            get => description;
            set { SetProperty(ref description, value); }
        }

        private int priority;
        public int Priority
        {
            get => priority;
            set { SetProperty(ref priority, value); }
        }

        private bool isContinuous;
        public bool IsContinuous
        {
            get => isContinuous;
            set { SetProperty(ref isContinuous, value); }
        }

        private bool isDone;
        public bool IsDone
        {
            get => isDone;
            set { SetProperty(ref isDone, value); }
        }

        public int ProjectId { get; set; } // Foreign key to link to a Project

        // PropertyChanged event for notifying UI changes
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class Project : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get => id;
            set { SetProperty(ref id, value); }
        }

        private string name;
        public string Name
        {
            get => name;
            set { SetProperty(ref name, value); }
        }

        private string description;
        public string Description
        {
            get => description;
            set { SetProperty(ref description, value); }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set { SetProperty(ref startDate, value); }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set { SetProperty(ref endDate, value); }
        }

        private string profilePicturePath;
        public string ProfilePicturePath
        {
            get => profilePicturePath;
            set { SetProperty(ref profilePicturePath, value); }
        }

        [Ignore]
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
