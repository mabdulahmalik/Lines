<script setup lang="ts">
const props = defineProps<{
  label: string;
  archived?: boolean;
}>();
</script>
<template>
  <div class="flex items-center">
    <span class="w-4 h-4 bg-gray-100 border border-gray-300 rounded-full inline-block"></span>
    <label v-if="props.label" class="ms-2" :class="{ 'line-through': archived }">
      {{ props.label }}
    </label>
  </div>
</template>
