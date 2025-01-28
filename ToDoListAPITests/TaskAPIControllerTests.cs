using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Controllers;
using ToDoListAPI.Data;
using ToDoListAPI.Model;
using ToDoListAPI.Model.Dto;

namespace ToDoListAPITests
{
    public class TaskAPIControllerTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();

            for(int i = 1; i <= 10; i++)
            {
                databaseContext.Add(new ToDoTask
                {
                    Name = "Task " + i,
                    Description = "Description of Task " + i,
                    DueDate = DateTime.Today.AddDays(i)
                });
            }
            await databaseContext.SaveChangesAsync();
            return databaseContext;
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnStatus200()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var result = controller.GetTasks();
            var statusResult = result.Result as ObjectResult;

            statusResult.Should().NotBeNull();
            statusResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnStatus200_WhenNewTaskIsValid()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var taskDto = new ToDoTaskDTO()
            {
                Name = "New Task",
                Description = "Description of new task",
                DueDate = DateTime.Today.AddDays(21)
            };

            var result = controller.CreateTask(taskDto).Result as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnStatus400_WhenNewTaskIsInvalid()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var taskDto = new ToDoTaskDTO();

            var result = controller.CreateTask(taskDto).Result as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnStatus200_WhenTaskIsDeleted()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var result = controller.DeleteTask(6) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnStatus400_WhenTaskIdIsLowerThanZero()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var result = controller.DeleteTask(-5) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnStatus400_WhenTaskIdIsZero()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var result = controller.DeleteTask(0) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnStatus404_WhenTaskNotFound()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var result = controller.DeleteTask(11) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task CompleteTask_ShouldReturnStatus200_WhenTaskExists()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var result = controller.CompleteTask(4).Result as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var task = result.Value as ToDoTask;

            task.Should().NotBeNull();
            task.Status.Should().Be("Completed");
        }

        [Fact]
        public async Task CompleteTask_ShouldReturnStatus400_WhenTaskIdIsLowerThanZero()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var result = controller.CompleteTask(-3).Result as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var task = result.Value as ToDoTask;

            task.Should().BeNull();
        }

        [Fact]
        public async Task CompleteTask_ShouldReturnStatus400_WhenTaskIdIsZero()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var result = controller.CompleteTask(0).Result as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var task = result.Value as ToDoTask;

            task.Should().BeNull();
        }

        [Fact]
        public async Task CompleteTask_ShouldReturnStatus404_WhenTaskNotFound()
        {
            var dbContext = await GetDbContext();
            var controller = new TaskAPIController(dbContext);

            var result = controller.CompleteTask(13).Result as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var task = result.Value as ToDoTask;

            task.Should().BeNull();
        }
    }
}