<template>
  <main>
    <v-container class="fill-height" max-width="1200px">
      <v-row>
        <v-col cols="12">
          <h1>Search Results</h1>
          <p class="text-subtitle-1">Todo lists with an item containing "{{ search }}"</p>
        </v-col>
        <v-col cols="12">
          <SearchLists :search="search" />
        </v-col>
      </v-row>
    </v-container>
  </main>
</template>

<script setup lang="ts">
import SearchLists from '@/components/SearchLists.vue';

const props = withDefaults(
  defineProps<{
    search: string;
  }>(),
  {
    search: ''
  }
);
</script>
