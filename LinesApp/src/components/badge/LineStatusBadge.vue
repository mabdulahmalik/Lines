<script setup lang="ts">
import { LineDwelling } from '@/api/__generated__/graphql';
import { FwbBadge } from 'flowbite-vue';

type BadgeSize = 'xs' | 'sm';
const props = withDefaults(
  defineProps<{
    badge?: LineDwelling;
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
    class="font-medium m-0 lg:rounded-full inline-block"
    :class="{
      'rounded-none w-full text-center': mobile_full,
      'rounded-full': !mobile_full,
      'leading-[18px]': size === 'xs',
      'bg-blue-100 text-blue-700': typeof badge === 'string' && badge === LineDwelling.In,
      'bg-slate-100 text-slate-900': typeof badge === 'string' && badge === LineDwelling.Out,
      'bg-brand-100 text-brand-700': typeof badge === 'string' && badge === LineDwelling.Undetermined,
    }"
  >
    <slot
      ><template v-if="badge">{{ badge }}</template></slot
    >
  </fwb-badge>
</template>
