<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 13 10"
    fill="none"
  >
    <path
      d="M11.9987 1.33333L4.66536 8.66666L1.33203 5.33333"
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
    default: "13",
  },
  height: {
    type: String,
    default: "10",
  },
});
</script>
