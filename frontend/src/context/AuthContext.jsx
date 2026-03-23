<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
import { createContext, useContext, useEffect, useMemo, useState } from 'react';
=======
import { createContext, useContext, useMemo, useState } from 'react';
>>>>>>> theirs
=======
import { createContext, useContext, useMemo, useState } from 'react';
>>>>>>> theirs
=======
import { createContext, useContext, useMemo, useState } from 'react';
>>>>>>> theirs
=======
import { createContext, useContext, useMemo, useState } from 'react';
>>>>>>> theirs
import api, { attachToken } from '../services/api';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [session, setSession] = useState(() => {
    const saved = localStorage.getItem('portal.session');
    return saved ? JSON.parse(saved) : null;
  });

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
  useEffect(() => {
    attachToken(session?.token ?? null);
  }, [session]);

  const persistSession = (data) => {
    setSession(data);
    localStorage.setItem('portal.session', JSON.stringify(data));
  };

  const login = async (email, password) => {
    const { data } = await api.post('/auth/login', { email, password });
    persistSession(data);
  };

  const registerCompany = async (payload) => {
    const { data } = await api.post('/auth/register-company', payload);
    persistSession(data);
  };

  const createUser = async (payload) => {
    const { data } = await api.post('/auth/register-user', payload);
    return data;
  };

  const logout = () => {
    attachToken(null);
    setSession(null);
    localStorage.removeItem('portal.session');
  };

  const value = useMemo(
    () => ({ session, login, registerCompany, createUser, logout }),
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
  if (session?.token) {
    attachToken(session.token);
  }
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
import { createContext, useContext, useEffect, useMemo, useState } from 'react';
import api, { attachToken } from '../services/api';

const AuthContext = createContext(null);
const SESSION_KEY = 'portal.session';

export function AuthProvider({ children }) {
  const [session, setSession] = useState(() => {
    const saved = localStorage.getItem(SESSION_KEY);
    return saved ? JSON.parse(saved) : null;
  });

  useEffect(() => {
    attachToken(session?.token ?? null);
    if (session) {
      localStorage.setItem(SESSION_KEY, JSON.stringify(session));
      return;
    }

    localStorage.removeItem(SESSION_KEY);
  }, [session]);
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs

  const login = async (email, password) => {
    const { data } = await api.post('/auth/login', { email, password });
    setSession(data);
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
    localStorage.setItem('portal.session', JSON.stringify(data));
    attachToken(data.token);
=======
    return data;
>>>>>>> theirs
=======
    return data;
>>>>>>> theirs
=======
    return data;
>>>>>>> theirs
  };

  const register = async (payload) => {
    const { data } = await api.post('/auth/register', payload);
    setSession(data);
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
    return data;
  };

  const logout = () => setSession(null);

  const value = useMemo(
    () => ({ session, login, register, logout, isAuthenticated: Boolean(session?.token) }),
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    [session],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('Auth context is missing');
  }
  return context;
};
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used inside AuthProvider');
  }

  return context;
}
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
