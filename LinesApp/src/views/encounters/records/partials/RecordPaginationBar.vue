<script setup lang="ts">
import { computed } from 'vue';
import { FwbButton } from 'flowbite-vue';
import { Record } from '@/types/records';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import { MedicalRecord } from '@/api/__generated__/graphql';

const props = defineProps<{
  record: MedicalRecord;
}>();

const medicalRecordsStore = useMedicalRecordsStore();
const allMedicalRecords = computed(() => medicalRecordsStore.medicalRecords);

const currentRecordIndex = computed(() => {
  return allMedicalRecords.value.findIndex((record) => record.id === props.record?.id);
});

function handlePrev() {
  const prevIndex = currentRecordIndex.value - 1;
  if (prevIndex >= 0) {
    medicalRecordsStore.selectedMedicalRecord = allMedicalRecords.value[prevIndex];
  }
}

function handleNext() {
  const nextIndex = currentRecordIndex.value + 1;
  if (nextIndex < allMedicalRecords.value.length) {
    medicalRecordsStore.selectedMedicalRecord = allMedicalRecords.value[nextIndex];
  }
}
</script>

<template>
  <div class="w-16 h-full border-r border-slate-300">
    <!-- Current index and total count -->
    <div class="text-sm font-medium text-slate-500 h-14 flex items-center justify-center">
      {{ currentRecordIndex + 1 }}/{{ allMedicalRecords.length }}
    </div>

    <!-- Navigation buttons -->
    <div class="flex flex-col items-center justify-center gap-2">
      <fwb-button
        @click="handlePrev"
        color="light"
        pill
        square
        size="lg"
        class="bg-white border-white text-slate-400 hover:text-slate-900"
        :disabled="currentRecordIndex === 0"
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
        :disabled="currentRecordIndex === allMedicalRecords.length - 1"
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
