<template>
  <v-row v-if="todoLists.length == 0">
    <v-col cols="12">
      <p class="text-center text-h2">No results</p>
    </v-col>
  </v-row>
  <v-row v-else>
    <v-col v-for="todo in todoLists" :key="todo.id" cols="12">
      <v-btn :text="todo.name" block color="primary" />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { TodoService } from '@/services/todoService';
import { ref, onMounted, defineProps } from 'vue';
import type { TodoList } from '@/models';
import { watch } from 'vue';

const props = defineProps<{
  search: string;
}>();

const todosService = new TodoService();
const todoLists = ref<TodoList[]>([]);

watch(
  () => props.search,
  async () => {
    await getResults();
  }
);
async function getResults() {
  if (!props.search || props.search.trim() == '') {
    todoLists.value = await todosService.getTodoLists();
  } else {
    todoLists.value = await todosService.searchTodoLists(props.search);
  }
}

onMounted(async () => {
  await getResults();
});
</script>
