<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 25 24"
    fill="none"
  >
    <path
      d="M2.5 2L10.5 10M10.5 10L10.5 2M10.5 10L2.5 10"
      stroke="currentColor"
      stroke-width="1.5"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
    <path
      d="M22.5 22L14.5 14M14.5 14L14.5 22M14.5 14L22.5 14"
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
    default: '25',
  },
  height: {
    type: String,
    default: '24',
  },
});
</script>
