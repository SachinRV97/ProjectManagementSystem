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
    </section>
  );
}
