<template>
  <svg
    :width="width"
    :height="height"
    fill="none"
    stroke="currentColor"
    viewBox="0 0 24 24"
    xmlns="http://www.w3.org/2000/svg"
  >
    <path
      d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
      stroke-linecap="round"
      stroke-linejoin="round"
      stroke-width="2"
    />
  </svg>
</template>

<script setup>

const props = defineProps({
  width: {
    type: String,
    default: "18",
  },
  height: {
    type: String,
    default: "18",
  },
});
</script>
