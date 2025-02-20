<script setup lang="ts">
import { FwbInput, FwbTextarea, FwbSelect, FwbButton, FwbRadio } from 'flowbite-vue';
import StatusBadge from '@/components/badge/StatusBadge.vue';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import { ref, computed, nextTick } from 'vue';
import { useRouter } from 'vue-router';
import VueTailwindDatepicker from 'vue-tailwind-datepicker';
import AutoComplete from '@/components/form/AutoComplete.vue';
import TimePicker from '@/components/form/TimePicker.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import ModalReview from '@/components/modal/ModalReview.vue';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import { EnactJobIntakePrc, FacilityRoom } from '@/api/__generated__/graphql';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useFacilitiesStore } from '@/stores/data/facilities';
import dayjs from 'dayjs';
import customParseFormat from 'dayjs/plugin/customParseFormat';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import { useLinesStore } from '@/stores/data/encounters/lines';
import StatusCheckbox from '@/components/form/StatusCheckbox.vue';
import { IconArrowNarrowDown } from '@/components/icons';
import AccordionHeaderCheck from '../AccordionHeaderCheck.vue';

dayjs.extend(customParseFormat);

const router = useRouter();
const jobsStore = useJobsStore();
const purposesStore = usePurposesStore();
const facilitiesStore = useFacilitiesStore();
const medicalRecordsStore = useMedicalRecordsStore();
const linesStore = useLinesStore();

// Modal
const modalCreateJobRef = ref<InstanceType<typeof ModalReview>>();
const setModalOpen = (value: boolean): void => {
  modalCreateJobRef.value?.setModalOpen(value);
};

// Validations
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schema = yup.object({
  purpose: yup.string().required('Select a purpose'),
  facility: yup.string().required('Select a facility'),
  room: yup.string().required('Select a room'),
});
const { handleSubmit, errors, resetForm } = useForm({
  validationSchema: schema,
});

// Form Fields
const { value: purpose, errorMessage: purposeError } = useField<string>('purpose');
const { value: facility, errorMessage: facilityError } = useField<string>('facility');
const { value: room, errorMessage: roomError } = useField<string>('room');
const note = ref<string>('');
const externalPlacementBy = ref<string>('');
const placementInsertedOn = ref<string>('');
const lineName = ref<string>('');
const contact = ref<string>('');
const isStat = ref<boolean>(false);
const statReason = ref<string>('');
const isOnHold = ref<boolean>(false);
const holdReason = ref<string>('');
const date = ref<string>('');
const time = ref<string>('');
const mrn = ref<string>('');
const medicalRecordId = ref<string>('');
const orderingProvider = ref<string>('');
const lineId = ref<string>('');

function clearFields() {
  purpose.value = '';
  facility.value = '';
  room.value = '';
  note.value = '';
  externalPlacementBy.value = '';
  placementInsertedOn.value = '';
  lineName.value = '';
  contact.value = '';
  isStat.value = false;
  statReason.value = '';
  isOnHold.value = false;
  holdReason.value = '';
  date.value = '';
  time.value = '';
  mrn.value = '';
  medicalRecordId.value = '';
  orderingProvider.value = '';
  lineId.value = '';

  activeFormStep.value = 1;
}

//// Patient ////

const mrnOptions = computed(() =>
  medicalRecordsStore.medicalRecords.map((m) => ({
    value: m.id,
    name: m.number ?? '',
    label: m.number ? getFacilityName(m.facilityId) : '',
  }))
);
function setMedicalRecord(value: string) {
  if (!value) {
    lineId.value = '';
  }
  medicalRecordId.value = value;
  const medicalRecord = medicalRecordsStore.medicalRecords.find((m) => m.id === value);
  mrn.value = medicalRecord?.number ?? '';
  if (!lineId.value && value) {
    facility.value = medicalRecord?.facilityId ?? '';
    setRoomOptions(medicalRecord?.facilityId ?? '');
    roomAutoComplete.value?.clearInput();
  }
  if (
    isAssociatedWithLine.value &&
    jobType.value === 'existing_line' &&
    lineOptions.value.length > 0
  ) {
    lineId.value = lineOptions.value[0].value;
    setLine(lineId.value);
  }
}

function createNewMedicalRecord(val: string) {
  medicalRecordId.value = '';
  mrn.value = val;
}

//// Location ////

const isLocationStepValid = computed(() => !!facility.value && !!room.value);
const facilityOptions = computed(() =>
  facilitiesStore.facilities
    .filter((f) => !f.archived)
    .map((p) => ({
      value: p.id,
      name: p.name ?? '',
    }))
);
function getFacilityName(facilityId: string): string {
  return facilitiesStore.facilities.find((f) => f.id === facilityId)?.name ?? '';
}
const rooms = ref<FacilityRoom[]>([]);
const roomOptions = computed(() =>
  rooms.value.map((r: FacilityRoom) => ({
    value: r?.id || '',
    name: r?.name || '',
  }))
);

function setRoomOptions(facilityId: string) {
  if (facilityId) {
    facilitiesStore.getRooms(facilityId)((result) => {
      if (result?.data?.facilityRooms?.items) {
        rooms.value = result.data.facilityRooms.items.filter((item) => item !== null);
      } else {
        rooms.value = [];
      }
    });
  } else {
    rooms.value = [];
  }
}

function addRoomToOptions(val: string) {
  room.value = val;
}

function existingRoomId(value: string): boolean {
  return rooms.value.some((room) => room.id === value);
}
const facilityAutoComplete = ref<InstanceType<typeof AutoComplete>>();
const roomAutoComplete = ref<InstanceType<typeof AutoComplete>>();

function handleFacilityChange(value: string) {
  setRoomOptions(value);
  roomAutoComplete.value?.clearInput();
}

//// Job Details ////

const jobType = ref<string>('new_job');
const isjobDetailsStepValid = computed(() => !!purpose.value);
const today = new Date().toISOString().split('T')[0];
const line = computed(() => {
  if (jobType.value === 'existing_line') {
    return lineId.value
      ? {
          id: lineId.value,
          name: null,
          externallyPlaced: false,
          placedBy: null,
          insertedOn: null,
        }
      : null;
  }
  if (jobType.value === 'placed_externally') {
    return {
      id: null,
      externallyPlaced: true,
      name: lineName.value || null,
      placedBy: externalPlacementBy.value || null,
      insertedOn: placementInsertedOn ? new Date(placementInsertedOn.value).toISOString() : null,
    };
  }

  return null;
});

function setLine(value: string) {
  const line = linesStore.lines.find((l) => l.id === value);
  const facilityId = line?.location?.facilityId;
  const roomId = line?.location?.roomId;
  if (facilityId) {
    facility.value = facilityId;
    setRoomOptions(facilityId);
    nextTick(() => {
      if (roomId) {
        room.value = roomId;
      } else {
        roomAutoComplete.value?.clearInput();
      }
    });
  }
}

function toggleJobType(value: string) {
  lineId.value = '';
  lineName.value = '';
  externalPlacementBy.value = '';
  placementInsertedOn.value = '';
  if (value === 'existing_line' && lineOptions.value.length > 0) {
    lineId.value = lineOptions.value[0].value;
    setLine(lineId.value);
  }
  if (value !== 'existing_line') {
  }
}

const purposeOptions = computed(() =>
  purposesStore.purposes
    .filter((purpose) => purpose.archived === false)
    .map((p) => ({
      value: p.id,
      name: p.name ?? '',
    }))
);

const lineOptions = computed(() =>
  linesStore.lines
    .filter((line) => line.dwelling === 'IN')
    .filter((lin) => lin.medicalRecordId === medicalRecordId.value)
    .map((l) => ({
      value: l.id,
      name: l.name ?? '',
    }))
);

const isAssociatedWithLine = computed(() => {
  return linesStore.lines
    .filter((line) => line.dwelling === 'IN')
    .some((line) => line.medicalRecordId === medicalRecordId.value);
});

//// Urgency/Schedule ////

const scheduleType = ref<string>('no_schedule');

function toggleSchedule() {
  isStat.value = false;
  isOnHold.value = false;
  holdReason.value = '';
  statReason.value = '';
  date.value = '';
  time.value = '';
}
function toggleIsStat() {
  isStat.value = !isStat.value;
}
function toggleIsOnHold() {
  isOnHold.value = !isOnHold.value;
}
const formatter = ref({
  date: 'DD MMM YYYY',
  month: 'MMM',
});

const parseDate = (dateStr: string): string => {
  return dayjs(dateStr, 'DD MMM YYYY').format('YYYY-MM-DD');
};
const parseTime = (timeStr: string): string => {
  return dayjs(timeStr, 'hh:mm A').format('HH:mm:ss');
};
const dDate = (dateToCheck: Date): boolean => {
  const today = new Date();
  return dateToCheck < today;
};
function getScheduled(date: string, time: string) {
  if (!date) {
    return null;
  }
  return {
    date: parseDate(date),
    time: time ? parseTime(time) : null,
  };
}

const getFacilityTimezone = (facilityId: string): string => {
  return facilitiesStore.facilities.find((f) => f.id === facilityId)?.timeZone ?? '';
};

//// Create Job ////
const extendedValues = ref<EnactJobIntakePrc>();
function handleClickCreateJob() {
  handleSubmit(async (values) => {
    extendedValues.value = {
      medicalRecord:
        medicalRecordId.value || mrn.value
          ? {
              id: medicalRecordId.value || null,
              number: mrn.value || null,
            }
          : null,
      location: {
        facilityId: values.facility,
        roomId: existingRoomId(room.value) ? room.value : null,
        roomName: existingRoomId(room.value) ? null : room.value,
      },
      purposeId: values.purpose,
      orderingProvider: orderingProvider.value ? orderingProvider.value : null,
      contact: contact.value ? contact.value : null,
      line: line.value,
      preNote: note.value ? note.value : null,
      schedule: getScheduled(date.value, time.value),
      urgency:
        scheduleType.value === 'schedule'
          ? null
          : {
              isOnHold: isOnHold.value,
              holdReason: holdReason.value?.trim() !== '' ? holdReason.value : null,
              isStat: isStat.value,
              statReason: statReason.value?.trim() !== '' ? statReason.value : null,
            },
    };
    // console.log('Form Values:', extendedValues.value);
    jobsStore.createJob(extendedValues.value);
    clearFields();
    resetForm();
    setModalOpen(false);

    router.push({ path: '/' });
  })();
}

// Steps
const activeFormStep = ref<number>(1);

// Expose functions
defineExpose({
  setModalOpen,
});
</script>
<template>
  <!-- Create Job Modal -->
  <ModalReview
    ref="modalCreateJobRef"
    max_width="full"
    title="New Job"
    :left_close_icon="true"
    @close="clearFields"
    :no_footer="activeFormStep < 5"
  >
    <template #body>
      <div class="flex flex-col px-4 py-2 lg:px-8 lg:py-4 max-w-xl mx-auto">
        <!-- Patient -->
        <AccordionDefault
          id="acc_patient"
          class="pb-2"
          :active="activeFormStep === 1"
          :class="activeFormStep < 1 ? 'pointer-events-none opacity-50' : ''"
        >
          <template #header
            ><AccordionHeaderCheck title="Patient" :isCheck="activeFormStep > 1"
          /></template>
          <div class="flex flex-col gap-4 mt-2 ml-8 pl-4 border-l border-slate-300">
            <AutoComplete
              v-model="mrn"
              :options="mrnOptions"
              create_option
              @create="createNewMedicalRecord"
              @update:modelValue="setMedicalRecord"
              label="MRN"
            ></AutoComplete>
          </div>
          <!-- Next button -->
          <div v-if="activeFormStep === 1" class="mt-4">
            <fwb-button
              @click="activeFormStep = 2"
              pill
              color="light"
              class="text-sm font-semibold text-slate-800 py-2.5 px-5"
            >
              <template v-if="mrn">Next</template>

              <template v-else>MRN not Available</template>
              <template #suffix><IconArrowNarrowDown width="20" height="20" /></template>
            </fwb-button>
          </div>
        </AccordionDefault>
        <!-- Job Details -->
        <AccordionDefault
          id="acc_details"
          class="py-2"
          :active="activeFormStep === 2"
          :class="activeFormStep < 2 ? 'pointer-events-none opacity-50' : ''"
        >
          <template #header
            ><AccordionHeaderCheck
              title="Job Details"
              :isCheck="activeFormStep > 3 && isjobDetailsStepValid"
            >
            </AccordionHeaderCheck>
          </template>
          <div class="flex flex-col gap-4 mt-2 ml-8 pl-4 border-l border-slate-300">
            <div class="bg-slate-50 p-4 rounded-lg radio-card">
              <div class="border-b border-slate-300 pb-4">
                <fwb-radio
                  v-model="jobType"
                  label="No Line"
                  name="create-job-option"
                  value="new_job"
                  @update:modelValue="toggleJobType"
                />
                <div class="ml-6 -mt-1">
                  <div class="text-xs text-gray-500">The Job is not for a monitored Line.</div>
                </div>
              </div>
              <div v-if="mrn && isAssociatedWithLine" class="border-b border-slate-300 py-4">
                <fwb-radio
                  v-model="jobType"
                  label="Current Dwelling"
                  name="create-job-option"
                  value="existing_line"
                  @update:modelValue="toggleJobType"
                />
                <div class="ml-6 -mt-1">
                  <div class="text-xs text-gray-500">
                    The Job is for a currently monitored Line.
                  </div>
                  <div v-if="jobType === 'existing_line'" class="flex flex-col gap-4 pt-4 pb-2">
                    <div>
                      <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                        >Choose Line</label
                      >
                      <fwb-select
                        v-model="lineId"
                        :options="lineOptions"
                        @update:modelValue="setLine"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div class="pt-4">
                <fwb-radio
                  v-model="jobType"
                  label="External Placement"
                  name="create-job-option"
                  value="placed_externally"
                  @update:modelValue="toggleJobType"
                />
                <div class="ml-6 -mt-1">
                  <div class="text-xs text-gray-500">
                    The Job is for a new, externally placed Line.
                  </div>
                  <div v-if="jobType === 'placed_externally'" class="flex flex-col gap-4 pt-4 pb-2">
                    <div>
                      <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                        >Placement Inserted on</label
                      >
                      <fwb-input
                        v-model="placementInsertedOn"
                        type="date"
                        :max="today"
                        @keydown.prevent
                        placeholder="Write text here"
                      />
                    </div>
                    <div>
                      <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                        >Placement Inserted by</label
                      >
                      <fwb-input
                        v-model="externalPlacementBy"
                        placeholder="provide name of person or entity ..."
                      />
                    </div>
                    <div>
                      <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                        >Line Name</label
                      >
                      <fwb-input v-model="lineName" placeholder="provide line name ..." />
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div>
              <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                >Purpose</label
              >
              <fwb-select
                v-model="purpose"
                :options="purposeOptions"
                :class="purposeError ? inputErrorClasses : ''"
              />
              <span v-if="purposeError" class="text-sm text-radical-red-600">{{
                purposeError
              }}</span>
            </div>
            <div>
              <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                >Contact</label
              >
              <fwb-input v-model="contact" placeholder="provide name of doctor, nurse, etc ..." />
            </div>
            <div>
              <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                >Ordering Provider
              </label>
              <fwb-input v-model="orderingProvider" placeholder="provide name ..." />
            </div>
          </div>
          <!-- Next button -->
          <div v-if="activeFormStep === 2" class="mt-4">
            <fwb-button
              @click="activeFormStep = 3"
              pill
              color="light"
              class="text-sm font-semibold text-slate-800 py-2.5 px-5"
              :class="[!purpose ? 'pointer-events-none opacity-50' : ''].join('')"
            >
              Next
              <template #suffix><IconArrowNarrowDown width="20" height="20" /></template>
            </fwb-button>
          </div>
        </AccordionDefault>
        <!-- Location -->
        <AccordionDefault
          id="acc_location"
          class="py-2"
          :active="activeFormStep === 3"
          :class="activeFormStep < 3 ? 'pointer-events-none opacity-50' : ''"
        >
          <template #header
            ><AccordionHeaderCheck
              title="Location"
              :isCheck="activeFormStep > 2 && isLocationStepValid"
          /></template>
          <div class="flex flex-col gap-4 mt-2 ml-8 pl-4 border-l border-slate-300">
            <AutoComplete
              ref="facilityAutoComplete"
              v-model="facility"
              :options="facilityOptions"
              label="Facility"
              :error="!!facilityError"
              :error_message="facilityError"
              @update:modelValue="handleFacilityChange"
            ></AutoComplete>
            <AutoComplete
              ref="roomAutoComplete"
              v-model="room"
              :options="roomOptions"
              label="Room"
              :error="!!roomError"
              :error_message="roomError"
              create_option
              @create="addRoomToOptions"
            ></AutoComplete>
          </div>
          <!-- Next button -->
          <div v-if="activeFormStep === 3" class="mt-4">
            <fwb-button
              @click="activeFormStep = 4"
              pill
              color="light"
              class="text-sm font-semibold text-slate-800 py-2.5 px-5"
              :class="[!facility || !room ? 'pointer-events-none opacity-50' : ''].join('')"
            >
              Next
              <template #suffix><IconArrowNarrowDown width="20" height="20" /></template>
            </fwb-button>
          </div>
        </AccordionDefault>

        <!-- Notes -->
        <AccordionDefault
          id="acc_notes"
          class="py-2"
          :active="activeFormStep === 4"
          :class="activeFormStep < 4 ? 'pointer-events-none opacity-50' : ''"
        >
          <template #header
            ><AccordionHeaderCheck title="Notes" :isCheck="activeFormStep > 4"
          /></template>
          <div class="flex flex-col gap-4 mt-2 ml-8 pl-4 border-l border-slate-300 -mb-2">
            <fwb-textarea
              v-model="note"
              :rows="4"
              label=""
              placeholder="provide any additional information about the patient or expected procedure(s) ..."
              class="-mt-2"
            />
          </div>
          <!-- Next button -->
          <div v-if="activeFormStep === 4" class="mt-4">
            <fwb-button
              @click="activeFormStep = 5"
              pill
              color="light"
              class="text-sm font-semibold text-slate-800 py-2.5 px-5"
            >
              <template v-if="note">Next</template>

              <template v-else>Continue without Notes</template>
              <template #suffix><IconArrowNarrowDown width="20" height="20" /></template>
            </fwb-button>
          </div>
        </AccordionDefault>
        <!-- Urgency/Schedule -->
        <AccordionDefault
          id="acc_schedule"
          class="py-2"
          :active="activeFormStep === 5"
          :class="activeFormStep < 5 ? 'pointer-events-none opacity-50' : ''"
        >
          <template #header
            ><AccordionHeaderCheck title="Urgency" :isCheck="activeFormStep > 5"
          /></template>
          <div class="flex flex-col gap-4 mt-2 ml-8 pl-4 border-l border-slate-300">
            <div class="bg-slate-50 p-4 rounded-lg radio-card">
              <div class="border-b border-slate-300 pb-4">
                <fwb-radio
                  v-model="scheduleType"
                  label="Place in the Queue"
                  name="urgency-schedule"
                  value="no_schedule"
                  @click="toggleSchedule"
                />
                <div class="-mt-1">
                  <div class="ml-6 text-xs text-gray-500">
                    Wil be put into the Live Queue immediately.
                  </div>
                  <div v-if="scheduleType === 'no_schedule'" class="flex flex-col pt-4">
                    <!-- Status Checkboxes -->
                    <div class="flex flex-col gap-4">
                      <div>
                        <StatusCheckbox
                          :model-value="isStat"
                          label="Mark as 'Stat'"
                          class="w-full"
                          @change="toggleIsStat"
                        >
                          <StatusBadge v-if="isStat" badge="STAT" />
                        </StatusCheckbox>
                        <div v-if="isStat" class="ml-6 pt-1.5">
                          <fwb-textarea
                            v-model="statReason"
                            :rows="2"
                            label=""
                            placeholder="why is this job so urgent? (optional)"
                            class="-mt-1"
                          />
                        </div>
                      </div>
                      <div>
                        <StatusCheckbox
                          :model-value="isOnHold"
                          label="Place on Hold"
                          class="w-full"
                          @change="toggleIsOnHold"
                        >
                          <StatusBadge v-if="isOnHold" badge="ON HOLD" />
                        </StatusCheckbox>
                        <div v-if="isOnHold" class="ml-6 pt-1.5">
                          <fwb-textarea
                            v-model="holdReason"
                            :rows="2"
                            label=""
                            placeholder="why is this job on hold? (optional)"
                            class="-mt-1"
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <div class="pt-4">
                <fwb-radio
                  v-model="scheduleType"
                  label="Defer to the Future"
                  name="urgency-schedule"
                  value="schedule"
                  @click="toggleSchedule"
                />
                <div class="-mt-1">
                  <div class="ml-6 text-xs text-gray-500">
                    Will put this in the Live Queue at scheduled date (and time) in the future.
                  </div>
                  <!-- Date and time -->
                  <div
                    v-show="scheduleType === 'schedule'"
                    class="flex flex-col gap-4 duration-300 ease pt-4 pb-2"
                  >
                    <div class="flex flex-wrap gap-4 justify-between">
                      <vue-tailwind-datepicker
                        v-model="date"
                        as-single
                        no-input
                        :formatter="formatter"
                        disable-in-range
                        :disable-date="dDate"
                        class="custom_dp mx-auto lg:mx-0 text-xs"
                      />
                      <div class="bg-slate-50 w-full lg:w-36">
                        <TimePicker v-model="time" />
                      </div>
                    </div>

                    <div
                      v-if="date || time"
                      class="py-2 px-4 w-full bg-brand-50 rounded-lg text-barnd-600 text-xs font-bold text-center"
                    >
                      {{ date }}<span v-if="date && time">, &nbsp;</span> {{ time }}
                      <span v-if="date && time && facility"
                        >({{ getFacilityTimezone(facility) }})</span
                      >
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </AccordionDefault>
      </div>
    </template>
    <template #footer>
      <div class="p-4 lg:px-6 flex items-center justify-end w-full">
        <div>
          <span
            v-if="Object.keys(errors).length"
            class="text-sm font-normal text-radical-red-600 mr-4"
            >{{ Object.keys(errors).length }} errors</span
          >
          <fwb-button
            @click="handleClickCreateJob"
            class="bg-primary-600 hover:bg-brand-600 w-full lg:w-auto"
            pill
          >
            Create Job
          </fwb-button>
        </div>
      </div>
    </template>
  </ModalReview>
</template>

<style scoped>
:deep(.custom_dp > .shadow-sm) {
  padding: 0;
  border: 0;
  box-shadow: none;
}
:deep(.custom_dp .relative.flex.flex-wrap.w-full) {
  padding: 0;
}
:deep(.custom_dp .relative.w-full.lg\:w-80) {
  width: 16rem;
}
:deep(.custom_dp .flex.justify-between.items-center.px-2) {
  border-bottom: solid 1px #e2e8f0;
}

/* Select flowbite validition classes */
:deep(.bg-radical-red-50 select) {
  border-color: #e11d47;
  background-color: inherit;
}
:deep(textarea),
:deep(input),
:deep(select) {
  background-color: white;
}
</style>
