<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 18 18"
    fill="none"
  >
    <g clip-path="url(#clip0_1555_26385)">
      <path
        d="M9 16.5C13.1421 16.5 16.5 13.1421 16.5 9C16.5 4.85786 13.1421 1.5 9 1.5C4.85786 1.5 1.5 4.85786 1.5 9C1.5 13.1421 4.85786 16.5 9 16.5Z"
        fill="#FFF1F2"
      />
      <path
        d="M6 9H12M16.5 9C16.5 13.1421 13.1421 16.5 9 16.5C4.85786 16.5 1.5 13.1421 1.5 9C1.5 4.85786 4.85786 1.5 9 1.5C13.1421 1.5 16.5 4.85786 16.5 9Z"
        stroke="currentColor"
        stroke-width="1.5"
        stroke-linecap="round"
        stroke-linejoin="round"
      />
    </g>
    <defs>
      <clipPath id="clip0_1555_26385">
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
