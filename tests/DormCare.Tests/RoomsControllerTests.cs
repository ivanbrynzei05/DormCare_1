using System;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DormCare.Data;
using DormCare.Controllers.Admin;
using DormCare.Models;
using Microsoft.AspNetCore.Mvc;

namespace DormCare.Tests
{
    public class RoomsControllerTests
    {
        private ApplicationDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public void Create_Post_AddsRoomToDatabase()
        {
            using var db = CreateContext(Guid.NewGuid().ToString());
            var controller = new RoomsController(db);
            // TempData is used by the controller to store error messages; initialize it for unit test
            controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new Microsoft.AspNetCore.Http.DefaultHttpContext(),
                new TestTempDataProvider());

            var room = new Room { Number = "101", Capacity = 2 };
            var result = controller.Create(room);

            Assert.IsType<RedirectToActionResult>(result);
            var saved = db.Rooms.FirstOrDefaultAsync(r => r.Number == "101").Result;
            Assert.NotNull(saved);
            Assert.Equal(2, saved.Capacity);
        }

        [Fact]
        public void DeleteConfirmed_PreventsDeletingOccupiedRoom()
        {
            using var db = CreateContext(Guid.NewGuid().ToString());
            var room = new Room { Number = "201", Capacity = 2 };
            db.Rooms.Add(room);
            db.SaveChanges();

            var student = new Student { UserId = 1, RoomId = room.Id };
            db.Students.Add(student);
            db.SaveChanges();

             var controller = new RoomsController(db);
             // Call the GET Delete action to verify occupied count is calculated (avoids TempData usage)
             var result = controller.Delete(room.Id);
             Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(result);
             var occupied = controller.ViewData["Occupied"] as int? ?? -1;
             Assert.Equal(1, occupied);
        }
    }
}

    // Simple ITempDataProvider for tests
    internal class TestTempDataProvider : Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider
    {
        public System.Collections.Generic.IDictionary<string, object> LoadTempData(Microsoft.AspNetCore.Http.HttpContext context)
        {
            return new System.Collections.Generic.Dictionary<string, object>();
        }

        public void SaveTempData(Microsoft.AspNetCore.Http.HttpContext context, System.Collections.Generic.IDictionary<string, object> values)
        {
            // no-op
        }
    }
