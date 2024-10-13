<template>
  <main>
    <v-container class="fill-height" max-width="1200px">
      <v-row>
        <v-col cols="12">
          <h1>My Todo Lists</h1>
        </v-col>
        <v-col cols="12">
          <v-btn
            color="secondary"
            text="New List"
            prepend-icon="mdi-plus"
            @click="router.push({ name: 'createTodoList' })"
          />
        </v-col>
        <v-col cols="12">
          <AllLists />
        </v-col>
      </v-row>
    </v-container>
  </main>
</template>

<script setup lang="ts">
import AllLists from '@/components/AllLists.vue';
import { useRouter } from 'vue-router';

const router = useRouter();
</script>
