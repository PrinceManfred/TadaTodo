<template>
  <v-container class="fill-height" max-width="1200px">
    <v-row>
      <v-col>
        <v-img class="mb-4" height="300" src="@/assets/logo.jpg" />

        <div class="text-center">
          <div class="text-body-1">Welcome to</div>
          <h1 class="text-h2 font-weight-bold">Tada Todo!</h1>
        </div>
      </v-col>
      <v-col cols="12">
        <v-card>
          <v-card-title>Get Started</v-card-title>
          <v-card-text>
            <div class="text-body-2">
              Tada Todo is your ultimate web app for managing and organizing your daily tasks and
              todo lists. Our intuitive interface ensures you stay productive and efficient. Join
              our community to start managing your tasks with ease.
            </div>
          </v-card-text>
          <v-card-actions>
            <v-btn color="primary" @click="router.push('/login')">Login</v-btn>
            <v-btn color="primary" @click="router.push('/register')">Register</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();
</script>
