<template>
  <label
    class="flex items-center leading-none border-[2px] border-slate-50 p-3 rounded-lg cursor-pointer bg-gray-50 has-[:checked]:border-brand-600 has-[:checked]:bg-slate-100 has-[:checked]:shadow"
    :class="{'!cursor-not-allowed': disabled }"
  >
    <input
      type="checkbox"
      :value="value"
      :checked="isChecked"
      class="w-5 h-5 text-brand-600 bg-gray-100 border-gray-300 rounded focus:ring-0 focus:outline-none focus:ring-offset-0"
      :class="{ 'cursor-not-allowed': disabled }"
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
  disabled: {
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
