import { type App, computed, type ComputedRef, type InjectionKey, ref } from 'vue';

export interface LoadingState {
  isLoading: ComputedRef<boolean>;
  startLoading: () => void;
  stopLoading: () => void;
}
export const LoadingSymbol: InjectionKey<LoadingState> = Symbol('LoadingSymbol');

const LoadingPlugin = {
  install(app: App) {
    const loadingCount = ref(0);
    const isLoading = computed(() => loadingCount.value > 0);
    function startLoading() {
      loadingCount.value++;
    }
    function stopLoading() {
      if (loadingCount.value > 0) {
        loadingCount.value--;
      }
    }

    app.provide(LoadingSymbol, { isLoading, startLoading, stopLoading });
  }
};

export default LoadingPlugin;
