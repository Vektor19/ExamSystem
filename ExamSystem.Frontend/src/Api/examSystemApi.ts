import axios from 'axios';

const examSystemApi = axios.create({
  baseURL: 'http://localhost:7777',
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
