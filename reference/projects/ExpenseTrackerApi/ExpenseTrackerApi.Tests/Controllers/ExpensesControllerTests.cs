using ExpenseTrackerApi.Controllers;
using ExpenseTrackerApi.Models;
using ExpenseTrackerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTrackerApi.Tests.Controllers
{
    public class ExpensesControllerTests
    {
        // TDD: Add tests for controller behavior here

        [Fact]
        public async Task GetExpensesByCategory_ReturnsExpensesForCategory()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            var category = "Food";
            var expected = new List<ExpenseDto> {
                new ExpenseDto { Title = "Lunch", Amount = 10, Category = "Food", Date = DateTime.Today },
                new ExpenseDto { Title = "Snacks", Amount = 5, Category = "Food", Date = DateTime.Today }
            };
            mockService.Setup(s => s.GetExpensesByCategoryAsync(category)).ReturnsAsync(expected);
            var controller = new ExpensesController(mockService.Object);

            // Act
            var result = await controller.GetExpensesByCategory(category);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsAssignableFrom<IEnumerable<ExpenseDto>>(okResult.Value);
            Assert.Equal(expected.Count, actual.Count());
        }
    }
}
