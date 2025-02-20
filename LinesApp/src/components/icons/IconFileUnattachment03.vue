<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 20 20"
    fill="none"
  >
    <g clip-path="url(#clip0_6621_101839)">
      <path
        d="M16.6663 5.83268V5.66602C16.6663 4.26588 16.6663 3.56582 16.3939 3.03104C16.1542 2.56063 15.7717 2.17818 15.3013 1.9385C14.7665 1.66602 14.0665 1.66602 12.6663 1.66602H7.33301C5.93288 1.66602 5.23281 1.66602 4.69803 1.9385C4.22763 2.17818 3.84517 2.56063 3.60549 3.03104C3.33301 3.56582 3.33301 4.26588 3.33301 5.66602V14.3327C3.33301 15.7328 3.33301 16.4329 3.60549 16.9677C3.84517 17.4381 4.22763 17.8205 4.69803 18.0602C5.23281 18.3327 5.93288 18.3327 7.33301 18.3327H10.4163M14.9997 14.9993V10.416C14.9997 9.72566 15.5593 9.16602 16.2497 9.16602C16.94 9.16602 17.4997 9.72566 17.4997 10.416V14.9993C17.4997 16.3801 16.3804 17.4993 14.9997 17.4993C13.619 17.4993 12.4997 16.3801 12.4997 14.9993V11.666"
        stroke="currentColor"
        stroke-width="1.5"
        stroke-linecap="round"
        stroke-linejoin="round"
      />
      <path d="M1 4L19 15" stroke="#475569" stroke-width="1.5" stroke-linecap="round" />
    </g>
    <defs>
      <clipPath id="clip0_6621_101839">
        <rect width="20" height="20" fill="white" />
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
