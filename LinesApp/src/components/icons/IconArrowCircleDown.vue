<template>
  <svg xmlns="http://www.w3.org/2000/svg" :width="width" :height="height" viewBox="0 0 20 20" fill="none">
    <g clip-path="url(#clip0_7987_1278)">
      <path
        d="M6.6665 9.99935L9.99984 13.3327M9.99984 13.3327L13.3332 9.99935M9.99984 13.3327V6.66602M18.3332 9.99935C18.3332 14.6017 14.6022 18.3327 9.99984 18.3327C5.39746 18.3327 1.6665 14.6017 1.6665 9.99935C1.6665 5.39698 5.39746 1.66602 9.99984 1.66602C14.6022 1.66602 18.3332 5.39698 18.3332 9.99935Z"
        stroke="currentColor"
        stroke-width="1.5"
        stroke-linecap="round"
        stroke-linejoin="round"
      />
    </g>
    <defs>
      <clipPath id="clip0_7987_1278">
        <rect :width="width" :height="height" fill="white" />
      </clipPath>
    </defs>
  </svg>
</template>

<script setup>
const props = defineProps({
  width: {
    type: String,
    default: '20',
  },
  height: {
    type: String,
    default: '20',
  },
});
</script>
