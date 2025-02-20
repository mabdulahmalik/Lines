<script setup lang="ts">
import { withDefaults, defineProps, computed } from 'vue';
import { IconChevronDown } from '@/components/icons/index';
import { Encounter, EncounterStage } from '@/api/__generated__/graphql';

const statusOrder = [
  EncounterStage.Waiting,
  EncounterStage.Assigned,
  EncounterStage.Attending,
  EncounterStage.Charting,
  EncounterStage.Completed
];

const props = withDefaults(
  defineProps<{
    progress?: number;
    duration?: number | null;
    encounter?: Encounter;
    status?: string;
    label?: string;
    size?: 'xs' | 'sm' | 'md' | 'lg';
    active?: boolean;
  }>(),
  {
    progress: 100,
    label: '',
    size: 'xs',
    active: false,
  }
);

const sizeClass = computed(() => {
  switch (props.size) {
    case 'xs':
      return 'h-1';
    case 'sm':
      return 'h-1.5';
    case 'lg':
      return 'h-4';
    default:
      return 'h-2.5';
  }
});

const colorClass = computed(() => {
  const currentStatus = props.encounter?.stage || null;
  const currentStatusIndex = currentStatus
    ? statusOrder.indexOf(currentStatus)
    : -1;

  // Validate `props.label` and match it to EncounterStage
  const labelStage = statusOrder.find(
    (stage) => stage.toUpperCase() === props.label?.toUpperCase()
  );
  const labelIndex = labelStage ? statusOrder.indexOf(labelStage) : -1;

  if (currentStatusIndex === statusOrder.length - 1) return 'orange-500';
  if (currentStatusIndex === labelIndex) return 'green-500';
  if (currentStatusIndex < labelIndex) return 'slate-600';
  if (currentStatusIndex > labelIndex) return 'orange-500';
  return 'bg-gray-300'; // Fallback color
});

const stageDuration = computed(() => {
  if (props.duration) {
    const hours = Math.floor(props.duration / 60);
    const minutes = props.duration % 60;
    return `${hours}:${minutes.toString().padStart(2, '0')}`;
  } else return '';
});
</script>

<template>
  <div class="flex flex-col flex-1 min-w-[150px]">
    <!-- Label -->
    <span
      class="mb-0.5 -mt-[3px] text-sm font-medium flex items-center"
      :class="`text-${colorClass}`"
    >
      <span v-if="label">
        {{ label }} <span v-if="stageDuration">({{ stageDuration }})</span>
      </span>
      <IconChevronDown v-if="encounter?.stage == label.toUpperCase() && label !=='Completed'" class="ml-2" />
    </span>
    <span class="hidden bg-orange-500"></span>
    <!-- Progress Container -->
    <div class="relative w-full bg-gray-200 rounded-full overflow-hidden" :class="sizeClass">
      <!-- Progress Bar -->
      <div
        class="absolute top-0 left-0 h-full rounded-full"
        :class="`bg-${colorClass}`"
        :style="{ width: `${progress}%` }"
      ></div>
    </div>
  </div>
</template>
