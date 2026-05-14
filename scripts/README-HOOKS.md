# Git Pre-Commit Hooks

This project uses automated git pre-commit hooks to enforce code quality standards on all developers' machines.

## Automatic Installation

The hooks are automatically installed when you run:

```bash
dotnet restore
```

or

```bash
dotnet build
```

This happens because of the MSBuild `Directory.Build.targets` file in the repository root, which runs a post-restore target to install the hooks into your local `.git/hooks` directory.

## What the Hooks Check

The `pre-commit` hook enforces the following quality gates **before any commit is created**:

1. **Commit Message Format** — Validates gitmoji format
   - Format: `<gitmoji> <message>` (max 50 chars after emoji)
   - Valid gitmojis:
     - 🚧 Work in progress
     - ⚡️ Improve performance
     - 🐛 Fix a bug
     - ✨ Introduce new feature
     - 📝 Add or update documentation
     - ✅ Add, update, or pass tests
     - 🔒️ Fix security issues
     - ♻️ Refactor code
     - ✏️ Update file content
   - Example: `✨ Added pre-commit hook system`

2. **Code Formatting** — Runs `dotnet format --verify-no-changes`
   - Automatically fixable with: `dotnet format`

3. **Build Validation** — Runs `dotnet build -c Debug`
   - Ensures all projects compile without errors

4. **Unit Tests** — Runs `dotnet test`
   - Ensures all tests pass before committing

## Bypassing Hooks (Use with Caution)

To bypass hooks in exceptional circumstances:

```bash
# Skip all hooks
git commit --no-verify

# Or specific hook
git commit -m "message" --no-verify
```

⚠️ **Note**: Only use `--no-verify` when absolutely necessary. Quality gates exist to protect the repository.

## Fixing Issues

### Format errors
```bash
dotnet format
git add .
git commit -m "style: format code"
```

### Build errors
```bash
dotnet build
# Fix compilation errors, then
git add .
git commit -m "fix: resolve build issues"
```

### Test failures
```bash
dotnet test
# Fix failing tests, then
git add .
git commit -m "fix: resolve test failures"
```

### Commit message format
Ensure your message follows: `<gitmoji> <message>` with max 50 chars after emoji

Examples:
```bash
git commit -m "✨ Added pre-commit hook validation"
git commit -m "🐛 Fixed formatting check logic"
git commit -m "♻️ Refactored hook script structure"
git commit -m "📝 Updated hook documentation"
```

## Reinstalling Hooks

If hooks don't install automatically or you need to reinstall:

```bash
dotnet restore
```

Or manually copy (all platforms use bash):

```bash
cp scripts/hooks/pre-commit .git/hooks/
chmod +x .git/hooks/pre-commit
```

## Disabling Hooks Temporarily

To temporarily disable the pre-commit hook for local testing:

```bash
# Disable
chmod -x .git/hooks/pre-commit

# Re-enable
chmod +x .git/hooks/pre-commit
```

## Updating Hooks

If hook scripts are updated in the repository:

```bash
dotnet restore
```

This will copy the latest hook scripts to `.git/hooks`.

## Troubleshooting

**Q: Hooks aren't running on commit**  
A: Ensure `.git/hooks/pre-commit` is executable:
```bash
chmod +x .git/hooks/pre-commit
```

**Q: Can I see what the hook does?**  
A: Yes! Check `scripts/hooks/pre-commit` (or `pre-commit.ps1` on Windows)

**Q: The hook keeps failing for an unrelated reason**  
A: Contact the team. You may need to update your local environment or dependencies.

## For CI/CD

CI pipelines should run these checks independently:
```bash
dotnet format --verify-no-changes
dotnet build
dotnet test
```

The pre-commit hooks are for local development feedback, not CI validation.
