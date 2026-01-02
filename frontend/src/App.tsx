import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import Home from './pages/Home/Home';
import Example from './pages/Example/Example';
import Splash from './pages/Auth/Splash';
import IntegrationAuth from './pages/Auth/IntegrationAuth';
import Flow from './pages/Collection/Hydraulic/Flow';
import Availability from './pages/Collection/Hydraulic/Availability';
import Balance from './pages/Collection/Hydraulic/Balance';
import Generation from './pages/Collection/Thermal/Generation';
import Inflexibility from './pages/Collection/Thermal/Inflexibility';
import OperatingMode from './pages/Collection/Thermal/OperatingMode';
import InflexibilityDispatch from './pages/Collection/Thermal/InflexibilityDispatch';
import ExportOffer from './pages/Collection/Thermal/ExportOffer';
import ExportOfferAnalysis from './pages/Collection/Thermal/ExportOfferAnalysis';
import RRO from './pages/Collection/Thermal/RRO';
import WeeklyDispatch from './pages/Collection/Thermal/WeeklyDispatch';
import FuelShortageRestriction from './pages/Collection/Thermal/FuelShortageRestriction';
import Load from './pages/Collection/Load/Load';
import Consumption from './pages/Collection/Load/Consumption';
import UnitRestriction from './pages/Collection/Restrictions/UnitRestriction';
import Energy from './pages/Collection/Electrical/Energy';
import ProgramacaoEnergeticaPage from './pages/Collection/Electrical/ProgramacaoEnergetica';
import ProgramacaoEletrica from './pages/Collection/Electrical/ProgramacaoEletrica';
import PrevisaoEolica from './pages/Collection/Electrical/PrevisaoEolica';
import GenerateModelFiles from './pages/ModelFiles/GenerateModelFiles';
import FinalizacaoProgramacao from './pages/Finalization/FinalizacaoProgramacao';
import GEC from './pages/Collection/Other/GEC';
import ReplacementEnergyPage from './pages/Collection/Other/PlantConverter';
import PlantConverterPage from './pages/Collection/Other/PlantConverter';
import Company from './pages/Administration/Company';
import UserManagementPage from './pages/Admin/User/UserManagement';
import UserAssociation from './pages/Administration/UserAssociation';
import PlantRegistry from './pages/Administration/PlantRegistry';
import ElectricalDispatchReasonPage from './pages/Administration/ElectricalDispatchReasonPage';
import ProgrammingMilestoneQuery from './pages/Query/Other/ProgrammingMilestoneQuery';
import RROQuery from './pages/Query/Other/RROQuery';
import Insumos from './pages/Collection/Insumos/Insumos';
import InflexibilityDispatchReasonPage from './pages/Administration/InflexibilityDispatchReasonPage';
import ContractedInflexibility from './pages/Administration/ContractedInflexibility';
import Comments from './pages/Query/DESSEM/Comments';
import ObservationQuery from './pages/Query/Other/ObservationQuery';
import AvailabilityQuery from './pages/Query/Hydraulic/AvailabilityQuery';
import CompanyManagement from './pages/Admin/Company/CompanyManagement';
import PlantManagement from './pages/Admin/Plant/PlantManagement';
import OfertasExportacaoManagement from './pages/Exportacao/OfertasExportacaoManagement';
import OfertasRVManagement from './pages/Exportacao/OfertasRVManagement';
import EnergiaVertidaManagement from './pages/EnergiaVertida/EnergiaVertidaManagement';

// Novas consultas
import CargaQuery from './pages/Query/Load/CargaQuery';
import GeracaoQuery from './pages/Query/Generation/GeracaoQuery';
import VazaoQuery from './pages/Query/Hydraulic/VazaoQuery';
import InflexibilidadeQuery from './pages/Query/Thermal/InflexibilidadeQuery';
import DisponibilidadeQuery from './pages/Query/Thermal/DisponibilidadeQuery';

// Novas consultas hidráulicas
import MaquinasParadasQuery from './pages/Query/Hydraulic/MaquinasParadasQuery';
import MaquinasOperandoQuery from './pages/Query/Hydraulic/MaquinasOperandoQuery';
import MaquinasGerandoQuery from './pages/Query/Hydraulic/MaquinasGerandoQuery';
import ParadaUGQuery from './pages/Query/Hydraulic/ParadaUGQuery';

// Novas consultas térmicas
import RazaoEnergeticaQuery from './pages/Query/Thermal/RazaoEnergeticaQuery';
import RazaoEletricaQuery from './pages/Query/Thermal/RazaoEletricaQuery';
import ExportacaoQuery from './pages/Query/Thermal/ExportacaoQuery';
import ImportacaoQuery from './pages/Query/Thermal/ImportacaoQuery';
import ConsumoQuery from './pages/Query/Thermal/ConsumoQuery';
import UnitCommitmentQuery from './pages/Query/Thermal/UnitCommitmentQuery';
import MotivoDespachoREQuery from './pages/Query/Thermal/MotivoDespachoREQuery';
import CompensacaoLastroQuery from './pages/Query/Thermal/CompensacaoLastroQuery';
import RestricaoCombustivelQuery from './pages/Query/Thermal/RestricaoCombustivelQuery';
import GarantiaEnergeticaQuery from './pages/Query/Thermal/GarantiaEnergeticaQuery';

// Novas consultas outras
import CreditoForaMeritoQuery from './pages/Query/Other/CreditoForaMeritoQuery';
import SuprimentoOrdemMeritoQuery from './pages/Query/Other/SuprimentoOrdemMeritoQuery';
import GECreditoQuery from './pages/Query/Other/GECreditoQuery';
import GESubstituicaoQuery from './pages/Query/Other/GESubstituicaoQuery';
import EnvioDadosEmpresaQuery from './pages/Query/Other/EnvioDadosEmpresaQuery';

// Novas páginas - Ferramentas
import UploadArquivos from './pages/Tools/UploadArquivos';
import DownloadArquivos from './pages/Tools/DownloadArquivos';
import VisualizacaoRecibos from './pages/Tools/VisualizacaoRecibos';
import RecuperarDadosDiaAnterior from './pages/Tools/RecuperarDadosDiaAnterior';

// Novas páginas - Coleta/Demanda
import ProgramacaoSemanal from './pages/Collection/Demand/ProgramacaoSemanal';
import ProgramacaoDiaria from './pages/Collection/Demand/ProgramacaoDiaria';
import RelatorioOfertaReducao from './pages/Collection/Demand/RelatorioOfertaReducao';

import './styles/global.css';

function App() {
  return (
    <BrowserRouter>
      <Layout userName="Usuário Teste">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/exemplo" element={<Example />} />
          <Route path="/splash" element={<Splash />} />
          
          {/* Coleta - Hidráulico */}
          <Route path="/coleta/hidraulico/vazao" element={<Flow />} />
          <Route path="/coleta/hidraulico/disponibilidade" element={<Availability initialType="H" />} />
          <Route path="/coleta/hidraulico/balanco" element={<Balance />} />
          
          {/* Coleta - Térmico */}
          <Route path="/coleta/termico/geracao" element={<Generation />} />
          <Route path="/coleta/termico/disponibilidade" element={<Availability initialType="T" />} />
          <Route path="/frmColDisponibilidade.aspx" element={<Availability initialType="T" />} />
          <Route path="/coleta/termico/inflexibilidade" element={<Inflexibility />} />
          <Route path="/coleta/termico/modalidade-operativa" element={<OperatingMode />} />
          <Route path="/coleta/termico/despacho-inflexibilidade" element={<InflexibilityDispatch />} />
          <Route path="/coleta/termico/oferta-exportacao" element={<ExportOffer />} />
          <Route path="/frmCnsOfertaExportacao.aspx" element={<ExportOffer />} />
          <Route path="/coleta/termico/analise-oferta-exportacao" element={<ExportOfferAnalysis />} />
          <Route path="/frmCnsAnaliseOfertaExportacao.aspx" element={<ExportOfferAnalysis />} />
          <Route path="/coleta/termico/rro" element={<RRO />} />
          <Route path="/frmColRRO.aspx" element={<RRO />} />
          <Route path="/coleta/termico/oferta-semanal" element={<WeeklyDispatch />} />
          <Route path="/frmColOfertaSemanalDespComp.aspx" element={<WeeklyDispatch />} />
          <Route path="/coleta/termico/restricao-combustivel" element={<FuelShortageRestriction />} />
          <Route path="/frmColResFaltaComb.aspx" element={<FuelShortageRestriction />} />
          
          {/* Coleta - Carga */}
          <Route path="/coleta/carga/carga" element={<Load />} />
          <Route path="/coleta/carga/consumo" element={<Consumption />} />
          
          {/* Coleta - Elétrica */}
          <Route path="/coleta/eletrica/energia" element={<Energy />} />
          <Route path="/frmColEnergetica.aspx" element={<Energy />} />
          <Route path="/coleta/eletrica/programacao" element={<ProgramacaoEnergeticaPage />} />
          <Route path="/frmColProgramacaoEnergetica.aspx" element={<ProgramacaoEnergeticaPage />} />
          <Route path="/coleta/eletrica/programacao-eletrica" element={<ProgramacaoEletrica />} />
          <Route path="/frmColProgramacaoEletrica.aspx" element={<ProgramacaoEletrica />} />
          <Route path="/coleta/eletrica/previsao-eolica" element={<PrevisaoEolica />} />
          <Route path="/frmColPrevisaoEolica.aspx" element={<PrevisaoEolica />} />
          
          {/* Coleta - Outros */}
          <Route path="/coleta/restricoes/restricao-ug" element={<UnitRestriction />} />
          <Route path="/coleta/outros/gec" element={<GEC />} />
          <Route path="/coleta/outros/energia-reposicao" element={<ReplacementEnergyPage />} />
          <Route path="/frmColEnergiaRepPer.aspx" element={<ReplacementEnergyPage />} />
          <Route path="/coleta/outros/usina-conversora" element={<PlantConverterPage />} />
          <Route path="/frmUsinaConversora.aspx" element={<PlantConverterPage />} />
          <Route path="/coleta/insumos" element={<Insumos />} />
          <Route path="/frmColInsumos.aspx" element={<Insumos />} />
          
          {/* Consultas - NOVAS */}
          <Route path="/consulta/carga" element={<CargaQuery />} />
          <Route path="/frmCnsCarga.aspx" element={<CargaQuery />} />
          <Route path="/consulta/geracao" element={<GeracaoQuery />} />
          <Route path="/frmCnsGeracao.aspx" element={<GeracaoQuery />} />
          <Route path="/consulta/vazao" element={<VazaoQuery />} />
          <Route path="/frmCnsVazao.aspx" element={<VazaoQuery />} />
          <Route path="/consulta/inflexibilidade" element={<InflexibilidadeQuery />} />
          <Route path="/frmCnsInflexibilidade.aspx" element={<InflexibilidadeQuery />} />
          <Route path="/consulta/disponibilidade" element={<DisponibilidadeQuery />} />
          <Route path="/frmCnsDisponibilidade.aspx" element={<DisponibilidadeQuery />} />
          
          {/* Consultas Hidráulicas - NOVAS */}
          <Route path="/consulta/maquinas-paradas" element={<MaquinasParadasQuery />} />
          <Route path="/frmCnsMaqParada.aspx" element={<MaquinasParadasQuery />} />
          <Route path="/consulta/maquinas-operando" element={<MaquinasOperandoQuery />} />
          <Route path="/frmCnsMaqOperando.aspx" element={<MaquinasOperandoQuery />} />
          <Route path="/consulta/maquinas-gerando" element={<MaquinasGerandoQuery />} />
          <Route path="/frmCnsMaqGerando.aspx" element={<MaquinasGerandoQuery />} />
          <Route path="/consulta/parada-ug" element={<ParadaUGQuery />} />
          <Route path="/frmCnsParadaUG.aspx" element={<ParadaUGQuery />} />
          
          {/* Consultas Térmicas - NOVAS */}
          <Route path="/consulta/razao-energetica" element={<RazaoEnergeticaQuery />} />
          <Route path="/frmCnsEnergetica.aspx" element={<RazaoEnergeticaQuery />} />
          <Route path="/consulta/razao-eletrica" element={<RazaoEletricaQuery />} />
          <Route path="/frmCnsEletrica.aspx" element={<RazaoEletricaQuery />} />
          <Route path="/consulta/exportacao" element={<ExportacaoQuery />} />
          <Route path="/frmCnsExportacao.aspx" element={<ExportacaoQuery />} />
          <Route path="/consulta/importacao" element={<ImportacaoQuery />} />
          <Route path="/frmCnsImportacao.aspx" element={<ImportacaoQuery />} />
          <Route path="/consulta/consumo" element={<ConsumoQuery />} />
          <Route path="/frmCnsConsumo.aspx" element={<ConsumoQuery />} />
          <Route path="/consulta/unit-commitment" element={<UnitCommitmentQuery />} />
          <Route path="/frmCnsDespInflex.aspx" element={<UnitCommitmentQuery />} />
          <Route path="/consulta/motivo-despacho-re" element={<MotivoDespachoREQuery />} />
          <Route path="/frmCnsDespRE.aspx" element={<MotivoDespachoREQuery />} />
          <Route path="/consulta/compensacao-lastro" element={<CompensacaoLastroQuery />} />
          <Route path="/frmCnsCompensacao.aspx" element={<CompensacaoLastroQuery />} />
          <Route path="/consulta/restricao-combustivel" element={<RestricaoCombustivelQuery />} />
          <Route path="/frmCnsResFaltaComb.aspx" element={<RestricaoCombustivelQuery />} />
          <Route path="/consulta/garantia-energetica" element={<GarantiaEnergeticaQuery />} />
          <Route path="/frmCnsRampa.aspx" element={<GarantiaEnergeticaQuery />} />
          
          {/* Consultas Outras - NOVAS */}
          <Route path="/consulta/credito-fora-merito" element={<CreditoForaMeritoQuery />} />
          <Route path="/frmCnsCreForaMerito.aspx" element={<CreditoForaMeritoQuery />} />
          <Route path="/consulta/suprimento-ordem-merito" element={<SuprimentoOrdemMeritoQuery />} />
          <Route path="/frmCnsSom.aspx" element={<SuprimentoOrdemMeritoQuery />} />
          <Route path="/consulta/ge-credito" element={<GECreditoQuery />} />
          <Route path="/frmCnsGEC.aspx" element={<GECreditoQuery />} />
          <Route path="/consulta/ge-substituicao" element={<GESubstituicaoQuery />} />
          <Route path="/frmCnsGES.aspx" element={<GESubstituicaoQuery />} />
          <Route path="/consulta/envio-dados-empresa" element={<EnvioDadosEmpresaQuery />} />
          <Route path="/frmCnsEnvioEmp.aspx" element={<EnvioDadosEmpresaQuery />} />
          
          {/* Consultas - Existentes */}
          <Route path="/consulta/dessem/comentarios" element={<Comments />} />
          <Route path="/frmCnsObservacoes.aspx" element={<Comments />} />
          <Route path="/consulta/outros/observacao" element={<ObservationQuery />} />
          <Route path="/frmCnsObservacao.aspx" element={<ObservationQuery />} />
          <Route path="/consulta/hidraulico/disponibilidade" element={<AvailabilityQuery />} />
          <Route path="/consulta/outros/rro" element={<RROQuery />} />
          <Route path="/frmCnsRRO.aspx" element={<RROQuery />} />
          <Route path="/consulta/outros/marcos-programacao" element={<ProgrammingMilestoneQuery />} />
          <Route path="/frmConsultaMarcoProgramacao.aspx" element={<ProgrammingMilestoneQuery />} />
          <Route path="/frmCnsEnergiaRepPer.aspx" element={<ReplacementEnergyPage />} />
          
          {/* Ferramentas */}
          <Route path="/gerar/arquivos-modelos" element={<GenerateModelFiles />} />
          <Route path="/frmGerArquivo.aspx" element={<GenerateModelFiles />} />
          <Route path="/finalizacao/programacao" element={<FinalizacaoProgramacao />} />
          <Route path="/frmFinalizaProgramacao.aspx" element={<FinalizacaoProgramacao />} />
          
          {/* Ferramentas - NOVAS */}
          <Route path="/ferramentas/upload" element={<UploadArquivos />} />
          <Route path="/frmUpload.aspx" element={<UploadArquivos />} />
          <Route path="/ferramentas/download" element={<DownloadArquivos />} />
          <Route path="/frmCnsArquivo.aspx" element={<DownloadArquivos />} />
          <Route path="/ferramentas/recibos" element={<VisualizacaoRecibos />} />
          <Route path="/frmCnsRecibo.aspx" element={<VisualizacaoRecibos />} />
          <Route path="/ferramentas/recuperar-dados" element={<RecuperarDadosDiaAnterior />} />
          <Route path="/frmRecuperarDados.aspx" element={<RecuperarDadosDiaAnterior />} />
          
          {/* Coleta - Gerenciamento da Demanda - NOVAS */}
          <Route path="/coleta/demanda/programacao-semanal" element={<ProgramacaoSemanal />} />
          <Route path="/PDPProgSemanal.aspx" element={<ProgramacaoSemanal />} />
          <Route path="/coleta/demanda/programacao-diaria" element={<ProgramacaoDiaria />} />
          <Route path="/PDPProgDiaria.aspx" element={<ProgramacaoDiaria />} />
          <Route path="/coleta/demanda/relatorio-oferta-reducao" element={<RelatorioOfertaReducao />} />
          <Route path="/frmRelOfertaReducaoSemana.aspx" element={<RelatorioOfertaReducao />} />
          
          {/* Admin/Cadastro */}
          <Route path="/admin/empresas" element={<CompanyManagement />} />
          <Route path="/admin/usuarios" element={<UserManagementPage />} />
          <Route path="/admin/associacao-usuario-empresa" element={<UserAssociation />} />
          <Route path="/admin/usinas" element={<PlantManagement />} />
          <Route path="/admin/motivos-despacho-eletrica" element={<ElectricalDispatchReasonPage />} />
          <Route path="/frmCnsMotivo.aspx" element={<ElectricalDispatchReasonPage />} />
          <Route path="/admin/motivos-despacho-inflexibilidade" element={<InflexibilityDispatchReasonPage />} />
          <Route path="/frmCnsMotivoInfl.aspx" element={<InflexibilityDispatchReasonPage />} />
          <Route path="/admin/inflexibilidade-contratada" element={<ContractedInflexibility />} />
          <Route path="/frmInflxContratada.aspx" element={<ContractedInflexibility />} />
          
          {/* Exportação */}
          <Route path="/ofertas-exportacao" element={<OfertasExportacaoManagement />} />
          <Route path="/ofertas-rv" element={<OfertasRVManagement />} />
          <Route path="/energia-vertida" element={<EnergiaVertidaManagement />} />
          
          {/* Auth */}
          <Route path="/auth/integration" element={<IntegrationAuth />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}

export default App;
