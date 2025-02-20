<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 16 16"
    fill="none"
  >
    <path
      d="M3.3335 8H12.6668M12.6668 8L8.00016 3.33333M12.6668 8L8.00016 12.6667"
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
    default: "16",
  },
  height: {
    type: String,
    default: "16",
  },
});
</script>
