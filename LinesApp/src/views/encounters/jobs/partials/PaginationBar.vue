<script setup lang="ts">
import { Encounter, EncounterStage, Job } from '@/api/__generated__/graphql';
import { useEncountersStore } from '@/stores/data/encounters';
import { FwbButton } from 'flowbite-vue';
import { computed } from 'vue';

const props = defineProps<{
  encounter?: Encounter;
  job?: Job;
}>();

const encountersStore = useEncountersStore();

const waitingEncounters = computed(() => {
  return encountersStore.encounters.filter((encounter) => encounter.stage === EncounterStage.Waiting);
});

const currentEncounterIndex = computed(() => {
  return waitingEncounters.value.findIndex((encounter) => encounter.id === props.encounter?.id);
});

function handlePrev() {
  encountersStore.selectedEncounter = waitingEncounters.value[currentEncounterIndex.value - 1];
}
function handleNext() {
  encountersStore.selectedEncounter = waitingEncounters.value[currentEncounterIndex.value + 1];
}

</script>
<template>
  <div class="w-16 h-full border-r border-slate-300">
    <div class="text-sm font-medium text-slate-500 h-14 flex items-center justify-center">{{currentEncounterIndex + 1}}/{{ waitingEncounters.length }}</div>
    <div class="flex flex-col items-center justify-center gap-2">
      <fwb-button
        @click="handlePrev"
        color="light"
        pill
        square
        size="lg"
        class="bg-white border-white text-slate-400 hover:text-slate-900"
        :disabled="currentEncounterIndex === 0"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="20"
          height="20"
          viewBox="0 0 20 20"
          fill="none"
        >
          <path
            d="M15 12.5L10 7.5L5 12.5"
            stroke="#94A3B8"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
          />
        </svg>
      </fwb-button>
      <fwb-button
        @click="handleNext"
        color="light"
        pill
        square
        size="lg"
        class="bg-white border-white text-slate-400 hover:text-slate-900"
        :disabled="currentEncounterIndex === waitingEncounters.length - 1"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="20"
          height="20"
          viewBox="0 0 20 20"
          fill="none"
        >
          <path
            d="M5 7.5L10 12.5L15 7.5"
            stroke="#94A3B8"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
          />
        </svg>
      </fwb-button>
    </div>
  </div>
</template>
