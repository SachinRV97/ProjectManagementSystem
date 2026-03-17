import { useEffect, useState } from 'react';
import api from '../services/api';
import { hasPermission, permissionNames } from '../services/access';

const themePresets = {
  aurora: {
    label: 'Aurora',
    heroBackground: 'linear-gradient(135deg, rgba(13, 110, 253, 0.92), rgba(11, 114, 133, 0.88))',
    surfaceBackground: 'linear-gradient(180deg, #ffffff, #eef6ff)',
  },
  ocean: {
    label: 'Ocean',
    heroBackground: 'linear-gradient(135deg, rgba(3, 105, 161, 0.94), rgba(8, 145, 178, 0.86))',
    surfaceBackground: 'linear-gradient(180deg, #f4fbff, #e0f2fe)',
  },
  sunrise: {
    label: 'Sunrise',
    heroBackground: 'linear-gradient(135deg, rgba(234, 88, 12, 0.9), rgba(251, 146, 60, 0.86))',
    surfaceBackground: 'linear-gradient(180deg, #fff8f1, #ffedd5)',
  },
  midnight: {
    label: 'Midnight',
    heroBackground: 'linear-gradient(135deg, rgba(15, 23, 42, 0.96), rgba(30, 41, 59, 0.9))',
    surfaceBackground: 'linear-gradient(180deg, #e2e8f0, #cbd5e1)',
  },
};

const createPageId = () => (
  typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function'
    ? crypto.randomUUID()
    : `page-${Date.now()}`
);

const slugify = (value, fallback) => {
  const normalized = (value || fallback)
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, '-')
    .replace(/^-+|-+$/g, '');

  return normalized || fallback;
};

const buildNewPage = (index) => ({
  id: createPageId(),
  name: `Page ${index}`,
  slug: `page-${index}`,
  heroTitle: `Page ${index} Hero`,
  heroText: 'Describe the page purpose, audience, and top message for this screen.',
  sectionTitle: 'Highlights',
  sectionText: 'Use this section to introduce content blocks, services, or portal actions.',
  bulletPoints: ['Flexible layout', 'Editable highlights', 'Reusable CTA'],
  ctaLabel: 'Contact Team',
  ctaLink: '#',
  themePreset: 'aurora',
});

export default function PortalDesigner({ session }) {
  const [design, setDesign] = useState(null);
  const [selectedPageId, setSelectedPageId] = useState('');
  const [status, setStatus] = useState('');
  const [error, setError] = useState('');

  useEffect(() => {
    let isMounted = true;

    api.get('/portaldesign/me')
      .then(({ data }) => {
        if (!isMounted) {
          return;
        }

        setDesign({
          ...data,
          pages: Array.isArray(data.pages) && data.pages.length > 0
            ? data.pages
            : [buildNewPage(1)],
        });
        setSelectedPageId((current) => current || data.pages?.[0]?.id || 'home');
      })
      .catch((requestError) => {
        if (isMounted) {
          setError(requestError?.response?.data?.message ?? 'Unable to load portal design');
        }
      });

    return () => {
      isMounted = false;
    };
  }, []);

  const canEdit = hasPermission(session, permissionNames.managePortal);
  const pages = design?.pages ?? [];
  const selectedPage = pages.find((page) => page.id === selectedPageId) ?? pages[0] ?? null;
  const activeTheme = themePresets[selectedPage?.themePreset] ?? themePresets.aurora;

  useEffect(() => {
    if (selectedPage?.id && selectedPage.id !== selectedPageId) {
      setSelectedPageId(selectedPage.id);
    }
  }, [selectedPage, selectedPageId]);

  const updateDesign = (patch) => {
    setDesign((current) => ({
      ...current,
      ...patch,
    }));
  };

  const updateSelectedPage = (patch) => {
    if (!selectedPage) {
      return;
    }

    setDesign((current) => ({
      ...current,
      pages: current.pages.map((page) => (
        page.id === selectedPage.id
          ? { ...page, ...patch }
          : page
      )),
    }));
  };

  const addPage = () => {
    setDesign((current) => {
      const newPage = buildNewPage((current.pages?.length ?? 0) + 1);
      setSelectedPageId(newPage.id);

      return {
        ...current,
        pages: [...current.pages, newPage],
      };
    });
    setStatus('');
  };

  const removePage = () => {
    if (!selectedPage || pages.length <= 1) {
      return;
    }

    const remainingPages = pages.filter((page) => page.id !== selectedPage.id);
    setDesign((current) => ({
      ...current,
      pages: remainingPages,
    }));
    setSelectedPageId(remainingPages[0]?.id ?? '');
    setStatus('');
  };

  const save = async () => {
    if (!design) {
      return;
    }

    setStatus('Saving...');
    setError('');

    try {
      const payload = {
        headerTitle: design.headerTitle,
        footerText: design.footerText,
        primaryColor: design.primaryColor,
        accentColor: design.accentColor,
        showAnnouncements: design.showAnnouncements,
        announcementText: design.announcementText,
        pages: design.pages.map((page, index) => ({
          ...page,
          name: page.name.trim() || `Page ${index + 1}`,
          slug: slugify(page.slug, `page-${index + 1}`),
          heroTitle: page.heroTitle.trim() || page.name.trim() || `Page ${index + 1}`,
          heroText: page.heroText,
          sectionTitle: page.sectionTitle,
          sectionText: page.sectionText,
          bulletPoints: page.bulletPoints.filter(Boolean),
          ctaLabel: page.ctaLabel,
          ctaLink: page.ctaLink,
          themePreset: page.themePreset,
        })),
      };

      const { data } = await api.put(`/portaldesign/${design.customerCode}`, payload);
      setDesign({
        ...data,
        pages: data.pages?.length ? data.pages : [buildNewPage(1)],
      });
      setSelectedPageId((current) => current || data.pages?.[0]?.id || 'home');
      setStatus('Saved successfully');
    } catch (requestError) {
      setError(requestError?.response?.data?.message ?? 'Unable to save portal design');
      setStatus('');
    }
  };

  if (error && !design) {
    return <div className="alert alert-danger">{error}</div>;
  }

  if (!design || !selectedPage) {
    return <div className="alert alert-secondary">Loading portal design...</div>;
  }

  return (
    <section className="card border-0 shadow-sm dashboard-card">
      <div className="card-body p-4">
        <div className="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4">
          <div>
            <h3 className="h4 mb-2">Portal Page Builder</h3>
            <p className="text-secondary mb-0">
              Create multiple branded pages for the active portal scope, not just the header and footer.
            </p>
          </div>
          <span className="badge text-bg-light">Scope: {design.customerCode}</span>
        </div>

        {error && <div className="alert alert-danger py-2 mb-3">{error}</div>}

        <div className="row g-4">
          <div className="col-xl-5">
            <div className="designer-form">
              <div className="portal-builder-section">
                <div className="d-flex justify-content-between align-items-center gap-3 mb-3">
                  <div>
                    <span className="overview-label">Portal Shell</span>
                    <h4 className="h5 mb-0">Branding and Global Layout</h4>
                  </div>
                  <span className="badge text-bg-secondary">{pages.length} page{pages.length === 1 ? '' : 's'}</span>
                </div>

                <div className="row g-3">
                  <div className="col-12">
                    <label className="form-label">Header Title</label>
                    <input className="form-control" value={design.headerTitle} onChange={(event) => updateDesign({ headerTitle: event.target.value })} disabled={!canEdit} />
                  </div>
                  <div className="col-12">
                    <label className="form-label">Footer Text</label>
                    <input className="form-control" value={design.footerText} onChange={(event) => updateDesign({ footerText: event.target.value })} disabled={!canEdit} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">Primary Color</label>
                    <input className="form-control form-control-color w-100" type="color" value={design.primaryColor} onChange={(event) => updateDesign({ primaryColor: event.target.value })} disabled={!canEdit} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">Accent Color</label>
                    <input className="form-control form-control-color w-100" type="color" value={design.accentColor} onChange={(event) => updateDesign({ accentColor: event.target.value })} disabled={!canEdit} />
                  </div>
                  <div className="col-12">
                    <div className="form-check form-switch">
                      <input
                        className="form-check-input"
                        id="announcementToggle"
                        type="checkbox"
                        checked={design.showAnnouncements}
                        onChange={(event) => updateDesign({ showAnnouncements: event.target.checked })}
                        disabled={!canEdit}
                      />
                      <label className="form-check-label" htmlFor="announcementToggle">Show announcement bar</label>
                    </div>
                  </div>
                  <div className="col-12">
                    <label className="form-label">Announcement Text</label>
                    <input className="form-control" value={design.announcementText || ''} onChange={(event) => updateDesign({ announcementText: event.target.value })} disabled={!canEdit || !design.showAnnouncements} />
                  </div>
                </div>
              </div>

              <div className="portal-builder-section mt-4">
                <div className="d-flex flex-wrap justify-content-between align-items-center gap-2 mb-3">
                  <div>
                    <span className="overview-label">Page List</span>
                    <h4 className="h5 mb-0">Manage Pages</h4>
                  </div>
                  {canEdit && (
                    <button type="button" className="btn btn-outline-primary btn-sm" onClick={addPage}>
                      Add Page
                    </button>
                  )}
                </div>

                <div className="page-tab-list">
                  {pages.map((page) => (
                    <button
                      key={page.id}
                      type="button"
                      className={`page-tab ${selectedPage.id === page.id ? 'active' : ''}`}
                      onClick={() => setSelectedPageId(page.id)}
                    >
                      <span className="page-tab-name">{page.name}</span>
                      <span className="page-tab-slug">/{page.slug}</span>
                    </button>
                  ))}
                </div>

                <div className="row g-3 mt-1">
                  <div className="col-md-6">
                    <label className="form-label">Page Name</label>
                    <input className="form-control" value={selectedPage.name} onChange={(event) => updateSelectedPage({ name: event.target.value })} disabled={!canEdit} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">Page Slug</label>
                    <input className="form-control" value={selectedPage.slug} onChange={(event) => updateSelectedPage({ slug: slugify(event.target.value, 'page') })} disabled={!canEdit} />
                  </div>
                  <div className="col-12">
                    <label className="form-label">Hero Title</label>
                    <input className="form-control" value={selectedPage.heroTitle} onChange={(event) => updateSelectedPage({ heroTitle: event.target.value })} disabled={!canEdit} />
                  </div>
                  <div className="col-12">
                    <label className="form-label">Hero Text</label>
                    <textarea className="form-control" rows="3" value={selectedPage.heroText} onChange={(event) => updateSelectedPage({ heroText: event.target.value })} disabled={!canEdit} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">Section Title</label>
                    <input className="form-control" value={selectedPage.sectionTitle} onChange={(event) => updateSelectedPage({ sectionTitle: event.target.value })} disabled={!canEdit} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">Theme Preset</label>
                    <select className="form-select" value={selectedPage.themePreset} onChange={(event) => updateSelectedPage({ themePreset: event.target.value })} disabled={!canEdit}>
                      {Object.entries(themePresets).map(([value, theme]) => (
                        <option key={value} value={value}>{theme.label}</option>
                      ))}
                    </select>
                  </div>
                  <div className="col-12">
                    <label className="form-label">Section Text</label>
                    <textarea className="form-control" rows="3" value={selectedPage.sectionText} onChange={(event) => updateSelectedPage({ sectionText: event.target.value })} disabled={!canEdit} />
                  </div>
                  <div className="col-12">
                    <label className="form-label">Highlight Points</label>
                    <textarea
                      className="form-control"
                      rows="4"
                      value={selectedPage.bulletPoints.join('\n')}
                      onChange={(event) => updateSelectedPage({
                        bulletPoints: event.target.value.split('\n').map((item) => item.trim()).filter(Boolean),
                      })}
                      disabled={!canEdit}
                    />
                    <div className="form-text">One line per highlight card.</div>
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">CTA Label</label>
                    <input className="form-control" value={selectedPage.ctaLabel} onChange={(event) => updateSelectedPage({ ctaLabel: event.target.value })} disabled={!canEdit} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">CTA Link</label>
                    <input className="form-control" value={selectedPage.ctaLink} onChange={(event) => updateSelectedPage({ ctaLink: event.target.value })} disabled={!canEdit} />
                  </div>
                </div>

                {canEdit && (
                  <div className="d-flex flex-wrap gap-2 mt-3">
                    <button type="button" className="btn btn-outline-danger btn-sm" onClick={removePage} disabled={pages.length <= 1}>
                      Remove Page
                    </button>
                  </div>
                )}
              </div>

              <div className="d-flex flex-wrap gap-2 mt-4">
                {canEdit ? (
                  <button className="btn btn-primary" onClick={save}>Save Design</button>
                ) : (
                  <span className="badge text-bg-secondary">View only for this role</span>
                )}
                {status && <span className="badge text-bg-success-subtle text-success-emphasis">{status}</span>}
              </div>
            </div>
          </div>

          <div className="col-xl-7">
            <div className="preview-frame">
              <div className="preview-browser-bar">
                <span />
                <span />
                <span />
              </div>
              <div className="preview preview-portal" style={{ borderColor: design.primaryColor }}>
                <header className="preview-portal-header" style={{ background: design.primaryColor }}>
                  <div>{design.headerTitle}</div>
                  <div className="preview-nav">
                    {pages.map((page) => (
                      <button
                        key={page.id}
                        type="button"
                        className={`preview-nav-link ${selectedPage.id === page.id ? 'active' : ''}`}
                        onClick={() => setSelectedPageId(page.id)}
                      >
                        {page.name}
                      </button>
                    ))}
                  </div>
                </header>
                {design.showAnnouncements && <div className="announcement">{design.announcementText}</div>}
                <main className="preview-page" style={{ background: activeTheme.surfaceBackground }}>
                  <section className="preview-hero" style={{ background: activeTheme.heroBackground }}>
                    <span className="preview-page-tag">/{selectedPage.slug}</span>
                    <h3>{selectedPage.heroTitle}</h3>
                    <p className="mb-0">{selectedPage.heroText}</p>
                  </section>

                  <section className="preview-section-grid">
                    <div className="preview-copy">
                      <h4>{selectedPage.sectionTitle}</h4>
                      <p>{selectedPage.sectionText}</p>
                      <button type="button" className="btn btn-light preview-cta">
                        {selectedPage.ctaLabel}
                      </button>
                      <div className="small text-secondary mt-2">Link target: {selectedPage.ctaLink || '#'}</div>
                    </div>

                    <div className="preview-feature-stack">
                      {selectedPage.bulletPoints.map((item) => (
                        <article key={item} className="preview-feature-card">
                          <span className="feature-label">Content Block</span>
                          <p className="mb-0">{item}</p>
                        </article>
                      ))}
                    </div>
                  </section>
                </main>
                <footer style={{ background: design.accentColor }}>{design.footerText}</footer>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
