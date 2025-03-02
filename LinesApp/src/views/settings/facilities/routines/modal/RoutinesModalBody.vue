<script setup lang="ts">
import {
  CreateRoutineCmd,
  Routine,
  ModifyRoutineCmd,
} from '@/api/__generated__/graphql';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import { IconArrowNarrowDown, IconLightBulb, IconXClose } from '@/components/icons/index';
import { FwbSelect, FwbButton } from 'flowbite-vue';
import { ref, computed, onMounted, toRaw, watch } from 'vue';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useRoutinesStore } from '@/stores/data/settings/routines';
import DescriptionPannel from './partials/DescriptionPannel.vue';
import ProcedureStepForm from './partials/ProcedureStepForm.vue';
import RollingInterval from './partials/RollingInterval.vue';
import RecurringEvents from './partials/RecurringEvents.vue';
import DateTimeFormatter from '@/utils/dateTimeFormatter';

const props = defineProps<{
  isCreate: boolean;
  routine?: Routine;
  name?: string;
  description?: string;
  followUp?: boolean;
}>();
const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'complete', value: boolean): void;
}>();

const purposesStore = usePurposesStore();
const routinesStore = useRoutinesStore();

//  Left pannel description
const description = ref(``);
onMounted(() => {
  if (props.description) {
    description.value = props.description;
  }
  if (props.routine?.description) {
    description.value = props.routine.description;
  }
});
function changeDescription(desc: string) {
  description.value = desc;
}

// const isFollowUp = ref(props.routine?.settings?.includes(RoutineSetting.FollowUp) || false);

//  Right panel

// Alert
const alert = ref(props.routine?.assignmentCount && props.routine.assignmentCount > 0);
const closeAlert = () => {
  alert.value = false;
};
const facilityAssignments = computed(() => props.routine?.assignmentCount);

// Trigger Points
const activeFormStep = ref(1);

const origins = ref([]);
const isCompleteTriggerPointsFields = ref(false);
onMounted(() => {
  if (!props.isCreate) {
    isCompleteTriggerPointsFields.value = true;
    isCompleteTerminationFields.value = true;
  }
});
const watchTriggerPointsFieldsComplete = (val: boolean) => {
  isCompleteTriggerPointsFields.value = val;
};
function updateTriggerPoints(triggerPoints: any) {
  origins.value = triggerPoints.map((t: any) => ({
    procedureId: t.selectedProcedure,
    propertyId:
      t.selectedProperty === 'any' || t.selectedProperty === '' ? null : t.selectedProperty,
    propertyValue: t.anySelectedValue || t.selectedValue === '' ? null : t.selectedValue,
  }));
}
const completeTriggerPointsStep = () => {
  activeFormStep.value = 2;
};

//  Purpose Selection

const purposeList = computed(() =>
  purposesStore.purposes
    .filter((purpose) => purpose.archived === false)
    .map((p) => ({
      value: p.id,
      name: p.name!,
    }))
);

const selectedPurpose = ref('');
onMounted(() => {
  if (props.routine) {
    selectedPurpose.value = props.routine.purposeId;
    activeSchedulingTab.value = props.routine.rolling ? 'rolling' : 'recurring';
  }
});
// validate purpose step
const isCompletePurposeField = computed(() => {
  return selectedPurpose.value !== null && selectedPurpose.value !== '';
});

const completePurposeStep = () => {
  if (isCompletePurposeField.value) {
    activeFormStep.value = 3;
  }
};

const isPurposeStepDisabled = computed(() => {
  if (props.isCreate) {
    return activeFormStep.value < 2;
  } else {
    return !isCompleteTriggerPointsFields.value;
  }
});

// Scheduling
const activeSchedulingTab = ref('rolling');
const isCompleteSchedulingFields = computed(() => {
  if (activeSchedulingTab.value === 'rolling') {
    return isRollingStepComplete.value;
  } else if (activeSchedulingTab.value === 'recurring') {
    return isRecurringStepComplete.value;
  }
});

const isRollingStepComplete = ref(false);
const rolling = ref(null);
const watchRollingComplete = (val: boolean) => {
  isRollingStepComplete.value = val;
};

function updateRolling(rollingVal: any) {
  rolling.value = rollingVal;
}

const recurrence = ref([]);
const isRecurringStepComplete = ref(false);
const watchRecurringComplete = (val: boolean) => {
  isRecurringStepComplete.value = val;
};
function updateRecurring(recurringVal: any) {
  recurrence.value = recurringVal;
}
const completeSchedulingStep = () => {
  if (isCompleteSchedulingFields.value) {
    activeFormStep.value = 4;
  }
};

const isSchedulingStepDisabled = computed(() => {
  if (props.isCreate) {
    return activeFormStep.value < 3;
  } else {
    return !isCompletePurposeField.value || !isCompleteTriggerPointsFields.value;
  }
});

// Termination
const isCompleteTerminationFields = ref(false);

const termini = ref([]);
const watchTerminationFieldsComplete = (val: boolean) => {
  isCompleteTerminationFields.value = val;
};
function updateTermination(terminiPoints: any) {
  termini.value = terminiPoints.map((t: any) => ({
    procedureId: t.selectedProcedure,
    propertyId:
      t.selectedProperty === 'any' || t.selectedProperty === '' ? null : t.selectedProperty,
    propertyValue: t.anySelectedValue || t.selectedValue === '' ? null : t.selectedValue,
  }));
}

const isTerminationStepDisabled = computed(() => {
  if (props.isCreate) {
    return activeFormStep.value < 4;
  } else {
    return (
      !isCompleteSchedulingFields.value ||
      !isCompletePurposeField.value ||
      !isCompleteTriggerPointsFields.value
    );
  }
});

// All Steps
const completeAllSetps = computed(() => {
  return !!(
    isCompleteTriggerPointsFields.value &&
    isCompletePurposeField.value &&
    isCompleteSchedulingFields.value &&
    isCompleteTerminationFields.value
  );
});
function submittedData() {
  if (props.name && completeAllSetps.value) {
    if (props.isCreate) {
      const newRoutine: CreateRoutineCmd = {
        name: props.name,
        description: description.value ? description.value : null,
        purposeId: selectedPurpose.value,
        isFollowUp: props.followUp,
        origins: toRaw(origins.value),
        rolling: activeSchedulingTab.value === 'rolling' ? toRaw(rolling.value) : null,
        recurrence: activeSchedulingTab.value === 'recurring' ? toRaw(recurrence.value) : null,
        termini: toRaw(termini.value),
      };
      routinesStore.createRoutine(newRoutine);
      console.log('new routine', newRoutine);
      closeModalDrawer();
    } else {
      const updatedRoutine: ModifyRoutineCmd = {
        id: props.routine?.id,
        name: props.name,
        description: description.value ? description.value : null,
        purposeId: selectedPurpose.value,
        origins: toRaw(origins.value),
        rolling: activeSchedulingTab.value === 'rolling' ? toRaw(rolling.value) : null,
        recurrence: activeSchedulingTab.value === 'recurring' ? toRaw(recurrence.value) : null,
        termini: toRaw(termini.value),
      };
      routinesStore.modifyRoutine(updatedRoutine);
      console.log('updated routine', updatedRoutine);
      closeModalDrawer();
    }
  }
}
const closeModalDrawer = () => {
  emit('close');
};
watch(completeAllSetps, (newValue) => {
  emit('complete', newValue);
});
defineExpose({
  submittedData,
});
</script>
<template>
  <div class="flex h-auto lg:h-full flex-col lg:flex-row">
    <!-- Left pannel -->
    <div
      class="h-auto lg:h-full w-full lg:w-80 lg:border-r border-slate-300 py-4 overflow-y-auto custom-scroll"
    >
      <div class="px-4">
        <!-- Description Accordian  -->
        <DescriptionPannel :description="description" @description-updated="changeDescription" />
      </div>
      <hr class="border-slate-300" />
      <div v-if="props.routine?.createdAt" class="p-4 flex flex-col gap-2">
        <div class="text-xs font-medium text-slate-500">
          Updated:
          {{
            DateTimeFormatter.formatDatetime(
              props.routine.modifiedAt ? props.routine.modifiedAt : props.routine.createdAt
            )
          }}
        </div>
        <div class="text-xs font-medium text-slate-500">
          Created: {{ DateTimeFormatter.formatDatetime(props.routine.createdAt) }}
        </div>
      </div>
    </div>

    <!-- Right pannel -->
    <div class="h-full w-full lg:flex-1 py-4 lg:px-10 px-4 overflow-y-auto custom-scroll">
      <div class="text-xl leading-[30px] font-semibold">Settings</div>

      <!-- Alert -->
      <div v-if="alert" class="bg-slate-100 w-full rounded-lg p-4 mb-6 mt-4">
        <div class="flex justify-between gap-1.5">
          <div class="flex gap-1.5 sm:items-center">
            <IconLightBulb />
            <div class="font-semibold text-base">Currently in Use</div>
          </div>
          <IconXClose @click="closeAlert" width="20px" height="20px" class="cursor-pointer" />
        </div>
        <div class="font-medium text-sm">
          This Routine is currently assigned to
          <strong
            >{{ facilityAssignments }} active
            {{ facilityAssignments && facilityAssignments > 1 ? 'Facilities' : 'Facility' }}</strong
          >. Any existing Jobs that have already been scheduled based on the current configuration
          will not be changed but will be reflected thereafter.
        </div>
      </div>

      <!-- Trigger Points -->
      <AccordionDefault
        id="trigger_points"
        :active="activeFormStep === 1"
        @open="activeFormStep = 1"
        title="Trigger Points"
        class="mt-8"
      >
        <ProcedureStepForm
          source="trigger"
          :routine="props.routine"
          @complete-setp="watchTriggerPointsFieldsComplete"
          @trigger-updated="updateTriggerPoints"
        />
        <!-- Next button -->
        <div class="mt-4">
          <fwb-button
            @click="completeTriggerPointsStep"
            pill
            color="light"
            class="text-sm font-semibold text-slate-800 py-2.5 px-5"
            :class="
              [!isCompleteTriggerPointsFields ? 'pointer-events-none opacity-50' : ''].join('')
            "
          >
            Next
            <template #suffix><IconArrowNarrowDown width="20" height="20" /></template>
          </fwb-button>
        </div>
      </AccordionDefault>

      <!--  Purpose Selection -->

      <AccordionDefault
        id="purpose_selection"
        title="Purpose Selection"
        class="mt-8"
        :active="activeFormStep === 2"
        @open="activeFormStep = 2"
        :class="{
          'pointer-events-none opacity-50': isPurposeStepDisabled,
        }"
      >
        <div class="pl-4 my-2 ml-8 border-l border-slate-300">
          <fwb-select v-model="selectedPurpose" :options="purposeList" placeholder="Select one" />
        </div>
        <!-- Next button -->
        <div class="mt-4">
          <fwb-button
            @click="completePurposeStep"
            pill
            color="light"
            class="text-sm font-semibold text-slate-800 py-2.5 px-5"
            :class="[!isCompletePurposeField ? 'pointer-events-none opacity-50' : ''].join('')"
          >
            Next
            <template #suffix><IconArrowNarrowDown width="20" height="20" /></template>
          </fwb-button>
        </div>
      </AccordionDefault>

      <!-- Scheduling -->

      <AccordionDefault
        id="scheduling"
        title="Scheduling"
        :class="[
          'mt-8',
          {
            'pointer-events-none opacity-50': isSchedulingStepDisabled,
          },
        ]"
        :active="activeFormStep === 3"
        @open="activeFormStep = 3"
      >
        <div class="pl-4 my-2 ml-8 border-l border-slate-300">
          <!-- tabs -->
          <span class="isolate inline-flex rounded-md">
            <button
              type="button"
              :class="[
                'relative inline-flex items-center rounded-l-md py-2 px-4 text-sm font-medium ring-1 ring-inset ring-gray-300 focus:z-10',
                activeSchedulingTab === 'rolling'
                  ? 'bg-slate-100 text-slate-900'
                  : 'bg-white text-slate-900',
              ]"
              @click="activeSchedulingTab = 'rolling'"
            >
              Rolling Interval
            </button>
            <button
              type="button"
              :class="[
                'relative -ml-px inline-flex items-center rounded-r-md py-2 px-4 text-sm font-medium ring-1 ring-inset ring-gray-300 focus:z-10',
                activeSchedulingTab === 'recurring'
                  ? 'bg-slate-100 text-slate-900'
                  : 'bg-white text-slate-900',
              ]"
              @click="activeSchedulingTab = 'recurring'"
            >
              Recurring Event
            </button>
          </span>
          <!-- tabs content -->
          <div class="mt-8">
            <RollingInterval
              v-show="activeSchedulingTab === 'rolling'"
              :routine="props.routine"
              @complete-setp="watchRollingComplete"
              @rolling-updated="updateRolling"
            />
            <RecurringEvents
              v-show="activeSchedulingTab === 'recurring'"
              :routine="props.routine"
              @complete-setp="watchRecurringComplete"
              @recurring-updated="updateRecurring"
            />
          </div>
        </div>
        <!-- next button -->
        <div class="mt-4">
          <fwb-button
            @click="completeSchedulingStep"
            pill
            class="text-sm font-semibold text-slate-800 py-2.5 px-5"
            :class="[!isCompleteSchedulingFields ? 'pointer-events-none opacity-50' : ''].join('')"
            color="light"
            >Next <template #suffix><IconArrowNarrowDown width="20" height="20" /></template>
          </fwb-button>
        </div>
      </AccordionDefault>

      <!-- Termination -->

      <AccordionDefault
        id="termination"
        title="Termination"
        :class="[
          'mt-8 pb-8',
          {
            'pointer-events-none opacity-50': isTerminationStepDisabled,
          },
        ]"
        :active="activeFormStep === 4"
        @open="activeFormStep = 4"
      >
        <ProcedureStepForm
          source="termination"
          :routine="props.routine"
          @complete-setp="watchTerminationFieldsComplete"
          @trigger-updated="updateTermination"
        />
      </AccordionDefault>
    </div>
  </div>
</template>
<style src="vue-multiselect/dist/vue-multiselect.css"></style>

<style scoped>
:deep(.multiselect__tag > span) {
  vertical-align: text-top;
}
:deep(.multiselect__tag-icon) {
  line-height: 20px;
}
</style>
