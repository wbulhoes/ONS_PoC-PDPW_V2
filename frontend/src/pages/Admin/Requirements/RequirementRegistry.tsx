import React, { useState, useEffect } from 'react';
import styles from './RequirementRegistry.module.css';

interface Requirement {
  id: number;
  titulo: string;
  descricao: string;
  tipo: string;
  prioridade: string;
  status: string;
  dataCriacao: string;
  responsavel: string;
}

const RequirementRegistry: React.FC = () => {
  const [requirements, setRequirements] = useState<Requirement[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedRequirements, setSelectedRequirements] = useState<number[]>([]);
  const [isEditing, setIsEditing] = useState(false);
  const [editingRequirement, setEditingRequirement] = useState<Requirement | null>(null);
  const [formData, setFormData] = useState({
    titulo: '',
    descricao: '',
    tipo: 'Funcional',
    prioridade: 'Média',
    status: 'Pendente',
    responsavel: '',
  });
  const [isLoading, setIsLoading] = useState(false);

  const itemsPerPage = 5;

  const tipos = ['Funcional', 'Não Funcional', 'Técnico', 'Regulatório'];
  const prioridades = ['Baixa', 'Média', 'Alta', 'Crítica'];
  const statusOptions = ['Pendente', 'Em Análise', 'Aprovado', 'Reprovado', 'Implementado'];

  // Mock data - será substituído pela API
  useEffect(() => {
    const mockRequirements: Requirement[] = [
      {
        id: 1,
        titulo: 'Implementar validação de dados hidráulicos',
        descricao: 'Sistema deve validar dados de vazão e volume antes do processamento',
        tipo: 'Funcional',
        prioridade: 'Alta',
        status: 'Em Análise',
        dataCriacao: '2023-01-15',
        responsavel: 'João Silva',
      },
      {
        id: 2,
        titulo: 'Otimizar performance de consultas térmicas',
        descricao: 'Melhorar tempo de resposta das consultas de dados térmicos',
        tipo: 'Técnico',
        prioridade: 'Média',
        status: 'Pendente',
        dataCriacao: '2023-02-20',
        responsavel: 'Maria Santos',
      },
      {
        id: 3,
        titulo: 'Implementar auditoria de alterações',
        descricao: 'Registrar todas as alterações nos dados do sistema',
        tipo: 'Não Funcional',
        prioridade: 'Média',
        status: 'Aprovado',
        dataCriacao: '2023-03-10',
        responsavel: 'Pedro Costa',
      },
      {
        id: 4,
        titulo: 'Atualizar conformidade com novas regulamentações',
        descricao: 'Adequar sistema às novas regras do setor elétrico',
        tipo: 'Regulatório',
        prioridade: 'Crítica',
        status: 'Em Análise',
        dataCriacao: '2023-04-05',
        responsavel: 'Ana Oliveira',
      },
      {
        id: 5,
        titulo: 'Melhorar interface de usuário',
        descricao: 'Redesenhar telas para melhor usabilidade',
        tipo: 'Funcional',
        prioridade: 'Baixa',
        status: 'Pendente',
        dataCriacao: '2023-05-12',
        responsavel: 'Carlos Mendes',
      },
      {
        id: 6,
        titulo: 'Implementar backup automático',
        descricao: 'Sistema de backup diário dos dados críticos',
        tipo: 'Técnico',
        prioridade: 'Alta',
        status: 'Implementado',
        dataCriacao: '2023-06-18',
        responsavel: 'Fernanda Lima',
      },
    ];
    setRequirements(mockRequirements);
  }, []);

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      if (isEditing && editingRequirement) {
        // Atualizar requisito existente
        const updatedRequirements = requirements.map((req) =>
          req.id === editingRequirement.id
            ? { ...req, ...formData, dataCriacao: req.dataCriacao }
            : req
        );
        setRequirements(updatedRequirements);
        setIsEditing(false);
        setEditingRequirement(null);
      } else {
        // Criar novo requisito
        const newRequirement: Requirement = {
          id: Math.max(...requirements.map((r) => r.id)) + 1,
          ...formData,
          dataCriacao: new Date().toISOString().split('T')[0],
        };
        setRequirements([...requirements, newRequirement]);
      }

      // Limpar formulário
      setFormData({
        titulo: '',
        descricao: '',
        tipo: 'Funcional',
        prioridade: 'Média',
        status: 'Pendente',
        responsavel: '',
      });
    } catch (error) {
      console.error('Erro ao salvar requisito:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleEdit = (requirement: Requirement) => {
    setIsEditing(true);
    setEditingRequirement(requirement);
    setFormData({
      titulo: requirement.titulo,
      descricao: requirement.descricao,
      tipo: requirement.tipo,
      prioridade: requirement.prioridade,
      status: requirement.status,
      responsavel: requirement.responsavel,
    });
  };

  const handleDelete = async () => {
    if (selectedRequirements.length === 0) return;

    setIsLoading(true);
    try {
      const updatedRequirements = requirements.filter(
        (req) => !selectedRequirements.includes(req.id)
      );
      setRequirements(updatedRequirements);
      setSelectedRequirements([]);
    } catch (error) {
      console.error('Erro ao excluir requisitos:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCancel = () => {
    setIsEditing(false);
    setEditingRequirement(null);
    setFormData({
      titulo: '',
      descricao: '',
      tipo: 'Funcional',
      prioridade: 'Média',
      status: 'Pendente',
      responsavel: '',
    });
  };

  const handleSelectRequirement = (requirementId: number) => {
    setSelectedRequirements((prev) =>
      prev.includes(requirementId)
        ? prev.filter((id) => id !== requirementId)
        : [...prev, requirementId]
    );
  };

  const handleSelectAll = () => {
    const currentRequirements = paginatedRequirements;
    if (selectedRequirements.length === currentRequirements.length) {
      setSelectedRequirements([]);
    } else {
      setSelectedRequirements(currentRequirements.map((req) => req.id));
    }
  };

  const paginatedRequirements = requirements.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  const totalPages = Math.ceil(requirements.length / itemsPerPage);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    setSelectedRequirements([]); // Limpar seleção ao mudar página
  };

  const getPriorityColor = (prioridade: string) => {
    switch (prioridade) {
      case 'Crítica':
        return '#dc3545';
      case 'Alta':
        return '#fd7e14';
      case 'Média':
        return '#ffc107';
      case 'Baixa':
        return '#28a745';
      default:
        return '#6c757d';
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Aprovado':
        return '#28a745';
      case 'Em Análise':
        return '#ffc107';
      case 'Implementado':
        return '#007bff';
      case 'Reprovado':
        return '#dc3545';
      case 'Pendente':
        return '#6c757d';
      default:
        return '#6c757d';
    }
  };

  return (
    <div className={styles.container} data-testid="requirement-registry-container">
      <div className={styles.header}>
        <h1 className={styles.title}>Cadastro de Requisitos</h1>
      </div>

      <div className={styles.content}>
        <form className={styles.form} onSubmit={handleSubmit}>
          <div className={styles.formRow}>
            <div className={styles.formGroup}>
              <label className={styles.label} htmlFor="titulo">
                Título:
              </label>
              <input
                type="text"
                id="titulo"
                name="titulo"
                value={formData.titulo}
                onChange={handleInputChange}
                className={styles.input}
                required
                maxLength={200}
              />
            </div>

            <div className={styles.formGroup}>
              <label className={styles.label} htmlFor="tipo">
                Tipo:
              </label>
              <select
                id="tipo"
                name="tipo"
                value={formData.tipo}
                onChange={handleInputChange}
                className={styles.select}
              >
                {tipos.map((tipo) => (
                  <option key={tipo} value={tipo}>
                    {tipo}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className={styles.formRow}>
            <div className={styles.formGroup}>
              <label className={styles.label} htmlFor="prioridade">
                Prioridade:
              </label>
              <select
                id="prioridade"
                name="prioridade"
                value={formData.prioridade}
                onChange={handleInputChange}
                className={styles.select}
              >
                {prioridades.map((prioridade) => (
                  <option key={prioridade} value={prioridade}>
                    {prioridade}
                  </option>
                ))}
              </select>
            </div>

            <div className={styles.formGroup}>
              <label className={styles.label} htmlFor="status">
                Status:
              </label>
              <select
                id="status"
                name="status"
                value={formData.status}
                onChange={handleInputChange}
                className={styles.select}
              >
                {statusOptions.map((status) => (
                  <option key={status} value={status}>
                    {status}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className={styles.formGroup}>
            <label className={styles.label} htmlFor="responsavel">
              Responsável:
            </label>
            <input
              type="text"
              id="responsavel"
              name="responsavel"
              value={formData.responsavel}
              onChange={handleInputChange}
              className={styles.input}
              maxLength={100}
            />
          </div>

          <div className={styles.formGroup}>
            <label className={styles.label} htmlFor="descricao">
              Descrição:
            </label>
            <textarea
              id="descricao"
              name="descricao"
              value={formData.descricao}
              onChange={handleInputChange}
              className={styles.textarea}
              rows={4}
              maxLength={1000}
            />
          </div>

          <div className={styles.formActions}>
            <button type="submit" className={styles.button} disabled={isLoading}>
              {isLoading ? 'Salvando...' : isEditing ? 'Atualizar' : 'Salvar'}
            </button>
            {isEditing && (
              <button type="button" onClick={handleCancel} className={styles.buttonSecondary}>
                Cancelar
              </button>
            )}
          </div>
        </form>

        <div className={styles.tableContainer}>
          {isLoading && requirements.length === 0 ? (
            <div className={styles.loading}>Carregando requisitos...</div>
          ) : (
            <>
              <table className={styles.table}>
                <thead>
                  <tr>
                    <th className={styles.checkboxColumn}>
                      <input
                        type="checkbox"
                        checked={
                          selectedRequirements.length === paginatedRequirements.length &&
                          paginatedRequirements.length > 0
                        }
                        onChange={handleSelectAll}
                        data-testid="select-all-checkbox"
                      />
                    </th>
                    <th>Título</th>
                    <th>Tipo</th>
                    <th>Prioridade</th>
                    <th>Status</th>
                    <th>Responsável</th>
                    <th>Data Criação</th>
                    <th>Ações</th>
                  </tr>
                </thead>
                <tbody>
                  {paginatedRequirements.map((requirement) => (
                    <tr
                      key={requirement.id}
                      className={
                        selectedRequirements.includes(requirement.id) ? styles.selectedRow : ''
                      }
                    >
                      <td className={styles.checkboxColumn}>
                        <input
                          type="checkbox"
                          checked={selectedRequirements.includes(requirement.id)}
                          onChange={() => handleSelectRequirement(requirement.id)}
                        />
                      </td>
                      <td className={styles.titleCell}>{requirement.titulo}</td>
                      <td>{requirement.tipo}</td>
                      <td>
                        <span
                          className={styles.priorityBadge}
                          style={{ backgroundColor: getPriorityColor(requirement.prioridade) }}
                        >
                          {requirement.prioridade}
                        </span>
                      </td>
                      <td>
                        <span
                          className={styles.statusBadge}
                          style={{ backgroundColor: getStatusColor(requirement.status) }}
                        >
                          {requirement.status}
                        </span>
                      </td>
                      <td>{requirement.responsavel}</td>
                      <td>{new Date(requirement.dataCriacao).toLocaleDateString('pt-BR')}</td>
                      <td>
                        <button
                          onClick={() => handleEdit(requirement)}
                          className={styles.actionButton}
                          disabled={isLoading}
                        >
                          Editar
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>

              {requirements.length === 0 && !isLoading && (
                <div className={styles.emptyState}>Nenhum requisito cadastrado.</div>
              )}

              {totalPages > 1 && (
                <div className={styles.pagination}>
                  <button
                    onClick={() => handlePageChange(currentPage - 1)}
                    disabled={currentPage === 1}
                    className={styles.pageButton}
                  >
                    Anterior
                  </button>
                  <span>
                    Página {currentPage} de {totalPages}
                  </span>
                  <button
                    onClick={() => handlePageChange(currentPage + 1)}
                    disabled={currentPage === totalPages}
                    className={styles.pageButton}
                  >
                    Próxima
                  </button>
                </div>
              )}

              <div className={styles.actions}>
                <button
                  onClick={handleDelete}
                  disabled={selectedRequirements.length === 0 || isLoading}
                  className={styles.deleteButton}
                >
                  Excluir Selecionados ({selectedRequirements.length})
                </button>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default RequirementRegistry;
