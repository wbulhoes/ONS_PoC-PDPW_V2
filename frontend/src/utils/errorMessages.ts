/**
 * Error Message Mapping
 * Maps HTTP status codes and error types to user-friendly PT-BR messages
 */

export interface ErrorContext {
  status?: number;
  code?: string;
  field?: string;
  message?: string;
}

/**
 * Get user-friendly error message based on HTTP status and context
 * All messages are in Portuguese (PT-BR)
 *
 * @param httpStatus - HTTP status code (400, 404, 409, 500, etc.)
 * @param serverData - Server error response data
 * @returns User-friendly error message in PT-BR
 */
export function getUserFriendlyMessage(httpStatus: number, serverData?: unknown): string {
  // Handle specific status codes
  switch (httpStatus) {
    case 400:
      return handleValidationError(serverData);

    case 401:
      return 'Sua sessão expirou. Por favor, faça login novamente.';

    case 403:
      return 'Você não tem permissão para realizar esta ação.';

    case 404:
      return 'O recurso solicitado não foi encontrado.';

    case 409:
      return handleConflictError(serverData);

    case 500:
      return 'Erro do sistema. Por favor, tente novamente mais tarde.';

    case 503:
      return 'Sistema temporariamente indisponível. Por favor, tente novamente mais tarde.';

    case 0:
    case undefined:
      // Network error, timeout, or AbortError
      return 'Erro de conexão. Verifique sua internet e tente novamente.';

    default:
      if (httpStatus >= 400 && httpStatus < 500) {
        return 'Solicitação inválida. Por favor, verifique os dados e tente novamente.';
      }
      if (httpStatus >= 500) {
        return 'Erro do servidor. Por favor, tente novamente mais tarde.';
      }
      return 'Ocorreu um erro. Por favor, tente novamente.';
  }
}

/**
 * Handle 400 validation errors
 * Checks for specific field-level error messages in server response
 */
function handleValidationError(serverData?: unknown): string {
  if (!serverData || typeof serverData !== 'object') {
    return 'Por favor, verifique seus dados e tente novamente.';
  }

  const data = serverData as Record<string, unknown>;

  // Check for specific validation error messages
  if (data.message && typeof data.message === 'string') {
    return data.message;
  }

  if (data.error && typeof data.error === 'string') {
    return data.error;
  }

  // Generic validation error
  return 'Por favor, verifique seus dados e tente novamente.';
}

/**
 * Handle 409 conflict errors
 * Checks for specific conflict reason (duplicate login, email, etc.)
 */
function handleConflictError(serverData?: unknown): string {
  if (!serverData || typeof serverData !== 'object') {
    return 'Este recurso já existe. Por favor, use um valor diferente.';
  }

  const data = serverData as Record<string, unknown>;

  // Check for specific conflict messages
  if (data.message && typeof data.message === 'string') {
    const msg = data.message.toLowerCase();
    if (msg.includes('login')) {
      return 'Este login já está em uso. Por favor, escolha outro.';
    }
    if (msg.includes('email')) {
      return 'Este e-mail já está registrado. Por favor, use outro.';
    }
    return data.message;
  }

  if (data.error && typeof data.error === 'string') {
    if (data.error.toLowerCase().includes('duplicate')) {
      return 'Este recurso já existe. Por favor, use um valor diferente.';
    }
    return data.error;
  }

  // Generic conflict error
  return 'Este recurso já existe. Por favor, use um valor diferente.';
}

/**
 * Get error severity level for UI styling
 * @param httpStatus - HTTP status code
 * @returns Severity level: 'info' | 'warning' | 'error' | 'critical'
 */
export function getErrorSeverity(httpStatus: number): 'info' | 'warning' | 'error' | 'critical' {
  if (httpStatus >= 500) {
    return 'critical';
  }
  if (httpStatus >= 400 && httpStatus < 500) {
    if (httpStatus === 409 || httpStatus === 404) {
      return 'warning';
    }
    return 'error';
  }
  return 'info';
}

/**
 * Get error category for logging/tracking
 * @param httpStatus - HTTP status code
 * @returns Error category: 'client' | 'server' | 'network' | 'validation' | 'conflict' | 'notfound'
 */
export function getErrorCategory(
  httpStatus: number
): 'client' | 'server' | 'network' | 'validation' | 'conflict' | 'notfound' {
  switch (httpStatus) {
    case 400:
    case 422:
      return 'validation';
    case 404:
      return 'notfound';
    case 409:
      return 'conflict';
    case 0:
    case undefined:
      return 'network';
    default:
      if (httpStatus >= 400 && httpStatus < 500) {
        return 'client';
      }
      if (httpStatus >= 500) {
        return 'server';
      }
      return 'client';
  }
}
