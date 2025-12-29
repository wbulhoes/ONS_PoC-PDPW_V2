import { BrowserRouter as Router, Routes, Route, NavLink } from 'react-router-dom';
import Dashboard from './pages/Dashboard';
import DadosEnergeticos from './pages/DadosEnergeticos';
import ProgramacaoEletrica from './pages/ProgramacaoEletrica';
import PrevisaoEolica from './pages/PrevisaoEolica';
import GeracaoArquivos from './pages/GeracaoArquivos';
import FinalizacaoProgramacao from './pages/FinalizacaoProgramacao';
import InsumosAgentes from './pages/InsumosAgentes';
import OfertasExportacao from './pages/OfertasExportacao';
import OfertasRespostaVoluntaria from './pages/OfertasRespostaVoluntaria';
import EnergiaVertida from './pages/EnergiaVertida';
import './App.css';

function App() {
  return (
    <Router>
      <div className="App">
        <header className="App-header">
          <div className="header-content">
            <h1>📊 PDPw - Sistema de Programação Diária</h1>
            <p className="header-subtitle">Operador Nacional do Sistema Elétrico - ONS</p>
          </div>
        </header>

        <div className="App-layout">
          <nav className="App-sidebar">
            <div className="nav-section">
              <h3>Principal</h3>
              <NavLink to="/" className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}>
                🏠 Dashboard
              </NavLink>
            </div>

            <div className="nav-section">
              <h3>Programação</h3>
              <NavLink
                to="/dados-energeticos"
                className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}
              >
                ⚡ 1. Dados Energéticos
              </NavLink>
              <NavLink
                to="/programacao-eletrica"
                className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}
              >
                🔌 2. Programação Elétrica
              </NavLink>
              <NavLink
                to="/previsao-eolica"
                className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}
              >
                💨 3. Previsão Eólica
              </NavLink>
              <NavLink
                to="/geracao-arquivos"
                className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}
              >
                📁 4. Geração de Arquivos
              </NavLink>
            </div>

            <div className="nav-section">
              <h3>Workflow</h3>
              <NavLink to="/finalizacao" className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}>
                ✅ 5. Finalização
              </NavLink>
            </div>

            <div className="nav-section">
              <h3>Recebimentos</h3>
              <NavLink to="/insumos-agentes" className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}>
                📥 6. Insumos Agentes
              </NavLink>
              <NavLink
                to="/ofertas-exportacao"
                className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}
              >
                🔥 7. Ofertas Térmicas
              </NavLink>
              <NavLink
                to="/ofertas-rv"
                className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}
              >
                📉 8. Ofertas RV
              </NavLink>
              <NavLink
                to="/energia-vertida"
                className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}
              >
                💧 9. Energia Vertida
              </NavLink>
            </div>
          </nav>

          <main className="App-main">
            <Routes>
              <Route path="/" element={<Dashboard />} />
              <Route path="/dados-energeticos" element={<DadosEnergeticos />} />
              <Route path="/programacao-eletrica" element={<ProgramacaoEletrica />} />
              <Route path="/previsao-eolica" element={<PrevisaoEolica />} />
              <Route path="/geracao-arquivos" element={<GeracaoArquivos />} />
              <Route path="/finalizacao" element={<FinalizacaoProgramacao />} />
              <Route path="/insumos-agentes" element={<InsumosAgentes />} />
              <Route path="/ofertas-exportacao" element={<OfertasExportacao />} />
              <Route path="/ofertas-rv" element={<OfertasRespostaVoluntaria />} />
              <Route path="/energia-vertida" element={<EnergiaVertida />} />
            </Routes>
          </main>
        </div>

        <footer className="App-footer">
          <p>© 2025 ONS - Operador Nacional do Sistema Elétrico | PDPw v2.0 - Modernização .NET 8</p>
        </footer>
      </div>
    </Router>
  );
}

export default App;
