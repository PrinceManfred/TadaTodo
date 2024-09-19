import { computed, ref } from 'vue';
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
