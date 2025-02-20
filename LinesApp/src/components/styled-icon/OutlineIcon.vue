<script setup lang="ts">
import { computed } from 'vue';

type Size = null | 'sm' | 'md' | 'lg' | 'xl';
type Type = null | 'error' | 'brand' | 'gray' | 'warning' | 'success';
const props = withDefaults(
  defineProps<{
    type?: Type;
    size?: Size;
  }>(),
  {
    type: 'gray',
    size: 'md',
  }
);

const iconSize = computed(() => {
  let size;
  switch (props.size) {
    case 'sm':
      size = { outer: '34px', inner: '24px' };
      break;
    case 'md':
      size = { outer: '38px', inner: '28px' };
      break;
    case 'lg':
      size = { outer: '42px', inner: '32px' };
      break;
    case 'xl':
      size = { outer: '46px', inner: '36px' };
      break;
    default:
      size = { outer: '38px', inner: '28px' };
  }
  return size;
});
</script>

<template>
  <div
    class="flex justify-center items-center rounded-full p-[2px] ring ring-[2px] ring-inset"
    :style="{ width: iconSize.outer, height: iconSize.outer }"
    :class="{
      'ring-radical-red-600/10': type === 'error',
      'ring-turquoise-blue-600/10': type === 'success',
      'ring-brand-700/10': type === 'brand',
      'ring-yellow-400/10': type === 'warning',
      'ring-gray-600/10': type === 'gray',
    }"
  >
    <div
      class="flex justify-center items-center rounded-full ring ring-[2px] ring-inset"
      :style="{ width: iconSize.inner, height: iconSize.inner }"
      :class="{
      'ring-radical-red-600/30 text-radical-red-600': type === 'error',
      'ring-turquoise-blue-600/30 text-turquoise-blue-600': type === 'success',
      'ring-brand-700/30 text-brand-700': type === 'brand',
      'ring-yellow-400/30 text-yellow-400': type === 'warning',
      'ring-slate-6500/30 text-slate-600': type === 'gray',
    }"
    >
      <slot></slot>
    </div>
  </div>
</template>
