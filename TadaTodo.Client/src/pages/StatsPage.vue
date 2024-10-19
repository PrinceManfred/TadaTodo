<template>
  <main>
    <v-container>
      <v-row>
        <v-col cols="12">
          <h1>Stats</h1>
          <p class="text-subtitle-1">
            {{
              new Date().toLocaleDateString('en-US', {
                month: 'numeric',
                day: 'numeric',
                year: 'numeric'
              })
            }}
          </p>
        </v-col>
        <v-col cols="12">
          <v-table class="first-column-fit-content">
            <thead>
              <tr>
                <th class="text-left">Category</th>
                <th class="text-left">Number of Items</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>Todo Lists</td>
                <td>{{ totalLists }}</td>
              </tr>
              <tr>
                <td>All Todo Items</td>
                <td>{{ totalItems }}</td>
              </tr>
              <tr>
                <td>Completed Todo Items</td>
                <td>{{ completedItems }}</td>
              </tr>
              <tr>
                <td>Incomplete Todo Items</td>
                <td>{{ incompleteItems }}</td>
              </tr>
            </tbody>
          </v-table>
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

<style>
.first-column-fit-content td:nth-child(1),
.first-column-fit-content th:nth-child(1) {
  white-space: nowrap;
  width: 1%;
}
</style>
