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
                    :rules="[required]"
                    class="my-2"
                  />
                  <v-text-field
                    v-model="password"
                    label="Password"
                    prepend-icon="mdi-lock"
                    :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                    :type="showPassword ? 'text' : 'password'"
                    name="password"
                    counter
                    @click:append-inner="showPassword = !showPassword"
                    :rules="[required]"
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
                <p>Invalid username or password.</p>
              </v-col>
              <v-col cols="12" class="text-right">
                <p>Don't have an account? <router-link to="/register">Register</router-link></p>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import { useUserStore } from '@/stores/user';
import { useRouter } from 'vue-router';
import { useLoading, useShake } from '@/composables';

const { startLoading, stopLoading } = useLoading();
const router = useRouter();
const userStore = useUserStore();
const username = ref('');
const password = ref('');
const error = ref(false);

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
    await userStore.login(username.value, password.value);
    router.push('/');
  } catch {
    error.value = true;
    shaker.shake();
  } finally {
    stopLoading();
  }
}

function required(value: string) {
  if (!value || value.trim().length === 0) {
    return "Can't be empty.";
  }

  return true;
}
</script>
