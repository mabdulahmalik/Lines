<script setup lang="ts">
import { FwbBadge } from 'flowbite-vue';

type BadgeSize = 'xs' | 'sm';
type RecordStatusBadge = 'Active' | 'Inactive' | 'ACTIVE' | 'INACTIVE' | 'active' | 'inactive';

const props = withDefaults(
  defineProps<{
    badge?: RecordStatusBadge;
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
      'bg-green-100 text-green-600': typeof badge === 'string' && badge.toLowerCase() === 'active',
      'bg-slate-100 text-slate-900': typeof badge === 'string' && badge.toLowerCase() === 'inactive',
    }"
  >
    <slot
      ><template v-if="badge">{{ badge }}</template></slot
    >
  </fwb-badge>
</template>
