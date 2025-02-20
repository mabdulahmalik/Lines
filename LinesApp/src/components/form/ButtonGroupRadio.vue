<script setup>
import { computed } from 'vue';

const props = defineProps({
  options: {
    type: Array,
    required: true,
  },
  modelValue: {
    type: String,
    required: true,
  },
});

const emits = defineEmits(['update:modelValue']);

const internalValue = computed({
  get: () => props.modelValue,
  set: (value) => emits('update:modelValue', value),
});
</script>
<template>
  <div class="flex py-1">
    <label
      v-for="option in options"
      :key="option.value"
      class="cursor-pointer group"
      :class="[
        'sm:px-4 px-3 py-2 border -mr-[1px] first:rounded-l-md last:rounded-r-md text-sm font-medium duration-300 ease text-nowrap',
        modelValue === option.value
          ? 'text-brand-600 bg-brand-50 border-brand-100'
          : 'text-slate-900 bg-white border-slate-200',
        'hover:text-brand-600 hover:bg-brand-50 hover:border-brand-100 hover:z-10 focus:10',
      ]"
    >
      <input type="radio" :value="option.value" v-model="internalValue" class="sr-only" />
      <span
      class="flex items-center flex-nowrap"
        >
        <span>
          {{ option.label }}
        </span>
        <span
          v-if="option.count !== undefined"
          :class="[
            'w-5 h-5 inline-flex justify-center items-center ml-2 px-1 py-0 text-xs font-semibold rounded duration-300 ease',
            modelValue === option.value
              ? 'bg-brand-100 text-brand-600'
              : 'text-slate-500 bg-slate-100 ',
            'group-hover:bg-brand-100 group-hover:text-brand-600',
          ]"
        >
          {{ option.count }}
        </span>
      </span>
    </label>
  </div>
</template>
