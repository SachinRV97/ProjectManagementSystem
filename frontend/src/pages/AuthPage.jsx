import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { getRoles } from '../services/access';

export default function AuthPage() {
  const { login, register } = useAuth();
  const [isRegister, setIsRegister] = useState(false);
  const [error, setError] = useState('');
  const [roles, setRoles] = useState([]);
  const [rolesError, setRolesError] = useState('');
  const [rolesLoading, setRolesLoading] = useState(true);
  const [form, setForm] = useState({
    name: '',
    email: '',
    password: '',
    role: 'Customer-User',
    customerCode: 'GLOBAL',
  });

  useEffect(() => {
    let isMounted = true;

    const loadRoles = async () => {
      setRolesLoading(true);
      setRolesError('');

      try {
        const data = await getRoles();
        if (!isMounted) {
          return;
        }

        setRoles(data);
        setForm((current) => {
          const selectedRole = data.some((role) => role.name === current.role)
            ? current.role
            : data.find((role) => role.name === 'Customer-User')?.name ?? data[0]?.name ?? '';

          return { ...current, role: selectedRole };
        });
      } catch (e) {
        if (isMounted) {
          setRolesError(e?.response?.data?.message ?? 'Unable to load roles');
        }
      } finally {
        if (isMounted) {
          setRolesLoading(false);
        }
      }
    };

    loadRoles();

    return () => {
      isMounted = false;
    };
  }, []);

  const submit = async (event) => {
    event.preventDefault();
    setError('');

    if (isRegister && !roles.some((role) => role.name === form.role)) {
      setError('Select a valid role before creating the account.');
      return;
    }

    try {
      if (isRegister) {
        await register(form);
      } else {
        await login(form.email, form.password);
      }
    } catch (e) {
      setError(e?.response?.data?.message ?? 'Unexpected error');
    }
  };

  const selectedRole = roles.some((role) => role.name === form.role) ? form.role : '';

  return (
    <section className="panel auth-card">
      <h2>{isRegister ? 'Register account' : 'Login'}</h2>
      <form onSubmit={submit}>
        {isRegister && (
          <label>
            Name
            <input required value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} />
          </label>
        )}
        <label>
          Email
          <input type="email" required value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
        </label>
        <label>
          Password
          <input type="password" required value={form.password} onChange={(e) => setForm({ ...form, password: e.target.value })} />
        </label>

        {isRegister && (
          <>
            <label>
              Role
              <select
                value={selectedRole}
                onChange={(e) => setForm({ ...form, role: e.target.value })}
                disabled={rolesLoading || roles.length === 0}
              >
                {rolesLoading && <option value="">Loading roles...</option>}
                {!rolesLoading && roles.length === 0 && <option value="">No active roles available</option>}
                {!rolesLoading && roles.map((role) => (
                  <option key={role.id} value={role.name}>{role.name}</option>
                ))}
              </select>
            </label>
            <label>
              Customer Code
              <input value={form.customerCode} onChange={(e) => setForm({ ...form, customerCode: e.target.value })} />
            </label>
            {rolesError && <p className="error">{rolesError}</p>}
          </>
        )}

        <button type="submit" disabled={isRegister && (rolesLoading || roles.length === 0)}>{isRegister ? 'Create account' : 'Sign in'}</button>
      </form>
      {error && <p className="error">{error}</p>}
      <button className="link" onClick={() => setIsRegister((s) => !s)}>
        {isRegister ? 'Already have account? Login' : 'Need account? Register'}
      </button>
    </section>
  );
}
