import { createApp } from 'vue';

import App from './App.vue';
import '@/styles/main.css';

import { registerPlugins } from '@/plugins';
import { useUserStore } from '@/stores/user';

const app = createApp(App);

await registerPlugins(app);

app.mount('#app');
