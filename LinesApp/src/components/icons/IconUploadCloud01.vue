<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 40 41"
    fill="none"
  >
    <path
      d="M6.66732 27.5704C4.65734 26.2249 3.33398 23.9337 3.33398 21.3333C3.33398 17.4274 6.31983 14.2188 10.1336 13.8656C10.9137 9.12022 15.0344 5.5 20.0007 5.5C24.9669 5.5 29.0876 9.12022 29.8677 13.8656C33.6815 14.2188 36.6673 17.4274 36.6673 21.3333C36.6673 23.9337 35.344 26.2249 33.334 27.5704M13.334 27.1667L20.0007 20.5M20.0007 20.5L26.6673 27.1667M20.0007 20.5V35.5"
      stroke="currentColor"
      stroke-width="2"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
  </svg>
</template>

<script setup>

const props = defineProps({
  width: {
    type: String,
    default: "40",
  },
  height: {
    type: String,
    default: "41",
  },
});
</script>
