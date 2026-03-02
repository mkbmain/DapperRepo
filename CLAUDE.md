# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run all tests (defaults to Sqlite)
dotnet test Mkb.DapperRepo.Tests/Mkb.DapperRepo.Tests.csproj

# Run tests for a specific DB provider
dotnet test -c Sqlite     # default — no real DB server needed
dotnet test -c SqlServer
dotnet test -c MySql
dotnet test -c Postgres

# Run a single test class or method
dotnet test --filter "FullyQualifiedName~AddTests"
dotnet test --filter "FullyQualifiedName~AddTests.Add_Should_Add_Item"

# Pack NuGet package
dotnet pack Mkb.DapperRepo/Mkb.DapperRepo.csproj -c Release
```

## Architecture

Two projects in the solution:

- **`Mkb.DapperRepo`** — the library (targets `net8.0`, `netstandard2.0`, `net10.0`). Only dependency is Dapper.
- **`Mkb.DapperRepo.Tests`** — xUnit integration tests targeting `net10.0`. These are **not** unit tests; they spin up a real database.

### Library internals

`SqlRepoBase` is the core base class that builds all SQL strings dynamically via reflection. `SqlRepo` (sync) and `SqlRepoAsync` (async, supports `CancellationToken`) both inherit from it. Both are constructed with a `Func<DbConnection>` factory — the library is provider-agnostic and has no direct dependency on any DB driver.

**Reflection layer** (`Mkb.DapperRepo/Reflection/`):
- `ReflectionUtils` — caches `EntityPropertyInfo` per type in a `ConcurrentDictionary`. Reads `[PrimaryKey]`, `[SqlIgnoreColumn]`, and `[SqlColumnName]` attributes on first use.
- `EntityPropertyInfo` — holds the property metadata for a type, including three dictionaries for fast lookup: by class property name, by lower-cased class property name, and by SQL column name.

**Mapping** (`Mkb.DapperRepo/Mappers/TableMapper.cs`):
- Registers a Dapper `CustomPropertyTypeMap` for each type on first use, enabling column-name mapping via `[SqlColumnName]`. Lookup is case-insensitive.

**Attributes** (`Mkb.DapperRepo/Attributes/`):
- `[PrimaryKey]` — required for `GetById`, `Update`, `Delete`
- `[SqlTableName("...")]` — overrides the table name (defaults to the class name)
- `[SqlColumnName("...")]` — maps a property to a differently-named DB column
- `[SqlIgnoreColumn]` — excludes a property from all SQL operations

**Search** (`Mkb.DapperRepo/Search/`):
- `SearchType` enum: `Equals`, `Like`, `IsNull`, etc.
- `SearchCriteria.Create(propertyName, searchType)` — used to build multi-criteria search queries.

### Tests

The test project selects the DB provider at **compile time** via `#if` preprocessor constants set by the build configuration (`MYSQL`, `POSTGRES`, `SQLSERVER`; default/`SQLITE` is the fallback). Connection strings live in `Connection.cs`.

Each test class that needs a database inherits `BaseDbSetupTestClass`, which:
1. Creates a uniquely-named database by running the appropriate script from `SqlScripts/` (e.g., `CreateDbWithTestTable.Sqlite`).
2. Drops it in `Dispose()`.

`DataBaseScriptRunnerAndBuilder` provides raw insert/query helpers used for test setup and verification that bypass the `SqlRepo` implementation under test.

`[InternalsVisibleTo("Mkb.DapperRepo.Tests")]` is declared in both `ReflectionUtils.cs` and `EntityPropertyInfo.cs` so tests can access internal types directly.
