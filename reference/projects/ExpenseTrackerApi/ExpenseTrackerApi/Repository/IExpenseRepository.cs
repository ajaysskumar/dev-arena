using ExpenseTrackerApi.Models;

namespace ExpenseTrackerApi.Repository
{
    public interface IExpenseRepository
    {
        void Add(Expense expense);
        // Other methods as needed
    }
}
