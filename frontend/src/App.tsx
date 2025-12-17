import { useState, useEffect } from 'react'
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom'
import DadosEnergeticosLista from './components/DadosEnergeticosLista'
import DadosEnergeticosForm from './components/DadosEnergeticosForm'
import './App.css'

function App() {
  return (
    <Router>
      <div className="App">
        <header className="App-header">
          <h1>PDPW - Programação Diária da Produção</h1>
          <nav>
            <Link to="/">Dados Energéticos</Link>
            <Link to="/novo">Novo Registro</Link>
          </nav>
        </header>
        <main className="App-main">
          <Routes>
            <Route path="/" element={<DadosEnergeticosLista />} />
            <Route path="/novo" element={<DadosEnergeticosForm />} />
            <Route path="/editar/:id" element={<DadosEnergeticosForm />} />
          </Routes>
        </main>
      </div>
    </Router>
  )
}

export default App
