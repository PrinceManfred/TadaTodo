<template>
  <v-row>
    <v-col cols="12">
      <v-card>
        <v-card-title
          ><v-text-field label="Title" v-model="modelValue.name" variant="underlined"
        /></v-card-title>
        <v-card-text>
          <div
            class="d-flex align-start"
            v-for="(item, index) in modelValue.todoItems"
            :key="index"
          >
            <v-checkbox v-model="item.isCompleted" class="flex-0-0" />
            <v-textarea
              v-model="item.value"
              class="flex-fill"
              auto-grow
              rows="1"
              max-rows="4"
              variant="underlined"
              append-icon="mdi-delete"
              @click:append="deleteItem(index)"
            />
          </div>
        </v-card-text>
        <v-card-actions>
          <v-btn prepend-icon="mdi-plus" text="Add Item" @click="addItem" color="primary" />
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import type { TodoList } from '@/models';

const modelValue = defineModel<TodoList>({ required: true });

function addItem() {
  modelValue.value.todoItems.push({ id: 0, value: '', isCompleted: false });
}

function deleteItem(index: number) {
  modelValue.value.todoItems.splice(index, 1);
}
</script>
