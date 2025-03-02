<script setup lang="ts">
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import FacilityModalHeader from './FacilityModalHeader.vue';
import { ref, computed, watch, onMounted, toRaw } from 'vue';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import { FwbButton, FwbInput, FwbSelect } from 'flowbite-vue';
import AccordionHeader from '../AccordionHeader.vue';
import CustomCheckbox from '@/components/form/CustomCheckbox.vue';
import {
  ModifyFacilityCmd,
  RequiredPatientData,
  RoutineAssignmentPrm,
} from '@/api/__generated__/graphql';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import Tabs from '@/components/tabs/Tabs.vue';
import RoomsTab from '../tabs/rooms/index.vue';
import RoutinesTab from '../tabs/routines/index.vue';
import PurposesTab from '../tabs/purposes/index.vue';
import ProceduresTab from '../tabs/procedures/index.vue';
import { Facility } from '@/api/__generated__/graphql';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useModalStore } from '@/stores/modal';
import { useStatesStore } from '@/stores/data/location/states';
import { useTimezonesStore } from '@/stores/data/location/timezones';
import DateTimeFormatter from '@/utils/dateTimeFormatter';

const modalStore = useModalStore();
const facilitiesStore = useFacilitiesStore();
const proceduresStore = useProceduresStore();
const purposesStore = usePurposesStore();
const statesStore = useStatesStore();
const timezonesStore = useTimezonesStore();

const props = defineProps<{
  facility: Facility;
}>();
const emit = defineEmits<{
  (e: 'width', val: string): void;
  (e: 'close'): void;
}>();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const modalWidth = ref('5xl');

const facilitiesModalDrawerRef = ref<InstanceType<typeof ModalDrawer>>();
function closeFacilitiesModalDrawer() {
  facilitiesModalDrawerRef.value?.setModalOpen(false);
}

function setModalWidth(val: string) {
  modalWidth.value = val;
}

const setModalOpen = (value: boolean): void => {
  facilitiesModalDrawerRef.value?.setModalOpen(value);
};

function FacilityModalClosed() {
  facilitiesStore.clearSelectedFacility();
  modalStore.isFacilitiesModalExpended = false;
  setModalWidth('5xl');
  emit('close');
}

defineExpose({
  setModalOpen,
});

// facility Name Edited
const facilityName = ref(props.facility.name);
const facilityNameEdited = (newName: string) => {
  facilityName.value = newName;
};

watch(
  () => props.facility.name,
  (newValue) => {
    facilityName.value = newValue;
  }
);

//  Left pannel

//  Details

const isEditDetails = ref(false);

// states & timezone options
interface Option {
  value: string;
  name: string;
}
const stateOptions = ref<Option[]>([]);
const timeZoneOptions = ref<Option[]>([]);
watch(
  () => statesStore.states,
  (newStates) => {
    stateOptions.value = newStates.map((state) => ({
      value: state.code,
      name: state.name,
    }));
  },
  { immediate: true }
);

watch(
  () => timezonesStore.timeZones,
  (newStates) => {
    timeZoneOptions.value = newStates.map((timeZone) => ({
      value: timeZone,
      name: timeZone,
    }));
  },
  { immediate: true }
);

const street = ref<string>('');
const city = ref<string>('');
const state = ref<string>('');
const zipcode = ref<string>('');
const timezone = ref<string>('');

onMounted(() => {
  street.value = props.facility?.address?.street ?? '';
  city.value = props.facility?.address?.city ?? '';
  state.value = props.facility?.address?.state ?? '';
  zipcode.value = props.facility?.address?.zipCode ?? '';
  timezone.value = props.facility?.timeZone ?? '';
});

watch(
  () => props.facility,
  (newValue) => {
    street.value = newValue?.address?.street || '';
    city.value = newValue?.address?.city || '';
    state.value = newValue?.address?.state || '';
    zipcode.value = newValue?.address?.zipCode || '';
    timezone.value = newValue?.timeZone || '';
  },
  { deep: true }
);

const handleClickDetailsEditIcon = () => {
  isEditDetails.value = true;

  streetEdit.value = street.value;
  cityEdit.value = city.value;
  stateEdit.value = state.value;
  zipCodeEdit.value = zipcode.value;
  timeZoneEdit.value = timezone.value;
};
const cancelEditDetails = () => {
  isEditDetails.value = false;
};

// Validation
const schemaEditDetails = yup.object({
  streetEdit: yup.string().required('This is a required field').trim(),
  cityEdit: yup.string().required('This is a required field').trim(),
  stateEdit: yup.string().required('This is a required field'),
  zipCodeEdit: yup
    .string()
    .required('This is a required field')
    .matches(/^\d{5}$/, 'Zip Code must be at 5 digits'),
  timeZoneEdit: yup.string().required('This is a required field'),
});

const { handleSubmit: handleSubmitEditDetails } = useForm({
  validationSchema: schemaEditDetails,
});

const { value: streetEdit, errorMessage: streetEditError } = useField<string>('streetEdit');
const { value: cityEdit, errorMessage: cityEditError } = useField<string>('cityEdit');
const { value: stateEdit, errorMessage: stateEditError } = useField<string>('stateEdit');
const { value: zipCodeEdit, errorMessage: zipCodeEditError } = useField<string>('zipCodeEdit');
const { value: timeZoneEdit, errorMessage: timeZoneEditError } = useField<string>('timeZoneEdit');

function handleEditDetails() {
  handleSubmitEditDetails(async () => {
    street.value = streetEdit.value;
    city.value = cityEdit.value;
    state.value = stateEdit.value;
    zipcode.value = zipCodeEdit.value;
    timezone.value = timeZoneEdit.value;
    isEditDetails.value = false;
  })();
}

// Required Facility Fields
const selectedRequiredData = ref<RequiredPatientData[]>([]);
watch(
  () => props.facility.requiredData,
  (newRequiredData) => {
    if (newRequiredData) {
      selectedRequiredData.value = newRequiredData.filter((item) => item !== null);
    }
  },
  { immediate: true }
);
//  Right pannel

// Tabs

const tabs = computed(() => [
  {
    label: 'Rooms',
    badge: rooms.value.length ? rooms.value.length.toString() : '',
  },
  {
    label: 'Routines',
    badge: routineExecutionCount.value ? routineExecutionCount.value.toString() : '',
  },
  {
    label: 'Purposes',
    badge: unselectedPurposesCount.value ? unselectedPurposesCount.value.toString() : '',
  },
  {
    label: 'Procedures',
    badge: unselectedProceduresCount.value ? unselectedProceduresCount.value.toString() : '',
  },
]);

// Rooms
const rooms = computed(() => facilitiesStore.rooms);

// purposes
const purposes = computed(() => purposesStore.purposes);
const selectedPurposeIds = ref<string[]>([]);
function handlePurposeIdsUpdate(purposeIds: string[]) {
  selectedPurposeIds.value = purposeIds;
  facilitiesStore.updatePurposeIds(purposeIds);
}
onMounted(() => {
  selectedPurposeIds.value = props.facility.purposeIds || [];
});
watch(
  () => props.facility.purposeIds,
  (newValue) => {
    selectedPurposeIds.value = newValue || [];
  },
  { deep: true }
);

// Calculate the count of unselected purposes
// Exclude archived purposes
const unselectedPurposesCount = computed(
  () => purposes.value.filter((purpose) => !selectedPurposeIds.value.includes(purpose.id) && purpose.archived === false).length
);

// Procedures
const procedures = computed(() => proceduresStore.procedures);
const selectedProcedureIds = ref<string[]>([]);
function handleProcedureIdsUpdate(procedureIds: string[]) {
  selectedProcedureIds.value = procedureIds;
  facilitiesStore.updateProcedureIds(procedureIds);
}

onMounted(() => {
  selectedProcedureIds.value = props.facility.procedureIds || [];
});
watch(
  () => props.facility.procedureIds,
  (newValue) => {
    selectedProcedureIds.value = newValue || [];
  },
  { deep: true }
);

// Calculate the count of unselected procedures
const unselectedProceduresCount = computed(
  () =>
    procedures.value.filter((procedure) => !selectedProcedureIds.value.includes(procedure.id) && procedure.archived === false)
      .length
);

// Routine Execution

onMounted(() => {
  routineExecutionCount.value = facilitiesStore.selectedFacility.routineAssignments?.length || 0;
  const assignments = props.facility.routineAssignments || [];
  routineExecutions.value = transformRoutineAssignments(assignments);
  routineExecutionCount.value = assignments.length;
});

watch(
  () => props.facility.routineAssignments,
  (newValue) => {
    routineExecutions.value = transformRoutineAssignments(newValue || []);
    routineExecutionCount.value = (newValue || []).length;
  },
  { deep: true }
);

function transformRoutineAssignments(assignments: any[]): any[] {
  return assignments.map((assignment: RoutineAssignmentPrm) => ({
    id: assignment.id,
    name: assignment.name,
    routineId: assignment.routineId,
    specs: assignment.specs
      ? assignment.specs.map((spec: any) => ({
          propertyId: spec.propertyId || null,
          propertyValue: spec.propertyValue || null,
        }))
      : [{ propertyId: null, propertyValue: null }],
  }));
}

watch(
  () => props.facility.routineAssignments?.length,
  (newValue) => {
    routineExecutionCount.value = newValue || 0;
  },
  { deep: true }
);

const routineExecutionCount = ref(0);
const routineExecutions = ref<RoutineAssignmentPrm[]>([]);
function handleExecutionsUpdate(executions: RoutineAssignmentPrm[]) {
  routineExecutions.value = executions;
  routineExecutionCount.value = executions.length;
  facilitiesStore.updateRoutineAssignments(executions);
  // console.log('watch exec', routineExecutions.value, executions);
}

// Modified Facility on save

const handleSaveForms = () => {
  const modifiedFacility: ModifyFacilityCmd = {
    address: {
      city: city.value,
      state: state.value,
      street: street.value,
      zipCode: zipcode.value,
    },
    assignments: toRaw(routineExecutions.value),
    id: props.facility.id,
    name: facilityName.value!,
    procedureIds: [...selectedProcedureIds.value],
    purposeIds: [...selectedPurposeIds.value],
    requiredData: [...selectedRequiredData.value],
    timeZone: timezone.value,
  };
  facilitiesStore.modifyFacility(modifiedFacility);
  // console.log('modify facility:', modifiedFacility);
  facilitiesStore.clearSelectedFacility();
  closeFacilitiesModalDrawer();
};
</script>

<template>
  <ModalDrawer
    ref="facilitiesModalDrawerRef"
    :z_index="46"
    :max_width="modalWidth"
    hide_header_close_on_mobile
    @close="FacilityModalClosed"
  >
    <template #header>
      <FacilityModalHeader
        @close="closeFacilitiesModalDrawer"
        @width="setModalWidth"
        :facility="props.facility"
        @facilityName="facilityNameEdited"
      />
    </template>
    <template #body>
      <div class="flex h-auto lg:h-full flex-col lg:flex-row">
        <!-- Left pannel -->
        <div
          class="h-auto lg:h-full w-full lg:w-80 lg:border-r border-slate-300 overflow-y-auto custom-scroll"
        >
          <!--  Details -->
          <AccordionDefault id="1" active class="p-4 border-b border-slate-300">
            <template #header>
              <AccordionHeader
                title="Details"
                @edit="handleClickDetailsEditIcon"
                :is-edit="isEditDetails"
                :archived="facility.archived ?? false"
              ></AccordionHeader>
            </template>

            <div v-if="!isEditDetails" class="flex flex-col gap-4 pt-2 self-stretch">
              <div class="flex self-stretch gap-2">
                <div
                  class="w-1/3 flex-shrink-0 text-xs leading-5 font-medium text-slate-500 text-nowrap"
                >
                  Location
                </div>
                <div class="flex-grow text-wrap text-sm font-semibold text-slate-900">
                  {{ street }}, {{ city }}, {{ statesStore.getStateFullName(state) }},
                  {{ zipcode }}
                </div>
              </div>
              <div class="flex self-stretch gap-2">
                <div
                  class="w-1/3 flex-shrink-0 text-xs leading-5 font-medium text-slate-500 text-nowrap"
                >
                  Time Zone
                </div>
                <div class="flex-grow break-all text-sm font-semibold text-slate-900">
                  {{ timezone }}
                </div>
              </div>
            </div>

            <div v-else class="flex flex-col gap-4 pt-2">
              <div>
                <label class="mb-2 block text-sm font-medium text-gray-900">Street</label>
                <fwb-input
                  v-model="streetEdit"
                  :class="streetEditError ? inputErrorClasses : ''"
                  placeholder="Write text here"
                />
                <span v-if="streetEditError" class="text-sm text-radical-red-600">{{
                  streetEditError
                }}</span>
              </div>

              <div>
                <label class="mb-2 block text-sm font-medium text-gray-900">City</label>
                <fwb-input
                  v-model="cityEdit"
                  :class="cityEditError ? inputErrorClasses : ''"
                  placeholder="Write text here"
                />
                <span v-if="cityEditError" class="text-sm text-radical-red-600">{{
                  cityEditError
                }}</span>
              </div>

              <div>
                <label class="mb-2 block text-sm font-medium text-gray-900">State</label>
                <fwb-select
                  v-model="stateEdit"
                  :options="stateOptions"
                  placeholder="Select"
                  :class="stateEditError ? inputErrorClasses : ''"
                />
                <span v-if="stateEditError" class="text-sm text-radical-red-600">{{
                  stateEditError
                }}</span>
              </div>

              <div>
                <label class="mb-1 block text-sm font-medium text-slate-900">Zip Code</label>
                <fwb-input
                  v-model="zipCodeEdit"
                  :class="zipCodeEditError ? inputErrorClasses : ''"
                />
                <span v-if="zipCodeEditError" class="text-sm text-radical-red-600">{{
                  zipCodeEditError
                }}</span>
              </div>

              <div>
                <label class="mb-2 block text-sm font-medium text-gray-900">Time Zone</label>
                <fwb-select
                  v-model="timeZoneEdit"
                  :class="timeZoneEditError ? inputErrorClasses : ''"
                  :options="timeZoneOptions"
                  placeholder="Select"
                />
                <span v-if="timeZoneEditError" class="text-sm text-radical-red-600">{{
                  timeZoneEditError
                }}</span>
              </div>

              <div class="flex gap-4 items-center justify-end">
                <fwb-button @click="cancelEditDetails" color="light" pill> Cancel </fwb-button>
                <fwb-button
                  @click.prevent="handleEditDetails"
                  class="bg-primary-600 hover:bg-brand-600"
                  pill
                >
                  Confirm
                </fwb-button>
              </div>
            </div>
          </AccordionDefault>

          <!-- Required Facility Fields  -->
          <AccordionDefault
            id="required_facility_files"
            active
            title="Required Facility Fields"
            class="p-4 border-b border-slate-300"
          >
            <div
              :class="[
                facility.archived ? 'pointer-events-none' : '',
                'flex flex-col gap-5 pt-4 px-1',
              ]"
            >
              <CustomCheckbox
                v-model="selectedRequiredData"
                :value="RequiredPatientData.FirstName"
                label="Patient First Name"
                class="font-medium text-sm"
              />
              <CustomCheckbox
                v-model="selectedRequiredData"
                :value="RequiredPatientData.LastName"
                label="Patient Last Name"
                class="font-medium text-sm"
              />
              <CustomCheckbox
                v-model="selectedRequiredData"
                :value="RequiredPatientData.DateOfBirth"
                label="Date of Birth"
                class="font-medium text-sm"
              />
              <CustomCheckbox
                v-model="selectedRequiredData"
                :value="RequiredPatientData.OrderingProvider"
                label="Ordering Provider"
                class="font-medium text-sm"
              />
              <CustomCheckbox
                v-model="selectedRequiredData"
                :value="RequiredPatientData.Mrn"
                label="MRN"
                class="font-medium text-sm"
              />
            </div>
          </AccordionDefault>

          <div v-if="props.facility?.createdAt" class="p-4 flex flex-col gap-2">
            <div class="text-xs font-medium text-slate-500">
              Updated:
              {{
                DateTimeFormatter.formatDatetime(
                  props.facility.modifiedAt ? props.facility.modifiedAt : props.facility.createdAt
                )
              }}
            </div>
            <div class="text-xs font-medium text-slate-500">
              Created: {{ DateTimeFormatter.formatDatetime(props.facility.createdAt) }}
            </div>
          </div>
        </div>

        <!-- Right pannel -->
        <div class="h-full w-full lg:flex-1 py-4 lg:px-10 px-4 overflow-y-auto custom-scroll">
          <div class="text-xl leading-[30px] font-semibold">Facility Settings</div>
          <!-- tabs -->
          <div class="mt-8">
            <Tabs :tabs="tabs">
              <template #tab-0>
                <div class="py-4 lg:py-8">
                  <RoomsTab
                    @width="setModalWidth"
                    :facility="props.facility"
                    @closeModal="closeFacilitiesModalDrawer"
                  />
                </div>
              </template>
              <template #tab-1>
                <div class="py-4 lg:py-8">
                  <RoutinesTab
                    @width="setModalWidth"
                    :facility="props.facility"
                    @updateExecutions="handleExecutionsUpdate"
                  />
                </div>
              </template>
              <template #tab-2>
                <div class="py-4 lg:py-8">
                  <PurposesTab
                    :facility="props.facility"
                    @updateExcludedPurposeIds="handlePurposeIdsUpdate"
                    @closeModal="closeFacilitiesModalDrawer"
                  />
                </div>
              </template>
              <template #tab-3>
                <div class="py-4 lg:py-8">
                  <ProceduresTab
                    :facility="props.facility"
                    @updateExcludedProcedureIds="handleProcedureIdsUpdate"
                    @closeModal="closeFacilitiesModalDrawer"
                  />
                </div>
              </template>
            </Tabs>
          </div>
        </div>
      </div>
    </template>

    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeFacilitiesModalDrawer" color="light" pill> Cancel</fwb-button>
        <fwb-button
          @click="handleSaveForms"
          class="bg-primary-600 hover:bg-brand-600 sm:w-auto w-full"
          pill
          :disabled="facility.archived??false"
          >Save</fwb-button
        >
      </div>
    </template>
  </ModalDrawer>
</template>
