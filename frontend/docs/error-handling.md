# Error Handling Guide

This document describes the error handling patterns and best practices for the user management feature.

## Architecture Overview

Error handling is implemented in 3 layers:

1. **HTTP Layer** (`HttpError` class)
   - Wraps HTTP responses with status codes
   - Provides predicates for error classification
   
2. **Message Layer** (`errorMessages.ts`)
   - Maps HTTP status codes to user-friendly PT-BR messages
   - Extracts domain-specific error reasons (e.g., duplicate login)

3. **Service Layer** (`userService.ts`)
   - Catches all errors and logs them for debugging
   - Transforms to `HttpError` for consistent handling
   - Supports request cancellation via `AbortSignal`

## HTTP Error Class

The `HttpError` class extends JavaScript's native `Error` and provides:

```typescript
import { HttpError } from '@/utils/httpError';

try {
  await userService.create(userData);
} catch (error) {
  if (error instanceof HttpError) {
    // Access error details
    console.log(error.status);        // HTTP status code
    console.log(error.data);          // Server response body
    console.log(error.message);       // Generic message
    
    // Use predicates for logic
    if (error.isValidationError()) {
      // Handle 400 validation error
    } else if (error.isConflictError()) {
      // Handle 409 duplicate error
    } else if (error.isNotFoundError()) {
      // Handle 404 not found
    }
  }
}
```

### Available Predicates

| Method | Checks | Status Codes |
|--------|--------|-------------|
| `isClientError()` | 4xx errors | 400-499 |
| `isServerError()` | 5xx errors | 500-599 |
| `isValidationError()` | Validation failed | 400, 422 |
| `isConflictError()` | Resource conflict | 409 |
| `isNotFoundError()` | Resource not found | 404 |
| `isAuthError()` | Auth/permission issues | 401, 403 |

## User-Friendly Messages

Convert HTTP errors to PT-BR user messages:

```typescript
import { getUserFriendlyMessage } from '@/utils/errorMessages';

try {
  await userService.update(userId, data);
} catch (error) {
  if (error instanceof HttpError) {
    // Get user-friendly message
    const message = getUserFriendlyMessage(error.status, error.data);
    // Example: "Este login já está em uso. Por favor, escolha outro."
    
    showToast({
      type: 'error',
      message: message,
    });
  }
}
```

### Supported Status Codes

| Status | Message |
|--------|---------|
| 400 | "Por favor, verifique seus dados e tente novamente." |
| 401 | "Sua sessão expirou. Por favor, faça login novamente." |
| 403 | "Você não tem permissão para realizar esta ação." |
| 404 | "O recurso solicitado não foi encontrado." |
| 409 | "Este recurso já existe. Por favor, use um valor diferente." |
| 500 | "Erro do sistema. Por favor, tente novamente mais tarde." |
| 503 | "Sistema temporariamente indisponível. Por favor, tente novamente mais tarde." |
| Network | "Erro de conexão. Verifique sua internet e tente novamente." |

### Conflict Detection

For 409 errors, the system checks server response for specific conflict reasons:

```typescript
// 409 with { message: 'duplicate login' } →
// "Este login já está em uso. Por favor, escolha outro."

// 409 with { message: 'duplicate email' } →
// "Este e-mail já está registrado. Por favor, use outro."

// 409 with other message →
// "Este recurso já existe. Por favor, use um valor diferente."
```

## Validation Error Extraction

Extract field-level errors from 400 responses:

```typescript
import { extractValidationErrors } from '@/utils/extractValidationErrors';

try {
  await userService.create(userData);
} catch (error) {
  if (error instanceof HttpError && error.isValidationError()) {
    // Extract field-level errors
    const fieldErrors = extractValidationErrors(error.data);
    // Result: { email: ["Invalid email"], login: ["Required"] }
    
    // Get first error for a field
    const emailError = fieldErrors.email?.[0];
    
    // Set form field errors
    setFormErrors(fieldErrors);
  }
}
```

### Supported Response Structures

The extractor handles multiple error response formats:

**Flat object:**
```typescript
{ email: "Invalid", login: "Required" }
```

**Nested errors object:**
```typescript
{ errors: { email: "Invalid" } }
```

**Array of errors:**
```typescript
{ email: ["Invalid", "Too long"] }
```

**Nested error objects:**
```typescript
{ email: { message: "Invalid email format" } }
```

## Error Severity Levels

Get error severity for UI styling:

```typescript
import { getErrorSeverity } from '@/utils/errorMessages';

const severity = getErrorSeverity(error.status);
// Returns: 'info' | 'warning' | 'error' | 'critical'

// 500+ → 'critical' (red, prominent)
// 409, 404 → 'warning' (yellow, less urgent)
// Other 4xx → 'error' (red)
// 2xx/3xx → 'info' (blue, informational)
```

## Error Categories

Categorize errors for logging and analytics:

```typescript
import { getErrorCategory } from '@/utils/errorMessages';

const category = getErrorCategory(error.status);
// Returns: 'validation' | 'conflict' | 'notfound' | 'client' | 'server' | 'network'
```

## Service Layer Error Handling

All service methods follow this pattern:

```typescript
export const userService = {
  list: async (params: UserPaginationParams, signal?: AbortSignal) => {
    try {
      const response = await apiClient.get('/usuarios?...', { signal });
      return response;
    } catch (error) {
      // Log full error context for debugging
      console.error('[userService.list] Error:', {
        status: error.response?.status,
        method: 'GET',
        url: '/usuarios',
        data: error.response?.data,
      });
      
      // Transform to HttpError
      throw new HttpError(
        error.response?.status || 500,
        error.response?.data,
        error.message
      );
    }
  },
};
```

### Key Patterns

1. **Always throw HttpError** - Never swallow errors or return error objects
2. **Log full context** - Status, method, URL, and payload for debugging
3. **Support AbortSignal** - Pass signal through to axios for cancellation
4. **Preserve stack traces** - HttpError extends Error to maintain stack

## Component Integration Example

Complete example of error handling in a component:

```typescript
import { useState } from 'react';
import { userService } from '@/services/userService';
import { HttpError } from '@/utils/httpError';
import { getUserFriendlyMessage } from '@/utils/errorMessages';
import { extractValidationErrors } from '@/utils/extractValidationErrors';

export const UserForm = () => {
  const [loading, setLoading] = useState(false);
  const [formErrors, setFormErrors] = useState<Record<string, string[]>>({});
  const [generalError, setGeneralError] = useState<string | null>(null);

  const handleSubmit = async (formData: UserFormData) => {
    setLoading(true);
    setFormErrors({});
    setGeneralError(null);

    try {
      await userService.create(formData);
      // Success - redirect or show success message
    } catch (error) {
      if (error instanceof HttpError) {
        if (error.isValidationError()) {
          // Show field-level errors
          const errors = extractValidationErrors(error.data);
          setFormErrors(errors);
        } else {
          // Show general error message
          const message = getUserFriendlyMessage(error.status, error.data);
          setGeneralError(message);
        }
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {generalError && <ErrorAlert message={generalError} />}
      
      {/* Form fields with individual error displays */}
      <input
        name="email"
        onBlur={() => {}}
        aria-invalid={!!formErrors.email}
        aria-describedby={formErrors.email ? 'email-error' : undefined}
      />
      {formErrors.email?.[0] && (
        <span id="email-error" role="alert">{formErrors.email[0]}</span>
      )}
      
      <button type="submit" disabled={loading}>
        {loading ? 'Salvando...' : 'Salvar'}
      </button>
    </form>
  );
};
```

## Request Cancellation

Support cancellation on component unmount:

```typescript
import { useEffect, useRef } from 'react';
import { userService } from '@/services/userService';

export const UserList = () => {
  const abortControllerRef = useRef<AbortController | null>(null);

  useEffect(() => {
    const controller = new AbortController();
    abortControllerRef.current = controller;

    userService.list(
      { page: 1, pageSize: 10 },
      controller.signal
    );

    return () => {
      // Cancel request on unmount
      controller.abort();
    };
  }, []);
};
```

## Debugging

All errors are logged to console with full context:

```
[userService.create] Error: {
  status: 409,
  method: 'POST',
  url: '/api/usuarios',
  data: { message: 'duplicate login' }
}
```

This enables developers to quickly identify:
- The HTTP method and endpoint
- The exact status code returned
- The server error response data

## Testing

Error handling is comprehensively tested:

- **Unit tests** (`httpError.test.ts`, `errorMessages.test.ts`)
  - 25+ error message mappings
  - 26+ validation error extraction patterns
  
- **Integration tests** (`userService.errors.test.ts`)
  - 29+ end-to-end error flow scenarios
  - Mocked API responses for all status codes
  - Field-level error extraction
  - Multi-step error recovery

Run tests:

```bash
npm test tests/utils/errorMessages.test.ts
npm test tests/utils/extractValidationErrors.test.ts
npm test tests/integration/userService.errors.test.ts
```

## Summary

| Layer | Responsibility | Key Files |
|-------|----------------|-----------|
| HTTP | Wrap responses, classify errors | `httpError.ts` |
| Message | Map to user-friendly PT-BR text | `errorMessages.ts` |
| Extraction | Parse field-level errors | `extractValidationErrors.ts` |
| Service | Log, transform, throw consistently | `userService.ts` |
| Component | Display errors, handle recovery | Your components |
| Testing | Verify all error paths | `.test.ts` files |
