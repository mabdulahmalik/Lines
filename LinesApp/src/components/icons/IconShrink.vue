<template>
  <svg xmlns="http://www.w3.org/2000/svg" :width="width" :height="width" viewBox="0 0 16 16" fill="none">
    <path
      d="M1.33329 1.33317L6.66663 6.6665M6.66663 6.6665L6.66663 1.33317M6.66663 6.6665L1.33329 6.6665"
      stroke="#1E293B"
      stroke-width="1.5"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
    <path
      d="M14.6667 14.6668L9.33337 9.3335M9.33337 9.3335V14.6668M9.33337 9.3335H14.6667"
      stroke="#0F172A"
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
    default: '16',
  },
  height: {
    type: String,
    default: '16',
  },
});
</script>
