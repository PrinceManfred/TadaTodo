<template>
  <main>
    <v-container>
      <v-row>
        <v-col cols="12">
          <h1>Stats</h1>
        </v-col>
        <v-col cols="12" sm="6">
          <v-card>
            <v-card-title>Total # Todo Lists</v-card-title>
            <v-card-text>{{ totalLists }}</v-card-text>
          </v-card>
        </v-col>
        <v-col cols="12" sm="6">
          <v-card>
            <v-card-title>Total # Todo Items</v-card-title>
            <v-card-text>{{ totalItems }}</v-card-text>
          </v-card>
        </v-col>
        <v-col cols="12" sm="6">
          <v-card>
            <v-card-title># Completed Todo Items</v-card-title>
            <v-card-text>{{ completedItems }}</v-card-text>
          </v-card>
        </v-col>
        <v-col cols="12" sm="6">
          <v-card>
            <v-card-title># Incomplete Todo Items</v-card-title>
            <v-card-text>{{ incompleteItems }}</v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </main>
</template>

<script setup lang="ts">
import { TodoService } from '@/services/todoService';
import { ref, reactive, onMounted } from 'vue';
import { useLoading, useSnackbar } from '@/composables';

const loading = reactive(useLoading());
const todosService = new TodoService();
const { showSnackbar } = useSnackbar();

const totalLists = ref(0);
const totalItems = ref(0);
const completedItems = ref(0);
const incompleteItems = ref(0);

onMounted(async () => {
  loading.startLoading();
  try {
    const todoLists = await todosService.getTodoLists();
    if (todoLists.length == 0) return;
    totalLists.value = todoLists.length;
    const items = todoLists.flatMap((list) => list.todoItems);
    totalItems.value = items.length;
    completedItems.value = items.filter((item) => item.isCompleted).length;
    incompleteItems.value = items.filter((item) => !item.isCompleted).length;
  } catch {
    showSnackbar('Failed to load data. Try again later.', 4000, 'error');
  } finally {
    loading.stopLoading();
  }
});
</script>
