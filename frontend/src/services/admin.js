import api from './api';

export const getCompanies = async (filters = {}) => {
  const { data } = await api.get('/admin/companies', { params: filters });
  return data;
};

export const getCompany = async (companyId) => {
  const { data } = await api.get(`/admin/companies/${companyId}`);
  return data;
};

export const updateCompany = async (companyId, payload) => {
  const { data } = await api.put(`/admin/companies/${companyId}`, payload);
  return data;
};

export const getManagedUsers = async (filters = {}) => {
  const { data } = await api.get('/admin/users', { params: filters });
  return data;
};

export const updateUserLoginStatus = async (userId, isLoginAllowed) => {
  const { data } = await api.put(`/admin/users/${userId}/login-status`, { isLoginAllowed });
  return data;
};
