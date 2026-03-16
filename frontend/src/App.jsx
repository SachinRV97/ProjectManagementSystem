import { useAuth } from './context/AuthContext';
import AuthPage from './pages/AuthPage';
import Dashboard from './pages/Dashboard';

export default function App() {
  const { session, logout } = useAuth();

  if (!session) {
    return (
      <div className="container">
        <h1>React + .NET + MSSQL Starter</h1>
        <p>Centralized portal setup with role-based access control.</p>
        <AuthPage />
      </div>
    );
  }

  return <Dashboard session={session} logout={logout} />;
}
