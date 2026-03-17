import { useState } from 'react';
import { useAuth } from '../context/AuthContext';

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
  });

  const submit = async (event) => {
    event.preventDefault();
    setError('');

    try {
      if (isCompanyRegistration) {
        await registerCompany(companyForm);
      } else {
        await login(loginForm.email, loginForm.password);
      }
    } catch (e) {
      setError(e?.response?.data?.message ?? 'API is not reachable. Start the backend on http://localhost:5000 and try again.');
    }
  };

  return (
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
    </section>
  );
}
