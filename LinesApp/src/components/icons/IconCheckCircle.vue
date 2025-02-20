<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 16 16"
    fill="none"
  >
    <g clip-path="url(#clip0_8591_209451)">
      <path
        d="M4.9987 8.00065L6.9987 10.0007L10.9987 6.00065M14.6654 8.00065C14.6654 11.6826 11.6806 14.6673 7.9987 14.6673C4.3168 14.6673 1.33203 11.6826 1.33203 8.00065C1.33203 4.31875 4.3168 1.33398 7.9987 1.33398C11.6806 1.33398 14.6654 4.31875 14.6654 8.00065Z"
        stroke="currentColor"
        stroke-width="1.5"
        stroke-linecap="round"
        stroke-linejoin="round"
      />
    </g>
    <defs>
      <clipPath id="clip0_8591_209451">
        <rect :width="width" :height="height" fill="white" />
      </clipPath>
    </defs>
  </svg>
</template>

<script setup>
const props = defineProps({
  width: {
    type: String,
    default: '16',
  },
  height: {
    type: String,
    default: '16',
  },
});
</script>
