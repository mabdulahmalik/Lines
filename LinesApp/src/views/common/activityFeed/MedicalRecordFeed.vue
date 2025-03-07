<script setup lang="ts">
import {
  FwbTimeline,
  FwbTimelineContent,
  FwbTimelineItem,
  FwbTimelinePoint
} from 'flowbite-vue'
// import { Activity } from '@/api/__generated__/graphql';
import moment from 'moment';
import { useActivitiesStore } from '@/stores/data/common/activities';
import { computed } from 'vue';

const props = defineProps<{
  aggregateId: string;
}>();

const activitiesStore = useActivitiesStore();

const formatDate = (datetime: string): string => {
  return moment(datetime).format('MMM D, YYYY h:mmA');
};


const activities = computed(() => {
  return activitiesStore.activities.filter((activity) => {
    if (activity.aggregateType === 'MedicalRecord' && activity.aggregateId === props.aggregateId) {
      return true;
    }

    const metadata = activity.metadata ? JSON.parse(activity.metadata) : null;

    return metadata?.MedicalRecordId === props.aggregateId;
  }).map((x) => ({...x, metadata: JSON.parse(x.metadata!)}));
});

</script>

<template>
  <fwb-timeline>
    <div v-for="(activity, index) in activities" :key="index">

      <fwb-timeline-item v-if="activity.aggregateType === 'MedicalRecord' && activity.activityType === 'AggregateCreated'">
        <fwb-timeline-point class="bg-slate-200">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 16 16" fill="none">
            <path
              d="M13.3337 7.00004V4.53337C13.3337 3.41327 13.3337 2.85322 13.1157 2.42539C12.9239 2.04907 12.618 1.74311 12.2416 1.55136C11.8138 1.33337 11.2538 1.33337 10.1337 1.33337H5.86699C4.74689 1.33337 4.18683 1.33337 3.75901 1.55136C3.38269 1.74311 3.07673 2.04907 2.88498 2.42539C2.66699 2.85322 2.66699 3.41327 2.66699 4.53337V11.4667C2.66699 12.5868 2.66699 13.1469 2.88498 13.5747C3.07673 13.951 3.38269 14.257 3.75901 14.4487C4.18683 14.6667 4.74689 14.6667 5.86699 14.6667H8.00033M9.33366 7.33337H5.33366M6.66699 10H5.33366M10.667 4.66671H5.33366M12.0003 14V10M10.0003 12H14.0003"
              stroke="#64748B" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
          </svg>
        </fwb-timeline-point>
        <fwb-timeline-content>
          <div class="">
            User created a Medical Record • {{formatDate(activity.timestamp)}}
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.aggregateType === 'MedicalRecord' && activity.activityType === 'AggregateModified'">
        <fwb-timeline-point class="bg-slate-300">
        </fwb-timeline-point>
        <fwb-timeline-content>
          <div>
            User modified the Medical Record • {{formatDate(activity.timestamp)}}
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else-if="activity.activityType === 'EncounterMedicalRecordChanged'">
        <fwb-timeline-point class="bg-slate-300">
        </fwb-timeline-point>
        <fwb-timeline-content>
          <div>
            User modified the Medical Record • {{formatDate(activity.timestamp)}}
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

      <fwb-timeline-item v-else>
        <fwb-timeline-point class="bg-slate-300">
        </fwb-timeline-point>
        <fwb-timeline-content>
          <div>
            {{ activity.aggregateType }} {{ activity.activityType }} • {{formatDate(activity.timestamp)}}
          </div>
        </fwb-timeline-content>
      </fwb-timeline-item>

    </div>
  </fwb-timeline>
</template>