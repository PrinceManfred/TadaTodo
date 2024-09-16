import { defineStore } from 'pinia';
import axios from 'axios';

export const useUserStore = defineStore('user', {
  state: () => ({
    id: 0,
    username: null
  }),
  getters: {
    isLoggedIn() {
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
        const res = await axios.get('/api/users/logout');
        this.$reset();
      } catch {
        throw new Error('Failed to login');
      }
    }
  }
});
