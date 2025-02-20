<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 18 18"
    fill="none"
  >
    <path
      d="M9.0003 6.75017V9.75017M9.0003 12.7502H9.0078M7.96179 2.91897L1.79313 13.5739C1.45097 14.1649 1.2799 14.4604 1.30518 14.7029C1.32724 14.9145 1.43806 15.1067 1.61008 15.2318C1.8073 15.3752 2.14875 15.3752 2.83164 15.3752H15.169C15.8519 15.3752 16.1933 15.3752 16.3905 15.2318C16.5625 15.1067 16.6734 14.9145 16.6954 14.7029C16.7207 14.4604 16.5496 14.1649 16.2075 13.5739L10.0388 2.91896C9.69789 2.33009 9.52743 2.03566 9.30503 1.93677C9.11104 1.85051 8.88957 1.85051 8.69558 1.93677C8.47318 2.03566 8.30272 2.33009 7.96179 2.91897Z"
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
    default: "18",
  },
  height: {
    type: String,
    default: "18",
  },
});
</script>
