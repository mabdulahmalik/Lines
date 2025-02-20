<script setup lang="ts">
import { IconCheckCircle } from '@/components/icons';
const props = withDefaults(
  defineProps<{
    title: string;
    isCheck?: boolean;
  }>(),
  {
    isCheck: false,
  }
);
</script>
<template>
  <div class="w-full flex items-center justify-between">
    <span class="text-base font-semibold"> {{ props.title }} </span>
    <div class="text-slate-500 hover:text-slate-900">
      <IconCheckCircle v-if="props.isCheck" class="text-green-500"> </IconCheckCircle>
    </div>
  </div>
</template>
