<script setup lang="ts">
import { FwbBadge } from 'flowbite-vue';
import { EncounterStage } from '@/api/__generated__/graphql';
import {
  IconArrowNarrowDownRight,
  IconArrowNarrowRight,
  IconClipborodCheck,
  IconHand,
} from '@/components/icons';

type BadgeSize = 'xs' | 'sm';

const props = withDefaults(
  defineProps<{
    badge?: EncounterStage;
    size?: BadgeSize;
  }>(),
  {
    size: 'xs',
  }
);
</script>

<template>
  <fwb-badge
    :size="props.size"
    class="rounded mr-0 text-orange-500 bg-orange-100 capitalize"
    :class="{
      'leading-[18px]': size === 'xs',
    }"
  >
    <slot
      ><template v-if="badge">
        {{ badge.toLowerCase() }}
          <IconArrowNarrowDownRight v-if="badge === EncounterStage.Assigned" class="ml-1.5" />
          <IconArrowNarrowRight v-if="badge === EncounterStage.Attending" class="ml-1.5" />
          <IconHand v-if="badge === EncounterStage.Waiting" class="ml-1.5" />
          <IconClipborodCheck v-if="badge === EncounterStage.Charting" class="ml-1.5" />
         </template
    ></slot>
  </fwb-badge>
</template>
