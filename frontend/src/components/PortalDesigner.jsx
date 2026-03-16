import { useEffect, useState } from 'react';
import api from '../services/api';
import { hasPermission, permissionNames } from '../services/access';

export default function PortalDesigner({ session }) {
  const [design, setDesign] = useState(null);
  const [status, setStatus] = useState('');

  useEffect(() => {
    api.get('/portaldesign/me').then(({ data }) => setDesign(data));
  }, []);

  const canEdit = hasPermission(session, permissionNames.managePortal);

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
      </div>
    </section>
  );
}
