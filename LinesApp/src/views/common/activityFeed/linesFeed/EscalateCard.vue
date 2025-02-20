<script setup lang="ts">
import { IconArrowNarrowRight } from '@/components/icons';
import StatusBadge from '@/components/badge/StatusBadge.vue';
import { EncounterPriority } from '@/api/__generated__/graphql';


const props = defineProps<{
  reason: String;
  // job: Job;
  // encounter: Encounter;
}>();
</script>

<template>
  <div>
    <div class="p-4 flex items-center gap-2">
      <StatusBadge :badge="EncounterPriority.Normal" :mobile_full="false" />
      <span><IconArrowNarrowRight width="16" height="16" /></span>
      <StatusBadge :badge="EncounterPriority.Stat" :mobile_full="false" />
    </div>

    <div class="p-4 rounded-lg bg-radical-red-600 text-white mt-4">
      <div class="font-bold text-sm">
        <span>STAT</span>
        <div class="pt-1">"{{ props.reason }}"</div>
      </div>
    </div>
  </div>
</template>
