<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 28 29"
    fill="none"
  >
    <path
      d="M23.3327 7.5L10.4993 20.3333L4.66602 14.5"
      stroke="currentColor"
      :stroke-width="strokeWidth"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
  </svg>
</template>

<script setup>
const props = defineProps({
  width: {
    type: String,
    default: '28',
  },
  height: {
    type: String,
    default: '29',
  },
  strokeWidth: {
    type: String,
    default: '1.5',
  },
});
</script>
