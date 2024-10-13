<template>
  <main>
    <v-container>
      <v-row>
        <v-col cols="12">
          <router-link to="" @click="router.back()">&lt; Back</router-link>
        </v-col>
        <v-col cols="12">
          <h1>Edit Todo List</h1>
        </v-col>
        <v-col cols="12">
          <EditTodoList
            v-model="todoList"
            @save="onSave"
            @cancel="router.push('/todos')"
            @delete:item="deletedIds.push($event.id)"
          />
        </v-col>
        <v-col class="d-flex justify-end">
          <v-btn
            class="d-flex"
            prepend-icon="mdi-delete"
            variant="text"
            size="small"
            color="error"
            text="Delete List"
            @click="deleteDialog = true"
          />
        </v-col>
      </v-row>

      <v-dialog v-model="deleteDialog" max-width="500">
        <v-card>
          <v-card-title class="headline">Confirm Deletion</v-card-title>
          <v-card-text>Are you sure you want to delete "{{ todoList.name }}"?</v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn variant="text" @click="deleteDialog = false">Cancel</v-btn>
            <v-btn color="error" variant="text" @click="deleteList">Confirm</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-container>
  </main>
</template>

<script setup lang="ts">
import EditTodoList from '@/components/EditTodoList.vue';
import type { CreateTodoItem, TodoItem, TodoList, UpdateTodoItem, UpdateTodoList } from '@/models';
import { onMounted } from 'vue';
import { ref, reactive } from 'vue';
import { TodoService } from '@/services/todoService';
import { useLoading, useSnackbar } from '@/composables';
import { useRouter } from 'vue-router';

const props = defineProps<{
  todoListId: number;
}>();

const { showSnackbar } = useSnackbar();
const todosService = new TodoService();
const loading = reactive(useLoading());
const router = useRouter();
const deleteDialog = ref(false);
const todoList = ref<TodoList>({
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
const deletedIds = ref<number[]>([]);

async function onSave() {
  loading.startLoading();
  const updateList: UpdateTodoList = {
    id: todoList.value.id,
    name: todoList.value.name,
    newItems: todoList.value.todoItems
      .filter((item: TodoItem) => item.id == 0)
      .map((item: TodoItem): CreateTodoItem => {
        return {
          value: item.value,
          isCompleted: item.isCompleted
        };
      }),
    updatedItems: todoList.value.todoItems
      .filter((item: TodoItem) => item.id > 0)
      .map((item: TodoItem): UpdateTodoItem => {
        return {
          id: item.id,
          value: item.value,
          isCompleted: item.isCompleted
        };
      }),
    deletedItems: deletedIds.value
  };
  try {
    todoList.value = await todosService.updateTodoList(updateList);
    deletedIds.value = [];
    showSnackbar('Save successful!', 3000, 'success');
  } catch {
    showSnackbar('Save failed!', 3000, 'error');
  } finally {
    loading.stopLoading();
  }
}

async function getTodoList() {
  if (typeof props.todoListId !== 'number') router.push({ name: 'notFound' });
  loading.startLoading();
  try {
    todoList.value = await todosService.getTodoList(props.todoListId);
  } catch {
    router.push({ name: 'notFound' });
  } finally {
    loading.stopLoading();
  }
}

async function deleteList() {
  loading.startLoading();
  try {
    await todosService.deleteTodoList(todoList.value.id);
    showSnackbar('List deleted!', 3000, 'success');
    router.push('/todos');
  } finally {
    loading.stopLoading();
  }
}

onMounted(async () => {
  await getTodoList();
});
</script>
