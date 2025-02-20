<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 16 16"
    fill="none"
  >
    <path
      d="M4.66667 2.66663V13.3333M4.66667 13.3333L2 10.6666M4.66667 13.3333L7.33333 10.6666M11.3333 13.3333V2.66663M11.3333 2.66663L8.66667 5.33329M11.3333 2.66663L14 5.33329"
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
    default: "16",
  },
  height: {
    type: String,
    default: "16",
  },
});
</script>
