import { useEffect, useState } from 'react';
import { getCompanies, getManagedUsers, updateUserLoginStatus } from '../services/admin';

const statusFilterToValue = (loginStatus) => {
  if (loginStatus === 'allowed') {
    return true;
  }

  if (loginStatus === 'blocked') {
    return false;
  }

  return undefined;
};

const formatDate = (value) => (
  value ? new Date(value).toLocaleDateString() : 'N/A'
);

export default function UserAccessManagementPanel({ session }) {
  const [companies, setCompanies] = useState([]);
  const [users, setUsers] = useState([]);
  const [filters, setFilters] = useState({
    search: '',
    companyCode: 'all',
    loginStatus: 'all',
  });
  const [loading, setLoading] = useState(true);
  const [togglingUserId, setTogglingUserId] = useState('');
  const [error, setError] = useState('');
  const [status, setStatus] = useState('');

  useEffect(() => {
    let isMounted = true;

    const loadCompanies = async () => {
      try {
        const data = await getCompanies();
        if (isMounted) {
          setCompanies(data);
        }
      } catch {
        if (isMounted) {
          setCompanies([]);
        }
      }
    };

    loadCompanies();

    return () => {
      isMounted = false;
    };
  }, []);

  const loadUsers = async () => {
    setLoading(true);
    setError('');

    try {
      const data = await getManagedUsers({
        search: filters.search.trim() || undefined,
        companyCode: filters.companyCode === 'all' ? undefined : filters.companyCode,
        isLoginAllowed: statusFilterToValue(filters.loginStatus),
      });

      setUsers(data);
    } catch (e) {
      setError(e?.response?.data?.message ?? 'Unable to load users');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadUsers();
  }, [filters.search, filters.companyCode, filters.loginStatus]);

  const toggleUser = async (user) => {
    setTogglingUserId(user.id);
    setError('');
    setStatus('');

    try {
      const updatedUser = await updateUserLoginStatus(user.id, !user.isLoginAllowed);
      setUsers((current) => current.map((item) => (
        item.id === updatedUser.id ? updatedUser : item
      )));
      setStatus(`${updatedUser.email} login is now ${updatedUser.isLoginAllowed ? 'allowed' : 'blocked'}.`);
    } catch (e) {
      setError(e?.response?.data?.message ?? 'Unable to update user login status');
    } finally {
      setTogglingUserId('');
    }
  };

  return (
    <section className="card border-0 shadow-sm dashboard-card">
      <div className="card-body p-4">
        <div className="d-flex flex-wrap justify-content-between align-items-start gap-3 mb-4">
          <div>
            <h3 className="h4 mb-2">User Login Control</h3>
            <p className="text-secondary mb-0">
              Global admin <strong>{session.name}</strong> can allow or block login for any company user from one screen.
            </p>
          </div>
          <button type="button" className="btn btn-outline-dark" onClick={loadUsers}>
            Refresh Users
          </button>
        </div>

        {error && <div className="alert alert-danger py-2 mb-3">{error}</div>}
        {status && <div className="alert alert-success py-2 mb-3">{status}</div>}

        <div className="row g-3 align-items-end">
          <div className="col-lg-5">
            <label className="form-label">Search</label>
            <input
              className="form-control"
              placeholder="Search by user, email, role, company, or customer code"
              value={filters.search}
              onChange={(event) => setFilters((current) => ({ ...current, search: event.target.value }))}
            />
          </div>
          <div className="col-md-4 col-lg-3">
            <label className="form-label">Company</label>
            <select
              className="form-select"
              value={filters.companyCode}
              onChange={(event) => setFilters((current) => ({ ...current, companyCode: event.target.value }))}
            >
              <option value="all">All companies</option>
              {companies.map((company) => (
                <option key={company.id} value={company.code}>
                  {company.name} ({company.code})
                </option>
              ))}
            </select>
          </div>
          <div className="col-md-4 col-lg-2">
            <label className="form-label">Login Status</label>
            <select
              className="form-select"
              value={filters.loginStatus}
              onChange={(event) => setFilters((current) => ({ ...current, loginStatus: event.target.value }))}
            >
              <option value="all">All users</option>
              <option value="allowed">Login allowed</option>
              <option value="blocked">Login blocked</option>
            </select>
          </div>
          <div className="col-md-4 col-lg-2">
            <div className="directory-stats">
              <span className="overview-label">Visible users</span>
              <h4 className="mb-0">{users.length}</h4>
            </div>
          </div>
        </div>

        <div className="table-responsive mt-4">
          <table className="table align-middle admin-table mb-0">
            <thead>
              <tr>
                <th>User</th>
                <th>Company</th>
                <th>Role</th>
                <th>Customer</th>
                <th>Status</th>
                <th>Created</th>
                <th className="text-end">Action</th>
              </tr>
            </thead>
            <tbody>
              {loading && (
                <tr>
                  <td colSpan="7">
                    <div className="alert alert-secondary py-2 mb-0">Loading users...</div>
                  </td>
                </tr>
              )}
              {!loading && users.length === 0 && (
                <tr>
                  <td colSpan="7">
                    <div className="alert alert-secondary py-2 mb-0">No users matched the current filter.</div>
                  </td>
                </tr>
              )}
              {!loading && users.map((user) => (
                <tr key={user.id}>
                  <td>
                    <div className="fw-semibold">{user.name}</div>
                    <div className="small text-secondary">{user.email}</div>
                  </td>
                  <td>
                    <div>{user.companyName}</div>
                    <div className="small text-secondary">{user.companyCode}</div>
                  </td>
                  <td>{user.role}</td>
                  <td>{user.customerCode || 'GLOBAL'}</td>
                  <td>
                    <span className={`badge ${user.isLoginAllowed ? 'text-bg-success-subtle text-success-emphasis' : 'text-bg-danger-subtle text-danger-emphasis'}`}>
                      {user.isLoginAllowed ? 'Allowed' : 'Blocked'}
                    </span>
                  </td>
                  <td>{formatDate(user.createdAtUtc)}</td>
                  <td className="text-end">
                    <button
                      type="button"
                      className={`btn btn-sm ${user.isLoginAllowed ? 'btn-outline-danger' : 'btn-outline-success'}`}
                      disabled={togglingUserId === user.id}
                      onClick={() => toggleUser(user)}
                    >
                      {togglingUserId === user.id
                        ? 'Updating...'
                        : user.isLoginAllowed ? 'Block Login' : 'Allow Login'}
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </section>
  );
}
