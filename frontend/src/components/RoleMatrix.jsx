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
    <section className="panel">
      <h3>Role Access Matrix</h3>
      {error && <p className="error">{error}</p>}
      {loading && !error && <p>Loading roles...</p>}
      {!loading && roles.length === 0 && !error && <p>No active roles are configured.</p>}
      <ul>
        {roles.map((role) => (
          <li key={role.id}>
            <strong>{role.name}</strong>: {describeRole(role)}
          </li>
        ))}
      </ul>
    </section>
  );
}
