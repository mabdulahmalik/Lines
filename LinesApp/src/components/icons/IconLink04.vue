<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 20 20"
    fill="none"
  >
    <path
      d="M6.24984 5.8335H5.83317C3.53198 5.8335 1.6665 7.69898 1.6665 10.0002C1.6665 12.3014 3.53198 14.1668 5.83317 14.1668H7.49984C9.80102 14.1668 11.6665 12.3014 11.6665 10.0002M13.7498 14.1668H14.1665C16.4677 14.1668 18.3332 12.3014 18.3332 10.0002C18.3332 7.69898 16.4677 5.8335 14.1665 5.8335H12.4998C10.1987 5.8335 8.33317 7.69898 8.33317 10.0002"
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
    default: "20",
  },
  height: {
    type: String,
    default: "20",
  },
});
</script>
