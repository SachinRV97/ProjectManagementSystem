import { useEffect, useState } from 'react';
import api from '../services/api';
import {
  buildPortalSiteUrl,
  getPortalRouteParts,
  slugifySiteValue,
  themePresets,
} from '../services/portalSite';

export default function PortalSitePreviewPage() {
  const [{ siteSegment, pageSegment }, setRouteState] = useState(() => getPortalRouteParts(window.location.pathname));
  const [design, setDesign] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    const syncRoute = () => {
      setRouteState(getPortalRouteParts(window.location.pathname));
    };

    window.addEventListener('popstate', syncRoute);
    return () => {
      window.removeEventListener('popstate', syncRoute);
    };
  }, []);

  useEffect(() => {
    let isMounted = true;

    api.get(`/portaldesign/site/${encodeURIComponent(siteSegment)}`)
      .then(({ data }) => {
        if (!isMounted) {
          return;
        }

        setDesign(data);
        setError('');
      })
      .catch((requestError) => {
        if (isMounted) {
          setDesign(null);
          setError(requestError?.response?.data?.message ?? 'Portal site was not found.');
        }
      });

    return () => {
      isMounted = false;
    };
  }, [siteSegment]);

  if (!siteSegment) {
    return null;
  }

  if (error) {
    return (
      <div className="portal-site-shell">
        <div className="container py-5">
          <div className="card border-0 shadow-sm dashboard-card">
            <div className="card-body p-4 p-lg-5 text-center">
              <span className="hero-kicker">Portal Preview</span>
              <h1 className="display-6 fw-semibold mt-3 mb-3">Portal site not found</h1>
              <p className="text-secondary mb-4">{error}</p>
              <a className="btn btn-primary" href="/">Back to Sign In</a>
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (!design) {
    return (
      <div className="portal-site-shell">
        <div className="container py-5">
          <div className="alert alert-secondary">Loading portal preview...</div>
        </div>
      </div>
    );
  }

  const pages = design.pages ?? [];
  const normalizedPageSegment = slugifySiteValue(pageSegment || pages[0]?.slug || 'home');
  const activePage = pages.find((page) => page.slug === normalizedPageSegment) ?? pages[0];
  const activeTheme = themePresets[activePage?.themePreset] ?? themePresets.aurora;

  return (
    <div className="portal-site-shell">
      <div className="portal-site-topbar">
        <div className="container d-flex flex-wrap justify-content-between align-items-center gap-3">
          <div>
            <span className="hero-kicker">Portal Preview</span>
            <div className="small text-light-emphasis mt-2">Site: {design.siteName}</div>
          </div>
          <div className="d-flex flex-wrap gap-2">
            <a className="btn btn-light btn-sm" href={buildPortalSiteUrl(design.siteName, design.siteSlug)}>
              Home
            </a>
            <a className="btn btn-outline-light btn-sm" href="/">
              Admin Login
            </a>
          </div>
        </div>
      </div>

      <main className="portal-site-page">
        <section className="portal-site-hero" style={{ background: activeTheme.heroBackground }}>
          <div className="container py-5">
            <div className="row g-4 align-items-center">
              <div className="col-lg-7">
                <span className="preview-page-tag">/{activePage.slug}</span>
                <h1 className="display-5 fw-semibold mt-3 mb-3">{activePage.heroTitle}</h1>
                <p className="lead mb-4">{activePage.heroText}</p>
                <a className="btn btn-light btn-lg" href={activePage.ctaLink || '#'}>
                  {activePage.ctaLabel}
                </a>
              </div>
              <div className="col-lg-5">
                <div className="portal-site-hero-card">
                  <div className="portal-site-brand">{design.headerTitle}</div>
                  <div className="portal-site-route">/{design.siteSlug}</div>
                  <div className="portal-site-route">{pageSegment ? `/${activePage.slug}` : '/home'}</div>
                </div>
              </div>
            </div>
          </div>
        </section>

        <section className="portal-site-nav-wrap" style={{ background: design.primaryColor }}>
          <div className="container">
            <div className="portal-site-nav">
              {pages.map((page) => (
                <a
                  key={page.id}
                  className={`portal-site-nav-link ${activePage.id === page.id ? 'active' : ''}`}
                  href={buildPortalSiteUrl(design.siteName, design.siteSlug, page.slug)}
                >
                  {page.name}
                </a>
              ))}
            </div>
          </div>
        </section>

        {design.showAnnouncements && (
          <section className="announcement">
            <div className="container">{design.announcementText}</div>
          </section>
        )}

        <section className="portal-site-content" style={{ background: activeTheme.surfaceBackground }}>
          <div className="container py-5">
            <div className="row g-4 align-items-start">
              <div className="col-lg-7">
                <article className="portal-site-copy">
                  <span className="overview-label">Page Section</span>
                  <h2 className="display-6 fw-semibold mt-2 mb-3">{activePage.sectionTitle}</h2>
                  <p className="lead text-secondary mb-0">{activePage.sectionText}</p>
                </article>
              </div>
              <div className="col-lg-5">
                <div className="portal-site-highlight-stack">
                  {activePage.bulletPoints.map((item) => (
                    <article key={item} className="portal-site-highlight-card">
                      <span className="feature-label">Highlight</span>
                      <p className="mb-0">{item}</p>
                    </article>
                  ))}
                </div>
              </div>
            </div>
          </div>
        </section>
      </main>

      <footer className="portal-site-footer" style={{ background: design.accentColor }}>
        <div className="container d-flex flex-wrap justify-content-between align-items-center gap-3">
          <div>{design.footerText}</div>
          <div className="small">{design.siteName}</div>
        </div>
      </footer>
    </div>
  );
}
