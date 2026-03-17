export const themePresets = {
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

export const createPageId = () => (
  typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function'
    ? crypto.randomUUID()
    : `page-${Date.now()}`
);

export const slugifySiteValue = (value, fallback = 'portal') => {
  const normalized = (value || fallback)
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, '-')
    .replace(/^-+|-+$/g, '');

  return normalized || fallback;
};

export const buildNewPage = (index) => ({
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

export const buildPortalSiteUrl = (siteName, siteSlug, pageSlug) => {
  const currentOrigin = typeof window !== 'undefined' ? window.location.origin : '';
  const resolvedSiteSegment = encodeURIComponent(siteSlug || slugifySiteValue(siteName));
  const pageSegment = pageSlug && pageSlug !== 'home'
    ? `/${encodeURIComponent(pageSlug)}`
    : '';

  return `${currentOrigin}/${resolvedSiteSegment}${pageSegment}`;
};

export const getPortalRouteParts = (pathname) => {
  const [siteSegment = '', pageSegment = ''] = pathname
    .split('/')
    .filter(Boolean)
    .map((segment) => decodeURIComponent(segment));

  return {
    siteSegment,
    pageSegment,
  };
};
