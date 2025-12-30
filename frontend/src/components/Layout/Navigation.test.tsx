import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Navigation from './Navigation';
import { MenuItem } from '../../types/menu';

const mockMenuItems: MenuItem[] = [
  {
    Title: 'Coleta',
    Description: 'Coleta',
    Url: '',
    CodAplication: 'PDPW',
    NodeOrder: 1,
    Published: true,
    Enabled: true,
    UrlHelp: '',
    PadraoBrowser: 'IE=Edge,chrome=1',
    FlgPublico: false,
    ClasseCSS: null,
    Childs: [
      {
        Title: 'Dados Hidráulicos',
        Description: 'Dados Hidráulicos',
        Url: '/frmColDadosHidraulicos.aspx',
        CodAplication: 'PDPW',
        NodeOrder: 1,
        Published: true,
        Enabled: true,
        UrlHelp: '',
        PadraoBrowser: 'IE=Edge,chrome=1',
        FlgPublico: false,
        ClasseCSS: null,
        Childs: [],
        Definicoes: ['ATUPDP'],
        UrlIcone: null,
        TipoRequisicao: 'POP',
        TipoSitemap: 'Tradicional',
      },
    ],
    Definicoes: [],
    UrlIcone: null,
    TipoRequisicao: 'POP',
    TipoSitemap: 'Tradicional',
  },
  {
    Title: 'Consultas',
    Description: 'Consultas',
    Url: '/consultas',
    CodAplication: 'PDPW',
    NodeOrder: 2,
    Published: true,
    Enabled: true,
    UrlHelp: '',
    PadraoBrowser: 'IE=Edge,chrome=1',
    FlgPublico: false,
    ClasseCSS: null,
    Childs: [],
    Definicoes: [],
    UrlIcone: null,
    TipoRequisicao: 'POP',
    TipoSitemap: 'Tradicional',
  },
];

describe('Navigation Component', () => {
  it('should render navigation menu items', () => {
    render(<Navigation menuItems={mockMenuItems} />);
    expect(screen.getByText('Coleta')).toBeInTheDocument();
    expect(screen.getByText('Consultas')).toBeInTheDocument();
  });

  it('should render mobile menu toggle button', () => {
    render(<Navigation menuItems={mockMenuItems} />);
    const toggleButton = screen.getByLabelText('Toggle menu');
    expect(toggleButton).toBeInTheDocument();
  });

  it('should toggle mobile menu when button is clicked', async () => {
    const user = userEvent.setup();
    render(<Navigation menuItems={mockMenuItems} />);

    const toggleButton = screen.getByLabelText('Toggle menu');
    await user.click(toggleButton);
  });

  it('should not render disabled menu items', () => {
    const disabledItems: MenuItem[] = [
      {
        ...mockMenuItems[0],
        Enabled: false,
      },
    ];

    render(<Navigation menuItems={disabledItems} />);
    expect(screen.queryByText('Coleta')).not.toBeInTheDocument();
  });

  it('should not render unpublished menu items', () => {
    const unpublishedItems: MenuItem[] = [
      {
        ...mockMenuItems[0],
        Published: false,
      },
    ];

    render(<Navigation menuItems={unpublishedItems} />);
    expect(screen.queryByText('Coleta')).not.toBeInTheDocument();
  });
});
