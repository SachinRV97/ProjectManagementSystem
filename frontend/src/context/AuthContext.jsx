import { createContext, useContext, useMemo, useState } from 'react';
import api, { attachToken } from '../services/api';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [session, setSession] = useState(() => {
    const saved = localStorage.getItem('portal.session');
    return saved ? JSON.parse(saved) : null;
  });

  if (session?.token) {
    attachToken(session.token);
  }

  const login = async (email, password) => {
    const { data } = await api.post('/auth/login', { email, password });
    setSession(data);
    localStorage.setItem('portal.session', JSON.stringify(data));
    attachToken(data.token);
  };

  const register = async (payload) => {
    const { data } = await api.post('/auth/register', payload);
    setSession(data);
    localStorage.setItem('portal.session', JSON.stringify(data));
    attachToken(data.token);
  };

  const logout = () => {
    setSession(null);
    localStorage.removeItem('portal.session');
    attachToken(null);
  };

  const value = useMemo(
    () => ({ session, login, register, logout }),
    [session],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('Auth context is missing');
  }
  return context;
};
