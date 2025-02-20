<script setup lang="ts">
import { IconClockFastForward } from '@/components/icons';


const props = defineProps<{
  reason: String;
  // job: Job;
  // encounter: Encounter;
}>();
</script>

<template>
  <div class="p-4 rounded-lg bg-yellow-400 text-white flex justify-between">
    <div class="font-bold text-sm">
      <span>ON HOLD</span>
      <div class="pt-1">"{{ props.reason }}"</div>
    </div>
    <div class="flex items-center gap-2">
      <span><IconClockFastForward /></span>
      <span>0:25</span>
    </div>
  </div>
</template>
