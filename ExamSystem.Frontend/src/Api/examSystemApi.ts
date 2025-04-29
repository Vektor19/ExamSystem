import axios from 'axios';

const examSystemApi = axios.create({
  baseURL: 'https://localhost:7197',
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
