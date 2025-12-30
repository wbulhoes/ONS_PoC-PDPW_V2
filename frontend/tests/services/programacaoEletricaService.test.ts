import { describe, it, expect } from 'vitest';
import { programacaoEletricaService } from '../../src/services/programacaoEletricaService';

describe('programacaoEletricaService', () => {
  it('getProgramacaoEletrica returns items array', async () => {
    const res = await programacaoEletricaService.getProgramacaoEletrica('20251225');
    expect(res).toHaveProperty('dataPdp', '20251225');
    expect(Array.isArray(res.itens)).toBe(true);
  });

  it('saveProgramacaoEletrica resolves', async () => {
    await expect(programacaoEletricaService.saveProgramacaoEletrica({})).resolves.toBeUndefined();
  });
});
