<script setup lang="ts">
import { FwbButton, FwbTooltip } from 'flowbite-vue';
import { Dropdown, DropdownMenu, DropdownItem, DropdownDivider } from '@/components/dropdown/index';
import ConformationModal from '@/components/modal/ConformationModal.vue';
import {
  IconMoreActions,
  IconPlayCircle,
  IconPauseCircle,
  IconUserX01,
  IconAlertCircle,
  IconArrowCircleDown,
  IconArrowCircleUp,
  IconSlashCircle01,
  IconTrash04,
  IconCalendar,
} from '@/components/icons';
import OutlineIcon from '@/components/styled-icon/OutlineIcon.vue';
import PutJobOnHoldModal from './modal/PutJobOnHoldModal.vue';
import { useEncountersStore } from '@/stores/data/encounters';
import { ref } from 'vue';
import { Encounter, EncounterAlertType, EncounterPriority, EncounterStage, Job, JobStatus } from '@/api/__generated__/graphql';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import CancelJobModal from './modal/CancelJobModal.vue';
import EscalateEncounterModal from './modal/EscalateEncounterModal.vue';
import RescheduleJobModal from './modal/RescheduleJobModal.vue';

const props = defineProps<{
  encounter?: Encounter;
  job: Job;
}>();
const emit = defineEmits<{
  (e: 'close'): void;
}>();

const encountersStore = useEncountersStore();
const jobsStore = useJobsStore();

const putJobOnHoldModalComRef = ref<InstanceType<typeof PutJobOnHoldModal>>();
const cancelJobModalComRef = ref<InstanceType<typeof CancelJobModal>>();
const escalateEncounterModalRef = ref<InstanceType<typeof EscalateEncounterModal>>();
const conformationModalRef = ref<InstanceType<typeof ConformationModal>>();
const rescheduleJobModalRef = ref<InstanceType<typeof RescheduleJobModal>>();

function openHoldModal() {
  putJobOnHoldModalComRef.value?.setModalOpen(true);
}

function handleEscalateEncounter() {
  escalateEncounterModalRef.value?.setModalOpen(true);
}

function handleClickJobCancel() {
  cancelJobModalComRef.value?.setModalOpen(true);
}

function handleClickJobDelete() {
  conformationModalRef.value?.setModalOpen(true);
}

function handleCancelDelete() {
  conformationModalRef.value?.setModalOpen(false);
}
function handleConfirmDelete() {
  jobsStore.deleteJob({ id: props.job.id });
  conformationModalRef.value?.setModalOpen(false);
  closeMainModal();
}
const closeMainModal = () => {
  emit('close');
};

function openRescheduleJobModal() {
  rescheduleJobModalRef.value?.setModalOpen(true);
}
</script>
<template>
  <div class="flex items-center">
    <Dropdown align-to-end class="rounded-lg" close-inside>
      <template #trigger>
        <fwb-tooltip placement="bottom">
          <template #trigger>
            <fwb-button color="light" class="bg-white border-white focus:ring-0" pill square>
              <IconMoreActions />
            </fwb-button>
          </template>
          <template #content> More actions </template>
        </fwb-tooltip>
      </template>
      <DropdownMenu>
        <!-- Withdraw me -->
        <DropdownItem
          v-if="
            props.encounter &&
            props.encounter.assignments?.length &&
            props.encounter?.stage !== EncounterStage.Charting &&
            props.encounter?.stage !== EncounterStage.Completed
          "
          @click="encountersStore.withdrawMeFromEncounter({ encounterId: props.encounter.id })"
        >
          <IconUserX01 class="mr-2 text-slate-500" height="20" width="20" />
          Withdraw me
        </DropdownItem>
        <!-- Reshedule Job -->
        <DropdownItem 
          v-if="
            props.job.status !== JobStatus.Canceled &&
            props.encounter?.stage !== EncounterStage.Charting &&
            props.encounter?.stage !== EncounterStage.Attending &&
            props.encounter?.stage !== EncounterStage.Completed
          " 
          @click="openRescheduleJobModal"
        >
          <IconCalendar class="mr-2 text-slate-500" height="20" width="20" />
          Reschedule
        </DropdownItem>
        <!-- Remove the hold -->
        <DropdownItem
          v-if="props.encounter?.alerts?.some((alert) => alert?.type === EncounterAlertType.OnHold)"
          @click="encountersStore.removeHoldFromEncounter({ encounterId: props.encounter.id })"
        >
          <IconPlayCircle class="mr-2 text-slate-500" height="20" width="20" />
          Remove the Hold
        </DropdownItem>
        <!-- Put On Hold -->
        <DropdownItem
          v-if="
            props.encounter?.alerts?.every((alert) => alert?.type !== EncounterAlertType.OnHold) &&
            props.encounter?.alerts?.every((alert) => alert?.type !== EncounterAlertType.AssistanceRequested) &&
            (props.encounter?.stage === EncounterStage.Assigned || props.encounter?.stage === EncounterStage.Attending)
          "
          @click="openHoldModal"
        >
          <IconPauseCircle class="mr-2 text-slate-500" height="20" width="20" />
          Place On Hold
        </DropdownItem>
        <!-- Escalate Encounter -->
        <DropdownItem
          v-if="
            props.encounter?.priority === EncounterPriority.Normal &&
            props.encounter?.stage !== EncounterStage.Charting &&
            props.encounter?.stage !== EncounterStage.Completed
          "
          @click="handleEscalateEncounter"
        >
          <IconArrowCircleUp class="mr-2 text-slate-500" height="20" width="20" />
          Escalate
        </DropdownItem>
        <!-- De-escalate Encounter -->
        <DropdownItem
          v-if="
            props.encounter?.priority === EncounterPriority.Stat &&
            props.encounter?.stage !== EncounterStage.Charting &&
            props.encounter?.stage !== EncounterStage.Completed
          "
          @click="encountersStore.deescalateEncounter({ encounterId: props.encounter.id })"
        >
          <IconArrowCircleDown class="mr-2 text-slate-500" height="20" width="20" />
          De-escalate
        </DropdownItem>
        <!-- Cancel Job -->
        <DropdownItem v-if="props.job.status !== JobStatus.Canceled" @click="handleClickJobCancel">
          <IconSlashCircle01 class="mr-2 text-slate-500" height="20" width="20" />
          Cancel Job
        </DropdownItem>
        <DropdownDivider />
        <DropdownItem class="!text-radical-red-700" @click="handleClickJobDelete">
          <IconTrash04 class="mr-2" height="20" width="20" />
          Delete Job
        </DropdownItem>
      </DropdownMenu>
    </Dropdown>
    <PutJobOnHoldModal ref="putJobOnHoldModalComRef" :encounter="encounter" />
    <CancelJobModal ref="cancelJobModalComRef" :job="job" />
    <RescheduleJobModal ref="rescheduleJobModalRef" :job="job" @close="closeMainModal" />
    <EscalateEncounterModal ref="escalateEncounterModalRef" :encounter="props.encounter" />
    <ConformationModal ref="conformationModalRef" :z_index="101" no_footer>
      <template #body>
        <div class="p-4 lg:p-6 !pt-0 flex flex-col gap-5 text-center">
          <div class="flex flex-col gap-1">
            <div class="flex justify-center">
              <OutlineIcon size="xl" type="error">
                <IconAlertCircle></IconAlertCircle>
              </OutlineIcon>
            </div>
            <div class="text-base font-medium text-slate-900">
              <p>Deleting a Job is an irreversible action and will remove all data associated with the Job and any associated Encounters. It is recommended that the Job be <em>Canceled</em> instead.</p>
              <br /><p>With that said, are you still sure you want to delete this job?</p>
            </div>
          </div>
          <div class="flex justify-center items-center gap-4 w-full">
            <fwb-button @click="handleCancelDelete" color="light" pill> No, leave it</fwb-button>
            <fwb-button
              @click="handleConfirmDelete"
              color="red"
              class="bg-radical-red-600 hover:bg-radical-red-600"
              pill
            >
              Yes, I’m sure
            </fwb-button>
          </div>
        </div>
      </template>
    </ConformationModal>
  </div>
</template>
