import { describe, it, expect } from 'vitest';
import { getUserFriendlyMessage } from '../../src/utils/errorMessages';

describe('errorMessages Snapshots', () => {
  it('should match snapshot for 400 validation error', () => {
    const message = getUserFriendlyMessage(400, {
      erros: [
        { campo: 'usuar_email', mensagem: 'Email inválido' },
        { campo: 'usuar_telefone', mensagem: 'Telefone obrigatório' },
      ],
    });
    expect(message).toMatchSnapshot();
  });

  it('should match snapshot for 401 unauthorized', () => {
    const message = getUserFriendlyMessage(401, {});
    expect(message).toMatchSnapshot();
  });

  it('should match snapshot for 403 forbidden', () => {
    const message = getUserFriendlyMessage(403, {});
    expect(message).toMatchSnapshot();
  });

  it('should match snapshot for 404 not found', () => {
    const message = getUserFriendlyMessage(404, {});
    expect(message).toMatchSnapshot();
  });

  it('should match snapshot for 409 conflict', () => {
    const message = getUserFriendlyMessage(409, {});
    expect(message).toMatchSnapshot();
  });

  it('should match snapshot for 500 server error', () => {
    const message = getUserFriendlyMessage(500, {});
    expect(message).toMatchSnapshot();
  });

  it('should match snapshot for 502 bad gateway', () => {
    const message = getUserFriendlyMessage(502, {});
    expect(message).toMatchSnapshot();
  });

  it('should match snapshot for 503 service unavailable', () => {
    const message = getUserFriendlyMessage(503, {});
    expect(message).toMatchSnapshot();
  });

  it('should match snapshot for unknown error status', () => {
    const message = getUserFriendlyMessage(599, {});
    expect(message).toMatchSnapshot();
  });

  it('should match snapshot for network error', () => {
    const message = getUserFriendlyMessage(0, { message: 'Network Error' });
    expect(message).toMatchSnapshot();
  });

  it('should match snapshot for all status codes', () => {
    const results: Record<number, string> = {};
    const statuses = [400, 401, 403, 404, 409, 500, 502, 503];

    statuses.forEach(status => {
      results[status] = getUserFriendlyMessage(status, {});
    });

    expect(results).toMatchSnapshot();
  });
});
