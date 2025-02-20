<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="height"
    :height="width"
    viewBox="0 0 14 14"
    fill="none"
  >
    <path
      d="M2.3335 7H11.6668M11.6668 7L8.16683 3.5M11.6668 7L8.16683 10.5"
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
    default: '14',
  },
  height: {
    type: String,
    default: '14',
  },
});
</script>
