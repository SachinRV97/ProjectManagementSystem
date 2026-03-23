import React from 'react';
import ReactDOM from 'react-dom/client';
<<<<<<< ours
<<<<<<< ours
import 'bootstrap/dist/css/bootstrap.min.css';
=======
>>>>>>> theirs
=======
>>>>>>> theirs
import App from './App';
import { AuthProvider } from './context/AuthContext';
import './styles.css';

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <AuthProvider>
      <App />
    </AuthProvider>
  </React.StrictMode>,
);
