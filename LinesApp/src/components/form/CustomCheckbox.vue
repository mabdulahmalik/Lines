<template>
    <label class="flex items-center text-sm font-medium text-gray-900 dark:text-gray-300">
      <input
        type="checkbox"
        :value="value"
        :checked="isChecked"
        class="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 dark:focus:ring-blue-600 dark:ring-offset-gray-800 focus:ring-2 dark:bg-gray-700 dark:border-gray-600"
        @change="handleChange"
        :disabled="disabled"
      />
      <span class="ms-2">{{ label }}</span>
    </label>
  </template>
  
  <script setup lang="ts">
  import { computed } from 'vue';
  
  const props = defineProps({
    label: {
      type: String,
      required: true,
    },
    value: {
      type: String,
      required: true,
    },
    modelValue: {
      type: Array as () => string[],
      required: true,
    },
    disabled : {
      type: Boolean,
      default: false,
    },
  });
  
  const emit = defineEmits(['update:modelValue']);
  
  // Computed property to check if the value is in the modelValue array
  const isChecked = computed(() => props.modelValue.includes(props.value));
  
  // Handle change event
  function handleChange(event: Event) {
    const newValue = [...props.modelValue];
  
    if ((event.target as HTMLInputElement).checked) {
      newValue.push(props.value);
    } else {
      const index = newValue.indexOf(props.value);
      if (index > -1) {
        newValue.splice(index, 1);
      }
    }
  
    emit('update:modelValue', newValue);
  }
  </script>
  