<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 18 18"
    fill="none"
  >
    <g clip-path="url(#clip0_1961_211)">
      <path
        d="M7.5 13.2439V15C7.5 15.8284 8.17157 16.5 9 16.5C9.82843 16.5 10.5 15.8284 10.5 15V13.2439M9 1.5V2.25M2.25 9H1.5M4.125 4.125L3.67493 3.67493M13.875 4.125L14.3252 3.67493M16.5 9H15.75M13.5 9C13.5 11.4853 11.4853 13.5 9 13.5C6.51472 13.5 4.5 11.4853 4.5 9C4.5 6.51472 6.51472 4.5 9 4.5C11.4853 4.5 13.5 6.51472 13.5 9Z"
        stroke="currentColor"
        stroke-width="1.5"
        stroke-linecap="round"
        stroke-linejoin="round"
      />
    </g>
    <defs>
      <clipPath id="clip0_1961_211">
        <rect width="18" height="18" fill="white" />
      </clipPath>
    </defs>
  </svg>
</template>

<script setup>

const props = defineProps({
  width: {
    type: String,
    default: "18",
  },
  height: {
    type: String,
    default: "18",
  },
});
</script>
