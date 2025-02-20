<template>
  <svg xmlns="http://www.w3.org/2000/svg" :width="width" :height="height" viewBox="0 0 20 20" fill="none">
    <g clip-path="url(#clip0_7169_87225)">
      <path
        d="M12.0827 15.8327L13.7493 17.4993L17.4993 13.7493M18.3203 10.4576C18.3285 10.3059 18.3327 10.1531 18.3327 9.99935C18.3327 5.39698 14.6017 1.66602 9.99935 1.66602C5.39698 1.66602 1.66602 5.39698 1.66602 9.99935C1.66602 14.5289 5.27978 18.2143 9.78142 18.3299M9.99935 4.99935V9.99935L13.1147 11.557"
        stroke="currentColor"
        stroke-width="1.5"
        stroke-linecap="round"
        stroke-linejoin="round"
      />
    </g>
    <defs>
      <clipPath id="clip0_7169_87225">
        <rect width="20" height="20" fill="white" />
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
