<template>
  <v-app>
    <v-app-bar scroll-behavior="elevate" color="primary" app>
      <template v-slot:prepend>
        <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      </template>
      <v-app-bar-title text="TadaTodo" />
      <v-spacer />
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
    <v-navigation-drawer v-model="drawer">
      <v-list>
        <v-list-item title="Navigation drawer"></v-list-item>
      </v-list>
    </v-navigation-drawer>
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
import { useDisplay } from 'vuetify';
import { useUserStore } from '@/stores/user';
import { useRouter } from 'vue-router';
import { type LoadingState, LoadingSymbol } from '@/plugins/loading';

const { isLoading, startLoading, stopLoading } = inject(LoadingSymbol) as LoadingState;
const router = useRouter();
const display = useDisplay();
const drawer = ref(!display.mobile.value);
const userStore = useUserStore();

async function logout() {
  startLoading();
  await userStore.logout();
  stopLoading();
  router.push('/');
}
</script>
