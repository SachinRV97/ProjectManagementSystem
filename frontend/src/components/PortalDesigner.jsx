<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
import { useEffect, useState } from 'react';
import api from '../services/api';
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
import { hasPermission, permissionNames } from '../services/access';
import {
  buildNewPage,
  buildPortalSiteUrl,
  slugifySiteValue,
  themePresets,
} from '../services/portalSite';

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
          siteName: data.siteName || data.headerTitle,
          siteSlug: data.siteSlug || slugifySiteValue(data.siteName || data.headerTitle),
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
  const previewUrl = buildPortalSiteUrl(design?.siteName, design?.siteSlug, selectedPage?.slug);

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
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
import { useEffect, useMemo, useState } from 'react';
import api from '../services/api';

const emptyNavigation = { label: '', href: '', sortOrder: 1, openInNewTab: false };
const emptySection = { sectionKey: '', title: '', body: '', sortOrder: 1 };

export default function PortalDesigner({ session }) {
  const [design, setDesign] = useState(null);
  const [selectedCustomerCode, setSelectedCustomerCode] = useState(session.customerCode || 'GLOBAL');
  const [status, setStatus] = useState('');
  const [error, setError] = useState('');

  const canSwitchCustomer = useMemo(
    () => ['Admin', 'Portal-Admin'].includes(session.role),
    [session.role],
  );

  const canEdit = useMemo(
    () => ['Admin', 'Portal-Admin', 'Customer-Employee'].includes(session.role),
    [session.role],
  );

  const loadDesign = async (customerCode) => {
    setError('');
    const endpoint = canSwitchCustomer ? `/portaldesign/${customerCode}` : '/portaldesign/me';
    const { data } = await api.get(endpoint);
    setDesign(data);
    setSelectedCustomerCode(data.customerCode);
  };

  useEffect(() => {
    loadDesign(selectedCustomerCode).catch((requestError) => {
      setError(requestError?.response?.data?.message ?? 'Unable to load portal design.');
    });
  }, [selectedCustomerCode]);

  const updateField = (field, value) => {
    setDesign((current) => ({ ...current, [field]: value }));
  };

  const updateNavigationItem = (index, field, value) => {
    setDesign((current) => ({
      ...current,
      navigationItems: current.navigationItems.map((item, itemIndex) => (
        itemIndex === index ? { ...item, [field]: value } : item
      )),
    }));
  };

  const updateSection = (index, field, value) => {
    setDesign((current) => ({
      ...current,
      contentSections: current.contentSections.map((item, itemIndex) => (
        itemIndex === index ? { ...item, [field]: value } : item
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
      )),
    }));
  };

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
        siteName: design.siteName,
        pages: design.pages.map((page, index) => ({
          ...page,
          name: page.name.trim() || `Page ${index + 1}`,
          slug: slugifySiteValue(page.slug, `page-${index + 1}`),
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
        siteName: data.siteName || data.headerTitle,
        siteSlug: data.siteSlug || slugifySiteValue(data.siteName || data.headerTitle),
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
                    <label className="form-label">Site Name</label>
                    <input
                      className="form-control"
                      value={design.siteName || ''}
                      onChange={(event) => updateDesign({
                        siteName: event.target.value,
                        siteSlug: slugifySiteValue(event.target.value, 'portal'),
                      })}
                      disabled={!canEdit}
                    />
                    <div className="form-text">This creates the public portal URL for preview and redirect.</div>
                  </div>
                  <div className="col-12">
                    <label className="form-label">Site Preview URL</label>
                    <input className="form-control" value={previewUrl} readOnly />
                    <div className="form-text">Save the design after changing the site name so the public route stays in sync.</div>
                  </div>
                  <div className="col-12 d-flex flex-wrap gap-2">
                    <a className="btn btn-outline-dark btn-sm" href={buildPortalSiteUrl(design.siteName, design.siteSlug)} target="_blank" rel="noreferrer">
                      Open Site Home
                    </a>
                    <a className="btn btn-outline-primary btn-sm" href={previewUrl} target="_blank" rel="noreferrer">
                      Open Selected Page
                    </a>
                  </div>
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
                    <input className="form-control" value={selectedPage.slug} onChange={(event) => updateSelectedPage({ slug: slugifySiteValue(event.target.value, 'page') })} disabled={!canEdit} />
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
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs

export default function PortalDesigner({ session }) {
  const [design, setDesign] = useState(null);
  const [status, setStatus] = useState('');

  useEffect(() => {
    api.get('/portaldesign/me').then(({ data }) => setDesign(data));
  }, []);

  const canEdit = ['Admin', 'Portal-Admin', 'Customer-Employee'].includes(session.role);

  const save = async () => {
    setStatus('Saving...');
    const { data } = await api.put(`/portaldesign/${design.customerCode}`, {
      headerTitle: design.headerTitle,
      footerText: design.footerText,
      primaryColor: design.primaryColor,
      accentColor: design.accentColor,
      showAnnouncements: design.showAnnouncements,
      announcementText: design.announcementText,
    });
    setDesign(data);
    setStatus('Saved successfully');
  };

  if (!design) return <p>Loading portal design...</p>;

  return (
    <section className="panel">
      <h3>Portal Layout Builder</h3>
      <div className="grid">
        <label>
          Header Title
          <input value={design.headerTitle} onChange={(e) => setDesign({ ...design, headerTitle: e.target.value })} disabled={!canEdit} />
        </label>
        <label>
          Footer Text
          <input value={design.footerText} onChange={(e) => setDesign({ ...design, footerText: e.target.value })} disabled={!canEdit} />
        </label>
        <label>
          Primary Color
          <input value={design.primaryColor} onChange={(e) => setDesign({ ...design, primaryColor: e.target.value })} disabled={!canEdit} />
        </label>
        <label>
          Accent Color
          <input value={design.accentColor} onChange={(e) => setDesign({ ...design, accentColor: e.target.value })} disabled={!canEdit} />
        </label>
        <label>
          Announcement Text
          <input value={design.announcementText || ''} onChange={(e) => setDesign({ ...design, announcementText: e.target.value })} disabled={!canEdit} />
        </label>
      </div>

      {canEdit ? <button onClick={save}>Save Design</button> : <p>Your role can view but not modify portal design.</p>}
      <p>{status}</p>

      <div className="preview" style={{ borderColor: design.primaryColor }}>
        <header style={{ background: design.primaryColor }}>{design.headerTitle}</header>
        {design.showAnnouncements && <div className="announcement">{design.announcementText}</div>}
        <main>
          This preview represents header, footer and theme setup managed by your centralized portal builder.
        </main>
        <footer style={{ background: design.accentColor }}>{design.footerText}</footer>
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
=======
>>>>>>> theirs
=======
>>>>>>> theirs
  const addNavigationItem = () => {
    setDesign((current) => ({
      ...current,
      navigationItems: [
        ...current.navigationItems,
        { ...emptyNavigation, sortOrder: current.navigationItems.length + 1 },
      ],
    }));
  };

  const addSection = () => {
    setDesign((current) => ({
      ...current,
      contentSections: [
        ...current.contentSections,
        { ...emptySection, sortOrder: current.contentSections.length + 1 },
      ],
    }));
  };

  const removeNavigationItem = (index) => {
    setDesign((current) => ({
      ...current,
      navigationItems: current.navigationItems.filter((_, itemIndex) => itemIndex !== index),
    }));
  };

  const removeSection = (index) => {
    setDesign((current) => ({
      ...current,
      contentSections: current.contentSections.filter((_, itemIndex) => itemIndex !== index),
    }));
  };

  const save = async () => {
    if (!canEdit || !design) {
      return;
    }

    setStatus('Saving changes...');
    setError('');

    try {
      const { data } = await api.put(`/portaldesign/${design.customerCode}`, design);
      setDesign(data);
      setStatus('Portal settings saved successfully.');
    } catch (requestError) {
      setStatus('');
      setError(requestError?.response?.data?.message ?? 'Unable to save portal settings.');
    }
  };

  if (!design) {
    return (
      <section className="panel">
        <p>Loading portal designer...</p>
        {error && <p className="error">{error}</p>}
      </section>
    );
  }

  return (
    <section className="panel">
      <div className="section-header">
        <div>
          <p className="eyebrow">Portal builder</p>
          <h2>Header, footer, menu, and homepage sections</h2>
        </div>
        <div className="toolbar compact">
          {canSwitchCustomer && (
            <input
              value={selectedCustomerCode}
              onChange={(event) => setSelectedCustomerCode(event.target.value.toUpperCase())}
              placeholder="Customer Code"
            />
          )}
          <span className="badge">{design.customerCode}</span>
        </div>
      </div>

      <div className="designer-layout">
        <div className="designer-form">
          <div className="grid two-col">
            <label>
              Portal name
              <input value={design.portalName} onChange={(event) => updateField('portalName', event.target.value)} disabled={!canEdit} />
            </label>
            <label>
              Header title
              <input value={design.headerTitle} onChange={(event) => updateField('headerTitle', event.target.value)} disabled={!canEdit} />
            </label>
            <label>
              Footer text
              <input value={design.footerText} onChange={(event) => updateField('footerText', event.target.value)} disabled={!canEdit} />
            </label>
            <label>
              Support email
              <input value={design.supportEmail} onChange={(event) => updateField('supportEmail', event.target.value)} disabled={!canEdit} />
            </label>
            <label>
              Primary color
              <input value={design.primaryColor} onChange={(event) => updateField('primaryColor', event.target.value)} disabled={!canEdit} />
            </label>
            <label>
              Secondary color
              <input value={design.secondaryColor} onChange={(event) => updateField('secondaryColor', event.target.value)} disabled={!canEdit} />
            </label>
            <label>
              Hero title
              <input value={design.heroTitle} onChange={(event) => updateField('heroTitle', event.target.value)} disabled={!canEdit} />
            </label>
            <label>
              Hero subtitle
              <textarea value={design.heroSubtitle} onChange={(event) => updateField('heroSubtitle', event.target.value)} disabled={!canEdit} rows="3" />
            </label>
            <label>
              Announcement text
              <textarea value={design.announcementText} onChange={(event) => updateField('announcementText', event.target.value)} disabled={!canEdit} rows="3" />
            </label>
            <label>
              Logo URL
              <input value={design.logoUrl} onChange={(event) => updateField('logoUrl', event.target.value)} disabled={!canEdit} />
            </label>
          </div>

          <label className="toggle-row">
            <input
              type="checkbox"
              checked={design.showAnnouncements}
              onChange={(event) => updateField('showAnnouncements', event.target.checked)}
              disabled={!canEdit}
            />
            <span>Show announcement bar</span>
          </label>

          <div className="nested-panel">
            <div className="section-header compact">
              <h3>Navigation links</h3>
              {canEdit && <button onClick={addNavigationItem}>Add menu item</button>}
            </div>

            {design.navigationItems.map((item, index) => (
              <div key={`${item.label}-${index}`} className="grid nav-row">
                <input value={item.label} onChange={(event) => updateNavigationItem(index, 'label', event.target.value)} placeholder="Label" disabled={!canEdit} />
                <input value={item.href} onChange={(event) => updateNavigationItem(index, 'href', event.target.value)} placeholder="Link" disabled={!canEdit} />
                <input
                  type="number"
                  value={item.sortOrder}
                  onChange={(event) => updateNavigationItem(index, 'sortOrder', Number(event.target.value))}
                  placeholder="Sort"
                  disabled={!canEdit}
                />
                <label className="toggle-row inline">
                  <input
                    type="checkbox"
                    checked={item.openInNewTab}
                    onChange={(event) => updateNavigationItem(index, 'openInNewTab', event.target.checked)}
                    disabled={!canEdit}
                  />
                  <span>New tab</span>
                </label>
                {canEdit && <button className="danger" onClick={() => removeNavigationItem(index)}>Remove</button>}
              </div>
            ))}
          </div>

          <div className="nested-panel">
            <div className="section-header compact">
              <h3>Homepage sections</h3>
              {canEdit && <button onClick={addSection}>Add content section</button>}
            </div>

            {design.contentSections.map((section, index) => (
              <div key={`${section.sectionKey}-${index}`} className="section-card-editor">
                <div className="grid two-col">
                  <input value={section.sectionKey} onChange={(event) => updateSection(index, 'sectionKey', event.target.value)} placeholder="Section key" disabled={!canEdit} />
                  <input
                    type="number"
                    value={section.sortOrder}
                    onChange={(event) => updateSection(index, 'sortOrder', Number(event.target.value))}
                    placeholder="Sort order"
                    disabled={!canEdit}
                  />
                </div>
                <input value={section.title} onChange={(event) => updateSection(index, 'title', event.target.value)} placeholder="Section title" disabled={!canEdit} />
                <textarea value={section.body} onChange={(event) => updateSection(index, 'body', event.target.value)} placeholder="Section body" disabled={!canEdit} rows="3" />
                {canEdit && <button className="danger" onClick={() => removeSection(index)}>Remove section</button>}
              </div>
            ))}
          </div>

          {canEdit ? (
            <div className="toolbar">
              <button onClick={save}>Save portal settings</button>
              {status && <span className="success-text">{status}</span>}
            </div>
          ) : (
            <p className="muted-text">Your role can view the portal configuration but cannot modify it.</p>
          )}

          {error && <p className="error">{error}</p>}
        </div>

        <div className="preview-shell">
          <div className="portal-preview" style={{ '--primary': design.primaryColor, '--secondary': design.secondaryColor }}>
            <div className="preview-topbar">
              <img src={design.logoUrl} alt={design.portalName} />
              <nav>
                {design.navigationItems
                  .slice()
                  .sort((left, right) => left.sortOrder - right.sortOrder)
                  .map((item) => (
                    <a key={`${item.label}-${item.href}`} href={item.href}>{item.label}</a>
                  ))}
              </nav>
            </div>
            {design.showAnnouncements && <div className="preview-announcement">{design.announcementText}</div>}
            <div className="preview-hero">
              <p className="eyebrow">{design.customerCode} portal</p>
              <h3>{design.heroTitle}</h3>
              <p>{design.heroSubtitle}</p>
            </div>
            <div className="preview-sections">
              {design.contentSections
                .slice()
                .sort((left, right) => left.sortOrder - right.sortOrder)
                .map((section) => (
                  <article key={`${section.sectionKey}-${section.sortOrder}`} className="preview-card">
                    <span className="badge muted">{section.sectionKey}</span>
                    <h4>{section.title}</h4>
                    <p>{section.body}</p>
                  </article>
                ))}
            </div>
            <footer>
              <strong>{design.footerText}</strong>
              <span>{design.supportEmail}</span>
            </footer>
          </div>
        </div>
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
      </div>
    </section>
  );
}
