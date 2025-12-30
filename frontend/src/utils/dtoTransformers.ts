/**
 * DTO Transformers for Backend Integration
 * 
 * Handles conversion between backend API responses (PascalCase)
 * and frontend models (camelCase), including date/time transformations
 */

/**
 * Transform API response from PascalCase to camelCase
 * Handles nested objects and arrays recursively
 */
export function transformFromApi<T>(data: any): T {
  if (data === null || data === undefined) {
    return data;
  }

  if (Array.isArray(data)) {
    return data.map(item => transformFromApi(item)) as T;
  }

  if (typeof data === 'object' && data.constructor === Object) {
    const transformed: any = {};

    Object.keys(data).forEach(key => {
      const value = data[key];
      const camelCaseKey = toCamelCase(key);

      if (value === null || value === undefined) {
        transformed[camelCaseKey] = value;
      } else if (isDateString(value)) {
        transformed[camelCaseKey] = new Date(value);
      } else if (typeof value === 'object') {
        transformed[camelCaseKey] = transformFromApi(value);
      } else {
        transformed[camelCaseKey] = value;
      }
    });

    return transformed as T;
  }

  return data as T;
}

/**
 * Transform object from camelCase to PascalCase for API requests
 * Handles nested objects and arrays recursively
 */
export function transformToApi<T>(data: any): T {
  if (data === null || data === undefined) {
    return data;
  }

  if (Array.isArray(data)) {
    return data.map(item => transformToApi(item)) as T;
  }

  if (typeof data === 'object' && data.constructor === Object) {
    const transformed: any = {};

    Object.keys(data).forEach(key => {
      const value = data[key];
      const pascalCaseKey = toPascalCase(key);

      if (value === null || value === undefined) {
        transformed[pascalCaseKey] = value;
      } else if (value instanceof Date) {
        transformed[pascalCaseKey] = value.toISOString();
      } else if (typeof value === 'object') {
        transformed[pascalCaseKey] = transformToApi(value);
      } else {
        transformed[pascalCaseKey] = value;
      }
    });

    return transformed as T;
  }

  return data as T;
}

/**
 * Convert PascalCase to camelCase
 */
export function toCamelCase(str: string): string {
  return str.replace(/^([A-Z])/, (char) => char.toLowerCase());
}

/**
 * Convert camelCase to PascalCase
 */
export function toPascalCase(str: string): string {
  return str.replace(/^([a-z])/, (char) => char.toUpperCase());
}

/**
 * Check if string is a date in ISO format
 */
export function isDateString(value: string): boolean {
  if (typeof value !== 'string') {
    return false;
  }

  // ISO 8601 date pattern: YYYY-MM-DD, YYYY-MM-DDTHH:mm:ss, etc
  const isoDateRegex = /^\d{4}-\d{2}-\d{2}(T|$)/;
  
  if (!isoDateRegex.test(value)) {
    return false;
  }

  const date = new Date(value);
  return !isNaN(date.getTime());
}

/**
 * Transform single energetic data from API
 */
export function transformEnergeticFromApi(data: any) {
  return transformFromApi(data);
}

/**
 * Transform energetic data list from API
 */
export function transformEnergeticListFromApi(data: any[]): any[] {
  return data.map(item => transformEnergeticFromApi(item));
}

/**
 * Transform energetic data to API format
 */
export function transformEnergeticToApi(data: any) {
  return transformToApi(data);
}

/**
 * Transform single electrical data from API
 */
export function transformElectricalFromApi(data: any) {
  return transformFromApi(data);
}

/**
 * Transform electrical data list from API
 */
export function transformElectricalListFromApi(data: any[]): any[] {
  return data.map(item => transformElectricalFromApi(item));
}

/**
 * Transform electrical data to API format
 */
export function transformElectricalToApi(data: any) {
  return transformToApi(data);
}

/**
 * Transform IR1 data from API
 */
export function transformIR1FromApi(data: any) {
  return transformFromApi(data);
}

/**
 * Transform IR1 list from API
 */
export function transformIR1ListFromApi(data: any[]): any[] {
  return data.map(item => transformIR1FromApi(item));
}

/**
 * Transform IR1 data to API
 */
export function transformIR1ToApi(data: any) {
  return transformToApi(data);
}

/**
 * Transform IR2 data from API
 */
export function transformIR2FromApi(data: any) {
  return transformFromApi(data);
}

/**
 * Transform IR2 list from API
 */
export function transformIR2ListFromApi(data: any[]): any[] {
  return data.map(item => transformIR2FromApi(item));
}

/**
 * Transform IR2 data to API
 */
export function transformIR2ToApi(data: any) {
  return transformToApi(data);
}

/**
 * Transform IR3 data from API
 */
export function transformIR3FromApi(data: any) {
  return transformFromApi(data);
}

/**
 * Transform IR3 list from API
 */
export function transformIR3ListFromApi(data: any[]): any[] {
  return data.map(item => transformIR3FromApi(item));
}

/**
 * Transform IR3 data to API
 */
export function transformIR3ToApi(data: any) {
  return transformToApi(data);
}

/**
 * Transform IR4 data from API
 */
export function transformIR4FromApi(data: any) {
  return transformFromApi(data);
}

/**
 * Transform IR4 list from API
 */
export function transformIR4ListFromApi(data: any[]): any[] {
  return data.map(item => transformIR4FromApi(item));
}

/**
 * Transform IR4 data to API
 */
export function transformIR4ToApi(data: any) {
  return transformToApi(data);
}

/**
 * Transform export offer from API
 */
export function transformOfertaExportacaoFromApi(data: any) {
  return transformFromApi(data);
}

/**
 * Transform export offer list from API
 */
export function transformOfertaExportacaoListFromApi(data: any[]): any[] {
  return data.map(item => transformOfertaExportacaoFromApi(item));
}

/**
 * Transform export offer to API
 */
export function transformOfertaExportacaoToApi(data: any) {
  return transformToApi(data);
}
