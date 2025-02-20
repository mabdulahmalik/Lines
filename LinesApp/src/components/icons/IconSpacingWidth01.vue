<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 28 28"
    fill="none"
  >
    <path
      d="M7 14H21M7 14L9.33333 10.5M7 14L9.33333 17.5M21 14L18.6667 10.5M21 14L18.6667 17.5M24.5 24.5V3.5M3.5 24.5V3.5"
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
    default: "28",
  },
  height: {
    type: String,
    default: "28",
  },
});
</script>
