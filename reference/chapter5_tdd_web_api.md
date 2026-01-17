# Chapter 5: TDD in a Web/API Context — A Practical Mini‑Project

## 1. Introduction
In this chapter, we bring together all the concepts learned so far and apply them to a real-world scenario: building a small Web/API feature using Test‑Driven Development.  
We will focus on writing thin controllers, clean service layers, and testable logic by isolating dependencies.

---

## 2. Problem Statement
We will build a minimal **Expense Tracker API** with one core feature:

### Feature: Add a new expense  
- Users can add an expense with:  
  - title  
  - amount  
  - category  
  - date  
- API must validate input and store the expense.

This is intentionally small to keep the TDD cycle focused.

---

## 3. System Breakdown
### Components
- **API Controller** — Receives the request  
- **Service Layer** — Contains business logic  
- **Repository** — Persists data  
- **Models/DTOs** — Define structure  

### Roles in TDD
- Controller tests → API behavior  
- Service tests → business logic  
- Repository tests → storage logic (can be mocked/faked based on scope)

---

## 4. Test Strategy
### 4.1 What to Test in Controller
- Should return 400 for invalid data  
- Should return 201 for valid expense  
- Should call service with right values  

### 4.2 What to Test in Service
- Should reject negative/zero amounts  
- Should reject empty titles  
- Should create a valid Expense model  
- Should call repository once  

### 4.3 What to Test in Repository
- For the mini-project: use an in‑memory fake  
- Test basic persistence behavior if necessary  

---

## 5. Step-by-Step TDD Implementation

### Step 1 — Write Service Tests First  
- Start from the core logic  
- Force domain rules to emerge from tests  
- Example rules:
  - amount > 0  
  - title cannot be empty  
  - category must be known  

### Step 2 — Implement Service Logic  
- Only write enough code to pass each failing test  
- Refactor frequently  

### Step 3 — Write Controller Tests  
- Mock the service  
- Validate input  
- Assert correct HTTP status codes  

### Step 4 — Implement Controller  
- Thin controllers  
- Delegate to service  
- Return consistent responses  

### Step 5 — Add Repository Fake  
- Keep storage simple (list/dictionary)  
- The goal is to keep focus on TDD of logic, not infra setup  

---

## 6. Full Example Flow
```
Red  → Write test for “service rejects negative amount”
Green → Implement minimal validation
Refactor → Move validation to separate private method if needed

Red  → Write test for “controller returns 201”
Green → Add endpoint & wire service
Refactor → Improve response models
```

TDD remains a tight, small-step process.

---

## 7. Lessons Learned
- Always start from the core business logic  
- Write API tests only after logic is stable  
- Mocks/stubs help isolate web layers from business rules  
- Thin controllers, fat services  
- Fakes are useful for fast TDD cycles  

---

## 8. Summary
This chapter demonstrated how to apply TDD to a real Web/API scenario by:  
- breaking the system into layers  
- writing tests layer by layer  
- implementing only what is needed to make tests pass  
- keeping dependencies controlled through mocks and fakes  

This mini‑project prepares us for more advanced TDD topics in later chapters.
