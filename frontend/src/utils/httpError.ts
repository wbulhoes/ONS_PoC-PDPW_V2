/**
 * HttpError Class
 * Typed error for HTTP response failures
 *
 * Usage: throw new HttpError(400, { message: "Bad Request", field: "email" })
 */

export class HttpError extends Error {
  public readonly status: number;
  public readonly data?: unknown;

  constructor(status: number, data?: unknown, message?: string) {
    const errorMessage =
      message || `HTTP Error ${status}`;
    super(errorMessage);

    // Maintain proper prototype chain for instanceof checks
    Object.setPrototypeOf(this, HttpError.prototype);

    this.name = 'HttpError';
    this.status = status;
    this.data = data;
  }

  /**
   * Get user-friendly error message based on HTTP status
   */
  getUserFriendlyMessage(): string {
    switch (this.status) {
      case 400:
        return 'Please check your input and try again';
      case 401:
        return 'Your session has expired. Please log in again.';
      case 403:
        return 'You do not have permission to perform this action.';
      case 404:
        return 'The requested resource was not found.';
      case 409:
        return 'This operation conflicts with existing data.';
      case 500:
      case 503:
        return 'System temporarily unavailable. Please try again later.';
      default:
        return 'An error occurred. Please try again.';
    }
  }

  /**
   * Check if error is a client error (4xx)
   */
  isClientError(): boolean {
    return this.status >= 400 && this.status < 500;
  }

  /**
   * Check if error is a server error (5xx)
   */
  isServerError(): boolean {
    return this.status >= 500 && this.status < 600;
  }

  /**
   * Check if this is a validation error (400 or 422)
   */
  isValidationError(): boolean {
    return this.status === 400 || this.status === 422;
  }

  /**
   * Check if this is a conflict error (409)
   */
  isConflictError(): boolean {
    return this.status === 409;
  }

  /**
   * Check if this is a not found error (404)
   */
  isNotFoundError(): boolean {
    return this.status === 404;
  }

  /**
   * Check if this is an unauthorized error (401/403)
   */
  isAuthError(): boolean {
    return this.status === 401 || this.status === 403;
  }
}
