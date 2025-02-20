import { ref, watch } from 'vue';
import { useEncountersStore } from '@/stores/data/encounters';
import { EncounterStage, JobStatus } from '@/api/__generated__/graphql';
import { useJobsStore } from '@/stores/data/encounters/jobs';

/**
 * A custom hook to manage and provide reactive counts of various records.
 */
export function useCounters() {
  const liveQueueCount = ref(0);
  
  const encountersStore = useEncountersStore();
  const jobsStore = useJobsStore();

  const getLiveQueueCount = () => 
    encountersStore.encounters.filter(
      e => e.stage !== EncounterStage.Completed &&
      jobsStore.jobs.find(j => j.id === e.jobId)?.status !== JobStatus.Canceled
    ).length;

  watch(
    () => [encountersStore.encounters, jobsStore.jobs],
    () => {
      liveQueueCount.value = getLiveQueueCount();
    },
    { deep: true }
  );

  return { liveQueueCount };
}
