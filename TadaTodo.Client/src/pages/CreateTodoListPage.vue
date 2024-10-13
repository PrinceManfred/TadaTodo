<template>
  <main>
    <v-container>
      <v-row>
        <v-col cols="12">
          <router-link to="/todos">&lt; Back</router-link>
        </v-col>
        <v-col cols="12">
          <h1>Create New List</h1>
        </v-col>
        <v-col cols="12">
          <EditTodoList v-model="todoList" @save="onSave" @cancel="router.push('/todos')" />
        </v-col>
      </v-row>
    </v-container>
  </main>
</template>

<script setup lang="ts">
import EditTodoList from '@/components/EditTodoList.vue';
import type { CreateTodoItem, CreateTodoList, TodoList } from '@/models';
import { ref, reactive } from 'vue';
import { TodoService } from '@/services/todoService';
import { useLoading, useSnackbar } from '@/composables';
import { useRouter } from 'vue-router';

const todosService = new TodoService();
const loading = reactive(useLoading());
const router = useRouter();
const { showSnackbar } = useSnackbar();
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

async function onSave() {
  loading.startLoading();
  const newList: CreateTodoList = {
    name: todoList.value.name,
    todoItems: todoList.value.todoItems.map((item): CreateTodoItem => {
      return { value: item.value, isCompleted: item.isCompleted };
    })
  };
  try {
    const createdList = await todosService.createTodoList(newList);
    showSnackbar('Save successful!', 3000, 'success');
    router.replace(`/todos/${createdList.id}`);
  } catch {
    showSnackbar('Save failed!', 3000, 'error');
  } finally {
    loading.stopLoading();
  }
}
</script>
