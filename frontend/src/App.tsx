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
import ReplacementEnergyPage from './pages/Collection/Other/ReplacementEnergy';
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
import './styles/global.css';
import CompanyManagement from './pages/Admin/Company/CompanyManagement';
import PlantManagement from './pages/Admin/Plant/PlantManagement';

function App() {
  return (
    <BrowserRouter>
      <Layout userName="Usuário Teste">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/exemplo" element={<Example />} />
          <Route path="/splash" element={<Splash />} />
          <Route path="/coleta/hidraulico/vazao" element={<Flow />} />
          <Route path="/coleta/hidraulico/disponibilidade" element={<Availability initialType="H" />} />
          <Route path="/coleta/hidraulico/balanco" element={<Balance />} />
          <Route path="/coleta/termico/geracao" element={<Generation />} />
          <Route path="/coleta/termico/disponibilidade" element={<Availability initialType="T" />} />
          <Route path="/frmColDisponibilidade.aspx" element={<Availability initialType="T" />} />
          <Route path="/coleta/termico/inflexibilidade" element={<Inflexibility />} />
          <Route path="/coleta/termico/modalidade-operativa" element={<OperatingMode />} />
          <Route
            path="/coleta/termico/despacho-inflexibilidade"
            element={<InflexibilityDispatch />}
          />
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
          <Route path="/coleta/carga/carga" element={<Load />} />
          <Route path="/coleta/carga/consumo" element={<Consumption />} />
          <Route path="/coleta/eletrica/energia" element={<Energy />} />
          <Route path="/frmColEnergetica.aspx" element={<Energy />} />
          <Route path="/coleta/eletrica/programacao" element={<ProgramacaoEnergeticaPage />} />
          <Route path="/frmColProgramacaoEnergetica.aspx" element={<ProgramacaoEnergeticaPage />} />
          <Route path="/coleta/eletrica/programacao-eletrica" element={<ProgramacaoEletrica />} />
          <Route path="/frmColProgramacaoEletrica.aspx" element={<ProgramacaoEletrica />} />
          <Route path="/coleta/eletrica/previsao-eolica" element={<PrevisaoEolica />} />
          <Route path="/frmColPrevisaoEolica.aspx" element={<PrevisaoEolica />} />
          <Route path="/gerar/arquivos-modelos" element={<GenerateModelFiles />} />
          <Route path="/frmGerArquivo.aspx" element={<GenerateModelFiles />} />
          <Route path="/finalizacao/programacao" element={<FinalizacaoProgramacao />} />
          <Route path="/frmFinalizaProgramacao.aspx" element={<FinalizacaoProgramacao />} />
          <Route path="/coleta/restricoes/restricao-ug" element={<UnitRestriction />} />
          <Route path="/coleta/outros/gec" element={<GEC />} />
          <Route path="/coleta/outros/energia-reposicao" element={<ReplacementEnergyPage />} />
          <Route path="/frmColEnergiaRepPer.aspx" element={<ReplacementEnergyPage />} />
          <Route path="/consulta/dessem/comentarios" element={<Comments />} />
          <Route path="/frmCnsObservacoes.aspx" element={<Comments />} />
          <Route path="/consulta/outros/observacao" element={<ObservationQuery />} />
          <Route path="/frmCnsObservacao.aspx" element={<ObservationQuery />} />
          <Route path="/consulta/hidraulico/disponibilidade" element={<AvailabilityQuery />} />
          <Route path="/frmCnsDisponibilidade.aspx" element={<AvailabilityQuery />} />
          <Route path="/consulta/outros/rro" element={<RROQuery />} />
          <Route path="/frmCnsRRO.aspx" element={<RROQuery />} />
          <Route path="/coleta/insumos" element={<Insumos />} />
          <Route path="/frmColInsumos.aspx" element={<Insumos />} />
          <Route path="/consulta/outros/marcos-programacao" element={<ProgrammingMilestoneQuery />} />
          <Route path="/frmConsultaMarcoProgramacao.aspx" element={<ProgrammingMilestoneQuery />} />
          <Route path="/frmCnsEnergiaRepPer.aspx" element={<ReplacementEnergyPage />} />
          <Route path="/coleta/outros/usina-conversora" element={<PlantConverterPage />} />
          <Route path="/frmUsinaConversora.aspx" element={<PlantConverterPage />} />
          <Route path="/auth/integration" element={<IntegrationAuth />} />
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
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}

export default App;
