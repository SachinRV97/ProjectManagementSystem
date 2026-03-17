import { useAuth } from './context/AuthContext';
import AuthPage from './pages/AuthPage';
import Dashboard from './pages/Dashboard';
import PortalSitePreviewPage from './pages/PortalSitePreviewPage';

export default function App() {
  const { session, logout } = useAuth();
  const isPortalPreviewRoute = typeof window !== 'undefined' && window.location.pathname !== '/';

  if (isPortalPreviewRoute) {
    return <PortalSitePreviewPage />;
  }

  if (!session) {
    return (
      <div className="app-shell">
        <div className="container py-5">
          <div className="row g-4 align-items-stretch">
            <div className="col-xl-6">
              <section className="hero-panel h-100">
                <span className="hero-kicker">Company-first Bootstrap workspace</span>
                <h1 className="display-5 fw-semibold mt-3 mb-3">Launch a polished multi-tenant portal with a cleaner registration flow.</h1>
                <p className="hero-copy mb-4">
                  Register a company, sign in with the primary login account, then create portal and customer users with clear access boundaries and a more modern dashboard.
                </p>
                <div className="row g-3">
                  <div className="col-sm-6">
                    <div className="feature-tile">
                      <span className="feature-label">Bootstrap UI</span>
                      <p className="mb-0">Responsive cards, structured forms, and stronger hierarchy across the app.</p>
                    </div>
                  </div>
                  <div className="col-sm-6">
                    <div className="feature-tile">
                      <span className="feature-label">Company Flow</span>
                      <p className="mb-0">Onboard a company first, then create portal and customer users from the dashboard.</p>
                    </div>
                  </div>
                  <div className="col-sm-6">
                    <div className="feature-tile">
                      <span className="feature-label">Dynamic Design</span>
                      <p className="mb-0">Live theme editing for each company or customer scope with real-time preview.</p>
                    </div>
                  </div>
                  <div className="col-sm-6">
                    <div className="feature-tile">
                      <span className="feature-label">Role Aware</span>
                      <p className="mb-0">The dashboard adapts to role permissions and shows the assignable path clearly.</p>
                    </div>
                  </div>
                </div>
              </section>
            </div>
            <div className="col-xl-6">
              <AuthPage />
            </div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="app-shell">
      <Dashboard session={session} logout={logout} />
    </div>
  );
}
