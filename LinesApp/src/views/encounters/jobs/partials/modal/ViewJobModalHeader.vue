<script setup lang="ts">
import { FwbButton, FwbBadge, FwbTooltip } from 'flowbite-vue';
import StatusBadge from '@/components/badge/StatusBadge.vue';
import MoreDropdown from '../../partials/MoreDropdown.vue';
import { useModalStore } from '@/stores/modal';
import {
  IconClockFastForward,
  IconArrowLeft,
  IconExpandWindows,
  IconShrinkWindows,
  IconShowSidebar,
  IconHideSidebar,
  IconShowActivity,
  IconHideActivity,
  IconAnnotationAlert,
  IconClockCheck,
} from '@/components/icons/index';
import { computed, ref } from 'vue';
import { Encounter, EncounterAlertType, EncounterStage, Job, JobStatus } from '@/api/__generated__/graphql';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import MoreActionsMobile from '../MoreActionsMobile.vue';
import RequestAssistanceModal from './RequestAssistanceModal.vue';
import { useEncounterTimers } from '@/hooks/useEncounterTimers';
import SkeletonItem from '@/components/skeletons/SkeletonItem.vue';
import { useLoaders } from '@/hooks/useLoaders';
const { isJobLoading: isLoading } = useLoaders();

const props = defineProps<{
  job: Job;
  encounter?: Encounter;
}>();
const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'saveProgress'): void;
  (e: 'width', val: string): void;
  (e: 'holdTimeElapsed', val: string): void;
}>();

const purposesStore = usePurposesStore();
const modalStore = useModalStore();

const { elapsedTime, attendingDuration, encounterTime } = useEncounterTimers(props);

const fullWidth = ref(false);

const requestAssistanceModalRef = ref<InstanceType<typeof RequestAssistanceModal>>();

const assistanceRequest = computed(() =>
  props.encounter?.alerts?.find((x) => x?.type == EncounterAlertType.AssistanceRequested)
);

function onRequestAssistanceClick() {
  requestAssistanceModalRef.value?.setModalOpen(true);
}

const closeModal = () => {
  emit('close');
};

const setModalWidth = (val: string) => {
  emit('width', val);
  if (val === 'full') {
    fullWidth.value = true;
    modalStore.isJobModalExpended = true;
  } else {
    fullWidth.value = false;
    modalStore.isJobModalExpended = false;
  }
};

function getPurposeNameById(id: string): string {
  const purpose = purposesStore.purposes.find((p) => p.id === id);
  return purpose?.name ?? '';
}
</script>

<template>
  <div>
  <!-- loading -->
  <div v-if="isLoading" class="flex items-center justify-between gap-x-2 py-2 px-4 lg:px-6 lg:gap-x-4 flex-wrap">
    <div class="flex items-center gap-2  w-full lg:w-auto lg:justify-none min-h-10 justify-start">
    <!-- Mobile close button -->
    <fwb-button
    @click="closeModal"
    color="light"
    pill
    square
    size="lg"
    class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
    >
    <IconArrowLeft width="24" height="24" class="text-[#334155] -ml-1" />
    </fwb-button>
    <SkeletonItem backgroundColor="#CBD5E1" class=" sm:w-56 w-40 h-4 rounded-3xl" />
    </div>
  <hr class="lg:hidden h-[1px] border-t border-slate-300 pt-2 mt-1.5 -mx-4 w-[calc(100%+32px)]" />
    <div class="flex items-center  w-full lg:w-auto justify-end flex-1 min-h-10 gap-4">
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    </div>
  </div>

<div v-else>
  <!-- Mobile priority badge -->
  <div
    v-if="
      encounter?.priority &&
      props.encounter?.stage !== EncounterStage.Charting &&
      props.encounter?.stage !== EncounterStage.Completed
    "
    class="flex lg:hidden"
  >
    <StatusBadge
      :badge="encounter?.priority"
      :mobile_full="true"
      class="w-100 py-[1px] px-4 text-sm font-semibold flex-1"
    />
  </div>
  <div class="flex items-center justify-between gap-x-2 py-2 px-4 lg:px-6 lg:gap-x-4 flex-wrap">
    <!-- Title -->
    <div class="flex items-center gap-2 justify-between w-full lg:w-auto lg:justify-none min-h-10">
      <div class="flex items-center">
        <!-- Mobile close button -->
        <fwb-button
          v-if="props.encounter?.stage !== EncounterStage.Charting"
          @click="closeModal"
          color="light"
          pill
          square
          size="lg"
          class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
        >
          <IconArrowLeft width="24" height="24" class="text-[#334155] -ml-1" />
          <span class="sr-only">Close modal</span>
        </fwb-button>
        <h2 class="text-lg lg:text-2xl font-semibold">{{ getPurposeNameById(job.purposeId) }}</h2>
        <fwb-badge
          v-if="props.job.status === JobStatus.Canceled"
          size="sm"
          class="rounded-xl ml-2"
          :class="`text-gray-700 bg-gray-100`"
          >{{ props.job.status }}</fwb-badge>
      </div>
    </div>
    <hr class="lg:hidden h-[1px] border-t border-slate-300 pt-2 mt-1.5 -mx-4 w-[calc(100%+32px)]" />
    <div class="flex items-center gap-3 lg:gap-4 w-full lg:w-auto justify-between">
      <div>
        <!-- Attending Timer -->
        <fwb-badge
          v-if="encounter?.stage === EncounterStage.Attending"
          type="dark"
          class="rounded-xl text-slate-900 mr-0"
          size="sm"
        >
          <template #icon>
            <IconClockFastForward />
          </template>
          <span class="pl-2">{{ elapsedTime }}</span>
        </fwb-badge>
        <!-- Attending Duration -->
        <fwb-badge
          v-if="encounter?.stage === EncounterStage.Charting"
          type="dark"
          class="rounded-xl text-slate-900 mr-0"
          size="sm"
        >
          <template #icon>
            <IconClockCheck />
          </template>
          <span class="pl-2">{{ attendingDuration }}</span>
        </fwb-badge>
        <!-- Encounter Full Timer -->
        <fwb-badge
          v-if="encounter?.stage === EncounterStage.Completed"
          type="dark"
          class="rounded-xl text-slate-900 mr-0"
          size="sm"
        >
          <template #icon>
            <IconClockCheck />
          </template>
          <span class="pl-2">{{ encounterTime }}</span>
        </fwb-badge>
      </div>

      <!-- Icon Buttons -->
      <div class="flex items-center gap-1 lg:gap-2">
        <!-- More actions dropdown  -->
        <MoreDropdown
          v-if="props.encounter?.stage !== EncounterStage.Completed && props.job.status !== JobStatus.Canceled"
          :encounter="props.encounter"
          :job="props.job"
          @close="closeModal"
          class="hidden lg:block"
        ></MoreDropdown>
        <!-- Mobile more actions  -->
        <MoreActionsMobile 
        v-if="props.encounter?.stage !== EncounterStage.Completed && props.job.status !== JobStatus.Canceled" :encounter="props.encounter" :job="props.job" class="block lg:hidden" />

        <!-- Request Assistance -->
        <fwb-tooltip
          v-if="
            !assistanceRequest &&
            props.encounter?.alerts?.every((alert) => alert?.type !== EncounterAlertType.OnHold) &&
            (props.encounter?.stage === EncounterStage.Assigned || props.encounter?.stage === EncounterStage.Attending)
          "
          placement="bottom"
        >
          <template #trigger>
            <fwb-button
              @click="onRequestAssistanceClick"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconAnnotationAlert />
            </fwb-button>
          </template>
          <template #content> Request assistance</template>
        </fwb-tooltip>
        <RequestAssistanceModal ref="requestAssistanceModalRef" :encounter="encounter!" />
        <!-- Expand -->
        <fwb-tooltip
          v-if="!fullWidth && props.encounter?.stage !== EncounterStage.Charting"
          placement="bottom"
          class="hidden lg:block"
        >
          <template #trigger>
            <fwb-button
              @click="setModalWidth('full')"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconExpandWindows width="24" height="24" />
            </fwb-button>
          </template>
          <template #content> Expand windows </template>
        </fwb-tooltip>
        <!-- Shrink -->
        <fwb-tooltip
          v-if="fullWidth && props.encounter?.stage !== EncounterStage.Charting"
          placement="bottom"
          class="hidden lg:block"
        >
          <template #trigger>
            <fwb-button
              @click="setModalWidth('7xl')"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconShrinkWindows width="24" height="24" />
            </fwb-button>
          </template>
          <template #content> Shrink windows </template>
        </fwb-tooltip>

        <!-- Hide Sidebar -->
        <fwb-tooltip placement="bottom" v-if="modalStore.isEncounterSidebarOpen">
          <template #trigger>
            <fwb-button
              @click="modalStore.isEncounterSidebarOpen = false"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconHideSidebar />
            </fwb-button>
          </template>
          <template #content> Hide Sidebar </template>
        </fwb-tooltip>
        <!-- Show Sidebar -->
        <fwb-tooltip placement="bottom" v-else>
          <template #trigger>
            <fwb-button
              @click="modalStore.isEncounterSidebarOpen = true"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconShowSidebar />
            </fwb-button>
          </template>
          <template #content> Show Sidebar </template>
        </fwb-tooltip>

        <!-- Hide Activity -->
        <fwb-tooltip placement="bottom" v-if="modalStore.isEncounterActitityOpen">
          <template #trigger>
            <fwb-button
              @click="modalStore.isEncounterActitityOpen = false"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconHideActivity />
            </fwb-button>
          </template>
          <template #content> Hide Activity </template>
        </fwb-tooltip>
        <!-- Show Activity -->
        <fwb-tooltip placement="bottom" v-else>
          <template #trigger>
            <fwb-button
              @click="modalStore.isEncounterActitityOpen = true"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconShowActivity />
            </fwb-button>
          </template>
          <template #content> Show Activity</template>
        </fwb-tooltip>
      </div>
    </div>
  </div>
</div>
</div>
</template>
