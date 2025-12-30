/**
 * Validation Error Extractor
 * Extracts field-level errors from server 400 responses
 */

/**
 * Extract field-level validation errors from server error response
 * Handles multiple response structures:
 * - Flat: { email: "Invalid", login: "Required" }
 * - Nested: { errors: { email: "Invalid" } }
 * - Array: { email: ["Invalid", "Too long"] }
 * - Mixed: Combination of above
 *
 * @param errorData - Server error response data
 * @returns Record of field name → array of error messages
 */
export function extractValidationErrors(errorData: unknown): Record<string, string[]> {
  if (!errorData || typeof errorData !== 'object') {
    return {};
  }

  const data = errorData as Record<string, unknown>;
  const errors: Record<string, string[]> = {};

  // Try to extract from nested 'errors' object first
  if (data.errors && typeof data.errors === 'object') {
    return flattenErrorObject(data.errors as Record<string, unknown>);
  }

  // Try to extract from nested 'validationErrors' object
  if (data.validationErrors && typeof data.validationErrors === 'object') {
    return flattenErrorObject(data.validationErrors as Record<string, unknown>);
  }

  // Try to extract from nested 'details' object
  if (data.details && typeof data.details === 'object') {
    return flattenErrorObject(data.details as Record<string, unknown>);
  }

  // If no nested structure, try flat structure
  return flattenErrorObject(data);
}

/**
 * Flatten error object into consistent structure
 * @param obj - Object to flatten
 * @returns Flattened error record
 */
function flattenErrorObject(obj: Record<string, unknown>): Record<string, string[]> {
  const errors: Record<string, string[]> = {};

  for (const [field, value] of Object.entries(obj)) {
    if (!value) {
      continue;
    }

    // Skip non-string/non-array values that aren't error-like
    if (typeof value !== 'string' && !Array.isArray(value) && typeof value !== 'object') {
      continue;
    }

    // Handle string values
    if (typeof value === 'string') {
      errors[field] = [value];
      continue;
    }

    // Handle array values
    if (Array.isArray(value)) {
      errors[field] = value.map(v => String(v)).filter(v => v.length > 0);
      continue;
    }

    // Handle nested object values (e.g., { field: { message: "error" } })
    if (typeof value === 'object' && value !== null) {
      const nested = value as Record<string, unknown>;
      const messages: string[] = [];

      // Collect from standard fields first
      if (nested.message && typeof nested.message === 'string') {
        messages.push(nested.message);
      }

      if (nested.error && typeof nested.error === 'string') {
        messages.push(nested.error);
      }

      // Collect from other string properties (if no standard fields found)
      if (messages.length === 0) {
        const otherMessages = Object.values(nested)
          .filter((v): v is string => typeof v === 'string')
          .filter(v => v.length > 0);

        messages.push(...otherMessages);
      }

      if (messages.length > 0) {
        errors[field] = messages;
      }
      continue;
    }
  }

  return errors;
}

/**
 * Get first error message for a field
 * Useful for displaying single error per field in forms
 *
 * @param validationErrors - Validation errors record
 * @param fieldName - Field name to get error for
 * @returns First error message or null
 */
export function getFieldError(
  validationErrors: Record<string, string[]>,
  fieldName: string
): string | null {
  const errors = validationErrors[fieldName];
  if (!errors || errors.length === 0) {
    return null;
  }
  return errors[0];
}

/**
 * Get all error messages for a field
 *
 * @param validationErrors - Validation errors record
 * @param fieldName - Field name to get errors for
 * @returns Array of error messages
 */
export function getFieldErrors(
  validationErrors: Record<string, string[]>,
  fieldName: string
): string[] {
  return validationErrors[fieldName] || [];
}

/**
 * Check if field has errors
 *
 * @param validationErrors - Validation errors record
 * @param fieldName - Field name to check
 * @returns True if field has errors
 */
export function hasFieldError(validationErrors: Record<string, string[]>, fieldName: string): boolean {
  return fieldName in validationErrors && validationErrors[fieldName].length > 0;
}

/**
 * Get all fields with errors
 *
 * @param validationErrors - Validation errors record
 * @returns Array of field names that have errors
 */
export function getErrorFields(validationErrors: Record<string, string[]>): string[] {
  return Object.keys(validationErrors).filter(field => validationErrors[field].length > 0);
}

/**
 * Check if any fields have errors
 *
 * @param validationErrors - Validation errors record
 * @returns True if any field has errors
 */
export function hasValidationErrors(validationErrors: Record<string, string[]>): boolean {
  return Object.keys(validationErrors).length > 0 && getErrorFields(validationErrors).length > 0;
}
