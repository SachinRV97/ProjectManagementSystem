import PortalDesigner from '../components/PortalDesigner';
import RoleMatrix from '../components/RoleMatrix';

export default function Dashboard({ session, logout }) {
  return (
    <div className="container">
      <header className="topbar">
        <h1>Project Management System</h1>
        <div>
          <span>{session.name} ({session.role})</span>
          <button onClick={logout}>Logout</button>
        </div>
      </header>
      <RoleMatrix />
      <PortalDesigner session={session} />
    </div>
  );
}
