<script setup lang="ts">
import { IconArrowNarrowRight } from '@/components/icons';
import StatusBadge from '@/components/badge/StatusBadge.vue';
import { EncounterPriority } from '@/api/__generated__/graphql';
</script>

<template>
  <div>
    <div class="p-4 flex items-center gap-2">
      <StatusBadge :badge="EncounterPriority.Stat" :mobile_full="false" />
      <span><IconArrowNarrowRight width="16" height="16" /></span>
      <StatusBadge :badge="EncounterPriority.Normal" :mobile_full="false" />
    </div>
  </div>
</template>
