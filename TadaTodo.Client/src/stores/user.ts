import { defineStore } from 'pinia';
import axios from 'axios';

export const useUserStore = defineStore('user', {
  state: () => ({
    id: 0,
    username: null
  }),
  getters: {
    isLoggedIn(): boolean {
      return this.id > 0;
    }
  },
  actions: {
    async login(username: string, password: string) {
      try {
        const res = await axios.post('/api/users/login', {
          username,
          password
        });

        this.id = res.data.id;
        this.username = res.data.username;
      } catch {
        throw new Error('Failed to login');
      }
    },
    async register(username: string, password: string) {
      try {
        const res = await axios.post('/api/users', {
          username,
          password
        });

        this.id = res.data.id;
        this.username = res.data.username;
      } catch (error: any) {
        throw error?.response?.data ?? 'Failed to register.';
      }
    },
    async checkLoginStatus() {
      try {
        const res = await axios.get('/api/users/me');
        this.id = res.data.id;
        this.username = res.data.username;
        return true;
      } catch {
        return false;
      }
    },
    async logout() {
      try {
        await axios.get('/api/users/logout');
        this.$reset();
      } catch {
        throw new Error('Failed to logout');
      }
    }
  }
});
