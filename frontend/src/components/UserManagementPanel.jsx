import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { getAssignableRoles, isCustomerRoleName } from '../services/access';

export default function UserManagementPanel({ session }) {
  const { createUser } = useAuth();
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [status, setStatus] = useState('');
  const [form, setForm] = useState({
    name: '',
    email: '',
    password: '',
    role: '',
    customerCode: '',
  });

  useEffect(() => {
    let isMounted = true;

    const loadRoles = async () => {
      setLoading(true);
      setError('');

      try {
        const data = await getAssignableRoles();
        if (!isMounted) {
          return;
        }

        setRoles(data);
        setForm((current) => ({
          ...current,
          role: data.some((role) => role.name === current.role) ? current.role : data[0]?.name ?? '',
        }));
      } catch (e) {
        if (isMounted) {
          setError(e?.response?.data?.message ?? 'Unable to load assignable roles');
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

  const selectedRole = roles.some((role) => role.name === form.role) ? form.role : '';
  const needsCustomerCode = isCustomerRoleName(selectedRole);

  const submit = async (event) => {
    event.preventDefault();
    setError('');
    setStatus('');

    if (!selectedRole) {
      setError('Select a valid role to create.');
      return;
    }

    if (needsCustomerCode && !form.customerCode.trim()) {
      setError('Customer code is required for customer users.');
      return;
    }

    try {
      const createdUser = await createUser({
        name: form.name,
        email: form.email,
        password: form.password,
        role: selectedRole,
        customerCode: needsCustomerCode ? form.customerCode : null,
      });

      setStatus(`${createdUser.role} created for ${createdUser.email}`);
      setForm((current) => ({
        ...current,
        name: '',
        email: '',
        password: '',
        customerCode: needsCustomerCode ? current.customerCode : '',
      }));
    } catch (e) {
      setError(e?.response?.data?.message ?? 'Unable to create user');
    }
  };

  return (
    <section className="card border-0 shadow-sm h-100 dashboard-card">
      <div className="card-body p-4">
        <div className="d-flex flex-wrap justify-content-between align-items-start gap-3 mb-4">
          <div>
            <h3 className="h4 mb-2">Company User Registration</h3>
            <p className="text-secondary mb-0">
              Company: <strong>{session.companyName || 'Global System'}</strong> ({session.companyCode || 'GLOBAL'})
            </p>
          </div>
          <div className="role-chip-wrap">
            {roles.map((role) => (
              <span key={role.id} className="badge text-bg-dark-subtle text-dark-emphasis">{role.name}</span>
            ))}
          </div>
        </div>

        {loading && <div className="alert alert-secondary py-2 mb-3">Loading allowed roles...</div>}
        {error && <div className="alert alert-danger py-2 mb-3">{error}</div>}
        {!loading && roles.length === 0 && !error && (
          <div className="alert alert-warning py-2 mb-3">Your role does not have permission to register more users.</div>
        )}

        {!loading && roles.length > 0 && (
          <form className="row g-3" onSubmit={submit}>
            <div className="col-md-6">
              <label className="form-label">Name</label>
              <input className="form-control" required value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} />
            </div>
            <div className="col-md-6">
              <label className="form-label">Role</label>
              <select className="form-select" value={selectedRole} onChange={(e) => setForm({ ...form, role: e.target.value })}>
                {roles.map((role) => (
                  <option key={role.id} value={role.name}>{role.name}</option>
                ))}
              </select>
            </div>
            <div className="col-md-6">
              <label className="form-label">Email</label>
              <input className="form-control" type="email" required value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
            </div>
            <div className="col-md-6">
              <label className="form-label">Password</label>
              <input className="form-control" type="password" required value={form.password} onChange={(e) => setForm({ ...form, password: e.target.value })} />
            </div>
            {needsCustomerCode && (
              <div className="col-md-6">
                <label className="form-label">Customer Code</label>
                <input className="form-control" value={form.customerCode} onChange={(e) => setForm({ ...form, customerCode: e.target.value })} />
              </div>
            )}
            <div className="col-12">
              <button className="btn btn-primary px-4" type="submit">Create User</button>
            </div>
          </form>
        )}

        {status && <div className="alert alert-success py-2 mt-3 mb-0">{status}</div>}
      </div>
    </section>
  );
}
