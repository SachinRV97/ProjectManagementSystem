import { useState } from 'react';
import { useAuth } from '../context/AuthContext';

const roles = [
  'Admin',
  'Portal-Admin',
  'Portal-Employee',
  'Customer-Admin',
  'Customer-Employee',
  'Customer-User',
];

export default function AuthPage() {
  const { login, register } = useAuth();
  const [isRegister, setIsRegister] = useState(false);
  const [error, setError] = useState('');
  const [form, setForm] = useState({
    name: '',
    email: '',
    password: '',
    role: 'Customer-User',
    customerCode: 'GLOBAL',
  });

  const submit = async (event) => {
    event.preventDefault();
    setError('');
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
              <select value={form.role} onChange={(e) => setForm({ ...form, role: e.target.value })}>
                {roles.map((role) => (
                  <option key={role} value={role}>{role}</option>
                ))}
              </select>
            </label>
            <label>
              Customer Code
              <input value={form.customerCode} onChange={(e) => setForm({ ...form, customerCode: e.target.value })} />
            </label>
          </>
        )}

        <button type="submit">{isRegister ? 'Create account' : 'Sign in'}</button>
      </form>
      {error && <p className="error">{error}</p>}
      <button className="link" onClick={() => setIsRegister((s) => !s)}>
        {isRegister ? 'Already have account? Login' : 'Need account? Register'}
      </button>
    </section>
  );
}
