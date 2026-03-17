import api from './api';

export const permissionNames = {
  managePortal: 'manage_portal',
  manageEmployees: 'manage_employees',
  manageCustomers: 'manage_customers',
};

const legacyRolePermissions = {
  Admin: [
    permissionNames.managePortal,
    permissionNames.manageEmployees,
    permissionNames.manageCustomers,
  ],
  'Portal-Admin': [permissionNames.managePortal],
  'Portal-Employee': [permissionNames.manageCustomers],
  'Customer-Admin': [permissionNames.manageEmployees],
  'Customer-Employee': [permissionNames.managePortal],
  'Customer-User': [],
};

const permissionLabels = {
  [permissionNames.managePortal]: 'Manage portal for employees and customers',
  [permissionNames.manageEmployees]: 'Manage employees and users',
  [permissionNames.manageCustomers]: 'Manage customer records',
};

export const getRoles = async () => {
  const { data } = await api.get('/auth/roles');
  return data;
};

export const getAssignableRoles = async () => {
  const { data } = await api.get('/auth/assignable-roles');
  return data;
};

export const isCustomerRoleName = (roleName) =>
  ['Customer-Admin', 'Customer-Employee', 'Customer-User'].includes(roleName);

export const canManageCompanyUsers = (session) =>
  ['Admin', 'Portal-Admin', 'Portal-Employee'].includes(session?.role);

export const isGlobalAdmin = (session) =>
  Boolean(session?.isGlobalAdmin || (
    session?.role === 'Admin' &&
    session?.companyCode === 'GLOBAL'
  ));

export const hasPermission = (session, permission) => {
  const sessionPermissions = Array.isArray(session?.permissions) ? session.permissions : null;

  if (sessionPermissions) {
    return sessionPermissions.includes(permission);
  }

  return (legacyRolePermissions[session?.role] ?? []).includes(permission);
};

export const describeRole = (role) => {
  if (role.description) {
    return role.description;
  }

  const labels = (role.permissions ?? [])
    .map((permission) => permissionLabels[permission] ?? permission);

  if (role.limitPortalManagementToOwnCustomer) {
    labels.push('Can manage only the assigned customer portal');
  }

  return labels.length > 0 ? labels.join(', ') : 'Login and normal application usage';
};
