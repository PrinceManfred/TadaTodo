<template>
  <v-container class="fill-height d-flex flex-column justify-center align-center">
    <v-icon size="96">mdi-alert-circle-outline</v-icon>
    <h1>Page Not Found</h1>
    <p>The page you're looking for doesn't exist.</p>
    <v-btn color="primary" @click="goHome" class="ma-2">Go Home</v-btn>
  </v-container>
</template>

<script setup>
import { useRouter } from 'vue-router';

const router = useRouter();

const goHome = () => {
  router.push('/');
};
</script>
