<script setup lang="ts">
import { computed, ref, watch, watchEffect } from 'vue';
import { FwbButton, FwbBadge } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import ViewJobModalHeader from './ViewJobModalHeader.vue';
import ViewJobModalBody from './ViewJobModalBody.vue';
import {
  IconAlertCircle,
  IconArrowNarrowDownRight,
  IconClockFastForward,
  IconPatientTarget,
} from '@/components/icons/index';
import ModalReview from '@/components/modal/ModalReview.vue';
import { useModalStore } from '@/stores/modal';
import { IconCheck } from '@/components/icons';
import { Encounter, EncounterAlertType, EncounterStage, Job, JobStatus } from '@/api/__generated__/graphql';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import { useEncountersStore } from '@/stores/data/encounters';
import ProgressBar from '../ProgressBar.vue';
import PaginationBar from '../PaginationBar.vue';
import CancelAssistanceRequestModal from './CancelAssistanceRequestModal.vue';
import IconClipboardCheck from '@/components/icons/IconClipboardCheck.vue';
import { useEncounterTimers } from '@/hooks/useEncounterTimers';
import RescheduleJobModal from './RescheduleJobModal.vue';
import dayjs from 'dayjs';
import SkeletonItem from '@/components/skeletons/SkeletonItem.vue';
import { useLoaders } from '@/hooks/useLoaders';

const props = defineProps<{
  job: Job;
  encounter?: Encounter;
  isLiveQueue?: boolean;
}>();
const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'onFollowUpClick', encounterId: string): void;
}>();
const { isJobLoading: isLoading } = useLoaders();

const jobsStore = useJobsStore();
const encountersStore = useEncountersStore();
const modalStore = useModalStore();

const { holdElapsedTime } = useEncounterTimers(props);

const modalViewJobRef = ref<InstanceType<typeof ModalDrawer>>();
const modalViewJobBodyRef = ref<InstanceType<typeof ViewJobModalBody>>();
const setModalOpen = (value: boolean): void => {
  modalViewJobRef.value?.setModalOpen(value);
};
function closeModal() {
  modalViewJobRef.value?.setModalOpen(false);
}

const modalWidth = ref('7xl');
function setModalWidth(val: string) {
  modalWidth.value = val;
}

const isJobReview = ref(
  props.encounter?.stage === EncounterStage.Charting || props.encounter?.stage === EncounterStage.Completed
);

const allowEdit = computed(() => props.job.status !== JobStatus.Canceled);


watch(
  () => props.encounter?.stage,
  (newVal) => {
    isJobReview.value = newVal === EncounterStage.Charting || newVal === EncounterStage.Completed;
    if (newVal === EncounterStage.Charting) {
      modalStore.isEncounterActitityOpen = false;
    }
  }
);
const modalViewJobReviewRef = ref<InstanceType<typeof ModalReview>>();
const setChartingModalOpen = (value: boolean): void => {
  modalViewJobReviewRef.value?.setModalOpen(value);
};
function closeChartingModal() {
  closeModal();
  modalViewJobReviewRef.value?.setModalOpen(false);
}

function handleChartLater() {
  setModalOpen(false);
  modalViewJobBodyRef.value?.saveProgress();
  setChartingModalOpen(false);
}

function handleCompleteJob() {
  setModalOpen(false);
  modalViewJobBodyRef.value?.saveProgress();
  encountersStore.completeEncounter({ id: props.encounter!.id });
  setChartingModalOpen(false);
}
function saveProgress() {
  modalViewJobBodyRef.value?.saveProgress();
}

function jobModalClosed() {
  jobsStore.clearSelectedJob();
  encountersStore.clearSelectedEncounter();
  modalStore.isJobModalExpended = false;
  setModalWidth('7xl');
  unSavedDetails.value = false;
  unSavedProcedures.value = false;
  emit('close');
}

const onHoldAlert = computed(() =>
  props.encounter?.alerts?.filter((alert) => alert?.type === EncounterAlertType.OnHold)
);
const onHoldAlertText = computed(() => onHoldAlert.value?.[0]?.text);

function assignToMe() {
  if (props.encounter && props.encounter.location?.roomId) {
    encountersStore.assignMeToEncounter({
      encounterId: props.encounter.id,
      facilityRoomId: props.encounter.location.roomId,
      medicalRecordId: props.encounter.medicalRecordId,
    });
  }
}
const cancelAssistanceRequestModalRef = ref<InstanceType<typeof CancelAssistanceRequestModal>>();

const assistanceRequest = computed(() =>
  props.encounter?.alerts?.find((x) => x?.type == EncounterAlertType.AssistanceRequested)
);

function onCancelRequestClick() {
  cancelAssistanceRequestModalRef.value?.setModalOpen(true);
}

function handleAttendToPatient() {
  modalViewJobBodyRef.value?.attendToPatient();
}

async function handleSubmitProcedures() {
  const result = await modalViewJobBodyRef.value?.submitProcedures();
  if (result) setChartingModalOpen(true);
}

function handleUpdateEncounter() {
  modalViewJobBodyRef.value?.saveProgress();
  setModalOpen(false);
}

const hasProcedures = ref(props.encounter?.procedures?.length ?? false);
function setHasProcedures(value: boolean) {
  hasProcedures.value = value;
}

const isSaveSubmitDisabled = ref(false);
function setIsSaveSubmitDisabled(value: boolean) {
  isSaveSubmitDisabled.value = value;
}

// Progress Bar
const stages = [
  { label: 'Waiting' },
  { label: 'Assigned' },
  { label: 'Attending' },
  { label: 'Charting' },
  { label: 'Completed' },
];
const progressBars = ref<InstanceType<typeof ProgressBar>[]>();
const progressBarsForChartingModal = ref<InstanceType<typeof ProgressBar>[]>();

watchEffect(() => {
  const currentStage = props.encounter?.stage;
  const stageIndex = stages.findIndex((stage) => stage.label.toUpperCase() === currentStage);
  if (stageIndex !== -1) {
    progressBars.value?.[stageIndex]?.$el.scrollIntoView({
      behavior: 'smooth',
      inline: 'end',
    });
    progressBarsForChartingModal.value?.[stageIndex]?.$el.scrollIntoView({
      behavior: 'smooth',
      inline: 'end',
    });
  }
});

const rescheduleJobModalRef = ref<InstanceType<typeof RescheduleJobModal>>();

function onRescheduleClick() {
  rescheduleJobModalRef.value?.setModalOpen(true);
}

function formatDate(dateString: string): string {
  return dayjs(dateString).format('ddd, D MMM YYYY');
}
function convertToAmPm(isoDuration: string): string {
  if(!isoDuration) return '';

  const matches = isoDuration.match(/PT(\d+H)?(\d+M)?/);

  if (!matches) {
    throw new Error('Invalid ISO 8601 duration format');
  }
  const hours = parseInt(matches[1]?.replace('H', '') || '0', 10);
  const minutes = parseInt(matches[2]?.replace('M', '') || '0', 10);
  const period = hours >= 12 ? 'PM' : 'AM';
  const adjustedHours = hours % 12 || 12; // Convert 0 or 24 to 12
  const formattedMinutes = minutes.toString().padStart(2, '0');
  return `${adjustedHours}:${formattedMinutes} ${period}`;
}

function getScheduledDateTime(date: string, time: string): string {
  const formattedTime = convertToAmPm(time);
  const formattedDate = formatDate(date);
  const currentDate = dayjs().format('YYYY-MM-DD');
  if (currentDate === date && time) {
    return `${formattedTime}`;
  }
  if(date && time) {
    return `${formattedDate}, ${formattedTime}`;
  }
  return formattedDate;

}
const unSavedDetails = ref(false)
const unSavedProcedures = ref(false)
const unSavedProgress = computed (() => {
  return  unSavedDetails.value || unSavedProcedures.value
})

defineExpose({
  setModalOpen,
  closeModal,
  setChartingModalOpen,
});
</script>
<template>
<div>
  <ModalDrawer
    ref="modalViewJobRef"
    :max_width="modalWidth"
    hide_header_close_on_mobile
    left_close_inside
    :z_index="46"
    @close="jobModalClosed"
  >
    <template v-if="props.encounter?.stage === EncounterStage.Waiting && props.isLiveQueue" #sidebar>
      <PaginationBar
        :encounter="props.encounter"
        :job="props.job"
        :is-jobs-view="true"
        class="hidden lg:block"
      />
    </template>
    <template v-if="props.encounter?.alerts?.some((alert) => alert?.type === EncounterAlertType.OnHold)" #onHold>
      <div
        class="flex items-center justify-between text-sm font-semibold lg:font-bold py-1 pl-4 pr-2 bg-yellow-400 text-white"
      >
        <div class="flex items-center flex-wrap">
          <span class="mr-2"> ON HOLD </span>
          <span v-if="onHoldAlertText"> - “{{ onHoldAlertText }}”</span>
        </div>
        <fwb-badge class="rounded-xl bg-transparent text-white mr-0" size="sm">
          <template #icon>
            <IconClockFastForward />
          </template>
          <span class="pl-2 text-xs font-medium">{{ holdElapsedTime }}</span>
        </fwb-badge>
      </div>
    </template>
    <template v-if="assistanceRequest" #assistance>
      <div
        class="flex items-center justify-between text-sm font-semibold lg:font-bold py-2 px-4 bg-radical-red-400 text-white"
      >
        <div class="flex items-center flex-wrap">
          <span class="mr-2"> ASSISTANCE NEEDED</span>
          <span v-if="assistanceRequest?.text && assistanceRequest?.text !== ''"
            >- &nbsp; "{{ assistanceRequest?.text }}"</span
          >
        </div>
        <fwb-button
          @click="onCancelRequestClick"
          class="bg-redical-red-400 hover:bg-redical-red-600 border-2 border-white whitespace-nowrap"
          pill
        >
          Cancel Request
        </fwb-button>
      </div>
      <CancelAssistanceRequestModal ref="cancelAssistanceRequestModalRef" :encounter="encounter!" />
    </template>

    <template
      v-if="
        props.job.schedule &&
        props.job.status !== JobStatus.Canceled &&
        props.encounter?.stage !== EncounterStage.Charting &&
        props.encounter?.stage !== EncounterStage.Attending &&
        props.encounter?.stage !== EncounterStage.Completed
      "
      #reschedule
    >
      <div
        class="flex items-center justify-between text-sm font-semibold lg:font-bold py-2 px-4 bg-turquoise-blue-600 text-white"
      >
        <div class="flex items-center flex-wrap">
          <span> SCHEDULED FOR:</span>
          <span class="pl-1">{{ getScheduledDateTime(props.job.schedule.date, props.job.schedule.time) }}</span>
        </div>
        <fwb-button
          @click="onRescheduleClick"
          class="bg-redical-red-400 hover:bg-redical-red-600 border-2 border-white whitespace-nowrap"
          pill
        >
          Reschedule
        </fwb-button>
      </div>
      <RescheduleJobModal ref="rescheduleJobModalRef" :job="props.job" />
    </template>

    <template #header>
      <ViewJobModalHeader
        :job="props.job"
        :encounter="props.encounter"
        @close="closeModal"
        @width="setModalWidth"
      />
    </template>
    <template #body>
      <ViewJobModalBody
        ref="modalViewJobBodyRef"
        :job="props.job"
        :encounter="props.encounter"
        @width="setModalWidth"
        @has-procedures="setHasProcedures"
        @disable-save-submit="setIsSaveSubmitDisabled"
        :is-review="isJobReview"
        @onFollowUpClick="(encounterId: string) => emit('onFollowUpClick', encounterId)"
        @unsaved-details="(val: boolean) => unSavedDetails = val"
        @unsaved-procedures="(val: boolean) => unSavedProcedures = val"
      />
    </template>
    <template #footer>
      <div class="flex flex-col w-full">
        <div v-if="props.encounter" class="border-b border-slate-300 p-3 lg:px-6">
          <!-- loading -->
          <div v-if="isLoading" class="flex space-x-1 overflow-x-auto items-center ">
           <div v-for="i in 5" :key="i" class="w-full">
            <div class="flex flex-col gap-1.5 w-full">
              <SkeletonItem class="sm:w-11 w-9 h-2.5 rounded-3xl" />
              <SkeletonItem backgroundColor="#CBD5E1" class="w-full h-1 rounded-lg" />
            </div>
           </div>
          </div>

          <div v-else class="flex space-x-1 overflow-x-auto">
            <ProgressBar
              ref="progressBars"
              v-for="(stage, index) in stages"
              :key="index"
              :encounter="props.encounter"
              :duration="props.encounter.progress?.[index]?.duration"
              :label="stage.label"
            />
          </div>
        </div>
        <div class="p-4 lg:px-6 flex justify-between items-center gap-2 lg:gap-4 w-full">
          <div class="flex items-center justify-start">
            <!-- Save progress -->
            <fwb-button
              v-if="
                props.encounter?.stage &&
                props.encounter.stage !== EncounterStage.Waiting &&
                props.encounter.stage !== EncounterStage.Completed &&
                allowEdit
              "
              @click="saveProgress"
              color="light"
              pill
              class="whitespace-nowrap"
              :disabled="isSaveSubmitDisabled"
            >
              <div class="flex items-center">
                <IconAlertCircle v-if="unSavedProgress" width="16" height="16" strokeWidth="2.5" class="text-orange-500 lg:hidden mr-2"/>
                <span class="whitespace-nowrap">Save Progress</span>
              </div>
            </fwb-button>
            <div v-if="unSavedProgress" class="hidden lg:flex ml-6 flex items-center text-sm font-medium text-slate-500 whitespace-nowrap">
              <IconAlertCircle width="16" height="16" strokeWidth="2.5" class="mr-1"/>
              <span>Unsaved progress</span>
            </div>
          </div>
          <!-- loading -->
          <div v-if="isLoading" class="flex justify-end items-center gap-2 lg:gap-4 w-full">
            <SkeletonItem backgroundColor="#94A3B8" class="w-28 h-8 rounded-full" />
          </div>
          <div v-else class="flex justify-end items-center gap-2 lg:gap-4 w-full">

            <fwb-button
              v-if="props.encounter && props.encounter.stage === EncounterStage.Waiting && allowEdit"
              @click="assignToMe"
              class="bg-primary-600 hover:bg-brand-600"
              pill
            >
              <span class="whitespace-nowrap"> Assign to me </span>
              <template #suffix>
                <IconArrowNarrowDownRight />
              </template>
            </fwb-button>

            <fwb-button
              v-if="props.encounter && props.encounter.stage === EncounterStage.Assigned"
              @click="handleAttendToPatient"
              class="bg-primary-600 hover:bg-brand-600"
              pill
            >
              <span class="whitespace-nowrap">Attend to Patient </span>
              <template #suffix>
                <IconPatientTarget />
              </template>
            </fwb-button>

            <template v-if="props.encounter && props.encounter.stage === EncounterStage.Attending && allowEdit" >
              <fwb-button
                @click="handleSubmitProcedures"
                class="bg-primary-600 hover:bg-brand-600"
                :disabled="!hasProcedures || isSaveSubmitDisabled"
                pill
              >
                <span class="whitespace-nowrap"> Submit Procedures </span>
                <template #suffix>
                  <IconClipboardCheck />
                </template>
              </fwb-button>
            </template>
            <template v-if="props.encounter && props.encounter.stage === EncounterStage.Charting && allowEdit">
              <fwb-button @click="handleChartLater" color="light" pill>
                <span class="hidden lg:inline">I'll Chart later </span>
                <span class="lg:hidden">Later </span>
              </fwb-button>
              <fwb-button
                @click="handleCompleteJob"
                class="bg-primary-600 hover:bg-brand-600"
                :disabled="!props.encounter?.assignments?.length"
                pill
              >
                <span class="whitespace-nowrap"> Complete Job </span>
                <template #suffix>
                  <IconCheck height="20" width="20" />
                </template>
              </fwb-button>
            </template>

            <template v-if="props.encounter && props.encounter.stage === EncounterStage.Completed && allowEdit">
              <fwb-button
                @click="handleUpdateEncounter"
                class="bg-primary-600 hover:bg-brand-600"
                pill
              >
                <span class="whitespace-nowrap"> Update Encounter </span>
                <template #suffix>
                  <IconCheck height="20" width="20" />
                </template>
              </fwb-button>
            </template>

            <template v-if="!props.encounter">
              <fwb-button @click="closeModal" color="light" pill> Close </fwb-button>
            </template>
          </div>
        </div>
      </div>
    </template>
  </ModalDrawer>
  <!-- Review Job Modal -->
  <ModalReview ref="modalViewJobReviewRef" :z_index="61" max_width="5xl">
    <template #header>
      <ViewJobModalHeader
        :job="props.job"
        :encounter="props.encounter"
        @close="closeChartingModal"
        @width="setModalWidth"
      />
    </template>
    <template #body>
      <ViewJobModalBody
        ref="modalViewJobBodyRef"
        :job="props.job"
        :encounter="props.encounter"
        :is-review="isJobReview"
      />
    </template>
    <template #footer>
      <div class="flex flex-col w-full">
        <div v-if="props.encounter" class="border-b border-slate-300 p-3 lg:px-6">
          <div class="flex space-x-1 overflow-x-auto">
            <ProgressBar
              ref="progressBarsForChartingModal"
              v-for="(stage, index) in stages"
              :key="index"
              :encounter="props.encounter"
              :duration="props.encounter.progress?.[index]?.duration"
              :label="stage.label"
            />
          </div>
        </div>
        <div class="p-4 lg:px-6 flex justify-between items-center gap-2 lg:gap-4 w-full">
          <div class="flex justify-end items-center gap-2 lg:gap-4 w-full">
            <template v-if="props.encounter && props.encounter.stage === EncounterStage.Charting && allowEdit">
              <fwb-button @click="handleChartLater" color="light" pill>
                <span class="hidden lg:inline">I'll Chart later </span>
                <span class="lg:hidden">Later </span>
              </fwb-button>
              <fwb-button
                @click="handleCompleteJob"
                class="bg-primary-600 hover:bg-brand-600"
                :disabled="!props.encounter?.assignments?.length"
                pill
              >
                <span class="whitespace-nowrap"> Complete Job </span>
                <template #suffix>
                  <IconCheck height="20" width="20" />
                </template>
              </fwb-button>
            </template>
          </div>
        </div>
      </div>
    </template>
  </ModalReview>
</div>
</template>
