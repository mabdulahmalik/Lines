<template>
  <svg xmlns="http://www.w3.org/2000/svg" :width="width" :height="height" viewBox="0 0 16 16" fill="none">
    <path
      d="M11.3346 4.66663L4.66797 11.3333M4.66797 4.66663L11.3346 11.3333"
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
    default: '16',
  },
  height: {
    type: String,
    default: '16',
  },
});
</script>
