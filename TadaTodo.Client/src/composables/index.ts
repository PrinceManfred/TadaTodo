import { computed, ref, inject } from 'vue';
import { type LoadingState, LoadingSymbol } from '@/plugins/loading';
import { SnackbarSymbol } from '@/symbols';
export function useShake() {
  const isActive = ref(false);
  const shakeClass = computed(() => ({
    shake: isActive.value
  }));

  const shake = () => {
    if (!isActive.value) {
      isActive.value = true;
      setTimeout(() => {
        isActive.value = false;
      }, 820);
    }
  };

  return { shakeClass, shake };
}

export function useLoading() {
  return inject(LoadingSymbol) as LoadingState;
}

export function useSnackbar() {
  const showSnackbar = inject(SnackbarSymbol) as (
    text: string,
    timeout?: number,
    color?: string
  ) => void;
  return { showSnackbar };
}
