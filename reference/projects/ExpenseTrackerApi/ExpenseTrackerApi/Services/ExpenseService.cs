using ExpenseTrackerApi.Models;
using ExpenseTrackerApi.Repository;

namespace ExpenseTrackerApi.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repository;

        public ExpenseService(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public Expense AddExpense(ExpenseDto dto)
        {
            if (dto.Amount <= 0)
                throw new ArgumentException("Amount must be positive.");
            // Implementation will be added via TDD
            throw new NotImplementedException();
        }
    }
}
