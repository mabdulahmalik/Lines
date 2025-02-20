<script setup lang="ts">
import DotIndicator from './DotIndicator.vue';
import { UserAvailability } from '@/api/__generated__/graphql';

const props = withDefaults(
  defineProps<{
    status?: UserAvailability;
    checked?: boolean;
  }>(),
  {
    status: UserAvailability.Free,
  }
);
</script>

<template>
  <div
    class="flex items-center justify-between rounded-lg font-medium text-sm text-slate-900 py-3 px-3 duration-300 ease-in-out hover:bg-brand-50 cursor-pointer"
    :class="{'bg-brand-50': props.checked, 'bg-slate-50': !props.checked}"
  >
    <div class="flex items-center justify-between">
      <!-- <span
        class="h-3.5 w-3.5 rounded-full mr-2"
        :class="{
          'bg-green-400': status === 'Available',
          'bg-radical-red-600': status === 'Unavailable',
          'bg-yellow-400': status === 'Lunch',
          'bg-slate-500': status === 'Offline',
        }"
      ></span> -->
      <DotIndicator :status="props.status" class="mr-2"/>
      <span>{{ props.status }}</span>
    </div>
    <div v-if="props.checked" class="flex">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="16"
        height="16"
        viewBox="0 0 16 16"
        fill="none"
      >
        <path
          d="M13.3332 4L5.99984 11.3333L2.6665 8"
          stroke="#6A5ACD"
          stroke-width="1.5"
          stroke-linecap="round"
          stroke-linejoin="round"
        />
      </svg>
    </div>
  </div>
</template>
