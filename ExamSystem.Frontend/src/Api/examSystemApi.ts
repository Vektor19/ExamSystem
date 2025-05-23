import axios from 'axios';

const examSystemApi = axios.create({
  baseURL: 'https://examsystem-backend-9a66e404de39.herokuapp.com',
  headers: {
    'Content-Type': 'application/json',
  },
});

examSystemApi.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default examSystemApi;
