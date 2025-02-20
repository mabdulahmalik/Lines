<script setup lang="ts">
import {
  FwbTimeline,
  FwbTimelineContent,
  FwbTimelineItem,
  FwbTimelinePoint,
  FwbTimelineTime,
} from 'flowbite-vue';
import { Encounter, Job } from '@/api/__generated__/graphql';
import { useActivitiesStore } from '@/stores/data/common/activities';
import { computed } from 'vue';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import { useUsersStore } from '@/stores/data/settings/users';
import { formatDatetime } from '@/utils/dateUtils';
import { IconHand,IconAnnotationAlert, IconClipborodCheck, IconCheck,IconFilePlus02,IconPauseCircle ,IconArrowNarrowRight,IconArrowNarrowDownRight} from '@/components/icons';

const props = defineProps<{
  job?: Job;
  encounter?: Encounter;
}>();

const activitiesStore = useActivitiesStore();
const proceduresStore = useProceduresStore();
const usersStore = useUsersStore();

const formatDate = (datetime: string): string => formatDatetime(datetime, 'MMM D, YYYY h:mm A');

const activities = computed(() => {
  return activitiesStore.activities
    .filter(
      (activity) =>
        activity.aggregateId === props.job?.id ||
        activity.aggregateId === props.encounter?.id ||
        activity.aggregateId === props.encounter?.medicalRecordId
    )
    .map((x) => ({ ...x, metadata: JSON.parse(x.metadata!) }));
});

const ENCOUNTER_PRIORITY = {
  STAT: 1,
  NORMAL: 2,
  OTHER: 0,
};

const ENCOUNTER_STATUS = {
  WAITING: 1,
  HOLDING: 3,
  ATTENDING: 4,
};

const JOB_STATUS = {
  CANCELED: 1,
  COMPLETED: 2,
  UNDERWAY: 3,
  SCHEDULED: 4,
};

const getProcedureName = (id: string) => proceduresStore.procedures.find((x) => x.id == id)?.name;
const getUsername = (id: string) => {
  const user = usersStore.users.find((x) => x.id === id);
  if(user){
    return user.firstName + ' ' + user.lastName;
  }
  return '';
};
</script>

<template>
  <fwb-timeline>
    <div v-for="(activity, index) in activities" :key="index">
      <!-- Job Activities -->
      <fwb-timeline-item
        v-if="activity.aggregateType === 'Job' && activity.activityType === 'AggregateCreated'"
        class="mb-6"
      >
        <fwb-timeline-point class="bg-[#FEECDC]">
          <IconFilePlus02 color="#FF5A1F" width="16px" height="16px" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500"> created  </span>the job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.aggregateType === 'Job' && activity.activityType === 'AggregateModified'
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500">  modified  </span>the job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'JobScheduled'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500"> rescheduled the </span> Job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <!-- Encounter Changes -->

      <fwb-timeline-item
        v-else-if="
          activity.aggregateType === 'Encounter' && activity.activityType === 'AggregateCreated'
        "
        class="mb-6"
      >
      </fwb-timeline-item>
      <fwb-timeline-item
        v-else-if="
          activity.aggregateType === 'Encounter' && activity.activityType === 'AggregateModified'
        "
        class="mb-6"
      >
      </fwb-timeline-item>
      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'EncounterPriorityChanged' &&
          activity.metadata.Priority === ENCOUNTER_PRIORITY.STAT
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Encounter priority has been set to <span class="font-semibold">STAT</span> 
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'EncounterPriorityChanged' &&
          activity.metadata.Priority === ENCOUNTER_PRIORITY.NORMAL
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Encounter priority has been set to <span class="font-semibold">Normal</span> 
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'EncounterAlertedAdded' && activity.metadata.Alert.Type === 1
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-[#FDF6B2]">
          <IconPauseCircle color="#E3A008" width="16px" height="16px" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500"> changed to </span> On Hold
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'EncounterAlertedAdded' && activity.metadata.Alert.Type === 3
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-slate-100">
          <IconAnnotationAlert color="#475569" width="16px" height="16px" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500"> requested </span>Assistance
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'EncounterAlertedRemoved' && activity.metadata.Type === 3
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-slate-100">
          <IconAnnotationAlert color="#475569" width="16px" height="16px" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500"> cancelled the  </span>Assistance
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'EncounterAlertedRemoved' && activity.metadata.Type === 1
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-[#FDF6B2]">
          <IconPauseCircle color="#E3A008" width="16px" height="16px" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
           Hold <span class="text-slate-500"> removed </span>from  the Encounter
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <!-- Clinician Activities -->
      <fwb-timeline-item v-else-if="activity.activityType === 'CliniciansAssigned'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Clinician <span class="text-slate-500">  assigned </span> to job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'CliniciansWithdrawn'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Clinician <span class="text-slate-500"> withdrawn </span> from job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <!-- Photo and Procedure Activities -->
      <fwb-timeline-item v-else-if="activity.activityType === 'EncounterPhotosAttached'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Photo <span class="text-slate-500"> attached </span> to job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'EncounterPhotosDetached'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Photo <span class="text-slate-500"> removed </span>  from job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'EncounterProceduresApplied'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
         <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
           <span class="font-bold">{{ getProcedureName(activity.metadata.Procedures[0].ProcedureId) }} </span>  was added to
            the job 
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'EncounterProcedureModified'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
           <span class="font-bold">{{ getProcedureName(activity.metadata.Procedure.ProcedureId) }} </span> in the job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'EncounterProceduresUndone'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
           <span class="font-bold">{{ getProcedureName(activity.metadata.Procedures[0].ProcedureId) }} </span>  was removed
           from the job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'JobNotesAdded'"  class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500"> added a </span> Note
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'JobNotesModified'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500"> modified  a </span> Note
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'JobNotesRemoved'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500"> Removed  a </span> Note
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'AggregateCreated' && activity.aggregateType == 'MedicalRecord'
        "
        class="mb-6"
      >
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'AggregateModified' && activity.aggregateType == 'MedicalRecord'
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} made changes to <span class="font-semibold"> Medical Record</span> 
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'EncounterMedicalRecordChanged'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500"> assigned </span> the new <span class="font-semibold">Medical Record</span>
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'EncounterRoomChanged'" class="mb-6">
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Location was <span class="text-slate-500"> changed for </span> the Job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'EncounterStatusChanged' &&
          activity.metadata.Status == ENCOUNTER_STATUS.HOLDING
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-[#FDF6B2]">
          <IconPauseCircle color="#E3A008" width="16px" height="16px" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Encounter status has been set to <span class="font-semibold">  On Hold </span>
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'EncounterStatusChanged' &&
          activity.metadata.Status == ENCOUNTER_STATUS.ATTENDING
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Encounter status has been set to <span class="font-semibold">Attending</span>
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'EncounterStatusChanged' &&
          activity.metadata.Status == ENCOUNTER_STATUS.WAITING
         "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Encounter status has been set to <span class="font-semibold">Waiting</span> 
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.activityType === 'JobStatusChanged' &&
          activity.metadata.Status == JOB_STATUS.CANCELED
        "
        class="mb-6"
      >
        <fwb-timeline-point class="bg-slate-300"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Job status has been set to <span class="font-semibold">Canceled</span> 
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 1"
        class="mb-6"
      >
        <fwb-timeline-point class="bg-[#FEECDC]">
          <IconHand color="#FF5A1F" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }}
            <span class="text-slate-500"> moved to </span> Waiting
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 2"
        class="mb-6"
      >
        <fwb-timeline-point class="bg-[#FEECDC]">
          <IconArrowNarrowDownRight color="#FF5A1F" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }}
            <span class="text-slate-500"> was assigned </span> Lead
          </div>
        </fwb-timeline-content>

      </fwb-timeline-item>
      <fwb-timeline-item
        v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 3"
        class="mb-6"
      >
        <fwb-timeline-point class="bg-[#FEECDC]">
          <IconArrowNarrowRight color="#FF5A1F" width="16px" height="16px" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }} <span class="text-slate-500"> started </span>  Attending
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 4"
        class="mb-6"
      >
        <fwb-timeline-point class="bg-[#FEECDC]">
          <IconClipborodCheck color="#FF5A1F" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }}
            <span class="text-slate-500"> moved to </span> Charting
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 5"
        class="mb-6"
      >
        <fwb-timeline-point class="bg-[#FEECDC]">
          <IconCheck color="#FF5A1F" width="16px" height="16px" strokeWidth="3" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }}
            <span class="text-slate-500"> Completed </span> Encounter
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="activity.activityType === 'JobStatusChanged' && activity.metadata.Status == 2"
        class="mb-6"
      >
        <fwb-timeline-point class="bg-[#FEECDC]">
          <IconCheck color="#FF5A1F" width="16px" height="16px" strokeWidth="3" />
        </fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            {{ getUsername(activity.userId) }}
            <span class="text-slate-500"> Completed </span> Job
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item
        v-else-if="
          activity.aggregateType === 'Line' && activity.activityType === 'AggregateCreated'
        "
        class="mb-6"
      >
      </fwb-timeline-item>
      <fwb-timeline-item
        v-else-if="
          activity.aggregateType === 'Line' && activity.activityType === 'AggregateModified'
        "
        class="mb-6"
      >
      </fwb-timeline-item>

      <!-- Fallback for Unhandled Activity Types -->
      <fwb-timeline-item v-else class="mb-6">
        <fwb-timeline-point class="bg-slate-400"></fwb-timeline-point>
        <fwb-timeline-content class="ps-1.5">
          <fwb-timeline-time class="text-slate-500">
            <span class="font-medium">{{ formatDate(activity.timestamp) }}</span>
          </fwb-timeline-time>
          <div class="text-sm font-medium">
            Activity: {{ activity.aggregateType }} - {{ activity.activityType }} -
            {{ activity.metadata }}
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>
    </div>
  </fwb-timeline>
</template>
