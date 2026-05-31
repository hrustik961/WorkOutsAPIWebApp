using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkOutsAPIWebApp.Controllers;
using WorkOutsAPIWebApp.Models;
using Xunit;

namespace WorkOutsAPIWebApp.Tests
{
    public static class TestDbContextFactory
    {
        public static WorkOutsAPIContext Create(string dbName) =>
            new WorkOutsAPIContext(new DbContextOptionsBuilder<WorkOutsAPIContext>()
                .UseInMemoryDatabase(databaseName: dbName).Options);
    }

    public class MiniWorkOutsTests
    {
        // 1.успішний пошук
        [Fact]
        public async Task GetWorkOut_ReturnsWorkout_WhenExists()
        {
            using var context = TestDbContextFactory.Create("wo_exist");
            context.WorkOuts.Add(new WorkOuts
            {
                Id = 1,
                Name = "Test Workout",
                Description = "Test Description",
                Equipment = "None", 
                Level = "Beginner"                
            });
            await context.SaveChangesAsync();
            var controller = new WorkOutsController(context);

            var result = await controller.GetWorkOuts(1);

            Assert.Equal("Test Workout", result.Value.Name);
        }

        // 2.відсутність id
        [Fact]
        public async Task GetWorkOut_ReturnsNotFound_WhenMissing()
        {
            using var context = TestDbContextFactory.Create("wo_missing");
            var controller = new WorkOutsController(context);

            var result = await controller.GetWorkOuts(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        // 3.BadRequest
        [Fact]
        public async Task PutWorkOut_ReturnsBadRequest_OnIdMismatch()
        {
            using var context = TestDbContextFactory.Create("wo_bad_req");
            var controller = new WorkOutsController(context);

            var result = await controller.PutWorkOuts(1, new WorkOuts { Id = 5 });

            Assert.IsType<BadRequestResult>(result);
        }
    }

    public class MiniCategoriesTests
    {
        // 4.отримання списку
        [Fact]
        public async Task GetCategories_ReturnsAll()
        {
            using var context = TestDbContextFactory.Create("cat_all");
            context.Categories.Add(new Category { Id = 1, CategoryName = "Cardio" });
            await context.SaveChangesAsync();
            var controller = new CategoriesController(context);

            var result = await controller.GetCategories();

            var list = Assert.IsAssignableFrom<IEnumerable<Category>>(result.Value);
            Assert.Single(list);
        }

        // 5.успішний POST
        [Fact]
        public async Task PostCategory_CreatesSuccessfully()
        {
            using var context = TestDbContextFactory.Create("cat_post");
            var controller = new CategoriesController(context);

            var result = await controller.PostCategory(new Category { CategoryName = "HIIT" });

            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.NotNull(createdAt.Value);
        }

        // 6.видалення
        [Fact]
        public async Task DeleteCategory_RemovesItem()
        {
            using var context = TestDbContextFactory.Create("cat_del");
            context.Categories.Add(new Category { Id = 1, CategoryName = "To Delete" });
            await context.SaveChangesAsync();
            var controller = new CategoriesController(context);

            var result = await controller.DeleteCategory(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}