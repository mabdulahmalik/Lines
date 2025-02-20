<script setup lang="ts">

const props = withDefaults(
  defineProps<{
    checked?: boolean;
  }>(),
  {
    checked: false,
  }
);
</script>

<template>
  <div
    class="flex items-center justify-between rounded-lg font-medium text-sm text-slate-900 py-3 px-3 duration-300 ease-in-out hover:bg-brand-50 cursor-pointer"
    :class="{'bg-brand-50': props.checked}"
  >
    <div class="flex items-center justify-between">
      <slot></slot>
    </div>
    <div v-if="props.checked" class="flex">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="16"
        height="16"
        viewBox="0 0 16 16"
        fill="none"
      >
        <path
          d="M13.3332 4L5.99984 11.3333L2.6665 8"
          stroke="#6A5ACD"
          stroke-width="1.5"
          stroke-linecap="round"
          stroke-linejoin="round"
        />
      </svg>
    </div>
  </div>
</template>
