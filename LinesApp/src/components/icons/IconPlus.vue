<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 20 20"
    fill="none"
  >
    <path
      d="M9.99984 4.1665V15.8332M4.1665 9.99984H15.8332"
      stroke="currentColor"
      stroke-width="2.5"
      stroke-linecap="round"
      stroke-linejoin="round"
    ></path>
  </svg>
</template>

<script setup>

const props = defineProps({
  width: {
    type: String,
    default: "20",
  },
  height: {
    type: String,
    default: "20",
  },
});
</script>
