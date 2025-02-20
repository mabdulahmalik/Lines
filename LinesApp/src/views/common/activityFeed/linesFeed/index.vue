<script setup lang="ts">
import FeedItem from './FeedItem.vue';
import OnHoldCard from './OnHoldCard.vue';
import EscalateCard from './EscalateCard.vue';
import { Line, MedicalRecord } from '@/api/__generated__/graphql';
import { computed } from 'vue';
import { useActivitiesStore } from '@/stores/data/common/activities';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import { useEncountersStore } from '@/stores/data/encounters';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import NormalCard from './NormalCard.vue';
import ProcedureCard from './ProcedureCard.vue';
import MedicalRecordCard from './MedicalRecordCard.vue';
import PhotosCard from './PhotosCard.vue';

const props = defineProps<{
  line?: Line;
  record?: MedicalRecord;
  activitySort?:String;
  dateRange?:String;
  filteredUsers?:  string[];
}>();

const activitiesStore = useActivitiesStore();
const jobs = useJobsStore();
const encounters = useEncountersStore();
const proceduresStore = useProceduresStore();
const medicalRecords = useMedicalRecordsStore();

const getProcedureName = (id: string) => proceduresStore.procedures.find(x => x.id === id)?.name;

const activities = computed(() =>{

 // Extract start and end dates
 let startDate: Date | null = null;
 let endDate: Date | null = null;
  if (props.dateRange) {
    const [start, end] = props.dateRange.split(" - ");
    startDate = new Date(start);
    endDate = new Date(end);
    endDate.setHours(23, 59, 59, 999); // Ensure full-day filtering
  }

  return activitiesStore.activities.filter((activity) => {
    if (props.line) {
      const encounter = props.line?.insertedWith 
      ? encounters.encounters.find(x => x.id === props.line?.insertedWith?.encounterId)
      : encounters.encounters.find(x=> x.lines?.includes(props.line?.id));
      const job = jobs.jobs.find(x => x.id === encounter?.jobId);
      const medicalRecord = medicalRecords.medicalRecords.find(x => x.id === encounter?.medicalRecordId);

      return activity.aggregateId === props.line.id ||
        activity.aggregateId === job?.id ||
        activity.aggregateId === encounter?.id ||
        activity.aggregateId === medicalRecord?.id
    }

    const jobIds = jobs.jobs.filter(x => x.medicalRecordId === props.record?.id).map(x => x.id);
    const encounterIds = encounters.encounters.filter(x => x.medicalRecordId === props.record?.id).map(x => x.id);

    return activity.aggregateId === props.record?.id ||
      jobIds.includes(activity.aggregateId) ||
      encounterIds.includes(activity.aggregateId)
  }).map((x) => ({ ...x, metadata: JSON.parse(x.metadata!) }))
    // filter by date
  .filter(activity => {
      if (!startDate || !endDate) return true;
      const activityDate = new Date(activity.timestamp);
      return activityDate >= startDate && activityDate <= endDate;
    })
    // filter by users
    .filter((activity) => {
      if (!props.filteredUsers || props.filteredUsers.length === 0) return true; 
      return props.filteredUsers?.includes(activity.userId); 
    })
    // sort
  .sort((a, b) => {
    if(props.activitySort === 'ASC') {
      return new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime();
    } else {
      return new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime();
    }
  })
});
</script>

<template>
  <div v-for="(activity, index) in activities" :key="index">
    <!-- Add On Hold Activity -->
    <FeedItem v-if="activity.activityType === 'EncounterPriorityChanged' && activity.metadata.Priority === 2"  
      :activity="activity">
      <template #action>
        <span class="text-slate-500">modified the priority of the</span>
        <span>Encounter</span>
      </template>
      <template #details>
        <NormalCard :reason="activity.metadata.Args"/>
      </template>
    </FeedItem>

    <!-- Escalate Encounter Activity -->
    <FeedItem v-else-if="activity.activityType === 'EncounterPriorityChanged' && activity.metadata.Priority === 1"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">escalated the</span>
        <span>Encounter</span>
      </template>
      <template #details>
        <EscalateCard :reason="activity.metadata.Args" />
      </template>
    </FeedItem>

    <!-- Procedure Applied, Modified, or Undone -->
    <FeedItem v-else-if="activity.activityType == 'EncounterProceduresApplied'"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">added procedure to the</span>
        <span>Job</span>
      </template>
      <template #details>
        <ProcedureCard :procedure="activity.metadata.Procedures[0]" />
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterProcedureModified'"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">modified the procedure on the</span>
        <span>Job</span>
      </template>
      <template #details>
        <ProcedureCard :procedure="activity.metadata.Procedure" />
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterProceduresUndone'"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">removed the</span>
        <span>{{getProcedureName(activity.metadata.Procedures[0].ProcedureId)}}</span>
        <span class="text-slate-500">procedure from the</span>
        <span>Job</span>
      </template>
    </FeedItem>

    <!-- Add Notes to Job -->
    <FeedItem v-else-if="activity.activityType === 'JobNotesAdded'"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">added notes to the</span>
        <span>Job</span>
      </template>
      <template #details>
        <div class="font-medium text-sm p-4 rounded-lg bg-slate-50 text-gray-900 leading-[21px]">
          {{ activity.metadata.Notes.at(-1).Text }}
        </div>
      </template>
    </FeedItem>

    <!-- Updated Medical Record -->
    <FeedItem v-else-if="activity.activityType === 'EncounterMedicalRecordChanged'"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">changed the</span>
        <span>Medical Record</span>
        <span class="text-slate-500">for the</span>
        <span>Job</span>
      </template>
      <template #details>
        <MedicalRecordCard :medical-record-id="activity.metadata.MedicalRecordId" />
      </template>
    </FeedItem>

    <FeedItem v-else-if="(activity.activityType === 'AggregateModified' || activity.activityType === 'AggregateCreated') && activity.aggregateType == 'MedicalRecord'"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">modified the</span>
        <span>Medical Record</span>
        <span class="text-slate-500">for the</span>
        <span>Job</span>
      </template>
      <template #details>
        <MedicalRecordCard :medical-record-id="activity.metadata.Id" />
      </template>
    </FeedItem>

    <!-- Created Encounter or Job -->
    <FeedItem v-else-if="activity.activityType === 'AggregateCreated' && activity.aggregateType == 'Encounter'"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">created the</span>
        <span>Encounter</span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'AggregateCreated' && activity.aggregateType == 'Job'"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">created the</span>
        <span>Job</span>
      </template>
    </FeedItem>

    <!-- Modified Line -->
    <FeedItem v-else-if="activity.activityType === 'AggregateModified' && activity.aggregateType == 'Line'"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">modified the</span>
        <span>Line</span>
      </template>
    </FeedItem>

    <!-- Encounter Progressed to Different Stages -->
    <FeedItem v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 1"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">moved Encounter to the</span>
        <span>Waiting</span>
        <span class="text-slate-500">Stage</span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 2"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">moved Encounter to the</span>
        <span>Assigned</span>
        <span class="text-slate-500">Stage</span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 3"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">moved Encounter to the</span>
        <span>Attending</span>
        <span class="text-slate-500">Stage</span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 4"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">moved Encounter to the</span>
        <span>Charting</span>
        <span class="text-slate-500">Stage</span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 5"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">completed the</span>
        <span>Encounter</span>
      </template>
    </FeedItem>

    <!-- Encounter Alert Added or Removed -->
    <FeedItem v-else-if="activity.activityType === 'EncounterAlertedAdded' && activity.metadata.Alert.Type === 3"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">requested for</span>
        <span>Assistance</span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterAlertedRemoved' && activity.metadata.Type === 3"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">cancelled the</span>
        <span>Assistance Request</span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterAlertedAdded' && activity.metadata.Alert.Type === 1"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">placed the Encounter on</span>
        <span>Hold</span>
      </template>
      <template #details>
        <OnHoldCard :reason="activity.metadata.Alert.Text" />
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterAlertedRemoved' && activity.metadata.Type === 1"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">removed Encounter from the</span>
        <span>Hold</span>
      </template>
    </FeedItem>

    <FeedItem  v-else-if="activity.activityType === 'EncounterPhotosAttached'"
      :activity="activity">
      <template #action>
        <span class="text-slate-500">added</span>
        <span>{{activity.metadata.Photos.length}} Photos</span>
      </template>
      <template #details>
        <PhotosCard :photos="activity.metadata.Photos" />
      </template>
    </FeedItem>
  </div>
</template>
