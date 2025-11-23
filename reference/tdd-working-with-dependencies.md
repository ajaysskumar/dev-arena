# 🧩 Working with Dependencies (Mocks, Stubs, Fakes — All in One)

## 🎯 Goal of the Article
When you first start writing tests, everything feels easy — until your code begins talking to the real world: APIs, databases, and email servers. Suddenly, your tests slow down, become flaky, or fail for no apparent reason.

This article will help you:
- Understand **why** dependencies make TDD tricky.  
- Learn **what** mocks, stubs, and fakes are (and when to use them).  
- Discover how **test doubles** make your tests faster, clearer, and your design better.  

---

## 🪄 1. When the Real World Gets in the Way of Your Tests
> “You just wrote a test that sends an email, hits an API, or queries a database — and suddenly your suite is failing for no reason.”

That’s when you realize the truth:  
**TDD isn’t about testing the world — it’s about isolating your code from it.**

Example:
```csharp
var userService = new UserService(new EmailService());
userService.RegisterUser("ajay@example.com");
```

Should this test actually send an email?  
Probably not.

So let’s meet our stand-ins — the *Test Doubles*.

---

## 🧱 2. Why Dependencies Make TDD Hard

Real dependencies are like external forces that resist testing:

- 🐢 **Slow** — database calls, API requests, etc.  
- 🎲 **Unreliable** — network issues, rate limits, external downtime.  
- 🎭 **Non-deterministic** — data changes or random outputs.  
- 🔗 **Tightly coupled** — hard to swap or replace.

> “The pain of testing dependencies is feedback. It’s your code telling you that your design could be cleaner.”

---

## 🧩 3. Meet the Test Doubles Family

Think of test doubles as *stunt performers* — they step in when the real thing is too risky for rehearsal.

| Type | What it does | Example | When to use |
|------|---------------|----------|--------------|
| **Dummy** | Passed but never used | A null logger | Required parameter but unused |
| **Stub** | Provides canned responses | `IUserRepo.GetUser()` returns a fake user | To control test data |
| **Fake** | Has a working but simplified logic | In-memory DB | Fast alternative for complex systems |
| **Mock** | Verifies interactions | `Mock<IEmailService>().Verify(...)` | To assert behavior |
| **Spy** | Records calls for later inspection | Capturing API calls | Debugging or behavior checks |

💡 *Mocks verify behavior, stubs control state.*

---

## 💡 4. Hands-on Example: Email Notification on Signup

### Step 1 — The Problem
```csharp
public class UserService {
    private readonly IEmailService _emailService;
    public UserService(IEmailService emailService) {
        _emailService = emailService;
    }

    public void Register(string email) {
        // ...registration logic
        _emailService.Send(email, "Welcome!");
    }
}
```

### Step 2 — The Naive Test
> “You don’t want to send an actual email during tests.”

### Step 3 — Introducing a Mock
```csharp
[Fact]
public void Should_Send_Welcome_Email_When_User_Registers() {
    var emailService = new Mock<IEmailService>();
    var sut = new UserService(emailService.Object);

    sut.Register("ajay@example.com");

    emailService.Verify(e => e.Send("ajay@example.com", "Welcome!"), Times.Once);
}
```

✅ Clear and fast  
🚫 But too many mocks can hide design issues.

---

## 🔍 5. Design Insight: What Your Mocks Reveal

Mocks don’t just help you test — they reveal design flaws.

- Too many mocks = **too many dependencies** → violates SRP.  
- Mock chains like `mock.Object.Property.Method()` = **tight coupling**.  
- Using fakes often hints you need **repository or service boundaries**.

> “Mocks don’t just test your code — they whisper where to refactor.”

---

## 🧰 6. Beyond Basics: Fakes & Hybrid Approaches

Sometimes you want *realistic yet fast* tests. That’s where **fakes** shine.

```csharp
public class InMemoryUserRepo : IUserRepo {
    private readonly List<User> _users = [];
    public void Save(User user) => _users.Add(user);
    public User? Find(string email) => _users.FirstOrDefault(u => u.Email == email);
}
```

> “Fakes give you integration realism without external pain.”

Compare:
- Unit tests → fast, isolated, use mocks.
- Integration tests → realistic, use fakes.

---

## ⚖️ 7. Common Gotchas

Let’s call out a few testing sins:

❌ **Mocking everything** — you’re testing mocks, not code.  
❌ **Verifying too much** — brittle tests that break on refactor.  
❌ **Inconsistent stubs** — cause confusion in results.  
✅ **Readable, intention-driven tests** — always the goal.

---

## 💬 8. Takeaway: Dependency Isolation = Design Clarity

> “Mocks and fakes aren’t testing tricks — they’re design feedback tools.”

They tell you:
- Where your abstractions are weak.
- Where your classes are doing too much.
- How to make collaboration explicit in your code.

If your code is hard to test, it’s not your tests’ fault — it’s your design asking for help.

---

## 🪶 9. Challenge for You

Refactor one of your existing services to use a **fake** instead of a real dependency.  
Run the test suite again.  
Does it feel faster, more predictable, more readable?

> “Great TDD isn’t about avoiding the real world.  
It’s about learning to simulate it — gracefully.”

---

*Next in the series → [TDD in a Web/API Context (Practical Mini-Project)](#)*
