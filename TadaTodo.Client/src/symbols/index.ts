import type { InjectionKey } from 'vue';
export const SnackbarSymbol: InjectionKey<
  (text: string, timeout?: number, color?: string) => void
> = Symbol('SnackbarSymbol');
