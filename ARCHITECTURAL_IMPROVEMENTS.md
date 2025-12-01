# Architectural Improvements for ReceiptReader

This document outlines 10 key improvements to enhance the architecture, organization, and maintainability of the ReceiptReader application.

---

## ✅ 1. **Move Business Logic from Controllers to Service Layer**

**Problem:** Controllers contain too much business logic, validation, and orchestration code.

**Solution:** Extract business logic into dedicated service classes.

**Example:**
```csharp
// Before: Controller handles everything
public async Task<ActionResult> Upload(IFormFile file) {
    // validation logic
    // file storage logic
    // analysis logic
    // error handling
}

// After: Controller delegates to service
public async Task<ActionResult> Upload(IFormFile file) {
    var result = await _receiptService.ProcessReceiptAsync(...);
    return result.IsSuccess ? Ok(result.Receipt) : BadRequest(result.ErrorMessage);
}
```

**Implementation:** Created `IReceiptService` and `IFileRetrievalService` interfaces in `Application/Services/`.

---

## 2. **Split Solution into Multiple Projects**

**Problem:** All code is in a single project, making it harder to enforce layer separation and reuse components.

**Solution:** Create separate projects following Clean Architecture principles.

**Example Structure:**
```
ReceiptReader.sln
├── src/
│   ├── ReceiptReader.Api/              # Controllers, Program.cs
│   ├── ReceiptReader.Application/      # Interfaces, DTOs, Services
│   ├── ReceiptReader.Domain/           # Domain entities
│   └── ReceiptReader.Infrastructure/   # Implementations, DbContext
└── tests/
    ├── ReceiptReader.UnitTests/
    └── ReceiptReader.IntegrationTests/
```

**Benefits:** Clear dependencies, easier testing, potential for microservices extraction.

---

## 3. **Move DTOs to Application Layer**

**Problem:** DTOs are in the root folder, not clearly associated with any layer.

**Solution:** Move DTOs to `Application/DTOs/` or `Application/Contracts/`.

**Example:**
```
Application/
├── DTOs/
│   ├── ReceiptDto.cs
│   ├── ReceiptLineItemDto.cs
│   └── ReceiptTaxLineDto.cs
```

**Benefit:** Clear ownership - DTOs are application contracts, not infrastructure or domain concerns.

---

## 4. **Move Mappers to Application Layer**

**Problem:** Mappers are in the root folder alongside controllers.

**Solution:** Move to `Application/Mapping/` or use AutoMapper.

**Example:**
```csharp
// Application/Mapping/ReceiptMappingProfile.cs
public class ReceiptMappingProfile : Profile {
    public ReceiptMappingProfile() {
        CreateMap<ReceiptInfo, ReceiptDto>();
        CreateMap<ReceiptLineItem, ReceiptLineItemDto>();
    }
}
```

**Benefit:** Centralized mapping logic, easier to maintain and test.

---

## 5. **Introduce Application Use Cases (CQRS Pattern)**

**Problem:** Services can become bloated with multiple responsibilities.

**Solution:** Organize by use case with Commands and Queries.

**Example:**
```
Application/
├── Commands/
│   ├── UploadReceipt/
│   │   ├── UploadReceiptCommand.cs
│   │   ├── UploadReceiptCommandHandler.cs
│   │   └── UploadReceiptValidator.cs
└── Queries/
    ├── GetReceipt/
    │   ├── GetReceiptQuery.cs
    │   └── GetReceiptQueryHandler.cs
```

**Library:** Consider MediatR for implementing this pattern.

---

## 6. **Add Domain Services for Business Rules**

**Problem:** Business rules scattered across infrastructure and application layers.

**Solution:** Create domain services for core business logic.

**Example:**
```csharp
// Domain/Services/IReceiptValidationService.cs
public interface IReceiptValidationService {
    bool IsReceiptContentValid(ReceiptInfo receipt);
    ValidationResult ValidateTotals(ReceiptInfo receipt);
}

// Usage in Application layer
var validationResult = _domainService.ValidateTotals(receipt);
```

**Benefit:** Core business rules stay in Domain layer, independent of frameworks.

---

## 7. **Separate Configuration Models**

**Problem:** Configuration classes mixed with infrastructure implementations.

**Solution:** Group all configuration in dedicated folder or project.

**Example:**
```
Application/
├── Configuration/
│   ├── FileSettings.cs
│   ├── TesseractOptions.cs
│   └── AzureAIOptions.cs
```

Or create: `ReceiptReader.Configuration` project.

---

## 8. **Implement Repository Pattern Properly**

**Problem:** Repositories expose EF Core specifics.

**Solution:** Make repositories truly persistence-agnostic.

**Example:**
```csharp
// Bad: EF-specific
public interface IReceiptRepository {
    IQueryable<ReceiptInfo> GetAll(); // Leaks EF Core
}

// Good: Persistence-agnostic
public interface IReceiptRepository {
    Task<IEnumerable<ReceiptInfo>> GetAllAsync();
    Task<ReceiptInfo?> GetByIdAsync(Guid id);
}
```

**Benefit:** Can swap EF Core for Dapper, ADO.NET, or NoSQL without changing application code.

---

## 9. **Add API Versioning and Separate API Contracts**

**Problem:** No versioning strategy for API evolution.

**Solution:** Implement API versioning and separate contracts.

**Example:**
```csharp
// Api/V1/Controllers/FilesController.cs
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FilesController : ControllerBase { }

// Api/V2/Controllers/FilesController.cs
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FilesController : ControllerBase { }
```

**Package:** `Microsoft.AspNetCore.Mvc.Versioning`

---

## 10. **Introduce Middleware for Cross-Cutting Concerns**

**Problem:** Error handling, logging, and validation repeated across controllers.

**Solution:** Use middleware for common concerns.

**Example:**
```csharp
// Infrastructure/Middleware/ExceptionHandlingMiddleware.cs
public class ExceptionHandlingMiddleware {
    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        }
        catch (ReceiptAnalyzerException ex) {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
        }
    }
}
```

**Benefit:** Controllers focus on HTTP concerns only, business logic stays clean.

---

## Summary of Changes Applied

1. ✅ Created `IReceiptService` interface in `Application/Services/`
2. ✅ Implemented `ReceiptService` in `Infrastructure/Services/`
3. ✅ Created `IFileRetrievalService` interface in `Application/Services/`
4. ✅ Implemented `FileRetrievalService` in `Infrastructure/Services/`
5. ✅ Refactored `FilesController` to use services instead of direct dependencies
6. ✅ Updated `Program.cs` to register new services

## Next Steps

- Consider implementing suggestions 2-10 for a more robust architecture
- Add unit tests for the new service layer
- Consider adding FluentValidation for request validation
- Implement health checks and observability
- Add API documentation with XML comments

---

**Note:** These improvements follow Clean Architecture and SOLID principles, making the codebase more maintainable, testable, and scalable.
