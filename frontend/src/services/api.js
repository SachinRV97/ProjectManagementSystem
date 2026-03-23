import axios from 'axios';

const api = axios.create({
<<<<<<< ours
<<<<<<< ours
  baseURL: 'http://localhost:5000/api',
=======
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
>>>>>>> theirs
=======
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
>>>>>>> theirs
});

export const attachToken = (token) => {
  if (token) {
    api.defaults.headers.common.Authorization = `Bearer ${token}`;
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
  } else {
    delete api.defaults.headers.common.Authorization;
  }
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    return;
  }

  delete api.defaults.headers.common.Authorization;
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
};

export default api;
