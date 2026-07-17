import { useState } from 'react'
import HomePage from './pages/HomePage.jsx'
import './App.css'
import { Routes, Route } from "react-router-dom";
import RegisterPage from './pages/RegisterPage.jsx'
import LoginPage from './pages/LoginPage.jsx';
import DashboardPage from './pages/DashboardPage.jsx';

function App() {
  const [count, setCount] = useState(0)

  return (
    <div className="App">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/register" element={<RegisterPage />} /> 
          <Route path="/login" element={<LoginPage />} />
          <Route path="/dashboard" element={<DashboardPage/>} />
        </Routes>
    </div>
  )
}

export default App
