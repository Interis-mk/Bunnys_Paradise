# Copilot Instructions (1st–2nd Year Student Repository)

These instructions define how GitHub Copilot (and other AI assistants) should generate changes in this repository so that the code is appropriate for **1st–2nd Year students** to read, review, and maintain.

## 1) Audience & Goals

**Audience:** early undergraduate / first two years of CS/SE coursework.

**Primary goals (in order):**
1. **Clarity over cleverness.**
2. **Correctness and safety.**
3. **Consistency with existing codebase conventions.**
4. **Appropriate learning value** (code should teach good habits).
5. **Incremental change size** (easy to review in small PRs).

When proposing changes, prefer a solution that a student can explain on a whiteboard.

---

## 2) General Code Style Requirements

### Keep it simple
- Prefer straightforward control flow over advanced patterns.
- Avoid “magic” metaprogramming, overly abstract frameworks, deep inheritance, or heavy functional tricks unless the repo already teaches them explicitly.
- Prefer **explicit names** to abbreviations:
    - ✅ `calculateFinalGrade()`
    - ❌ `calcFG()`

### Readability rules
- Functions should usually fit on one screen (~30–60 lines). Split if longer.
- Prefer early returns to reduce nesting, but don’t overuse.
- Use consistent formatting and follow the repo’s formatter/linter if present.

### Comments & docstrings
- Write comments explaining **why**, not what.
- Add short docstrings/JSDoc when it helps students understand inputs/outputs and edge cases.
- Do not add large tutorial essays inside code; keep explanations concise.

---

## 3) “Student-Friendly” Design Guidelines

### Prefer beginner-friendly constructs
- Use basic loops/conditionals where reasonable.
- Use standard library features that are commonly taught early (e.g., lists/arrays, dictionaries/maps, sets).
- Avoid advanced concurrency patterns unless required.

### Minimize cognitive load
- Keep data transformations in small steps.
- Avoid chaining many operations in one line if it hurts readability.
- Prefer explicit intermediate variables with good names.

### Error handling expectations
- Fail loudly and clearly in student exercises:
    - Provide helpful error messages.
    - Validate inputs at boundaries (user input, file IO, network).
- Avoid swallowing exceptions/errors silently.
- Prefer returning structured error info (where idiomatic) or throwing exceptions with context.

---

## 4) Testing Requirements (Educational)

When adding or changing behavior:
- Add/adjust tests that demonstrate:
    - **Happy path**
    - **One or two edge cases**
    - **One failure/invalid input case** (when applicable)

Test style:
- Keep tests readable and small.
- Prefer descriptive test names: `returns_error_when_input_is_empty`.
- Avoid overly clever test setups; clarity beats DRY in tests.

If the repository has no test framework yet, propose one only if requested; otherwise, include a short “manual test plan” in the PR description.

---

## 5) Documentation Requirements

Any change that affects usage must include one of:
- A small README update
- A usage example in docs
- A brief comment in the relevant file (if that’s the repo convention)

Examples should be minimal and runnable.

---

## 6) Security, Safety, and Privacy

- Do not introduce insecure patterns (e.g., SQL injection, `eval`, unsafe deserialization).
- Do not log secrets or include credentials.
- Be cautious with file paths (path traversal) and user input.
- Prefer safe defaults.

---

## 7) Dependency Policy

Because this is a student repo:
- Avoid adding new dependencies unless necessary.
- Prefer standard library solutions.
- If a dependency is required:
    - Keep it popular, well-documented, and stable.
    - Justify it briefly in the PR description (what it does and why it’s needed).

---

## 8) Code Review Checklist (What reviewers should look for)

Review changes using this checklist:

### Clarity
- [ ] Can a 1st–2nd year student explain this approach?
- [ ] Names clearly express intent.
- [ ] Code avoids unnecessary abstraction.

### Correctness
- [ ] Handles common edge cases.
- [ ] Input validation exists where needed.
- [ ] No obvious bugs (off-by-one, null/undefined, etc.).

### Maintainability
- [ ] Functions are small and focused.
- [ ] Duplication is acceptable if it improves readability (within reason).
- [ ] No premature optimization.

### Tests & Docs
- [ ] Tests cover the new behavior.
- [ ] Documentation/examples updated if user-facing behavior changed.

### Style & Consistency
- [ ] Matches existing project conventions.
- [ ] Formatting/linting passes (if applicable).

---

## 9) What Copilot Should NOT Do

- Do not rewrite large sections of the codebase for style alone.
- Do not introduce advanced patterns (monads, heavy generics, complex decorators, runtime reflection) unless the repo explicitly teaches them.
- Do not “optimize” by making code less readable.
- Do not add broad architectural layers (“service”, “repository”, “factory”, etc.) unless already used and necessary.

---

## 10) How Copilot Should Structure Contributions

When generating or suggesting changes:
1. **State intent** in 1–2 sentences.
2. Make the **smallest reasonable change**.
3. Include tests or a minimal test plan.
4. If multiple solutions exist, choose the most student-readable one and mention tradeoffs briefly.

---

## 11) Commit / PR Guidance (Optional but Recommended)

- Prefer small PRs: one feature/fix at a time.
- PR description should include:
    - What changed
    - Why it changed
    - How to test
    - Any known limitations

---

## 12) Clarifications Copilot Should Ask For (Instead of Guessing)

Copilot should ask maintainers/students when unclear about:
- Required language version (e.g., Python 3.10 vs 3.12, Node 18 vs 20)
- Expected input/output formats
- Performance constraints (usually not a priority here)
- Whether this is a teaching exercise (leave TODOs/prompts) vs production-like code

---

### Final note
This repository is for learning. Code should be **simple, readable, and explainable** even if it’s not the most “professional enterprise” architecture.