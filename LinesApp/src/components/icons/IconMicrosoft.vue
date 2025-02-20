<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 16 16"
    fill="none"
  >
    <g clip-path="url(#clip0_1702_66512)">
      <path d="M0 0H16V16H0V0Z" fill="white" />
      <path d="M0.695312 0.695312H7.65183V7.65183H0.695312V0.695312Z" fill="#F35325" />
      <path d="M8.34766 0.695312H15.3042V7.65183H8.34766V0.695312Z" fill="#81BC06" />
      <path d="M0.695312 8.34766H7.65183V15.3042H0.695312V8.34766Z" fill="#05A6F0" />
      <path d="M8.34766 8.34766H15.3042V15.3042H8.34766V8.34766Z" fill="#FFBA08" />
    </g>
    <defs>
      <clipPath id="clip0_1702_66512">
        <rect width="16" height="16" fill="white" />
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
