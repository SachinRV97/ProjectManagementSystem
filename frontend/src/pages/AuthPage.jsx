import { useState } from 'react';
import { useAuth } from '../context/AuthContext';

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
export default function AuthPage() {
  const { login, registerCompany } = useAuth();
  const [isCompanyRegistration, setIsCompanyRegistration] = useState(false);
  const [error, setError] = useState('');
  const [loginForm, setLoginForm] = useState({
    email: '',
    password: '',
  });
  const [companyForm, setCompanyForm] = useState({
    companyName: '',
    companyCode: '',
    contactEmail: '',
    contactPhone: '',
    website: '',
    addressLine1: '',
    addressLine2: '',
    city: '',
    state: '',
    country: '',
    postalCode: '',
    adminName: '',
    adminEmail: '',
    adminPassword: '',
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
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
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
  const [isRegister, setIsRegister] = useState(false);
=======
  const [mode, setMode] = useState('login');
>>>>>>> theirs
=======
  const [mode, setMode] = useState('login');
>>>>>>> theirs
=======
  const [mode, setMode] = useState('login');
>>>>>>> theirs
  const [error, setError] = useState('');
  const [form, setForm] = useState({
    name: '',
    email: '',
    password: '',
    role: 'Customer-User',
    customerCode: 'GLOBAL',
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
  });

  const submit = async (event) => {
    event.preventDefault();
    setError('');
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours

    try {
      if (isCompanyRegistration) {
        await registerCompany(companyForm);
      } else {
        await login(loginForm.email, loginForm.password);
      }
    } catch (e) {
      setError(e?.response?.data?.message ?? 'API is not reachable. Start the backend on http://localhost:5000 and try again.');
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    try {
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
  });

  const isRegister = mode === 'register';

  const submit = async (event) => {
    event.preventDefault();
    setError('');

    try {
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
      if (isRegister) {
        await register(form);
      } else {
        await login(form.email, form.password);
      }
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
    } catch (e) {
      setError(e?.response?.data?.message ?? 'Unexpected error');
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
    } catch (requestError) {
      setError(requestError?.response?.data?.message ?? 'Authentication failed.');
>>>>>>> theirs
=======
    } catch (requestError) {
      setError(requestError?.response?.data?.message ?? 'Authentication failed.');
>>>>>>> theirs
=======
    } catch (requestError) {
      setError(requestError?.response?.data?.message ?? 'Authentication failed.');
>>>>>>> theirs
    }
  };

  return (
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
    <section className="card border-0 auth-card shadow-lg">
      <div className="card-body p-4 p-lg-5">
        <div className="d-flex flex-wrap gap-2 mb-4">
          <button
            type="button"
            className={`btn ${!isCompanyRegistration ? 'btn-primary' : 'btn-outline-secondary'}`}
            onClick={() => setIsCompanyRegistration(false)}
          >
            Sign In
          </button>
          <button
            type="button"
            className={`btn ${isCompanyRegistration ? 'btn-primary' : 'btn-outline-secondary'}`}
            onClick={() => setIsCompanyRegistration(true)}
          >
            Register Company
          </button>
        </div>

        <div className="mb-4">
          <h2 className="h3 mb-2">{isCompanyRegistration ? 'Create your company workspace' : 'Welcome back'}</h2>
          <p className="text-secondary mb-0">
            {isCompanyRegistration
              ? 'Set up the company profile and the primary login account in one step.'
              : 'Sign in to manage portal users, customers, and portal design.'}
          </p>
        </div>

        {error && <div className="alert alert-danger py-2">{error}</div>}

        <form className="row g-3" onSubmit={submit}>
          {isCompanyRegistration ? (
            <>
              <div className="col-12">
                <span className="auth-hint-label">Company Details</span>
              </div>
              <div className="col-12">
                <label className="form-label">Company Name</label>
                <input
                  className="form-control"
                  required
                  value={companyForm.companyName}
                  onChange={(e) => setCompanyForm({ ...companyForm, companyName: e.target.value })}
                />
              </div>
              <div className="col-md-6">
                <label className="form-label">Company Code</label>
                <input
                  className="form-control"
                  required
                  value={companyForm.companyCode}
                  onChange={(e) => setCompanyForm({ ...companyForm, companyCode: e.target.value })}
                />
              </div>
              <div className="col-md-6">
                <label className="form-label">Contact Email</label>
                <input
                  className="form-control"
                  type="email"
                  required
                  value={companyForm.contactEmail}
                  onChange={(e) => setCompanyForm({ ...companyForm, contactEmail: e.target.value })}
                />
              </div>
              <div className="col-md-6">
                <label className="form-label">Contact Phone</label>
                <input
                  className="form-control"
                  value={companyForm.contactPhone}
                  onChange={(e) => setCompanyForm({ ...companyForm, contactPhone: e.target.value })}
                />
              </div>
              <div className="col-12">
                <label className="form-label">Website</label>
                <input
                  className="form-control"
                  placeholder="https://example.com"
                  value={companyForm.website}
                  onChange={(e) => setCompanyForm({ ...companyForm, website: e.target.value })}
                />
              </div>
              <div className="col-md-6">
                <label className="form-label">Address Line 1</label>
                <input
                  className="form-control"
                  value={companyForm.addressLine1}
                  onChange={(e) => setCompanyForm({ ...companyForm, addressLine1: e.target.value })}
                />
              </div>
              <div className="col-md-6">
                <label className="form-label">Address Line 2</label>
                <input
                  className="form-control"
                  value={companyForm.addressLine2}
                  onChange={(e) => setCompanyForm({ ...companyForm, addressLine2: e.target.value })}
                />
              </div>
              <div className="col-md-4">
                <label className="form-label">City</label>
                <input
                  className="form-control"
                  value={companyForm.city}
                  onChange={(e) => setCompanyForm({ ...companyForm, city: e.target.value })}
                />
              </div>
              <div className="col-md-4">
                <label className="form-label">State</label>
                <input
                  className="form-control"
                  value={companyForm.state}
                  onChange={(e) => setCompanyForm({ ...companyForm, state: e.target.value })}
                />
              </div>
              <div className="col-md-4">
                <label className="form-label">Postal Code</label>
                <input
                  className="form-control"
                  value={companyForm.postalCode}
                  onChange={(e) => setCompanyForm({ ...companyForm, postalCode: e.target.value })}
                />
              </div>
              <div className="col-12">
                <label className="form-label">Country</label>
                <input
                  className="form-control"
                  value={companyForm.country}
                  onChange={(e) => setCompanyForm({ ...companyForm, country: e.target.value })}
                />
              </div>
              <div className="col-12 pt-2">
                <span className="auth-hint-label">Primary Login Account</span>
              </div>
              <div className="col-md-6">
                <label className="form-label">Full Name</label>
                <input
                  className="form-control"
                  required
                  value={companyForm.adminName}
                  onChange={(e) => setCompanyForm({ ...companyForm, adminName: e.target.value })}
                />
              </div>
              <div className="col-12">
                <label className="form-label">Login Email</label>
                <input
                  className="form-control"
                  type="email"
                  required
                  value={companyForm.adminEmail}
                  onChange={(e) => setCompanyForm({ ...companyForm, adminEmail: e.target.value })}
                />
              </div>
              <div className="col-12">
                <label className="form-label">Password</label>
                <input
                  className="form-control"
                  type="password"
                  required
                  value={companyForm.adminPassword}
                  onChange={(e) => setCompanyForm({ ...companyForm, adminPassword: e.target.value })}
                />
              </div>
            </>
          ) : (
            <>
              <div className="col-12">
                <label className="form-label">Email</label>
                <input
                  className="form-control"
                  type="email"
                  required
                  value={loginForm.email}
                  onChange={(e) => setLoginForm({ ...loginForm, email: e.target.value })}
                />
              </div>
              <div className="col-12">
                <label className="form-label">Password</label>
                <input
                  className="form-control"
                  type="password"
                  required
                  value={loginForm.password}
                  onChange={(e) => setLoginForm({ ...loginForm, password: e.target.value })}
                />
              </div>
            </>
          )}

          <div className="col-12 pt-2">
            <button className="btn btn-primary btn-lg w-100" type="submit">
              {isCompanyRegistration ? 'Create Company Workspace' : 'Access Dashboard'}
            </button>
          </div>
        </form>

        <div className="auth-hint mt-4">
          <span className="auth-hint-label">Flow</span>
          <p className="mb-0">
            Company registers first with its basic details, then the first login user can sign in and create portal or customer users.
          </p>
        </div>
      </div>
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
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
    </section>
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    <div className="auth-layout">
      <section className="panel auth-marketing">
        <p className="eyebrow">Reusable full-stack starter</p>
        <h2>Build customer and employee portals from one admin-controlled platform.</h2>
        <ul className="feature-list">
          <li>React dashboard with portal designer preview</li>
          <li>.NET API with JWT auth and role-based authorization</li>
          <li>MSSQL-ready EF Core data model for users, customers, and portal configuration</li>
        </ul>
      </section>

      <section className="panel auth-card">
        <div className="section-header compact">
          <div>
            <p className="eyebrow">Access portal</p>
            <h2>{isRegister ? 'Create account' : 'Login'}</h2>
          </div>
          <span className="badge muted">{isRegister ? 'Register' : 'Sign in'}</span>
        </div>

        <form onSubmit={submit} className="grid auth-form">
          {isRegister && (
            <label>
              Name
              <input required value={form.name} onChange={(event) => setForm({ ...form, name: event.target.value })} />
            </label>
          )}

          <label>
            Email
            <input required type="email" value={form.email} onChange={(event) => setForm({ ...form, email: event.target.value })} />
          </label>

          <label>
            Password
            <input required type="password" value={form.password} onChange={(event) => setForm({ ...form, password: event.target.value })} />
          </label>

          {isRegister && (
            <>
              <label>
                Role
                <select value={form.role} onChange={(event) => setForm({ ...form, role: event.target.value })}>
                  {roles.map((role) => (
                    <option key={role} value={role}>{role}</option>
                  ))}
                </select>
              </label>

              <label>
                Customer code
                <input value={form.customerCode} onChange={(event) => setForm({ ...form, customerCode: event.target.value.toUpperCase() })} />
              </label>
            </>
          )}

          <button type="submit">{isRegister ? 'Register and continue' : 'Login to dashboard'}</button>
        </form>

        {error && <p className="error">{error}</p>}

        <button className="link" onClick={() => setMode(isRegister ? 'login' : 'register')}>
          {isRegister ? 'Already have an account? Sign in' : 'Need a new account? Register'}
        </button>
      </section>
    </div>
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
  );
}
