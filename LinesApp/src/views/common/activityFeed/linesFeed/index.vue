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
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useLinesStore } from '@/stores/data/encounters/lines';

const props = defineProps<{
  line?: Line;
  record?: MedicalRecord;
  activitySort?:String;
  dateRange?:String;
  filteredUsers?:  string[];
}>();

const activitiesStore = useActivitiesStore();
const encounters = useEncountersStore();
const jobs = useJobsStore();
const lines = useLinesStore();
const proceduresStore = useProceduresStore();
const medicalRecords = useMedicalRecordsStore();
const purposesStore=usePurposesStore();

const getProcedureName = (id: string) => proceduresStore.procedures.find(x => x.id === id)?.name;
const getPurposeNameByJobId = (jobId: string) => {
  const job = jobs.jobs.find(job => job.id === jobId);
  if (!job) return ""; 
  const purpose = purposesStore.purposes.find(purpose => purpose.id === job.purposeId);
  return purpose?.name || "";
};
const getPurposeNameByEncounterId = (encounterId: string) => {
  const encounter = encounters.encounters.find(en => en.id === encounterId);
  if (!encounter) return ""; 
  const purpose = purposesStore.purposes.find(purpose => purpose.id === encounter.purposeId);
  return purpose?.name || "";
};
const getPurposeName = (lineId: string) => {    
  const encounter = props.line?.insertedWith 
      ? encounters.encounters.find(x => x.id === props.line?.insertedWith?.encounterId)
      : encounters.encounters.find(x=> x.lines?.includes(lineId));      

  if (!encounter) return ""; 
  const purpose = purposesStore.purposes.find(purpose => purpose.id === encounter.purposeId);
  return purpose?.name || "";
};
const getMRN = (medicalRecordId: string) => {  
  return medicalRecords.medicalRecords.find(x => x.id === medicalRecordId)?.number;
}

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

    const lineIds = lines.lines.filter(x=> x.medicalRecordId === props.record?.id).map(x=> x.id);
    const jobIds = jobs.jobs.filter(x => x.medicalRecordId === props.record?.id).map(x => x.id);
    const encounterIds = encounters.encounters.filter(x => x.medicalRecordId === props.record?.id).map(x => x.id);

    return activity.aggregateId === props.record?.id ||
      jobIds.includes(activity.aggregateId) ||
      encounterIds.includes(activity.aggregateId) ||
      lineIds.includes(activity.aggregateId)
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
        <span>
          Descalated {{ getPurposeNameByEncounterId(activity.metadata.EncounterId) }} 
        </span>
      </template>
      <template #details>
        <NormalCard :reason="activity.metadata.Args"/>
      </template>
    </FeedItem>

    <!-- Escalate Encounter Activity -->
    <FeedItem v-else-if="activity.activityType === 'EncounterPriorityChanged' && activity.metadata.Priority === 1"
      :activity="activity">
      <template #action>
        <span>
          Escalated {{ getPurposeNameByEncounterId(activity.metadata.EncounterId) }} 
        </span>
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
         <span> 
          Note
          <span class="text-slate-500"> Added </span>
          to  {{ getPurposeNameByJobId(activity.metadata.JobId) }} 
        </span>
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
    <!-- Modified -->
    <FeedItem v-else-if="(activity.activityType === 'AggregateModified' || activity.activityType === 'AggregateCreated') && activity.aggregateType == 'MedicalRecord'"
      :activity="activity">
      <template #action>
         <span>
          Record details were updated
         </span>
      </template>
      <template #details>
        <MedicalRecordCard :medical-record-id="activity.metadata.Id" />
      </template>
    </FeedItem>

    <!-- Created Encounter or Job -->
    <FeedItem v-else-if="activity.activityType === 'AggregateCreated' && activity.aggregateType == 'Encounter'"
      :activity="activity">
      <template #action>
        <span>created the Encounter</span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'AggregateCreated' && activity.aggregateType == 'Job'"
      :activity="activity">
      <template #action>
        <span>created the Job</span>
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
        <span>
          Clinician
          <span class="text-slate-500">Assigned</span> 
           as Primary to {{ getPurposeNameByJobId(activity.metadata.JobId) }}
        </span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 3"
      :activity="activity">
      <template #action>
        <span>
          Attending to {{ getPurposeNameByJobId(activity.metadata.JobId) }}
        </span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 4"
      :activity="activity">
      <template #action>
        <span>Charting 
          <span class="text-slate-500">Procedures</span>
          for {{ getPurposeNameByJobId(activity.metadata.JobId) }}
        </span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterProgressed' && activity.metadata.Stage == 5"
      :activity="activity">
      <template #action>
        <span>Completed {{ getPurposeNameByJobId(activity.metadata.JobId) }}
        </span>
      </template>
    </FeedItem>

    <!-- Encounter Alert Added or Removed -->
    <FeedItem v-else-if="activity.activityType === 'EncounterAlertedAdded' && activity.metadata.Alert.Type === 3"
      :activity="activity">
      <template #action>
        <span>Assistance was
          <span class="text-slate-500">requested</span>
          for {{ getPurposeNameByJobId(activity.metadata.JobId) }}
        </span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterAlertedRemoved' && activity.metadata.Type === 3"
      :activity="activity">
      <template #action>
        <span>Assistance was
          <span class="text-slate-500">cancelled</span>
          for {{ getPurposeNameByEncounterId(activity.metadata.EncounterId) }}
        </span>
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterAlertedAdded' && activity.metadata.Alert.Type === 1"
      :activity="activity">
      <template #action>
        <span>
          Hold
          <span class="text-slate-500"> Placed </span>
          on  {{ getPurposeNameByJobId(activity.metadata.JobId) }} 
        </span>
      </template>
      <template #details>
        <OnHoldCard :reason="activity.metadata.Alert.Text" />
      </template>
    </FeedItem>

    <FeedItem v-else-if="activity.activityType === 'EncounterAlertedRemoved' && activity.metadata.Type === 1"
      :activity="activity">
      <template #action>
        <span>
          Hold
          <span class="text-slate-500"> Released </span>
          for  {{ getPurposeNameByEncounterId(activity.metadata.EncounterId) }} 
        </span>
      </template>
    </FeedItem>

    <FeedItem  v-else-if="activity.activityType === 'EncounterPhotosAttached'"
      :activity="activity">
      <template #action>
        <span> 
          {{activity.metadata.Photos.length}} Photos were
          <span class="text-slate-500"> added </span>
          for a  {{ getPurposeNameByJobId(activity.metadata.JobId) }} 
        </span>
      </template>
      <template #details>
        <PhotosCard :photos="activity.metadata.Photos" />
      </template>
    </FeedItem>
    
    <FeedItem  v-else-if="activity.activityType === 'CliniciansWithdrawn'"
      :activity="activity">
      <template #action>
        <span>
          Clinician
          <span class="text-slate-500"> Withdrew </span>
          from  {{ getPurposeNameByEncounterId(activity.metadata.EncounterId) }} 
        </span>
      </template>
    </FeedItem>

    <template v-else-if="activity.activityType === 'LineMedicalRecordModified'">
      <FeedItem
        :activity="activity">
        <template #action>
          <span>
            Added to
            <span class="text-slate-500"> Medical Record: </span>
            {{ getMRN(activity.metadata.MedicalRecordId) }} 
          </span>
        </template>
      </FeedItem>
      <FeedItem v-if="activity.metadata.OldMedicalRecordId != null"
        :activity="activity">
        <template #action>
          <span>
            Removed from
            <span class="text-slate-500"> Medical Record: </span>
            {{ getMRN(activity.metadata.OldMedicalRecordId) }} 
          </span>
        </template>
      </FeedItem>
    </template>

    <FeedItem   v-else-if="activity.activityType === 'LineFacilityRoomChanged'"
        :activity="activity">
        <template #action>
          <span>
            Room  
            <span class="text-slate-500"> Changed </span>
            for  {{ getPurposeName(activity.metadata.LineId) }} 
          </span>
        </template>
    </FeedItem>

    <template v-else-if="activity.activityType === 'LineInfectionStatusChanged'">
      <FeedItem v-if="activity.metadata.HasInfection"
          :activity="activity">
          <template #action>
            <span>
              Infected  
              <span class="text-slate-500"> on </span>
              {{ activity.metadata.InfectedOn }} 
            </span>
          </template>
      </FeedItem>      
      <FeedItem v-else
          :activity="activity">
          <template #action>
            <span>
              Infected  
              <span class="text-slate-500"> Cleared </span>
            </span>
          </template>
      </FeedItem>      
    </template>
    
    <FeedItem  v-else-if="activity.activityType === 'LineRenamed'"
      :activity="activity">
      <template #action>
        <span>
          Line
          <span class="text-slate-500"> Renamed </span>
        </span>
      </template>
    </FeedItem>

    <!-- <FeedItem  v-else
        :activity="activity">
        <template #action>
          <span>
            Room  
            <span class="text-slate-500"> Changed </span>
            for  {{ getPurposeNameByEncounterId(activity.metadata.EncounterId) }} 
          </span>
        </template>
    </FeedItem> -->

  </div>
</template>
