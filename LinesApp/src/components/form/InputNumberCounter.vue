<template>
  <div class="relative flex items-center">
    <button
      type="button"
      @click="decrement"
      :class="[
        'border border-slate-300 bg-slate-100 text-slate-900 rounded-s-lg py-3 px-4 lg:px-5 h-11 focus:ring-0 focus:outline-none',
        btnClass,
      ]"
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="16"
        height="16"
        viewBox="0 0 16 16"
        fill="none"
      >
        <path d="M12.6673 8L3.33398 8L12.6673 8Z" fill="currentColor" />
        <path
          d="M12.6673 8L3.33398 8"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
        />
      </svg>
    </button>
    <input
      type="text"
      :id="inputId"
      v-model="quantity"
      :min="min"
      :max="max"
      class="bg-gray-50 border-x-0 border-slate-300 h-11 text-center text-gray-900 text-sm block w-full py-2.5 focus:ring-0 focus:border-slate-300"
      required
    />
    <button
      type="button"
      @click="increment"
      :class="[
        'border border-slate-300 bg-slate-100 text-slate-900 rounded-e-lg py-3 px-4 lg:px-5 h-11 focus:ring-0 focus:outline-none',
        btnClass,
      ]"
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="16"
        height="16"
        viewBox="0 0 16 16"
        fill="none"
      >
        <path d="M8.00065 8.00016H3.33398H8.00065Z" fill="currentColor" />
        <path
          d="M8.00065 3.3335V8.00016M8.00065 8.00016V12.6668M8.00065 8.00016H12.6673M8.00065 8.00016H3.33398"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
        />
      </svg>
    </button>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue';

const props = defineProps({
  modelValue: {
    type: Number,
    required: true,
  },
  min: {
    type: Number,
    default: 0,
  },
  max: {
    type: Number,
    default: 999,
  },
  btnClass: {
    type: String,
    default: '',
  },
});

const emit = defineEmits(['update:modelValue']);

const quantity = ref(props.modelValue);
const inputId = ref('quantity-input');

watch(quantity, (newVal) => {
  if (newVal < props.min) {
    quantity.value = props.min;
  } else if (newVal > props.max) {
    quantity.value = props.max;
  }
  emit('update:modelValue', quantity.value);
});

const decrement = () => {
  if (quantity.value > props.min) {
    quantity.value -= 1;
  }
};

const increment = () => {
  if (quantity.value < props.max) {
    quantity.value += 1;
  }
};
</script>
