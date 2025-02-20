<script setup lang="ts">
import { ref, watch } from 'vue';
import { FwbButton } from 'flowbite-vue';

const props = defineProps({
  modelValue: {
    type: String,
    default: '',
  },
});

const emits = defineEmits(['update:modelValue']);

const ampm = ref<'AM' | 'PM'>('AM');
const selectedTime = ref<string>(props.modelValue);
const times = ref<string[]>(generateTimes());

// function generateTimes(): string[] {
//   const times = [];
//   for (let hour = 0; hour < 12; hour++) {
//     for (let minute = 0; minute < 60; minute += 10) {
//       const time = `${hour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')}`;
//       times.push(time);
//     }
//   }
//   return times;
// }

function generateTimes(): string[] {
  const times = [];
  for (let hour = 0; hour < 12; hour++) {
    const formattedHour = hour === 0 ? '12' : hour.toString().padStart(2, '0');
    for (let minute = 0; minute < 60; minute += 10) {
      const time = `${formattedHour}:${minute.toString().padStart(2, '0')}`;
      times.push(time);
    }
  }
  return times;
}

function formatTime(time: string): string {
  return `${time} ${ampm.value}`;
}

function selectTime(time: string): void {
  selectedTime.value = `${time} ${ampm.value}`;
  emits('update:modelValue', selectedTime.value);
}

function toggleAMPM(): void {
  ampm.value = ampm.value === 'AM' ? 'PM' : 'AM';
}

// Watch for changes in the prop and update the selected time
watch(
  () => props.modelValue,
  (newValue) => {
    selectedTime.value = newValue;
    if (newValue.includes('AM')) {
      ampm.value = 'AM';
    } else if (newValue.includes('PM')) {
      ampm.value = 'PM';
    }
  }
);
</script>

<template>
  <div class="overflow-hidden bg-white">
    <div
      class="min-w-36 flex justify-between items-center border-b border-slate-200 p-2 mb-2 -mt-[2px]"
    >
      <fwb-button
        @click="toggleAMPM"
        color="light"
        square
        pill
        class="border-transparent text-slate-700 focus:ring-0 hover:bg-brand-50 focus:bg-brand-50"
      >
        <svg
          class="w-5 h-5"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="1.5"
            d="M15 19l-7-7 7-7"
          ></path>
        </svg>
      </fwb-button>
      <span class="text-slate-500 text-xs font-medium">{{ ampm }}</span>
      <fwb-button
        @click="toggleAMPM"
        color="light"
        square
        pill
        class="border-transparent text-slate-700 focus:ring-0 hover:bg-brand-50 focus:bg-brand-50"
      >
        <svg
          class="w-5 h-5"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="1.5"
            d="M9 5l7 7-7 7"
          ></path>
        </svg>
      </fwb-button>
    </div>
    <div class="max-h-72 overflow-y-auto custom-scroll">
      <div class="flex flex-col gap-2 items-center">
        <div v-for="(time, index) in times" :key="index" @click="selectTime(time)">
          <div
            :class="[
              'inline-flex px-3 py-2 cursor-pointer rounded-full font-medium text-xs',
              formatTime(time) === modelValue ? 'bg-brand-600 text-white' : 'hover:bg-brand-50',
            ]"
          >
            {{ formatTime(time) }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>