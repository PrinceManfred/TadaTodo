import { createRouter, createWebHistory } from 'vue-router';
import HomePage from '@/pages/HomePage';
import LoginPage from '@/pages/LoginPage';
import { useUserStore } from '@/stores/user';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomePage
    },
    {
      path: '/login',
      name: 'login',
      component: LoginPage,
      beforeEnter: () => {
        if (useUserStore().isLoggedIn) return { path: '/' };
      }
    }
  ]
});

export default router;
