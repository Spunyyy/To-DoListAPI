# To-Do List API

A simple REST API for a To-Do List application, created with ASP.NET Core, Entity Framework Core.

## Installation

**This API requires a PostgreSQL database.**

Before running the API, you need to set up the database connection in `appsettings.json`
```json
    "DefaultConnection": "Host=ipAdress;Database=dbName;Username=user;Password=password"
```
After setting up the database, run the ToDoListAPI app. It will automatically set up the database and start listening on port 5000, accepting requests from all sources.
### Windows
- Download zip [here](https://github.com/Spunyyy/To-DoListAPI/releases/download/Publish/To-Do_List_API_Win.zip)
### Linux
- Download zip [here](https://github.com/Spunyyy/To-DoListAPI/releases/download/Publish/To-Do_List_API_Linux.zip)

## Endpoints

### GET /api/TaskAPI
- **Description:** Get list of all tasks
- **Parameters:** None
- **Response codes:** 200 - Success
- **Return:**
```json
[
  {
    "id": 1,
    "name": "Task 1",
    "description": "Description of Task 1",
    "dueDate": "2025-01-30T00:00:00",
    "status": "Completed"
  },
  {
    "id": 3,
    "name": "Task 3",
    "description": "Description of Task 3",
    "dueDate": "2025-01-31T00:00:00",
    "status": null
  }
]
```
- **Usage:**
```csharp
private static async Task<List<ToDoTask>> GetTasks(HttpClient client)
{
    try
    {
        HttpResponseMessage response = await client.GetAsync("api/TaskAPI");

        List<ToDoTask> tasks = await response.Content.ReadFromJsonAsync<List<ToDoTask>>();

        if(tasks != null)
        {
            return tasks;
        }
    }
    catch(HttpRequestException ex)
    {
        Console.WriteLine($"Request error: {ex.Message}");
        return new List<ToDoTask>();
    }
    return new List<ToDoTask>();
}
```

### POST /api/TaskAPI
- **Description:** Add new task to the list
- **Parameters:** None
- **Response codes:** 200 - Success, 400 - Bad Request (if the task is null)
- **Return:**
```json
{
  "id": 6,
  "name": "Task 6",
  "description": "Description of Task 6",
  "dueDate": "2025-02-02T00:00:00",
  "status": null
}
```
- **Usage:**
```csharp
private static async Task AddTask(HttpClient client, ToDoTask task)
{
    try
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("api/TaskAPI", task);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Success");
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode);
        }
    }
    catch(HttpRequestException ex)
    {
        Console.WriteLine($"Request error: {ex.Message}");
    }
}
```

### PUT /api/TaskAPI/Complete/{id}
- **Description:** Change status of the task to completed
- **Parameters:** ID (integer)
- **Response codes:** 200 - Success, 400 - Bad Request (if ID is lower than 0, 404 - Not Found (if the task doesn't exist)
- **Return:**
```json
{
  "id": 1,
  "name": "Task 1",
  "description": "Description of Task 1",
  "dueDate": "2025-01-30T00:00:00",
  "status": "Completed"
}
```
- **Usage:**
```csharp
private static async Task CompleteTask(HttpClient client, int id)
{
    try
    {
        HttpResponseMessage response = await client.PutAsync($"api/TaskAPI/Complete/{id}", null);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Success");
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode);
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Request error: {ex.Message}");
    }
}
```

### DELETE /api/TaskAPI/{id}
- **Description:** Deletes the task from the list if it exists
- **Parameters:** ID (integer)
- **Response codes:** 200 - Success, 400 - Bad Request (if ID is lower than 0, 404 - Not Found (if the task doesn't exist)
- **Usage:**
```csharp
private static async Task DeleteTask(HttpClient client, int id)
{
    try
    {
        HttpResponseMessage response = await client.DeleteAsync($"api/TaskAPI/{id}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Success");
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode);
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Request error: {ex.Message}");
    }
}
```