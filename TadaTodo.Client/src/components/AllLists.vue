<template>
  <v-container class="fill-height" max-width="1200px">
    <v-row>
      <v-col cols="12">
        <div class="text-center">
          <div class="text-body-2 font-weight-light mb-n1">All Lists</div>
        </div>
        <div class="py-4" />
      </v-col>
      <v-col v-for="todo in todoLists" :key="todo.id" cols="12" sm="6">
        <v-card variant="text">
          <v-card-text>{{ todo.name }} - {{ todo.todoItems.length }}</v-card-text>
        </v-card>
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
