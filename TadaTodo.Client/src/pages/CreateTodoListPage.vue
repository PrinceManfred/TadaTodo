<template>
  <main>
    <v-container>
      <v-row>
        <v-col cols="12">
          <h1>Create New List</h1>
        </v-col>
        <v-col cols="12">
          <EditTodoList v-model="todoList" />
        </v-col>
      </v-row>
    </v-container>
  </main>
</template>

<script setup lang="ts">
import EditTodoList from '@/components/EditTodoList.vue';
import type { TodoList } from '@/models';
import { ref } from 'vue';

let todoList = ref<TodoList>({
  id: 0,
  name: '',
  todoItems: [
    {
      id: 0,
      isCompleted: false,
      value: ''
    }
  ]
});
</script>
