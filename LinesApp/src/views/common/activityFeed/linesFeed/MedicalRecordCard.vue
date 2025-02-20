<script setup lang="ts">
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import { computed } from 'vue';

const props = defineProps<{
  medicalRecordId: string
}>();

const medicalRecords = useMedicalRecordsStore();

const medicalRecord = computed(() => medicalRecords.medicalRecords.find(x => x.id === props.medicalRecordId));

</script>

<template>
  <div class="font-medium text-sm p-4 rounded-lg bg-slate-50 flex md:justify-normal justify-between md:gap-16">
    <div class="text-slate-500 flex flex-col gap-4">
      <span>Number</span>
      <span>Name</span>
      <span>Date of Birth </span>
    </div>
    <div class="text-slate-800 flex flex-col gap-4">
      <span>{{ medicalRecord?.number }}</span>
      <span>{{ medicalRecord?.firstName }} {{ medicalRecord?.lastName }}</span>
      <span>{{ medicalRecord?.birthday }}</span>
    </div>
  </div>
</template>
