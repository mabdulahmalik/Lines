<script setup>
defineProps({
  backgroundColor: {
    type: String,
    default: '#E2E8F0',
  },
});
</script>

<template>
  <div
    :class="['skeleton-box relative overflow-hidden']"
    :style="`background-color:${backgroundColor};`"
  ></div>
</template>

<style scoped>
.skeleton-box::after {
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  transform: translateX(-100%);
  background-image: linear-gradient(
    90deg,
    rgba(255, 255, 255, 0) 0%,
    rgba(255, 255, 255, 0.2) 20%,
    rgba(255, 255, 255, 0.5) 60%,
    rgba(255, 255, 255, 0)
  );
  animation: shimmer 1.5s infinite linear;
  content: '';
}

@keyframes shimmer {
  100% {
    transform: translateX(100%);
  }
}
</style>
