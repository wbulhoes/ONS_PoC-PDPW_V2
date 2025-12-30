import { describe, it, expect } from 'vitest';
import { insumosService } from '../../src/services/insumosService';

describe('insumosService', () => {
  it('getInsumos returns data for date', async () => {
    const res = await insumosService.getInsumos('20251225');
    expect(res).toHaveProperty('dataPdp', '20251225');
    expect(Array.isArray(res.insumos)).toBe(true);
  });

  it('saveInsumos resolves', async () => {
    await expect(insumosService.saveInsumos({})).resolves.toBeUndefined();
  });
});
