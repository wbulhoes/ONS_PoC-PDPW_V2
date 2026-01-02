/**
 * Compatibilidade temporária - redireciona para apiClient
 * TODO: Migrar todos os arquivos para usar apiClient diretamente
 */
import { apiClient } from './apiClient';

// Export default para compatibilidade com imports antigos
export default apiClient;

// Exports nomeados para compatibilidade
export { apiClient };
export * from './apiClient';
