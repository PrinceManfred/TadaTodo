/**
 * plugins/vuetify.ts
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Styles
import '@mdi/font/css/materialdesignicons.css';
import 'vuetify/styles';

// Composables
import { createVuetify } from 'vuetify';

const prefersDarkScheme = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)');

const vuetify = createVuetify({
  theme: {
    defaultTheme: prefersDarkScheme.matches ? 'dark' : 'light',
  },
});

prefersDarkScheme.addEventListener('change', event => {
  vuetify.theme.global.name.value = event.matches ? 'dark' : 'light';
});

// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides
export default vuetify;
