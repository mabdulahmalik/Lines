<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 14 8"
    fill="none"
  >
    <path
      fill-rule="evenodd"
      clip-rule="evenodd"
      d="M4.66797 0.0625C2.49335 0.0625 0.730469 1.82538 0.730469 4C0.730469 6.17462 2.49335 7.9375 4.66797 7.9375H9.33463C11.5093 7.9375 13.2721 6.17462 13.2721 4C13.2721 1.82538 11.5093 0.0625 9.33463 0.0625H4.66797ZM9.33463 1.8125C8.12651 1.8125 7.14714 2.79188 7.14714 4C7.14714 5.20812 8.12651 6.1875 9.33463 6.1875C10.5428 6.1875 11.5221 5.20812 11.5221 4C11.5221 2.79188 10.5428 1.8125 9.33463 1.8125Z"
      fill="currentColor"
    />
  </svg>
</template>

<script setup>
const props = defineProps({
  width: {
    type: String,
    default: '14',
  },
  height: {
    type: String,
    default: '8',
  },
});
</script>
