import { describe, it, expect } from 'vitest';
import { previsaoEolicaService } from '../../src/services/previsaoEolicaService';

describe('previsaoEolicaService', () => {
  it('getPrevisao returns parques', async () => {
    const res = await previsaoEolicaService.getPrevisao('20251225');
    expect(res).toHaveProperty('dataPdp', '20251225');
    expect(Array.isArray(res.parques)).toBe(true);
  });

  it('savePrevisao resolves', async () => {
    await expect(previsaoEolicaService.savePrevisao({})).resolves.toBeUndefined();
  });
});
