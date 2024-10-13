<template>
  <v-row>
    <v-col v-for="todo in todoLists" :key="todo.id" cols="12" sm="6">
      <v-btn :text="todo.name" block color="primary" @click="router.push(`/todos/${todo.id}`)" />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { TodoService } from '@/services/todoService';
import { ref, reactive, onMounted } from 'vue';
import type { TodoList } from '@/models';
import { useRouter } from 'vue-router';
import { useLoading, useSnackbar } from '@/composables';

const loading = reactive(useLoading());
const todosService = new TodoService();
const todoLists = ref<TodoList[]>([]);
const router = useRouter();
const { showSnackbar } = useSnackbar();

onMounted(async () => {
  loading.startLoading();
  try {
    todoLists.value = await todosService.getTodoLists();
  } catch {
    showSnackbar('Failed to load lists. Try again later.', 4000, 'error');
  } finally {
    loading.stopLoading();
  }
});
</script>
