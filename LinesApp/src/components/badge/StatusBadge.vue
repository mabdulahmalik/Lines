<script setup lang="ts">
import { EncounterPriority } from '@/api/__generated__/graphql';
import { StatusBadgeType } from '@/types/badge';
import { FwbBadge } from 'flowbite-vue';

type BadgeSize = 'xs' | 'sm';

const props = withDefaults(
  defineProps<{
    badge?: StatusBadgeType;
    size?: BadgeSize;
    mobile_full?: boolean;
  }>(),
  {
    size: 'xs',
  }
);
</script>

<template>
  <fwb-badge
    :size="props.size"
    class="lg:rounded text-sm font-semibold m-0 justify-start min-w-20"
    :class="{
      'rounded-none': mobile_full,
      'text-white bg-radical-red-700': badge === 'SOS',
      'text-white bg-radical-red-600': badge === EncounterPriority.Stat,
      'text-white bg-turquoise-blue-500': badge === EncounterPriority.Normal,
      'text-white bg-blue-700': badge === EncounterPriority.Routine,
      'text-white bg-yellow-400': badge === 'ON HOLD',
    }"
  >
    <template v-if="badge === EncounterPriority.Stat" #icon>
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="16"
        height="16"
        viewBox="0 0 16 16"
        fill="none"
        class="mr-1"
      >
        <path d="M4 14V10" stroke="white" stroke-width="2" stroke-linecap="round" />
        <path d="M8 14V6" stroke="white" stroke-width="2" stroke-linecap="round" />
        <path d="M12 14V2" stroke="white" stroke-width="2" stroke-linecap="round" />
      </svg>
    </template>
    <template v-if="badge === EncounterPriority.Normal" #icon>
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="16"
        height="16"
        viewBox="0 0 16 16"
        fill="none"
        class="mr-1"
      >
        <path d="M4 14V10" stroke="white" stroke-width="2" stroke-linecap="round" />
        <path d="M8 14V6" stroke="white" stroke-width="2" stroke-linecap="round" />
      </svg>
    </template>
    <template v-if="badge === EncounterPriority.Routine" #icon>
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="16"
        height="16"
        viewBox="0 0 16 16"
        fill="none"
        class="mr-1"
      >
        <path d="M4 14V10" stroke="white" stroke-width="2" stroke-linecap="round" />
      </svg>
    </template>
    <slot
      ><template v-if="badge">{{ badge }}</template></slot
    >
  </fwb-badge>
</template>

<style scoped>
.rounded-none {
  border-radius: 0 !important;
}
</style>
