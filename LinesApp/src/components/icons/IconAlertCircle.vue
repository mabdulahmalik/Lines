<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 28 28"
    fill="none"
  >
    <path
      d="M14.0007 9.3335V14.0002M14.0007 18.6668H14.0123M25.6673 14.0002C25.6673 20.4435 20.444 25.6668 14.0007 25.6668C7.55733 25.6668 2.33398 20.4435 2.33398 14.0002C2.33398 7.55684 7.55733 2.3335 14.0007 2.3335C20.444 2.3335 25.6673 7.55684 25.6673 14.0002Z"
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
    default: "28",
  },
  height: {
    type: String,
    default: "28",
  },
  strokeWidth: {
    type: String,
    default: "1.5",
  }
});
</script>
