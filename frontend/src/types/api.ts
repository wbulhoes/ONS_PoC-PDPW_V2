/**
 * Common API Types and Interfaces
 * 
 * Shared types used across all backend integration services
 */

/**
 * Standard API response wrapper
 */
export interface ApiResponse<T> {
  data?: T;
  success: boolean;
  message?: string;
  errors?: Record<string, string[]>;
}

/**
 * Paginated response wrapper
 */
export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageIndex: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

/**
 * Pagination parameters for queries
 */
export interface PaginationParams {
  pageIndex: number;
  pageSize: number;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}

/**
 * Standard error response from API
 */
export interface ErrorResponse {
  title?: string;
  message: string;
  errors?: Record<string, string[]>;
  statusCode: number;
  timestamp?: string;
}

/**
 * Standard list filter parameters
 */
export interface ListFilterParams {
  search?: string;
  skip?: number;
  take?: number;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
  filters?: Record<string, unknown>;
}

/**
 * Query response metadata
 */
export interface QueryMetadata {
  executedAt: string;
  duration: number;
  cacheHit: boolean;
}

/**
 * Combined response with metadata
 */
export interface QueryResponse<T> {
  data: T[];
  metadata: QueryMetadata;
  total: number;
}

/**
 * Standard entity base interface
 */
export interface BaseEntity {
  id: number;
  createdAt?: string;
  updatedAt?: string;
}

/**
 * Standard DTO for creating entities
 */
export interface BaseCreateDto {
  // Marker interface for typed DTOs
}

/**
 * Standard DTO for updating entities
 */
export interface BaseUpdateDto {
  // Marker interface for typed DTOs
}

/**
 * Audit trail information
 */
export interface AuditInfo {
  createdBy?: string;
  createdAt?: string;
  modifiedBy?: string;
  modifiedAt?: string;
  deletedBy?: string;
  deletedAt?: string;
}

/**
 * Validation error detail
 */
export interface ValidationErrorDetail {
  field: string;
  message: string;
  code?: string;
}

/**
 * Batch operation result
 */
export interface BatchOperationResult<T> {
  successful: T[];
  failed: Array<{
    item: T;
    error: string;
  }>;
  totalProcessed: number;
}

/**
 * Async operation status
 */
export interface OperationStatus {
  status: 'pending' | 'processing' | 'completed' | 'failed';
  progress?: number;
  error?: string;
  result?: unknown;
}

/**
 * Query request wrapper
 */
export interface QueryRequest {
  query: Record<string, unknown>;
  pagination?: PaginationParams;
  filters?: Record<string, unknown>;
}

/**
 * Common company/usina/plant entity
 */
export interface Company {
  id: number;
  name: string;
  code: string;
}

/**
 * Common plant entity
 */
export interface Plant {
  id: number;
  name: string;
  code: string;
  type: string;
  companyId: number;
}

/**
 * Common plant type entity
 */
export interface PlantType {
  id: number;
  name: string;
  code: string;
}

/**
 * Date range for queries
 */
export interface DateRange {
  startDate: string;
  endDate: string;
}

/**
 * Time interval specification
 */
export interface TimeInterval {
  startHour: number;
  endHour: number;
  interval: number; // minutes
}
