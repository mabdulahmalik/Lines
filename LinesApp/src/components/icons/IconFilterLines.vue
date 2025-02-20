<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 16 16"
    fill="none"
  >
    <path
      d="M4 8H12M2 4H14M6 12H10"
      stroke="currentColor"
      stroke-width="1.5"
      stroke-linecap="round"
      stroke-linejoin="round"
    ></path>
  </svg>
</template>

<script setup>

const props = defineProps({
  width: {
    type: String,
    default: "16",
  },
  height: {
    type: String,
    default: "16",
  },
});
</script>
