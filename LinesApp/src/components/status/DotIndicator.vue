<script setup lang="ts">
import { UserAvailability } from '@/api/__generated__/graphql';

withDefaults(
  defineProps<{
    status?: UserAvailability;
    size?: 'sm' | 'md' | 'lg';
  }>(),
  {
    status: UserAvailability.Free,
    size: 'sm',
  }
);
</script>

<template>
  <span
    class="rounded-full"
    :class="{
      'bg-green-400': status === UserAvailability.Free,
      'bg-radical-red-600': status === UserAvailability.Busy,
      'bg-yellow-400': status === UserAvailability.Away,
      'bg-slate-500': status === UserAvailability.Offline,
      'h-3 w-3': size === 'sm',
      'h-3.5 w-3.5': size === 'md',
      'h-4 w-4': size === 'lg',
    }"
  ></span>
</template>
