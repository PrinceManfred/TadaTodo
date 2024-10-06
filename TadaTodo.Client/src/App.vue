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
            <v-btn block variant="text" @click.stop>Account Details</v-btn>
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
  </v-app>
</template>

<script lang="ts" setup>
import { inject, ref } from 'vue';
import { useUserStore } from '@/stores/user';
import { useRouter } from 'vue-router';
import { type LoadingState, LoadingSymbol } from '@/plugins/loading';
import { useDisplay } from 'vuetify';

const { isLoading, startLoading, stopLoading } = inject(LoadingSymbol) as LoadingState;
const router = useRouter();
const userStore = useUserStore();
const display = useDisplay();
const searchQuery = ref('');

async function logout() {
  startLoading();
  await userStore.logout();
  stopLoading();
  router.push('/');
}

async function performSearch() {
  router.push({ path: '/search', query: { q: searchQuery.value } });
}
</script>
