<script setup lang="ts">
import { DurationUnit, Routine, RollingSchedule } from '@/api/__generated__/graphql';
import { FwbSelect, FwbInput } from 'flowbite-vue';
import { reactive, computed, watch, onMounted } from 'vue';

const props = defineProps<{
  routine?: Routine;
}>();
const emit = defineEmits(['complete-setp', 'rolling-updated']);

onMounted(() => {
  if (props.routine?.rolling) {
    updateRolling(props.routine.rolling); // Initial assignment on mount
  }
});

function updateRolling(newRolling: Partial<RollingSchedule>) {
  if (newRolling.delay) {
    rolling.delay.unit = newRolling.delay.unit || rolling.delay.unit;
    rolling.delay.value = newRolling.delay.value.toString() || rolling.delay.value;
  }

  if (newRolling.interval) {
    rolling.interval.unit = newRolling.interval.unit || rolling.interval.unit;
    rolling.interval.value = newRolling.interval.value.toString() || rolling.interval.value;
  }
}

const rolling = reactive({
  delay: { unit: DurationUnit.Days, value: '' },
  interval: { unit: DurationUnit.Hours, value: '' },
});

const durationUnits = reactive([
  { id: 1, value: DurationUnit.Days, name: 'Days' },
  { id: 2, value: DurationUnit.Hours, name: 'Hours' },
  { id: 3, value: DurationUnit.Minutes, name: 'Minutes' },
]);

const isRollingStepComplete = computed(() => {
  return !!(rolling.delay.value && rolling.interval.value);
});

watch(isRollingStepComplete, (newValue) => {
  emit('complete-setp', newValue);
});

watch(rolling, () => {
  emit('rolling-updated', {
    ...rolling,
    delay: {
      ...rolling.delay,
      value: Number(rolling.delay.value), // Ensure it's a number
    },
    interval: {
      ...rolling.interval,
      value: Number(rolling.interval.value), // Ensure it's a number
    },
  });
});
</script>

<template>
  <div>
    <div class="flex sm:flex-row flex-col sm:items-center gap-4">
      <div class="w-24 text-sm font-medium text-black">Every</div>
      <FwbInput
        v-model="rolling.delay.value"
        type="number"
        class="sm:w-36"
        placeholder="Input number"
      />
      <FwbSelect
        v-model="rolling.delay.unit"
        :options="durationUnits"
        class="flex-1"
        placeholder="Select"
      />
    </div>
    <div class="flex sm:flex-row flex-col sm:items-center gap-4 mt-4">
      <div class="w-24 text-sm font-medium text-black">Beginning in</div>
      <FwbInput
        v-model="rolling.interval.value"
        type="number"
        class="sm:w-36"
        placeholder="Input number"
      />
      <FwbSelect
        v-model="rolling.interval.unit"
        :options="durationUnits"
        class="flex-1"
        placeholder="Select"
      />
    </div>
  </div>
</template>
