# First Steps with TDD: A Simple Example (FizzBuzz/String Calculator)

## 🍽 Food for Thought

Think about the last time you tried to solve a small programming problem.  
Did you jump straight into writing the solution?  
Or did you pause and think about the *tests* that could prove your code works?  

Most of us (myself included, once upon a time) start by writing code first and only later—sometimes much later—think about whether it actually works. TDD flips that instinct. It asks us to **trust small steps, guided by tests, before rushing to code.**

In this post, let’s start with a ridiculously simple problem: **FizzBuzz**. It’s the kind of problem that feels too trivial for TDD, which makes it the *perfect* place to explore its gotchas and mindset shifts.

---

## 🎯 The Challenge: FizzBuzz (Classic Edition)

Write a function that:
- Returns `"Fizz"` if the number is divisible by 3  
- Returns `"Buzz"` if the number is divisible by 5  
- Returns `"FizzBuzz"` if divisible by both  
- Returns the number itself otherwise  

Seems easy, right? Let’s TDD it step by step.

---

## 🧪 Step 1: Write the First Test

```csharp
[Fact]
public void ReturnsNumber_WhenNotDivisibleBy3Or5()
{
    var fizzBuzz = new FizzBuzz();
    var result = fizzBuzz.GetValue(1);
    Assert.Equal("1", result);
}
```

Notice something?  
We’re not even thinking about multiples of 3 or 5 yet—we’re **starting small**.  
This is one of the hardest mindset shifts in TDD: *don’t solve the whole problem at once*. Solve the simplest case first.

---

## ⚡ Gotcha #1: The Urge to Over-Engineer

At this point, you might be tempted to write the full FizzBuzz solution right away.  
But TDD isn’t about showing off. It’s about trust.  
So let’s write the simplest thing that makes the test pass:

```csharp
public string GetValue(int number)
{
    return "1";
}
```

It feels wrong, doesn’t it? Hardcoding `"1"`?  
But here’s the trick: **we’ll generalize when the next test forces us to.**

---

## 🧪 Step 2: Add Another Test

```csharp
[Fact]
public void ReturnsNumberAsString_WhenNotDivisibleBy3Or5()
{
    var fizzBuzz = new FizzBuzz();
    var result = fizzBuzz.GetValue(2);
    Assert.Equal("2", result);
}
```

Now the hardcoded `"1"` fails. Great!  
Now we generalize:

```csharp
public string GetValue(int number)
{
    return number.ToString();
}
```

---

## ⚡ Gotcha #2: The “Leap Ahead” Temptation

It’s tempting to add the `if (number % 3 == 0)` logic *now*.  
But resist! The test hasn’t asked for it yet.  
If you do, you risk adding untested, potentially buggy code.

---

## 🧪 Step 3: Finally Test Multiples of 3

```csharp
[Fact]
public void ReturnsFizz_WhenDivisibleBy3()
{
    var fizzBuzz = new FizzBuzz();
    var result = fizzBuzz.GetValue(3);
    Assert.Equal("Fizz", result);
}
```

Now and only now do we add:

```csharp
public string GetValue(int number)
{
    if (number % 3 == 0)
        return "Fizz";
    return number.ToString();
}
```

---

## 🚧 We’ll Stop Here (For Now)

I won’t spoil the whole FizzBuzz here—you can imagine how the rest of the tests and implementation unfold.  

What matters more are the **TDD lessons hiding inside this toy problem**:

- Start with the simplest test you can think of.  
- Don’t solve tomorrow’s problem today.  
- Let tests “pull” the implementation forward, one failing test at a time.  
- Hardcoding is *not* a sin—it’s a step toward generalization.  

---

## ✨ Takeaway

FizzBuzz isn’t about impressing interviewers here—it’s about rewiring your instincts.  
Instead of asking *“How do I solve this problem?”* start asking *“What test will prove my code works?”*  

That’s the first real step toward Test-Driven Development.
