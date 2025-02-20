<script setup lang="ts">
import { Encounter, Job, Line } from '@/api/__generated__/graphql';

import JobFeed from './JobFeed.vue';
import MedicalRecordFeed from './MedicalRecordFeed.vue';
import LineFeed from './LineFeed.vue';

const props = defineProps<{
  aggregateType: string;
  aggregateId: string;
  job?: Job;
  encounter?: Encounter;
  line?: Line;
}>();

</script>

<template>
    <JobFeed v-if="props.aggregateType == 'Job'" :aggregate-id="props.aggregateId" :job="job" :encounter="encounter"/>
    <MedicalRecordFeed v-if="props.aggregateType == 'MedicalRecord'" :aggregate-id="props.aggregateId" :job="job" :encounter="encounter"/>
    <LineFeed v-if="props.aggregateType == 'Line'" :aggregate-id="props.aggregateId" :job="job" :encounter="encounter"/>
</template>

<style scoped>
:deep(.border-white.absolute) {
  border-color: rgb(248 250 252 / 1);
  background-color: inherit;
  --tw-ring-color: rgb(248 250 252 / 1);
  --tw-ring-offset-shadow: var(--tw-ring-inset) 0 0 0 var(--tw-ring-offset-width) var(--tw-ring-offset-color);
  --tw-ring-shadow: var(--tw-ring-inset) 0 0 0 calc(8px + var(--tw-ring-offset-width)) var(--tw-ring-color);
  box-shadow: var(--tw-ring-offset-shadow), var(--tw-ring-shadow), var(--tw-shadow, 0 0 #0000);
}

:deep(.ring-8) {
  margin-top: 2px;
}
</style>