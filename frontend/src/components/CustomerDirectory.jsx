import { useEffect, useState } from 'react';
import api from '../services/api';

export default function CustomerDirectory() {
  const [customers, setCustomers] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    api.get('/customers')
      .then(({ data }) => setCustomers(data))
      .catch((requestError) => setError(requestError?.response?.data?.message ?? 'Unable to load customers.'));
  }, []);

  return (
    <section className="panel">
      <div className="section-header">
        <div>
          <p className="eyebrow">Customer operations</p>
          <h2>Customer directory</h2>
        </div>
        <span className="badge">{customers.length} customers</span>
      </div>

      {error && <p className="error">{error}</p>}

      <div className="card-grid">
        {customers.map((customer) => (
          <article key={customer.customerCode} className="summary-card">
            <p className="eyebrow">{customer.customerCode}</p>
            <h3>{customer.totalUsers} total users</h3>
            <ul>
              <li>{customer.portalEditors} portal editors/admins</li>
              <li>{customer.activeUsers} active users</li>
              <li>Last portal update: {customer.lastPortalUpdateUtc ?? 'N/A'}</li>
            </ul>
          </article>
        ))}
      </div>
    </section>
  );
}
