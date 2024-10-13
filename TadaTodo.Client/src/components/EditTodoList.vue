<template>
  <v-row>
    <v-col cols="12">
      <v-form @submit.prevent.stop="onSave" v-model="isValid">
        <v-card :class="shaker.shakeClass">
          <v-card-title>
            <v-text-field
              label="Title"
              v-model="modelValue.name"
              variant="underlined"
              :rules="[lengthBetween(1, 255)]"
              counter
            />
          </v-card-title>
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
                :rules="[maxLength(500)]"
              />
            </div>
            <v-btn
              class="my-3"
              prepend-icon="mdi-plus"
              text="Add Item"
              @click="addItem"
              color="primary"
            />
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn type="submit" variant="flat" color="primary" rounded>Save</v-btn>
            <v-btn variant="flat" color="error" @click="emit('cancel')" rounded>Cancel</v-btn>
          </v-card-actions>
        </v-card>
      </v-form>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import type { TodoList } from '@/models';
import { useShake } from '@/composables';

const emit = defineEmits(['save', 'cancel', 'delete:item']);
const shaker = reactive(useShake());
const modelValue = defineModel<TodoList>({ required: true });
const isValid = ref<boolean | null>(null);

function onSave() {
  if (!isValid.value) {
    shaker.shake();
    return;
  }

  emit('save');
}
function addItem() {
  modelValue.value.todoItems.push({ id: 0, value: '', isCompleted: false });
}

function deleteItem(index: number) {
  const item = modelValue.value.todoItems.splice(index, 1)[0];
  emit('delete:item', item);
}

function lengthBetween(min: number, max: number) {
  return (value: string) => {
    if (value) value = value.trim();
    if (!value || value.length < min || value.length > max)
      return `Must be between ${min} and ${max} characters long.`;

    return true;
  };
}

function maxLength(max: number) {
  return (value: string) => {
    if (value && value.length > max) return `Can't be longer than ${max} characters.`;

    return true;
  };
}
</script>
