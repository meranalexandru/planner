Multiplatform Planner application in .NET MAUI
 
Introduction

The Planner Application is a feature-rich, cross-platform productivity tool designed using .NET MAUI to cater to users' project and task management needs. By combining local task management capabilities with external integrations like Todoist, the app offers a unified platform for planning, organizing, and tracking tasks. Whether for personal organization or professional project management, the Planner Application delivers a seamless experience to help users stay on top of their responsibilities.
Key Features
•	Project Management
Users can create, edit, and manage projects, allowing them to group related tasks for better organization. Each project serves as a container for tasks, helping to structure workflows effectively.
•	Task Management
Tasks can be assigned to specific projects, ensuring a clear association between objectives and deliverables. Users can update task statuses by marking them as complete and track their overall progress.
•	Task Import from Todoist
The app integrates with the Todoist API, enabling users to import tasks from their Todoist accounts. This feature allows seamless synchronization between Todoist and the Planner Application, ensuring no task is overlooked.
•	Statistics and Insights
A built-in analytics feature provides insights into task completion rates and productivity levels. This empowers users to monitor their progress and make informed decisions to improve their planning.
•	Upcoming Tasks View
The app includes a dedicated section for viewing tasks scheduled in the upcoming week. This feature helps users prioritize and prepare for immediate tasks, ensuring timely completion.
•	Data Persistence
Using a local SQLite database, the app ensures all data is stored locally on the user's device. This guarantees offline access to projects and tasks, ensuring reliability even without an internet connection.

Purpose and Goals
The Planner Application is designed with the following objectives:
•	Streamline Task and Project Management
By providing a central hub for tasks and projects, the app simplifies the management process and ensures all information is easily accessible.
•	Facilitate Task Synchronization
With Todoist API integration, the app allows users to bridge their existing Todoist workflows with the Planner Application, offering flexibility in task tracking.
•	Enhance Productivity and Focus
With features like task statistics, upcoming task views, and intuitive navigation, the app helps users focus on their priorities and achieve their goals effectively.
•	Ensure Accessibility and Reliability
The SQLite-based local database ensures that users can access their data anytime, making the app a reliable solution for task management, even offline.

Target Audience
The Planner Application is ideal for:
Individuals: People looking for a personal task manager to organize daily tasks, set priorities, and stay productive.
Professionals: Project managers, team leads, and employees who need to manage multiple tasks across projects and monitor their progress.
Todoist Users: Individuals who already use Todoist and want to integrate it into a broader planning ecosystem.
By combining powerful features with ease of use, the Planner Application serves as a versatile tool for a wide range of users seeking to improve their task and project management capabilities.
 
Getting started
This chapter provides a step-by-step guide to setting up and running the Planner Application. Whether you're a developer exploring the app's codebase or an end-user looking to install and use the application, this section covers all the essentials to get started.
Minimum requirements to get started
•	Hardware Requirements
Windows or macOS: The application is cross-platform and runs on both operating systems.
Mobile Devices: Android 8.0+ or iOS 12.0+ for deployment on smartphones.
RAM: Minimum 4 GB (8 GB recommended).
Storage: At least 100 MB of free disk space.
•	Software Requirements
.NET SDK: Version 7.0 or higher for building the project.
Visual Studio: 2022 or newer with .NET MAUI workload installed.
SQLite: Pre-installed on mobile devices; the app manages SQLite integration automatically.
Postman (optional): For testing Todoist API integration.
Dependencies
Todoist API Key: Required for importing tasks from a Todoist account.
NuGet Packages:
SQLite-net
Newtonsoft.Json for JSON parsing
Microsoft.Maui.Controls

ToDoist Tasks integration
In order to properly work the integration with ToDoist Tasks API, please create an account and obain an API key. To do that, please log in into your Todoist account and navigating to the API & Integration section.
Add your API key to TodoistIntegration.xaml.cs file as presented below:
 
Download and Install
For desktop platforms: Download the application installer (e.g., .exe for Windows or .dmg for macOS).
For mobile devices: Install the app via the respective app store or sideload it.
First-Time Setup
Launch the application and create your first project to get started.
If using Todoist integration, log in with your Todoist credentials to enable task import.
Troubleshooting
SQLite Errors: Ensure SQLite is installed correctly on the system and is compatible with the app’s version.
API Issues: Verify your Todoist API key and check your internet connection.
Build Failures: Confirm all NuGet packages are installed and up to date.
By following the steps outlined in this chapter, users and developers can successfully set up, build, and run the Planner Application, ensuring a smooth experience.
 
Application Features

This chapter provides an overview of the core features of the Planner Application. Each feature is designed to enhance user productivity by offering tools for efficient task and project management. 
List of application features
1. Managing Projects
Projects provide a way to organize related tasks under a common category, making it easier to manage workflows.
Features:
•	Create New Projects: Users can add projects with custom names to group tasks logically.
•	Edit Projects: Modify existing project details as requirements change.
•	Delete Projects: Remove projects that are no longer relevant, including their associated tasks.
•	Project List View: Displays all projects in a scrollable list for quick access.
2. Managing Tasks
Tasks are the primary units of work in the application. Users can create, manage, and track tasks within their projects.
Features:
•	Add Tasks to Projects: Assign tasks to specific projects, ensuring proper categorization.
•	Task Status Management: Mark tasks as Complete or Incomplete based on progress.
•	Edit Tasks: Update task details, such as due dates, descriptions, or priorities.
•	Delete Tasks: Remove tasks that are no longer needed.
•	Search and Filter: Quickly find tasks based on project, due date, or status.
3. Importing Tasks from Todoist
The app integrates with the Todoist API to enable task import, ensuring synchronization between Todoist and the Planner Application.
Features:
•	Connect Todoist Account: Use an API key to link your Todoist account to the app.
•	Import Tasks: Pull tasks from Todoist projects into the Planner Application.
•	Sync with Local Data: Imported tasks can be linked to local projects for unified task management.
 
4. Task Completion Statistics
The app provides insights into task completion and overall productivity, helping users evaluate their performance.
Features:
•	Completion Rate: View the percentage of tasks marked as complete across all projects.
•	Task Breakdown: Analyze task completion by project or priority.
•	Visual Reports: Use charts and graphs to track progress over time.
5. Viewing Upcoming Tasks
Stay organized by viewing a list of tasks scheduled for the next seven days.
Features:
•	Weekly Task View: Displays tasks due in the upcoming week.
•	Task Prioritization: Highlights high-priority tasks to help users focus on what’s most important.
•	Notifications: Optional reminders for tasks due soon (if supported by the device).
6. Data Persistence
All user data is stored locally on the device using an SQLite database, ensuring reliability and offline access.
Features:
•	Local Storage: Tasks and projects are stored securely on the device.
•	Offline Access: Access and manage tasks even without an internet connection.
•	Data Security: Local storage minimizes risks associated with online storage and ensures data privacy.
These features collectively provide a comprehensive toolset for users to organize their tasks, track progress, and maintain productivity. Each feature is designed to work seamlessly with the others, creating a cohesive and intuitive experience.
 
Technical Overview
This chapter outlines the technical architecture and key components of the Planner Application, providing insights into how the system works behind the scenes. It covers database integration, API usage, and JSON parsing, which are the building blocks of the app's functionality.
1. SQLite Database Integration
The database implementation in your Planner Application uses SQLite for efficient local data storage and management. The DatabaseHelper class serves as the primary interface for interacting with the database, handling CRUD (Create, Read, Update, Delete) operations for tasks and projects. Below is an analysis of the database structure and functionality.
Database Schema
1.	TaskItem Table
Stores information about individual tasks.
Fields:
o	ID: Primary key, auto-incremented.
o	Name: Name of the task.
o	Description: Detailed description of the task.
o	DueDate: Deadline for the task.
o	Priority: Priority level of the task (e.g., 1 = Low, 5 = High).
o	IsContinuous: Indicates if the task is recurring.
o	IsDone: Tracks whether the task is completed.
o	ProjectId: Foreign key linking the task to a project.
2.	Project Table
Stores details about projects to which tasks belong.
Fields:
o	ID: Primary key, auto-incremented.
o	Name: Name of the project.
o	Description: Brief description of the project.
o	StartDate: Start date of the project.
o	EndDate: End date of the project.
o	ProfilePicturePath: Optional path for a project-related image.
 
3.	Relationships
o	A one-to-many relationship exists between projects and tasks. Each project can have multiple tasks, but each task belongs to a single project.
Database Helper Functions
The DatabaseHelper class encapsulates database operations and ensures consistency and modularity.
1.	General Database Setup
Tables are created during initialization with the CreateTableAsync method.
 
Task Management
Retrieve Tasks: Fetches all tasks from the TaskItem table.
 
Save or Update Tasks: Adds new tasks or updates existing ones based on the ID.
Delete Tasks: Removes a task from the database.
 
Pending Tasks: Retrieves tasks marked as incomplete.
2.	Project Management
o	Similar methods for GetProjectsAsync, SaveProjectAsync, UpdateProjectAsync, and DeleteProjectAsync.
o	Tasks by Project:
Fetches all tasks linked to a specific project.
 
3.	Upcoming Tasks
o	Retrieves tasks due within the next seven days and groups them by date.

1.Models
1.	TaskItem
Implements INotifyPropertyChanged to allow real-time updates in the UI when properties change.
Fields:
o	Encapsulated fields like Name, DueDate, and IsDone are tracked.
o	Helper methods like SetProperty ensure changes are efficiently managed.
2.	Project
Similar to TaskItem, with additional fields for project-specific attributes like StartDate and EndDate.
 

Key Features of the Database
1.	Asynchronous Operations
o	The use of async ensures non-blocking calls, critical for maintaining a smooth user experience.
var tasks = await _database.Table<TaskItem>().ToListAsync();
2.	Property Change Notification
o	Changes to task or project properties are automatically reflected in the UI, leveraging INotifyPropertyChanged.
3.	Data Filtering and Aggregation
o	Methods like GetTodayPendingTasksAsync and GetTasksByDueDateAsync provide powerful tools to retrieve and analyze data.
2.Todoist API Integration
The Planner Application integrates with the Todoist API to allow users to import tasks seamlessly.
API Workflow:
Authentication:
The app requires a Todoist API key to authenticate requests.
This key is stored securely and used to access the user's Todoist data.
Fetching Tasks:
The app sends a GET request to the Todoist API to retrieve tasks.
Example endpoint:
GET https://api.todoist.com/rest/v1/tasks
Authorization: Bearer <API_KEY>
Mapping Data:
Tasks fetched from Todoist are parsed and mapped to the app's local database schema.
Relevant details such as task description, due date, and priority are stored locally.
Error Handling:
API rate limits are managed by implementing retries with exponential backoff.


3.JSON Parsing

JSON is the primary format used to exchange data with the Todoist API. The app employs the Newtonsoft.Json library for parsing and handling JSON data.
Process:
•	Deserialize JSON: Converts JSON responses from the API into C# objects for easy manipulation.
Example:
var tasks = JsonConvert.DeserializeObject<List<Task>>(jsonResponse);
•	Serialize JSON: Converts C# objects into JSON format when preparing API requests.
Example:
var json = JsonConvert.SerializeObject(newTask);
Common Data Structure for Tasks:
[
    {
        "id": "12345",
        "content": "Finish documentation",
        "due": {
            "date": "2024-11-18"
        },
        "priority": 4,
        "completed": false
    }
]
 
4. Application Architecture

The Planner Application follows a modular architecture to ensure scalability and maintainability.
•	Presentation Layer: Implements the user interface using .NET MAUI XAML pages, including views for projects, tasks, statistics, and upcoming tasks.
•	Business Logic Layer: Handles core functionalities like task management, API interaction, and data processing.
•	Data Access Layer: Manages SQLite operations such as CRUD (Create, Read, Update, Delete) and synchronization with Todoist data.
This technical overview highlights the robust and flexible foundation of the Planner Application, making it an effective tool for managing projects and tasks while ensuring data integrity and seamless integration.
 
User Experience
![image](https://github.com/user-attachments/assets/2c336426-5113-43b4-b7a8-03a11587c71c)

1.Adding, editing and deleting a task

![image](https://github.com/user-attachments/assets/abcf2cbe-2515-4445-a95b-6b937f26d57e)
   
2. Adding, editing, deleting and viewing projects
![image](https://github.com/user-attachments/assets/3eeebd97-659d-4efc-86e4-74f120e62c2f)
![image](https://github.com/user-attachments/assets/083155f5-ac4c-4eed-97c7-fb0cd89049c9)
![image](https://github.com/user-attachments/assets/2d5d4616-268b-445e-a59b-300cad191818)
3. ToDoist Tasks integration
  ![image](https://github.com/user-attachments/assets/6f086ba8-78c8-4377-b68f-ba3c1e9a8341)
The changes in this screen impact the todoist account directly.

4.Task done till Due Date statistics
![image](https://github.com/user-attachments/assets/4859af83-21b7-48f4-9d42-35ed3d632604)


