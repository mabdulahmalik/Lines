<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 20 20"
    fill="none"
  >
    <path
      d="M11.6668 9.1665H6.66683M8.3335 12.4998H6.66683M13.3335 5.83317H6.66683M16.6668 5.6665V14.3332C16.6668 15.7333 16.6668 16.4334 16.3943 16.9681C16.1547 17.4386 15.7722 17.821 15.3018 18.0607C14.767 18.3332 14.067 18.3332 12.6668 18.3332H7.3335C5.93336 18.3332 5.2333 18.3332 4.69852 18.0607C4.22811 17.821 3.84566 17.4386 3.60598 16.9681C3.3335 16.4334 3.3335 15.7333 3.3335 14.3332V5.6665C3.3335 4.26637 3.3335 3.56631 3.60598 3.03153C3.84566 2.56112 4.22811 2.17867 4.69852 1.93899C5.2333 1.6665 5.93336 1.6665 7.3335 1.6665H12.6668C14.067 1.6665 14.767 1.6665 15.3018 1.93899C15.7722 2.17867 16.1547 2.56112 16.3943 3.03153C16.6668 3.56631 16.6668 4.26637 16.6668 5.6665Z"
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
    default: "20",
  },
  height: {
    type: String,
    default: "20",
  },
});
</script>
