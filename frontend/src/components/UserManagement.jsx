import { useEffect, useState } from 'react';
import api from '../services/api';

const roles = ['Customer-Admin', 'Customer-Employee', 'Customer-User', 'Portal-Admin', 'Portal-Employee', 'Admin'];

export default function UserManagement({ session }) {
  const [users, setUsers] = useState([]);
  const [form, setForm] = useState({
    name: '',
    email: '',
    password: '',
    role: session.role === 'Customer-Admin' ? 'Customer-Employee' : 'Customer-User',
    customerCode: session.customerCode,
  });
  const [status, setStatus] = useState('');
  const [error, setError] = useState('');

  const allowedRoles = session.role === 'Admin'
    ? roles
    : ['Customer-Admin', 'Customer-Employee', 'Customer-User'];

  const loadUsers = async () => {
    const { data } = await api.get('/users');
    setUsers(data);
  };

  useEffect(() => {
    loadUsers().catch((requestError) => {
      setError(requestError?.response?.data?.message ?? 'Unable to load users.');
    });
  }, []);

  const submit = async (event) => {
    event.preventDefault();
    setStatus('');
    setError('');

    try {
      await api.post('/users', form);
      setStatus('User created successfully.');
      setForm({
        name: '',
        email: '',
        password: '',
        role: allowedRoles[0],
        customerCode: session.customerCode,
      });
      await loadUsers();
    } catch (requestError) {
      setError(requestError?.response?.data?.message ?? 'Unable to create user.');
    }
  };

  return (
    <section className="panel">
      <div className="section-header">
        <div>
          <p className="eyebrow">User administration</p>
          <h2>Manage employees and users</h2>
        </div>
        <span className="badge">{users.length} users</span>
      </div>

      <div className="dashboard-grid two-pane">
        <form className="nested-panel" onSubmit={submit}>
          <h3>Create managed user</h3>
          <label>
            Name
            <input required value={form.name} onChange={(event) => setForm({ ...form, name: event.target.value })} />
          </label>
          <label>
            Email
            <input required type="email" value={form.email} onChange={(event) => setForm({ ...form, email: event.target.value })} />
          </label>
          <label>
            Temporary password
            <input required type="password" value={form.password} onChange={(event) => setForm({ ...form, password: event.target.value })} />
          </label>
          <label>
            Role
            <select value={form.role} onChange={(event) => setForm({ ...form, role: event.target.value })}>
              {allowedRoles.map((role) => (
                <option key={role} value={role}>{role}</option>
              ))}
            </select>
          </label>
          <label>
            Customer code
            <input value={form.customerCode} onChange={(event) => setForm({ ...form, customerCode: event.target.value.toUpperCase() })} />
          </label>
          <button type="submit">Create user</button>
          {status && <p className="success-text">{status}</p>}
          {error && <p className="error">{error}</p>}
        </form>

        <div className="nested-panel">
          <h3>Current users</h3>
          <div className="table-wrap">
            <table className="role-table compact-table">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Email</th>
                  <th>Role</th>
                  <th>Customer</th>
                </tr>
              </thead>
              <tbody>
                {users.map((user) => (
                  <tr key={user.id}>
                    <td>{user.name}</td>
                    <td>{user.email}</td>
                    <td>{user.role}</td>
                    <td>{user.customerCode}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </section>
  );
}
