<template>
  <v-app>
    <v-app-bar scroll-behavior="elevate" color="primary" app>
      <template v-if="userStore.isLoggedIn && display.xs.value" v-slot:extension>
        <v-text-field
          v-model="searchQuery"
          label="Search"
          single-line
          append-inner-icon="mdi-magnify"
          @click:append-inner="performSearch"
          @keyup.enter="performSearch"
          hide-details
          variant="solo"
        />
      </template>

      <router-link to="/" style="height: 100%">
        <img height="100%" src="@/assets/logo.jpg" alt="Tada Todo Logo" class="pl-1 py-1 logo" />
      </router-link>
      <v-app-bar-title
        @click="router.push('/')"
        text="TadaTodo"
        class="flex-0-0"
        style="cursor: pointer"
      />

      <v-spacer />

      <v-text-field
        v-if="userStore.isLoggedIn && !display.xs.value"
        v-model="searchQuery"
        label="Search"
        single-line
        append-inner-icon="mdi-magnify"
        @click:append-inner="performSearch"
        @keyup.enter="performSearch"
        hide-details
        max-width="300px"
        variant="solo"
        density="compact"
      />

      <v-btn v-if="!userStore.isLoggedIn" size="small" to="/login" prepend-icon="mdi-login"
        >Login</v-btn
      >
      <v-menu v-else location="bottom">
        <template v-slot:activator="{ props }">
          <v-btn v-bind="props" variant="plain" size="small">
            {{ userStore.username }}
            <v-icon right>mdi-menu-down</v-icon>
          </v-btn>
        </template>
        <v-list>
          <v-list-item>
            <v-btn block variant="text" @click="router.push('/stats')">Stats</v-btn>
          </v-list-item>
          <v-list-item>
            <v-btn block variant="text" @click="logout">Logout</v-btn>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>
    <v-main>
      <router-view />
    </v-main>
    <v-overlay :model-value="isLoading" persistent class="align-center justify-center">
      <v-progress-circular color="primary" size="128" indeterminate></v-progress-circular>
    </v-overlay>
    <v-snackbar v-model="snackbar" :timeout="snackbarTimeout" :color="snackbarColor">
      {{ snackbarText }}
      <template v-slot:actions>
        <v-btn variant="text" @click="snackbar = false"> Close </v-btn>
      </template>
    </v-snackbar>
  </v-app>
</template>

<script lang="ts" setup>
import { ref, provide } from 'vue';
import { useUserStore } from '@/stores/user';
import { useRouter } from 'vue-router';
import { useDisplay } from 'vuetify';
import { useLoading } from '@/composables';
import { SnackbarSymbol } from '@/symbols';

const { isLoading, startLoading, stopLoading } = useLoading();
const router = useRouter();
const userStore = useUserStore();
const display = useDisplay();
const searchQuery = ref('');
const snackbar = ref(false);
const snackbarTimeout = ref(2000);
const snackbarText = ref('');
const snackbarColor = ref('primary');

async function logout() {
  startLoading();
  await userStore.logout();
  stopLoading();
  router.push('/');
}

async function performSearch() {
  router.push({ path: '/search', query: { q: searchQuery.value } });
}

function showSnackbar(text: string, timeout = 2000, color = 'primary') {
  snackbarText.value = text;
  snackbarTimeout.value = timeout;
  snackbar.value = true;
  snackbarColor.value = color;
}

provide(SnackbarSymbol, showSnackbar);
</script>

<style>
.logo {
  border-radius: 15px;
}
</style>
