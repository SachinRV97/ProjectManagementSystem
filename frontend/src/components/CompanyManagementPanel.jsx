import { useEffect, useState } from 'react';
import { getCompanies, getCompany, updateCompany } from '../services/admin';

const emptyForm = {
  name: '',
  code: '',
  contactEmail: '',
  contactPhone: '',
  website: '',
  addressLine1: '',
  addressLine2: '',
  city: '',
  state: '',
  country: '',
  postalCode: '',
  isLoginAllowed: true,
};

const statusFilterToValue = (loginStatus) => {
  if (loginStatus === 'allowed') {
    return true;
  }

  if (loginStatus === 'blocked') {
    return false;
  }

  return undefined;
};

const formatDateTime = (value) => (
  value ? new Date(value).toLocaleString() : 'N/A'
);

const formatLocation = (company) => (
  [company.city, company.state, company.country].filter(Boolean).join(', ') || 'Not provided'
);

export default function CompanyManagementPanel() {
  const [companies, setCompanies] = useState([]);
  const [filters, setFilters] = useState({
    search: '',
    loginStatus: 'all',
  });
  const [selectedCompanyId, setSelectedCompanyId] = useState('');
  const [form, setForm] = useState(emptyForm);
  const [listLoading, setListLoading] = useState(true);
  const [detailsLoading, setDetailsLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [editorError, setEditorError] = useState('');
  const [status, setStatus] = useState('');

  const loadCompanies = async () => {
    setListLoading(true);
    setError('');

    try {
      const data = await getCompanies({
        search: filters.search.trim() || undefined,
        isLoginAllowed: statusFilterToValue(filters.loginStatus),
      });

      setCompanies(data);

      if (selectedCompanyId && !data.some((company) => company.id === selectedCompanyId)) {
        setSelectedCompanyId('');
        setForm(emptyForm);
      }
    } catch (e) {
      setError(e?.response?.data?.message ?? 'Unable to load companies');
    } finally {
      setListLoading(false);
    }
  };

  useEffect(() => {
    loadCompanies();
  }, [filters.search, filters.loginStatus]);

  const openCompany = async (companyId, options = {}) => {
    const { clearStatus = true } = options;
    setSelectedCompanyId(companyId);
    setDetailsLoading(true);
    setEditorError('');
    if (clearStatus) {
      setStatus('');
    }

    try {
      const company = await getCompany(companyId);
      setForm({
        name: company.name ?? '',
        code: company.code ?? '',
        contactEmail: company.contactEmail ?? '',
        contactPhone: company.contactPhone ?? '',
        website: company.website ?? '',
        addressLine1: company.addressLine1 ?? '',
        addressLine2: company.addressLine2 ?? '',
        city: company.city ?? '',
        state: company.state ?? '',
        country: company.country ?? '',
        postalCode: company.postalCode ?? '',
        isLoginAllowed: Boolean(company.isLoginAllowed),
      });
    } catch (e) {
      setEditorError(e?.response?.data?.message ?? 'Unable to load company details');
    } finally {
      setDetailsLoading(false);
    }
  };

  const submit = async (event) => {
    event.preventDefault();
    if (!selectedCompanyId) {
      return;
    }

    setSaving(true);
    setEditorError('');
    setStatus('');

    try {
      await updateCompany(selectedCompanyId, {
        ...form,
        name: form.name.trim(),
        code: form.code.trim(),
        contactEmail: form.contactEmail.trim(),
        contactPhone: form.contactPhone.trim() || null,
        website: form.website.trim() || null,
        addressLine1: form.addressLine1.trim() || null,
        addressLine2: form.addressLine2.trim() || null,
        city: form.city.trim() || null,
        state: form.state.trim() || null,
        country: form.country.trim() || null,
        postalCode: form.postalCode.trim() || null,
      });

      setStatus('Company profile updated successfully.');
      await loadCompanies();
      await openCompany(selectedCompanyId, { clearStatus: false });
    } catch (e) {
      setEditorError(e?.response?.data?.message ?? 'Unable to update company');
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="row g-4">
      <div className="col-12">
        <section className="card border-0 shadow-sm dashboard-card">
          <div className="card-body p-4">
            <div className="d-flex flex-wrap justify-content-between align-items-start gap-3 mb-4">
              <div>
                <h3 className="h4 mb-2">Company Directory</h3>
                <p className="text-secondary mb-0">
                  Search companies, review their contact details, and block or restore company login access.
                </p>
              </div>
              <button type="button" className="btn btn-outline-dark" onClick={loadCompanies}>
                Refresh List
              </button>
            </div>

            {error && <div className="alert alert-danger py-2 mb-3">{error}</div>}

            <div className="row g-3 align-items-end">
              <div className="col-lg-6">
                <label className="form-label">Search</label>
                <input
                  className="form-control"
                  placeholder="Search by company, code, email, phone, or location"
                  value={filters.search}
                  onChange={(event) => setFilters((current) => ({ ...current, search: event.target.value }))}
                />
              </div>
              <div className="col-sm-6 col-lg-3">
                <label className="form-label">Login Status</label>
                <select
                  className="form-select"
                  value={filters.loginStatus}
                  onChange={(event) => setFilters((current) => ({ ...current, loginStatus: event.target.value }))}
                >
                  <option value="all">All companies</option>
                  <option value="allowed">Login allowed</option>
                  <option value="blocked">Login blocked</option>
                </select>
              </div>
              <div className="col-sm-6 col-lg-3">
                <div className="directory-stats">
                  <span className="overview-label">Visible companies</span>
                  <h4 className="mb-0">{companies.length}</h4>
                </div>
              </div>
            </div>

            <div className="table-responsive mt-4">
              <table className="table align-middle admin-table mb-0">
                <thead>
                  <tr>
                    <th>Company</th>
                    <th>Contact</th>
                    <th>Location</th>
                    <th>Users</th>
                    <th>Status</th>
                    <th className="text-end">Action</th>
                  </tr>
                </thead>
                <tbody>
                  {!listLoading && companies.length === 0 && (
                    <tr>
                      <td colSpan="6">
                        <div className="alert alert-secondary py-2 mb-0">No companies matched the current filter.</div>
                      </td>
                    </tr>
                  )}
                  {listLoading && (
                    <tr>
                      <td colSpan="6">
                        <div className="alert alert-secondary py-2 mb-0">Loading companies...</div>
                      </td>
                    </tr>
                  )}
                  {!listLoading && companies.map((company) => (
                    <tr key={company.id} className={selectedCompanyId === company.id ? 'table-active' : ''}>
                      <td>
                        <div className="fw-semibold">{company.name}</div>
                        <div className="small text-secondary">{company.code}</div>
                      </td>
                      <td>
                        <div>{company.contactEmail}</div>
                        <div className="small text-secondary">{company.contactPhone || 'No phone'}</div>
                      </td>
                      <td className="small">{formatLocation(company)}</td>
                      <td>{company.userCount}</td>
                      <td>
                        <span className={`badge ${company.isLoginAllowed ? 'text-bg-success-subtle text-success-emphasis' : 'text-bg-danger-subtle text-danger-emphasis'}`}>
                          {company.isLoginAllowed ? 'Allowed' : 'Blocked'}
                        </span>
                      </td>
                      <td className="text-end">
                        <button type="button" className="btn btn-outline-primary btn-sm" onClick={() => openCompany(company.id)}>
                          Edit
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </section>
      </div>

      <div className="col-12">
        <section className="card border-0 shadow-sm dashboard-card">
          <div className="card-body p-4">
            <div className="d-flex flex-wrap justify-content-between align-items-start gap-3 mb-4">
              <div>
                <h3 className="h4 mb-2">Company Editor</h3>
                <p className="text-secondary mb-0">
                  Select a company from the directory to update contact details, address, code, or login access.
                </p>
              </div>
              {selectedCompanyId && (
                <span className="badge text-bg-info-subtle text-info-emphasis">Editing selected company</span>
              )}
            </div>

            {editorError && <div className="alert alert-danger py-2 mb-3">{editorError}</div>}
            {status && <div className="alert alert-success py-2 mb-3">{status}</div>}

            {!selectedCompanyId && !detailsLoading && (
              <div className="alert alert-secondary py-2 mb-0">Choose a company from the list above to load the editor.</div>
            )}

            {detailsLoading && <div className="alert alert-secondary py-2 mb-0">Loading company details...</div>}

            {selectedCompanyId && !detailsLoading && (
              <form className="row g-3" onSubmit={submit}>
                <div className="col-md-6">
                  <label className="form-label">Company Name</label>
                  <input
                    className="form-control"
                    required
                    value={form.name}
                    onChange={(event) => setForm((current) => ({ ...current, name: event.target.value }))}
                  />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Company Code</label>
                  <input
                    className="form-control"
                    required
                    value={form.code}
                    onChange={(event) => setForm((current) => ({ ...current, code: event.target.value }))}
                  />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Contact Email</label>
                  <input
                    className="form-control"
                    type="email"
                    required
                    value={form.contactEmail}
                    onChange={(event) => setForm((current) => ({ ...current, contactEmail: event.target.value }))}
                  />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Contact Phone</label>
                  <input
                    className="form-control"
                    value={form.contactPhone}
                    onChange={(event) => setForm((current) => ({ ...current, contactPhone: event.target.value }))}
                  />
                </div>
                <div className="col-12">
                  <label className="form-label">Website</label>
                  <input
                    className="form-control"
                    placeholder="https://example.com"
                    value={form.website}
                    onChange={(event) => setForm((current) => ({ ...current, website: event.target.value }))}
                  />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Address Line 1</label>
                  <input
                    className="form-control"
                    value={form.addressLine1}
                    onChange={(event) => setForm((current) => ({ ...current, addressLine1: event.target.value }))}
                  />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Address Line 2</label>
                  <input
                    className="form-control"
                    value={form.addressLine2}
                    onChange={(event) => setForm((current) => ({ ...current, addressLine2: event.target.value }))}
                  />
                </div>
                <div className="col-md-4">
                  <label className="form-label">City</label>
                  <input
                    className="form-control"
                    value={form.city}
                    onChange={(event) => setForm((current) => ({ ...current, city: event.target.value }))}
                  />
                </div>
                <div className="col-md-4">
                  <label className="form-label">State</label>
                  <input
                    className="form-control"
                    value={form.state}
                    onChange={(event) => setForm((current) => ({ ...current, state: event.target.value }))}
                  />
                </div>
                <div className="col-md-4">
                  <label className="form-label">Postal Code</label>
                  <input
                    className="form-control"
                    value={form.postalCode}
                    onChange={(event) => setForm((current) => ({ ...current, postalCode: event.target.value }))}
                  />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Country</label>
                  <input
                    className="form-control"
                    value={form.country}
                    onChange={(event) => setForm((current) => ({ ...current, country: event.target.value }))}
                  />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Login Access</label>
                  <select
                    className="form-select"
                    value={form.isLoginAllowed ? 'allowed' : 'blocked'}
                    onChange={(event) => setForm((current) => ({ ...current, isLoginAllowed: event.target.value === 'allowed' }))}
                  >
                    <option value="allowed">Allow company login</option>
                    <option value="blocked">Block company login</option>
                  </select>
                </div>
                <div className="col-12">
                  <div className="editor-meta">
                    <span>Created: {formatDateTime(companies.find((company) => company.id === selectedCompanyId)?.createdAtUtc)}</span>
                    <span>Updated: {formatDateTime(companies.find((company) => company.id === selectedCompanyId)?.updatedAtUtc)}</span>
                  </div>
                </div>
                <div className="col-12">
                  <button className="btn btn-primary px-4" type="submit" disabled={saving}>
                    {saving ? 'Saving...' : 'Save Company'}
                  </button>
                </div>
              </form>
            )}
          </div>
        </section>
      </div>
    </div>
  );
}
