<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 24 24"
    fill="none"
  >
    <path
      d="M14 10L21 3M21 3H15M21 3V9M10 14L3 21M3 21H9M3 21L3 15"
      stroke="currentColor"
      stroke-width="1.5"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
  </svg>
</template>

<script setup>
const props = defineProps({
  width: {
    type: String,
    default: '24',
  },
  height: {
    type: String,
    default: '24',
  },
});
</script>
