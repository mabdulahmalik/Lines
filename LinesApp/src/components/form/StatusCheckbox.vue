<template>
    <label
      class="flex items-center min-h-14 leading-none border border-slate-300 text-slate-900 p-3 rounded-lg cursor-pointer bg-white has-[:checked]:border-brand-600 has-[:checked]:bg-brand-50 has-[:checked]:shadow"
    >
      <input
        type="checkbox"
        :checked="modelValue"
        class="w-4 h-4 text-brand-600 bg-gray-100 border-gray-300 rounded focus:ring-0 focus:outline-none focus:ring-offset-0"
        @change="handleChange"
      />
      <div class="flex flex-1 items-center justify-between gap-2">
          <span class="ms-2 text-sm font-medium">{{ props.label }}</span>
          <slot></slot>
    </div>
        
    </label>
  </template>
  
  <script setup lang="ts">
  const props = defineProps({
    label: {
      type: String,
      required: true,
    },
    modelValue: {
      type: Boolean,
      required: true,
    },
  });
  
  const emit = defineEmits(['update:modelValue']);
  
  // Handle change event
  function handleChange(event: Event) {
    emit('update:modelValue', (event.target as HTMLInputElement).checked);
  }
  </script>
  