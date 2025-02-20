<template>
  <svg xmlns="http://www.w3.org/2000/svg" :width="width" :height="height" viewBox="0 0 20 20" fill="none">
    <path
      d="M13.3333 10L10 6.66667M10 6.66667L6.66667 10M10 6.66667V13.3333M18.3333 10C18.3333 14.6019 14.6019 18.3333 10 18.3333C5.3981 18.3333 1.66667 14.6019 1.66667 10C1.66667 5.3981 5.3981 1.66667 10 1.66667C14.6019 1.66667 18.3333 5.3981 18.3333 10Z"
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
    default: '20',
  },
  height: {
    type: String,
    default: '20',
  },
});
</script>
