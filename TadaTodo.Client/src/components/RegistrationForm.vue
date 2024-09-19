<template>
  <v-container class="fill-height" max-width="700px">
    <v-row>
      <v-col>
        <v-card
          :class="shaker.shakeClass"
          prepend-icon="mdi-login"
          class="py-4"
          rel="noopener noreferrer"
          title="Login"
        >
          <v-card-text>
            <v-row>
              <v-col cols="12">
                <v-form @submit.prevent.stop="login" v-model="isValid">
                  <v-text-field
                    v-model="username"
                    label="Username"
                    prepend-icon="mdi-account"
                    counter
                    persistent-counter
                    :rules="[usernameCharacters, lengthBetween(3, 50)]"
                    class="my-2"
                  />
                  <v-text-field
                    v-model="passwordOne"
                    label="Password"
                    prepend-icon="mdi-lock"
                    :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                    :type="showPassword ? 'text' : 'password'"
                    name="passwordOne"
                    counter
                    persistent-counter
                    @input="passwordOneChanged"
                    @click:append-inner="showPassword = !showPassword"
                    :rules="[minimumLength(6)]"
                    class="my-2"
                  />
                  <v-text-field
                    v-model="passwordTwo"
                    ref="passwordTwoRef"
                    label="Reenter Password"
                    prepend-icon="mdi-lock"
                    :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                    :type="showPassword ? 'text' : 'password'"
                    name="passwordTwo"
                    counter
                    persistent-counter
                    @click:append-inner="showPassword = !showPassword"
                    :rules="[matchPasswords, minimumLength(6)]"
                    class="my-2"
                  />
                  <v-btn
                    type="submit"
                    prepend-icon="mdi-login"
                    block
                    variant="flat"
                    color="primary"
                    rounded
                    >Login</v-btn
                  >
                </v-form>
              </v-col>
              <v-col v-if="error" class="text-error">
                <p>{{ error }}</p>
              </v-col>
              <v-col cols="12" class="text-right">
                <p>Already have an account? <router-link to="/login">Login</router-link></p>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { inject, reactive, ref, useTemplateRef } from 'vue';
import { useUserStore } from '@/stores/user';
import { useRouter } from 'vue-router';
import { type LoadingState, LoadingSymbol } from '@/plugins/loading';
import { useShake } from '@/composables';

const { startLoading, stopLoading } = inject(LoadingSymbol) as LoadingState;
const router = useRouter();
const userStore = useUserStore();
const username = ref('');
const passwordOne = ref('');
const passwordTwo = ref('');
const passwordTwoRef = useTemplateRef('passwordTwoRef');

const error = ref<string | null>(null);

const showPassword = ref(false);
const isValid = ref<boolean | null>(null);
const shaker = reactive(useShake());

async function login() {
  if (!isValid.value) {
    shaker.shake();
    return;
  }
  try {
    startLoading();
    await userStore.register(username.value, passwordOne.value);
    router.push('/');
  } catch (e: any) {
    error.value = e as string;
    shaker.shake();
  } finally {
    stopLoading();
  }
}

function lengthBetween(min: number, max: number) {
  return (value: string) => {
    if (!value || value.length < min || value.length > max)
      return `Must be between ${min} and ${max} characters long.`;

    return true;
  };
}

function minimumLength(min: number) {
  return (value: string) => {
    if (!value || value.length < min) return `Must be at least ${min} characters long.`;

    return true;
  };
}

function usernameCharacters(value: string) {
  const regex = /^[a-zA-Z0-9-_]*$/;
  if (!value.match(regex)) {
    return 'Can only contain alphanumeric characters, dashes (-), and underscores (_).';
  }
  return true;
}

function matchPasswords(value: string) {
  if (value !== passwordOne.value) {
    return 'Passwords must match.';
  }
  return true;
}

function passwordOneChanged() {
  passwordTwoRef.value?.validate();
}
</script>
