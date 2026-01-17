using System;
using ExpenseTrackerApi.Models;
using ExpenseTrackerApi.Services;
using ExpenseTrackerApi.Repository;
using Xunit;

namespace ExpenseTrackerApi.Tests.Services
{
    public class ExpenseServiceTests
    {
        // Helper to create a valid ExpenseDto
        private ExpenseDto CreateValidExpenseDto()
        {
            return new ExpenseDto
            {
                Title = "Lunch",
                Amount = 10.5m,
                Category = "Food",
                Date = DateTime.Today
            };
        }

        [Fact]
        public void AddExpense_ShouldThrow_WhenAmountIsNegative()
        {
            // Arrange: negative amount
            var repo = new InMemoryExpenseRepository();
            var service = new ExpenseService(repo);
            var dto = CreateValidExpenseDto();
            dto.Amount = -5;

            // Act & Assert: should throw
            Assert.Throws<ArgumentException>(() => service.AddExpense(dto));
        }

        [Fact]
        public void AddExpense_ShouldThrow_WhenAmountIsZero()
        {
            // Arrange: zero amount
            var repo = new InMemoryExpenseRepository();
            var service = new ExpenseService(repo);
            var dto = CreateValidExpenseDto();
            dto.Amount = 0;

            // Act & Assert: should throw
            Assert.Throws<ArgumentException>(() => service.AddExpense(dto));
        }

        [Fact (Skip = "Not implemented yet")]
        public void AddExpense_ShouldThrow_WhenTitleIsEmpty()
        {
            // Arrange: empty title
            var repo = new InMemoryExpenseRepository();
            var service = new ExpenseService(repo);
            var dto = CreateValidExpenseDto();
            dto.Title = "";

            // Act & Assert: should throw
            Assert.Throws<ArgumentException>(() => service.AddExpense(dto));
        }

        [Fact (Skip = "Not implemented yet")]
        public void AddExpense_ShouldThrow_WhenTitleIsNull()
        {
            // Arrange: null title
            var repo = new InMemoryExpenseRepository();
            var service = new ExpenseService(repo);
            var dto = CreateValidExpenseDto();
            dto.Title = null;

            // Act & Assert: should throw
            Assert.Throws<ArgumentException>(() => service.AddExpense(dto));
        }

        [Fact (Skip = "Not implemented yet")]
        public void AddExpense_ShouldReturnExpense_WhenValid()
        {
            // Arrange: valid expense
            var repo = new InMemoryExpenseRepository();
            var service = new ExpenseService(repo);
            var dto = CreateValidExpenseDto();

            // Act
            var result = service.AddExpense(dto);

            // Assert: returned expense matches input
            Assert.NotNull(result);
            Assert.Equal(dto.Title, result.Title);
            Assert.Equal(dto.Amount, result.Amount);
            Assert.Equal(dto.Category, result.Category);
            Assert.Equal(dto.Date, result.Date);
        }

        [Fact (Skip = "Not implemented yet")]
        public void AddExpense_ShouldCallRepositoryOnce_WhenValid()
        {
            // Arrange: use a mock to verify repository call
            var mockRepo = new Moq.Mock<IExpenseRepository>();
            var service = new ExpenseService(mockRepo.Object);
            var dto = CreateValidExpenseDto();

            // Act
            service.AddExpense(dto);

            // Assert: repository Add called once
            mockRepo.Verify(r => r.Add(Moq.It.IsAny<Expense>()), Moq.Times.Once);
        }
    }
}
