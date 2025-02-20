<script setup>
import { FwbButton } from 'flowbite-vue';
import { IconXClose } from '../icons';
defineProps({
  file: { type: Object, required: true },
  tag: { type: String, default: 'li' },
});
defineEmits(['remove']);
</script>
<template>
  <component :is="tag" class="file-preview relative w-1/5 m-2 overflow-hidden">
    <fwb-button
      @click="$emit('remove', file)"
      class="bg-radical-red-600 hover:bg-radical-red-600 absolute top-1 right-1 text-white h-5 w-5 text-xs flex items-center justify-center shadow-md"
      pill
      square
    >
      <IconXClose height="16" width="16" />
    </fwb-button>
    <img
      :src="file.url"
      :alt="file.file.name"
      :title="file.file.name"
      class="w-full h-full object-cover"
    />

    <span
      class="status-indicator loading-indicator absolute bottom-1 right-1 text-xs px-2 py-1"
      v-show="file.status == 'loading'"
      >In Progress</span
    >
    <span
      class="status-indicator success-indicator absolute bottom-1 right-1 text-xs px-2 py-1 bg-green-600 text-green-900"
      v-show="file.status == true"
      >Uploaded</span
    >
    <span
      class="status-indicator failure-indicator absolute bottom-1 right-1 text-xs px-2 py-1 bg-red-700 text-white"
      v-show="file.status == false"
      >Error</span
    >
  </component>
</template>
<style scoped>
@keyframes pulse {
  0% {
    background: #fff;
  }
  50% {
    background: #ddd;
  }
  100% {
    background: #fff;
  }
}

.loading-indicator {
  animation: pulse 1.5s linear infinite;
  color: #000;
}
</style>
