import { createApp } from 'vue';

import App from './App.vue';
import '@/styles/main.css';

import { registerPlugins } from '@/plugins';

const app = createApp(App);

registerPlugins(app).then(() => {
  app.mount('#app');
});
