import { computed } from 'vue';
import { useEncountersStore } from '@/stores/data/encounters';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { useUsersStore } from '@/stores/data/settings/users';
import { useLinesStore } from '@/stores/data/encounters/lines';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import { useActivitiesStore } from '@/stores/data/common/activities';
import { useRoutinesStore } from '@/stores/data/settings/routines';

/**
 * A custom hook to manage and provide reactive isLoading state.
 */
export function useLoaders() {
  
  const jobsStore = useJobsStore();
  const encountersStore = useEncountersStore();
  const purposesStore = usePurposesStore();
  const facilitiesStore = useFacilitiesStore();
  const usersStore = useUsersStore();
  const linesStore = useLinesStore();
  const medicalRecordsStore = useMedicalRecordsStore();
  const proceduresStore = useProceduresStore();
  const activitiesStore = useActivitiesStore();
  const routinesStore = useRoutinesStore();


  const isLiveQueueLoading = computed(() => 
    encountersStore.isLoading || 
    jobsStore.isLoading || 
    facilitiesStore.isFacilitiesLoading || 
    facilitiesStore.isRoomsLoading || 
    purposesStore.isLoading || 
    usersStore.isLoading ||  
    linesStore.isLoading
  );

  const isJobsLoading = computed(() => 
    encountersStore.isLoading || 
    jobsStore.isLoading || 
    facilitiesStore.isFacilitiesLoading || 
    facilitiesStore.isRoomsLoading || 
    purposesStore.isLoading || 
    usersStore.isLoading  ||
    linesStore.isLoading
  );
    // For single job
  const isJobLoading = computed(() => 
    encountersStore.isLoading || 
    jobsStore.isLoading || 
    facilitiesStore.isFacilitiesLoading || 
    facilitiesStore.isRoomsLoading || 
    purposesStore.isLoading || 
    usersStore.isLoading  ||
    linesStore.isLoading ||
    proceduresStore.isLoading ||
    medicalRecordsStore.isLoading ||
    routinesStore.isLoading ||
    activitiesStore.isLoading 
  );

  const isLinesLoading = computed(() => 
    facilitiesStore.isFacilitiesLoading || 
    facilitiesStore.isRoomsLoading || 
    medicalRecordsStore.isLoading ||
    linesStore.isLoading 
  );
  // For single line
  const isLineLoading = computed(() => 
    facilitiesStore.isFacilitiesLoading || 
    facilitiesStore.isRoomsLoading || 
    medicalRecordsStore.isLoading ||
    linesStore.isLoading  ||
    encountersStore.isLoading ||
    proceduresStore.isLoading ||
    activitiesStore.isLoading 
  );

  const isRecordsLoading = computed(() => 
    linesStore.isLoading ||
    medicalRecordsStore.isLoading 
  );
  // For single record
  const isRecordLoading = computed(() => 
    linesStore.isLoading ||
    medicalRecordsStore.isLoading ||
    activitiesStore.isLoading
  );

  return { isLiveQueueLoading,isJobsLoading,isLinesLoading,isRecordsLoading ,isJobLoading,isLineLoading,isRecordLoading};
}
