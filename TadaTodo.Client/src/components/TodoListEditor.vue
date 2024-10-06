<template>
  <v-container class="fill-height" max-width="1200px">
    <v-row>
      <v-col cols="12">
        <h1>My Todo Lists</h1>
      </v-col>
      <v-col v-for="todo in todoLists" :key="todo.id" cols="12" sm="6">
        <v-btn :text="todo.name" block color="primary" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { TodoService } from '@/services/todoService';
import { ref, onMounted } from 'vue';
import type { TodoList } from '@/models';

const todosService = new TodoService();
const todoLists = ref<TodoList[]>([]);

onMounted(async () => {
  todoLists.value = await todosService.getTodoLists();
});
</script>
