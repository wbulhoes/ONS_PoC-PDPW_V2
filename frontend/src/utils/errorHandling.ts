/**
 * Error Handling Utilities for Backend Integration
 * 
 * Provides standardized error handling for API responses
 * and error normalization across the application.
 */

export interface NormalizedError {
  code: string;
  message: string;
  field?: string;
  details?: Record<string, string[]>;
  statusCode: number;
}

export interface ApiClientError extends Error {
  status: number;
  errors?: Record<string, string[]>;
}

/**
 * Normalize API error responses to consistent format
 */
export function normalizeError(error: unknown): NormalizedError {
  // Handle ApiClientError from apiClient
  if (error instanceof Error && 'status' in error) {
    const apiError = error as ApiClientError;
    
    return {
      code: getErrorCode(apiError.status),
      message: error.message || 'Erro desconhecido',
      statusCode: apiError.status,
      details: apiError.errors,
    };
  }

  // Handle generic errors
  if (error instanceof Error) {
    return {
      code: 'UNKNOWN_ERROR',
      message: error.message || 'Erro desconhecido',
      statusCode: 0,
    };
  }

  // Handle string errors
  if (typeof error === 'string') {
    return {
      code: 'UNKNOWN_ERROR',
      message: error,
      statusCode: 0,
    };
  }

  // Fallback
  return {
    code: 'UNKNOWN_ERROR',
    message: 'Erro desconhecido',
    statusCode: 0,
  };
}

/**
 * Get standardized error code from HTTP status code
 */
export function getErrorCode(statusCode: number): string {
  switch (statusCode) {
    case 400:
      return 'VALIDATION_ERROR';
    case 401:
      return 'UNAUTHORIZED';
    case 403:
      return 'FORBIDDEN';
    case 404:
      return 'NOT_FOUND';
    case 409:
      return 'CONFLICT';
    case 500:
      return 'SERVER_ERROR';
    case 502:
      return 'BAD_GATEWAY';
    case 503:
      return 'SERVICE_UNAVAILABLE';
    default:
      return statusCode >= 500 ? 'SERVER_ERROR' : 'CLIENT_ERROR';
  }
}

/**
 * Get user-friendly error message
 */
export function getErrorMessage(code: string, defaultMessage: string): string {
  const messages: Record<string, string> = {
    VALIDATION_ERROR: 'Os dados fornecidos são inválidos',
    UNAUTHORIZED: 'Você não está autenticado. Por favor, faça login',
    FORBIDDEN: 'Você não tem permissão para acessar este recurso',
    NOT_FOUND: 'O recurso solicitado não foi encontrado',
    CONFLICT: 'Conflito: O recurso já existe ou está em uso',
    SERVER_ERROR: 'Erro no servidor. Por favor, tente novamente',
    BAD_GATEWAY: 'Erro de comunicação com o servidor',
    SERVICE_UNAVAILABLE: 'Serviço indisponível. Por favor, tente mais tarde',
    UNKNOWN_ERROR: 'Ocorreu um erro inesperado',
    NETWORK_ERROR: 'Erro de conexão. Verifique sua internet',
  };

  return messages[code] || defaultMessage;
}

/**
 * Check if error is retryable
 */
export function isRetryableError(error: NormalizedError): boolean {
  const retryableCodes = ['SERVER_ERROR', 'BAD_GATEWAY', 'SERVICE_UNAVAILABLE', 'NETWORK_ERROR'];
  return retryableCodes.includes(error.code);
}

/**
 * Check if error is user validation error
 */
export function isValidationError(error: NormalizedError): boolean {
  return error.code === 'VALIDATION_ERROR' && error.statusCode === 400;
}

/**
 * Extract field-specific errors
 */
export function getFieldErrors(error: NormalizedError): Record<string, string> {
  if (!error.details) {
    return {};
  }

  const fieldErrors: Record<string, string> = {};
  
  Object.entries(error.details).forEach(([field, messages]) => {
    fieldErrors[field] = messages[0] || 'Campo inválido';
  });

  return fieldErrors;
}

/**
 * Check if error is authentication-related
 */
export function isAuthError(error: NormalizedError): boolean {
  return error.code === 'UNAUTHORIZED' || error.code === 'FORBIDDEN';
}

/**
 * Create a timeout error
 */
export function createTimeoutError(): NormalizedError {
  return {
    code: 'TIMEOUT_ERROR',
    message: 'A requisição demorou muito tempo. Por favor, tente novamente',
    statusCode: 408,
  };
}

/**
 * Create a network error
 */
export function createNetworkError(): NormalizedError {
  return {
    code: 'NETWORK_ERROR',
    message: 'Erro de conexão. Verifique sua internet',
    statusCode: 0,
  };
}
