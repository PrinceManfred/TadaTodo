<template>
  <v-row v-if="todoLists.length == 0">
    <v-col cols="12">
      <p class="text-center text-h2">No results</p>
    </v-col>
  </v-row>
  <v-row v-else>
    <v-col v-for="todo in todoLists" :key="todo.id" cols="12">
      <v-btn :text="todo.name" block color="primary" @click="router.push(`/todos/${todo.id}`)" />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { TodoService } from '@/services/todoService';
import { ref, onMounted } from 'vue';
import type { TodoList } from '@/models';
import { watch } from 'vue';
import { useLoading, useSnackbar } from '@/composables';
import { useRouter } from 'vue-router';

const { startLoading, stopLoading } = useLoading();
const props = defineProps<{
  search: string;
}>();

const todosService = new TodoService();
const router = useRouter();
const { showSnackbar } = useSnackbar();
const todoLists = ref<TodoList[]>([]);

watch(
  () => props.search,
  async () => {
    await getResults();
  }
);
async function getResults() {
  startLoading();
  try {
    if (!props.search || props.search.trim() == '') {
      todoLists.value = await todosService.getTodoLists();
    } else {
      todoLists.value = await todosService.searchTodoLists(props.search);
    }
  } catch {
    showSnackbar('Search unavailable. Try again later.', 4000, 'error');
  } finally {
    stopLoading();
  }
}

onMounted(async () => {
  await getResults();
});
</script>
