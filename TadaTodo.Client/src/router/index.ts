import { createRouter, createWebHistory } from 'vue-router';
import HomePage from '@/pages/HomePage.vue';
import LoginPage from '@/pages/LoginPage.vue';
import { useUserStore } from '@/stores/user';
import RegistrationPage from '@/pages/RegistrationPage.vue';
import NotFoundPage from '@/pages/NotFoundPage.vue';
import TodosPage from '@/pages/TodosPage.vue';
import CreateTodoListPage from '@/pages/CreateTodoListPage.vue';
import EditTodoListPage from '@/pages/EditTodoListPage.vue';
import SearchPage from '@/pages/SearchPage.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomePage,
      beforeEnter: () => {
        if (useUserStore().isLoggedIn) return { path: '/todos' };
      }
    },
    {
      path: '/login',
      name: 'login',
      component: LoginPage,
      props: (route) => ({ errorMessage: route.query.errorMessage }),
      beforeEnter: () => {
        if (useUserStore().isLoggedIn) return { path: '/todos' };
      }
    },
    {
      path: '/register',
      name: 'register',
      component: RegistrationPage,
      beforeEnter: () => {
        if (useUserStore().isLoggedIn) return { path: '/todos' };
      }
    },
    {
      path: '/todos',
      name: 'todos',
      component: TodosPage,
      beforeEnter: () => {
        if (!useUserStore().isLoggedIn) return { path: '/login' };
      }
    },
    {
      path: '/todos/create',
      name: 'createTodoList',
      component: CreateTodoListPage,
      beforeEnter: () => {
        if (!useUserStore().isLoggedIn) return { path: '/login' };
      }
    },
    {
      path: '/todos/:todoListId(\\d+)',
      name: 'editTodoList',
      component: EditTodoListPage,
      beforeEnter: () => {
        if (!useUserStore().isLoggedIn) return { path: '/login' };
      },
      props: (route) => ({ todoListId: Number(route.params.todoListId) })
    },
    {
      path: '/search',
      name: 'search',
      component: SearchPage,
      beforeEnter: () => {
        if (!useUserStore().isLoggedIn) return { path: '/login' };
      },
      props: (route) => ({ search: route.query.q ?? '' })
    },
    {
      path: '/:catchAll(.*)*',
      name: 'notFound',
      component: NotFoundPage
    }
  ]
});

export default router;
