<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 24 24"
    fill="none"
  >
    <path
      d="M9 13C9.55228 13 10 12.5523 10 12C10 11.4477 9.55228 11 9 11C8.44772 11 8 11.4477 8 12C8 12.5523 8.44772 13 9 13Z"
      fill="#64748B"
    />
    <path
      d="M9 6C9.55228 6 10 5.55228 10 5C10 4.44772 9.55228 4 9 4C8.44772 4 8 4.44772 8 5C8 5.55228 8.44772 6 9 6Z"
      fill="#64748B"
    />
    <path
      d="M9 20C9.55228 20 10 19.5523 10 19C10 18.4477 9.55228 18 9 18C8.44772 18 8 18.4477 8 19C8 19.5523 8.44772 20 9 20Z"
      fill="#64748B"
    />
    <path
      d="M9 13C9.55228 13 10 12.5523 10 12C10 11.4477 9.55228 11 9 11C8.44772 11 8 11.4477 8 12C8 12.5523 8.44772 13 9 13Z"
      stroke="currentColor"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
    <path
      d="M9 6C9.55228 6 10 5.55228 10 5C10 4.44772 9.55228 4 9 4C8.44772 4 8 4.44772 8 5C8 5.55228 8.44772 6 9 6Z"
      stroke="currentColor"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
    <path
      d="M9 20C9.55228 20 10 19.5523 10 19C10 18.4477 9.55228 18 9 18C8.44772 18 8 18.4477 8 19C8 19.5523 8.44772 20 9 20Z"
      stroke="currentColor"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
    <path
      d="M14 13C14.5523 13 15 12.5523 15 12C15 11.4477 14.5523 11 14 11C13.4477 11 13 11.4477 13 12C13 12.5523 13.4477 13 14 13Z"
      fill="#64748B"
    />
    <path
      d="M14 6C14.5523 6 15 5.55228 15 5C15 4.44772 14.5523 4 14 4C13.4477 4 13 4.44772 13 5C13 5.55228 13.4477 6 14 6Z"
      fill="#64748B"
    />
    <path
      d="M14 20C14.5523 20 15 19.5523 15 19C15 18.4477 14.5523 18 14 18C13.4477 18 13 18.4477 13 19C13 19.5523 13.4477 20 14 20Z"
      fill="#64748B"
    />
    <path
      d="M14 13C14.5523 13 15 12.5523 15 12C15 11.4477 14.5523 11 14 11C13.4477 11 13 11.4477 13 12C13 12.5523 13.4477 13 14 13Z"
      stroke="currentColor"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
    <path
      d="M14 6C14.5523 6 15 5.55228 15 5C15 4.44772 14.5523 4 14 4C13.4477 4 13 4.44772 13 5C13 5.55228 13.4477 6 14 6Z"
      stroke="currentColor"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
    <path
      d="M14 20C14.5523 20 15 19.5523 15 19C15 18.4477 14.5523 18 14 18C13.4477 18 13 18.4477 13 19C13 19.5523 13.4477 20 14 20Z"
      stroke="currentColor"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
  </svg>
</template>

<script setup>

const props = defineProps({
  width: {
    type: String,
    default: "24",
  },
  height: {
    type: String,
    default: "24",
  },
});
</script>
