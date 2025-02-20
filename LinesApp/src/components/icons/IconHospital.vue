<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    :width="width"
    :height="height"
    viewBox="0 0 24 24"
    fill="none"
  >
    <path d="M7.20018 12H5.6001V13.5998H7.20018V12Z" fill="currentColor"></path>
    <path
      d="M10.4001 12H8.80029V13.5998H10.4001V12Z"
      fill="currentColor"
    ></path>
    <path
      d="M10.4001 7.20018H8.80026V5.6001H7.20018V7.20018H5.6001V8.80026H7.20018V10.4001H8.80026V8.80026H10.4001V7.20018Z"
      fill="currentColor"
    ></path>
    <path d="M13.6001 12H12V13.5998H13.6001V12Z" fill="currentColor"></path>
    <path
      d="M13.6001 8.80029H12V10.4004H13.6001V8.80029Z"
      fill="currentColor"
    ></path>
    <path
      d="M13.6001 5.6001H12V7.20018H13.6001V5.6001Z"
      fill="currentColor"
    ></path>
    <path
      d="M7.20018 15.2002H5.6001V16.8H7.20018V15.2002Z"
      fill="currentColor"
    ></path>
    <path
      d="M10.4001 15.2002H8.80029V16.8H10.4001V15.2002Z"
      fill="currentColor"
    ></path>
    <path
      d="M13.6001 15.2002H12V16.8H13.6001V15.2002Z"
      fill="currentColor"
    ></path>
    <path
      d="M16.7999 7.1999V3.3999C16.7999 2.84762 16.3522 2.3999 15.7999 2.3999H3.3999C2.84762 2.3999 2.3999 2.84762 2.3999 3.3999V21.5999H8.79998V19.9998H10.3998V21.5999H21.5999V8.1999C21.5999 7.64762 21.1522 7.1999 20.5999 7.1999H16.7999ZM11.9999 19.9998V18.4H7.1999V19.9998H3.99998V3.99998H15.1998V20.0001L11.9999 19.9998ZM19.9998 19.9998H16.7999V18.4H18.4V16.7999H16.7999V15.2001H18.4V13.5997H16.7999V11.9999H18.4V10.3998H16.7999V8.79974H19.9998V19.9998Z"
      fill="currentColor"
    ></path>
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
