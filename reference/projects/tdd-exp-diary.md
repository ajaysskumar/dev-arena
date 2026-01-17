# TDD Expense Tracker Development Diary

This file documents the step-by-step TDD process for developing the Expense Tracker API.

---

## Feature: Service rejects negative amount

### Step 1: Write a test for the expected behaviour

**Test:** The service should throw an ArgumentException when trying to add an expense with a negative amount.

```csharp
[Fact]
public void AddExpense_ShouldThrow_WhenAmountIsNegative()
{
    var repo = new InMemoryExpenseRepository();
    var service = new ExpenseService(repo);
    var dto = new ExpenseDto { Title = "Lunch", Amount = -5, Category = "Food", Date = DateTime.Today };
    Assert.Throws<ArgumentException>(() => service.AddExpense(dto));
}
```

**Reasoning:**
- Business rule: Expenses must have a positive amount.
- This test enforces the rule at the service layer.

---

## Step 2: Run the test and document the failure

**Test Run Log:**

When running the test `AddExpense_ShouldThrow_WhenAmountIsNegative`, the test fails because the current implementation of `ExpenseService.AddExpense` does not throw an exception for negative amounts.

**Error Message:**
```
X AddExpense_ShouldThrow_WhenAmountIsNegative [123 ms]
  Error Message:
   Xunit.Sdk.ThrowsException: Assert.Throws() Failure
  Expected: typeof(System.ArgumentException)
  Actual:   (No exception was thrown)
```

**Analysis:**
- The test fails as expected, confirming that the business rule is not yet implemented.
- Next step: Write the minimum code in `ExpenseService` to make the test pass.

---

## Step 3: Write the minimum code to make the test pass

**Implementation:**
Added a guard clause in `ExpenseService.AddExpense` to throw an `ArgumentException` if the amount is less than or equal to zero.

```csharp
public Expense AddExpense(ExpenseDto dto)
{
    if (dto.Amount <= 0)
        throw new ArgumentException("Amount must be positive.");
    // ...existing code...
}
```

**Reasoning:**
- This is the simplest change to make the test pass.
- It enforces the business rule at the service layer.

---

## Step 4: Run the test again to confirm it passes

**Test Run Log:**

After implementing the guard clause, the test `AddExpense_ShouldThrow_WhenAmountIsNegative` now passes.

**Result:**
```
Test summary: total: 1, failed: 0, succeeded: 1, skipped: 0, duration: 0.8s
```

**Analysis:**
- The business rule is now enforced at the service layer.
- The TDD cycle for this feature is complete.

---
