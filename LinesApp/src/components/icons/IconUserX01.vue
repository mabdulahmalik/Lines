<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 18 18"
    fill="none"
  >
    <path
      d="M12.375 12L16.125 15.75M16.125 12L12.375 15.75M9 11.625H5.625C4.57833 11.625 4.05499 11.625 3.62914 11.7542C2.67034 12.045 1.92003 12.7953 1.62918 13.7541C1.5 14.18 1.5 14.7033 1.5 15.75M10.875 5.625C10.875 7.48896 9.36396 9 7.5 9C5.63604 9 4.125 7.48896 4.125 5.625C4.125 3.76104 5.63604 2.25 7.5 2.25C9.36396 2.25 10.875 3.76104 10.875 5.625Z"
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
    default: "18",
  },
  height: {
    type: String,
    default: "18",
  },
});
</script>
