<template>
    <svg
      xmlns="http://www.w3.org/2000/svg"
      :width="width"
      :height="height"
      viewBox="0 0 16 16"
      fill="none"
    >
      <path
        d="M8 13.3333V2.66667M8 2.66667L4 6.66667M8 2.66667L12 6.66667"
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
      default: "16",
    },
    height: {
      type: String,
      default: "16",
    },
  });
  </script>
  