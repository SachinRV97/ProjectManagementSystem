import { useEffect, useRef, useState } from 'react';
import CompanyManagementPanel from '../components/CompanyManagementPanel';
import PortalDesigner from '../components/PortalDesigner';
import RoleMatrix from '../components/RoleMatrix';
import UserManagementPanel from '../components/UserManagementPanel';
import UserAccessManagementPanel from '../components/UserAccessManagementPanel';
import { canManageCompanyUsers, hasPermission, isGlobalAdmin, permissionNames } from '../services/access';

export default function Dashboard({ session, logout }) {
  const globalAdmin = isGlobalAdmin(session);
  const bannerRef = useRef(null);
  const sections = [
    {
      id: 'overview',
      label: 'Overview',
      description: globalAdmin ? 'System-wide company and access summary' : 'Company and access summary',
      visible: true,
    },
    {
      id: 'companies',
      label: 'Companies',
      description: 'Filter, edit, and block company login',
      visible: globalAdmin,
    },
    {
      id: 'access',
      label: 'User Access',
      description: 'Allow or block individual user login',
      visible: globalAdmin,
    },
    {
      id: 'users',
      label: 'User Setup',
      description: 'Register users for the current company',
      visible: canManageCompanyUsers(session),
    },
    {
      id: 'roles',
      label: 'Role Access',
      description: 'Review role capabilities',
      visible: true,
    },
    {
      id: 'portal',
      label: 'Portal Builder',
      description: 'Design pages, layout, and theme',
      visible: hasPermission(session, permissionNames.managePortal),
    },
  ].filter((section) => section.visible);

  const [activeSection, setActiveSection] = useState(sections[0]?.id ?? 'overview');
  const [isMenuVisible, setIsMenuVisible] = useState(true);
  const lastScrollYRef = useRef(0);

  useEffect(() => {
    if (!sections.some((section) => section.id === activeSection)) {
      setActiveSection(sections[0]?.id ?? 'overview');
    }
  }, [activeSection, sections]);

  useEffect(() => {
    const handleScroll = () => {
      const currentScrollY = window.scrollY || 0;
      const previousScrollY = lastScrollYRef.current;
      const scrollDelta = currentScrollY - previousScrollY;
      const bannerBottom = bannerRef.current?.getBoundingClientRect().bottom ?? 0;

      if (bannerBottom > 24) {
        setIsMenuVisible(true);
        lastScrollYRef.current = currentScrollY;
        return;
      }

      if (currentScrollY <= 24) {
        setIsMenuVisible(true);
      } else if (scrollDelta > 8) {
        setIsMenuVisible(false);
      } else if (scrollDelta < -8) {
        setIsMenuVisible(true);
      }

      lastScrollYRef.current = currentScrollY;
    };

    lastScrollYRef.current = window.scrollY || 0;
    window.addEventListener('scroll', handleScroll, { passive: true });

    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, []);

  const currentSection = sections.find((section) => section.id === activeSection) ?? sections[0];

  const renderOverview = () => {
    const customerScope = session.customerCode && session.customerCode !== 'GLOBAL'
      ? session.customerCode
      : 'Company-wide';
    const capabilityLabels = [];

    if (hasPermission(session, permissionNames.managePortal)) {
      capabilityLabels.push('Portal design');
    }

    if (hasPermission(session, permissionNames.manageEmployees)) {
      capabilityLabels.push('Employee management');
    }

    if (hasPermission(session, permissionNames.manageCustomers)) {
      capabilityLabels.push('Customer management');
    }

    if (canManageCompanyUsers(session)) {
      capabilityLabels.push('User registration');
    }

    if (globalAdmin) {
      capabilityLabels.push('Company control');
      capabilityLabels.push('User login blocking');
    }

    return (
      <div className="row g-4">
        <div className="col-md-6 col-xl-3">
          <article className="overview-card">
            <span className="overview-label">Signed in user</span>
            <h3>{session.name}</h3>
            <p className="mb-0">{session.role}</p>
          </article>
        </div>
        <div className="col-md-6 col-xl-3">
          <article className="overview-card">
            <span className="overview-label">Company</span>
            <h3>{session.companyName || 'Global System'}</h3>
            <p className="mb-0">{session.companyCode || 'GLOBAL'}</p>
          </article>
        </div>
        <div className="col-md-6 col-xl-3">
          <article className="overview-card">
            <span className="overview-label">Customer scope</span>
            <h3>{customerScope}</h3>
            <p className="mb-0">Current access boundary</p>
          </article>
        </div>
        <div className="col-md-6 col-xl-3">
          <article className="overview-card">
            <span className="overview-label">Capabilities</span>
            <h3>{capabilityLabels.length}</h3>
            <p className="mb-0">Enabled admin actions</p>
          </article>
        </div>
        <div className="col-lg-7">
          <section className="card border-0 shadow-sm dashboard-card h-100">
            <div className="card-body p-4">
              <h3 className="h4 mb-3">Access Summary</h3>
              <p className="text-secondary mb-4">
                Use the menu bar to move between company administration, access controls, role review, and the live portal designer.
              </p>
              <div className="role-chip-wrap justify-content-start">
                {capabilityLabels.map((label) => (
                  <span key={label} className="badge text-bg-primary-subtle text-primary-emphasis">{label}</span>
                ))}
                {capabilityLabels.length === 0 && (
                  <span className="badge text-bg-secondary">No elevated admin permissions</span>
                )}
              </div>
            </div>
          </section>
        </div>
        <div className="col-lg-5">
          <section className="card border-0 shadow-sm dashboard-card h-100">
            <div className="card-body p-4">
              <h3 className="h4 mb-3">Quick Guidance</h3>
              <div className="list-group list-group-flush">
                <div className="list-group-item px-0 bg-transparent border-secondary-subtle">
                  <div className="fw-semibold">1. Register company users</div>
                  <div className="text-secondary small">Use `User Setup` to create portal admins, portal employees, and customer users.</div>
                </div>
                {globalAdmin && (
                  <div className="list-group-item px-0 bg-transparent border-secondary-subtle">
                    <div className="fw-semibold">2. Review companies</div>
                    <div className="text-secondary small">Open `Companies` to filter the tenant list, edit company profiles, or block company login.</div>
                  </div>
                )}
                <div className="list-group-item px-0 bg-transparent border-secondary-subtle">
                  <div className="fw-semibold">{globalAdmin ? '3. Review role access' : '2. Review role access'}</div>
                  <div className="text-secondary small">Open `Role Access` to verify what each role can do before assigning it.</div>
                </div>
                <div className="list-group-item px-0 bg-transparent border-secondary-subtle">
                  <div className="fw-semibold">{globalAdmin ? '4. Customize the portal' : '3. Customize the portal'}</div>
                  <div className="text-secondary small">Use `Portal Builder` to create page layouts, preview content, and tune portal branding.</div>
                </div>
              </div>
            </div>
          </section>
        </div>
      </div>
    );
  };

  const renderActiveSection = () => {
    if (activeSection === 'companies') {
      return <CompanyManagementPanel />;
    }

    if (activeSection === 'access') {
      return <UserAccessManagementPanel session={session} />;
    }

    if (activeSection === 'users') {
      return <UserManagementPanel session={session} />;
    }

    if (activeSection === 'roles') {
      return <RoleMatrix />;
    }

    if (activeSection === 'portal') {
      return <PortalDesigner session={session} />;
    }

    return renderOverview();
  };

  return (
    <div className="container py-4 py-lg-5">
      <section ref={bannerRef} className="dashboard-banner mb-4">
        <div className="row g-4 align-items-center">
          <div className="col-lg-8">
            <span className="hero-kicker">Company control center</span>
            <h1 className="display-6 fw-semibold mt-3 mb-3">Project Management System</h1>
            <p className="mb-0 text-light-emphasis">
              Manage companies, control user access, assign customer roles, and shape the live portal experience from one dashboard.
            </p>
          </div>
          <div className="col-lg-4">
            <div className="dashboard-meta">
              <span className="badge text-bg-light">{session.name}</span>
              <span className="badge text-bg-warning-subtle text-warning-emphasis">{session.role}</span>
              <span className="badge text-bg-info-subtle text-info-emphasis">
                {(session.companyName || 'Global System')} ({session.companyCode || 'GLOBAL'})
              </span>
            </div>
          </div>
        </div>
      </section>

      <nav className={`card border-0 shadow-sm dashboard-card admin-nav mb-4 ${isMenuVisible ? 'admin-nav-visible' : 'admin-nav-hidden'}`}>
        <div className="card-body p-3">
          <div className="d-flex flex-column flex-lg-row align-items-lg-center justify-content-between gap-3">
            <div className="nav nav-pills admin-menu">
              {sections.map((section) => (
                <button
                  key={section.id}
                  type="button"
                  className={`nav-link ${activeSection === section.id ? 'active' : ''}`}
                  onClick={() => setActiveSection(section.id)}
                >
                  <span className="menu-title">{section.label}</span>
                  <span className="menu-description">{section.description}</span>
                </button>
              ))}
            </div>
            <button type="button" className="btn btn-outline-dark btn-sm px-4" onClick={logout}>Logout</button>
          </div>
        </div>
      </nav>

      <section className="section-shell">
        <div className="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4">
          <div>
            <span className="overview-label">Active section</span>
            <h2 className="h3 mb-1">{currentSection?.label}</h2>
            <p className="text-secondary mb-0">{currentSection?.description}</p>
          </div>
        </div>
        {renderActiveSection()}
      </section>
    </div>
  );
}
