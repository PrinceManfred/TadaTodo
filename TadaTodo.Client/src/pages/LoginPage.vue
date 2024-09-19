<template>
  <main>
    <p v-if="props.errorMessage" class="text-error text-center ma-4 text-subtitle-2">
      {{ props.errorMessage }}
    </p>
    <LoginForm />
  </main>
</template>

<script setup lang="ts">
import LoginForm from '@/components/LoginForm.vue';

const props = defineProps<{
  errorMessage?: string;
}>();
</script>
