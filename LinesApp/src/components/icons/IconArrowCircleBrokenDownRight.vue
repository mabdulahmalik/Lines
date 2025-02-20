<template>
  <svg xmlns="http://www.w3.org/2000/svg" :width="width" :height="height" viewBox="0 0 18 18" fill="none">
    <g clip-path="url(#clip0_2884_3471)">
      <path
        d="M7.05784 1.75457C9.55996 1.08638 12.3403 1.73377 14.3033 3.69674C17.2322 6.62567 17.2322 11.3744 14.3033 14.3033C11.3744 17.2323 6.62563 17.2323 3.6967 14.3033C1.73373 12.3404 1.08634 9.56 1.75454 7.05787M11.2501 6.75004V11.25M11.2501 11.25H6.7501M11.2501 11.25L3.74996 3.75"
        stroke="currentColor"
        stroke-width="1.5"
        stroke-linecap="round"
        stroke-linejoin="round"
      />
    </g>
    <defs>
      <clipPath id="clip0_2884_3471">
        <rect width="18" height="18" fill="white" />
      </clipPath>
    </defs>
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
    default: '14',
  },
  strokeWidth: {
    type: Number,
    default: 1.5,
  },
});
</script>
