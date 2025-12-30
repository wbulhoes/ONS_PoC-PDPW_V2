export interface MenuItem {
  Title: string;
  Description: string;
  Url: string;
  CodAplication: string;
  NodeOrder: number;
  Published: boolean;
  Enabled: boolean;
  UrlHelp: string;
  PadraoBrowser: string;
  FlgPublico: boolean;
  ClasseCSS: string | null;
  Childs: MenuItem[];
  Definicoes: string[];
  UrlIcone: string | null;
  TipoRequisicao: string;
  TipoSitemap: string;
}
