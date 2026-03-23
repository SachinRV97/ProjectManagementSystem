<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
import { useEffect, useState } from 'react';
import { describeRole, getRoles } from '../services/access';

export default function RoleMatrix() {
  const [roles, setRoles] = useState([]);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let isMounted = true;

    const loadRoles = async () => {
      try {
        const data = await getRoles();
        if (isMounted) {
          setRoles(data);
        }
      } catch (e) {
        if (isMounted) {
          setError(e?.response?.data?.message ?? 'Unable to load roles');
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    };

    loadRoles();

    return () => {
      isMounted = false;
    };
  }, []);

  return (
    <section className="card border-0 shadow-sm h-100 dashboard-card">
      <div className="card-body p-4">
        <div className="d-flex justify-content-between align-items-center mb-3">
          <h3 className="h4 mb-0">Role Access Matrix</h3>
          <span className="badge text-bg-primary-subtle text-primary-emphasis">{roles.length} roles</span>
        </div>
        {error && <div className="alert alert-danger py-2">{error}</div>}
        {loading && !error && <div className="alert alert-secondary py-2">Loading roles...</div>}
        {!loading && roles.length === 0 && !error && <div className="alert alert-warning py-2 mb-0">No active roles are configured.</div>}
        {roles.length > 0 && (
          <div className="list-group list-group-flush">
            {roles.map((role) => (
              <div key={role.id} className="list-group-item px-0 bg-transparent border-secondary-subtle">
                <div className="fw-semibold">{role.name}</div>
                <div className="text-secondary small mt-1">{describeRole(role)}</div>
              </div>
            ))}
          </div>
        )}
      </div>
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
const rights = {
  Admin: ['All access'],
  'Portal-Admin': ['Manage portal for employees and customers'],
  'Portal-Employee': ['Manage customer records'],
  'Customer-Admin': ['Manage employees and users'],
  'Customer-Employee': ['Change portal design and content'],
  'Customer-User': ['Login/registration and normal app usage'],
};

export default function RoleMatrix() {
  return (
    <section className="panel">
      <h3>Role Access Matrix</h3>
      <ul>
        {Object.entries(rights).map(([role, capabilities]) => (
          <li key={role}>
            <strong>{role}</strong>: {capabilities.join(', ')}
          </li>
        ))}
      </ul>
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
const rows = [
  {
    role: 'Admin',
    access: 'All access',
    responsibilities: 'Manage platform configuration, customer tenants, portal design, and users.',
  },
  {
    role: 'Portal-Admin',
    access: 'Portal governance',
    responsibilities: 'Manage portal structure and experience for employee and customer portals.',
  },
  {
    role: 'Portal-Employee',
    access: 'Customer operations',
    responsibilities: 'Track customer onboarding, tenant activity, and portal rollout readiness.',
  },
  {
    role: 'Customer-Admin',
    access: 'Tenant user management',
    responsibilities: 'Create and manage employee and customer users inside one customer account.',
  },
  {
    role: 'Customer-Employee',
    access: 'Portal designer',
    responsibilities: 'Update the customer portal design, messaging, and content sections.',
  },
  {
    role: 'Customer-User',
    access: 'Portal consumption',
    responsibilities: 'Register, log in, and use the portal like a normal application user.',
  },
];

export default function RoleMatrix() {
  return (
    <section className="panel">
      <div className="section-header">
        <div>
          <p className="eyebrow">Access model</p>
          <h2>Role matrix</h2>
        </div>
        <span className="badge muted">6 roles included</span>
      </div>

      <div className="table-wrap">
        <table className="role-table">
          <thead>
            <tr>
              <th>Role</th>
              <th>Access</th>
              <th>Responsibilities</th>
            </tr>
          </thead>
          <tbody>
            {rows.map((row) => (
              <tr key={row.role}>
                <td>{row.role}</td>
                <td>{row.access}</td>
                <td>{row.responsibilities}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    </section>
  );
}
