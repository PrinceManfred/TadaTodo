<template>
  <main>
    <v-container>
      <v-row>
        <v-col cols="12">
          <h1>Edit Todo List</h1>
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
import { onMounted } from 'vue';
import { ref, inject } from 'vue';
import { TodoService } from '@/services/todoService';
import { type LoadingState, LoadingSymbol } from '@/plugins/loading';

const props = defineProps<{
  todoListId: number;
}>();

const todosService = new TodoService();
const { startLoading, stopLoading } = inject(LoadingSymbol) as LoadingState;
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

async function getTodoList() {
  startLoading();
  todoList.value = await todosService.getTodoList(props.todoListId);
  stopLoading();
}

onMounted(async () => {
  await getTodoList();
});
</script>
