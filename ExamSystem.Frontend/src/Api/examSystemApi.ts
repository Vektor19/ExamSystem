import axios from 'axios';

const examSystemApi = axios.create({
  baseURL: 'https://examsystembackend-brbzbwgzhbe3eufv.canadacentral-01.azurewebsites.net',
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
