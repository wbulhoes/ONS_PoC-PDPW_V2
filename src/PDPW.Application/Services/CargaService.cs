using PDPW.Application.DTOs.Carga;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Carga
/// </summary>
public class CargaService : ICargaService
{
    private readonly ICargaRepository _repository;

    public CargaService(ICargaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CargaDto>> GetAllAsync()
    {
        var cargas = await _repository.GetAllAsync();
        return cargas.Select(MapToDto);
    }

    public async Task<CargaDto?> GetByIdAsync(int id)
    {
        var carga = await _repository.GetByIdAsync(id);
        return carga != null ? MapToDto(carga) : null;
    }

    public async Task<CargaDto> CreateAsync(CreateCargaDto dto)
    {
        // Validações conforme CargaDAO.vb
        if (dto.DataReferencia == default)
        {
            throw new ArgumentException("Data de referência não informada");
        }

        if (string.IsNullOrWhiteSpace(dto.SubsistemaId))
        {
            throw new ArgumentException("Subsistema não informado");
        }

        if (dto.CargaMWmed < 0)
        {
            throw new ArgumentException("Carga MW média não pode ser negativa");
        }

        var carga = new Carga
        {
            DataReferencia = dto.DataReferencia,
            SubsistemaId = dto.SubsistemaId,
            CargaMWmed = dto.CargaMWmed,
            CargaVerificada = dto.CargaVerificada,
            PrevisaoCarga = dto.PrevisaoCarga,
            Observacoes = dto.Observacoes,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        var created = await _repository.AddAsync(carga);
        return MapToDto(created);
    }

    public async Task<CargaDto> UpdateAsync(int id, UpdateCargaDto dto)
    {
        // Validações conforme CargaDAO.vb
        if (dto.DataReferencia == default)
        {
            throw new ArgumentException("Data de referência não informada");
        }

        if (string.IsNullOrWhiteSpace(dto.SubsistemaId))
        {
            throw new ArgumentException("Subsistema não informado");
        }

        if (dto.CargaMWmed < 0)
        {
            throw new ArgumentException("Carga MW média não pode ser negativa");
        }

        var carga = await _repository.GetByIdAsync(id);
        if (carga == null)
            throw new KeyNotFoundException($"Carga com ID {id} não encontrada");

        carga.DataReferencia = dto.DataReferencia;
        carga.SubsistemaId = dto.SubsistemaId;
        carga.CargaMWmed = dto.CargaMWmed;
        carga.CargaVerificada = dto.CargaVerificada;
        carga.PrevisaoCarga = dto.PrevisaoCarga;
        carga.Observacoes = dto.Observacoes;
        carga.DataAtualizacao = DateTime.UtcNow;

        await _repository.UpdateAsync(carga);
        return MapToDto(carga);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var carga = await _repository.GetByIdAsync(id);
        if (carga == null)
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<CargaDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        var cargas = await _repository.GetByPeriodoAsync(dataInicio, dataFim);
        return cargas.Select(MapToDto);
    }

    public async Task<IEnumerable<CargaDto>> GetBySubsistemaAsync(string subsistemaId)
    {
        var cargas = await _repository.GetBySubsistemaAsync(subsistemaId);
        return cargas.Select(MapToDto);
    }

    public async Task<IEnumerable<CargaDto>> GetByDataReferenciaAsync(DateTime dataReferencia)
    {
        // Validação conforme CargaDAO.vb
        if (dataReferencia == default)
        {
            throw new ArgumentException("Data de referência não informada");
        }

        var cargas = await _repository.GetByDataReferenciaAsync(dataReferencia);
        return cargas.Select(MapToDto);
    }

    private static CargaDto MapToDto(Carga carga)
    {
        return new CargaDto
        {
            Id = carga.Id,
            DataReferencia = carga.DataReferencia,
            SubsistemaId = carga.SubsistemaId,
            CargaMWmed = carga.CargaMWmed,
            CargaVerificada = carga.CargaVerificada,
            PrevisaoCarga = carga.PrevisaoCarga,
            Observacoes = carga.Observacoes,
            Ativo = carga.Ativo,
            DataCriacao = carga.DataCriacao
        };
    }
}
