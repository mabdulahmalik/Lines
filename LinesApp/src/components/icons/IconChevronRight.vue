<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 20 20"
    fill="none"
  >
    <path
      d="M7.5 15L12.5 10L7.5 5"
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
    default: "20",
  },
  height: {
    type: String,
    default: "20",
  },
});
</script>
