import { describe, it, expect } from 'vitest';
import { programacaoService } from '../../src/services/programacaoService';

describe('programacaoService', () => {
  it('getProgramacao returns expected shape', async () => {
    const res = await programacaoService.getProgramacao('20251225', 'EMP001');
    expect(res).toHaveProperty('dataPdp', '20251225');
    expect(res).toHaveProperty('codEmpresa', 'EMP001');
    expect(Array.isArray(res.usinas)).toBe(true);
    expect(res.usinas.length).toBeGreaterThan(0);
  });

  it('saveProgramacao resolves without error', async () => {
    await expect(programacaoService.saveProgramacao({ dataPdp: '20251225', codEmpresa: 'EMP001', usinas: [] })).resolves.toBeUndefined();
  });
});
