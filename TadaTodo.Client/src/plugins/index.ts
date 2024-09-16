/**
 * plugins/index.ts
 *
 * Automatically included in `./src/main.ts`
 */

// Plugins
import { createPinia } from 'pinia';
import vuetify from './vuetify';
import router from '@/router';

// Types
import type { App } from 'vue';
import { useUserStore } from '@/stores/user';
import loading from '@/plugins/loading';

export async function registerPlugins(app: App) {
  app.use(createPinia());
  await useUserStore().checkLoginStatus();
  app.use(loading).use(vuetify).use(router);
}
