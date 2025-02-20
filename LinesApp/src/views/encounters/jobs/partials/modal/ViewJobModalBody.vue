<script setup lang="ts">
import { FwbInput, FwbButton, FwbBadge } from 'flowbite-vue';
import AutoComplete from '@/components/form/AutoComplete.vue';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import AccordionHeader from '../../partials/AccordionHeader.vue';
import Tabs from '@/components/tabs/Tabs.vue';
import ProcedureTab from '../tabs/procedure/index.vue';
import PhotosTab from '../tabs/photos/index.vue';
import NotesTab from '../tabs/notes/index.vue';
import ObservationsTab from '../tabs/observations/index.vue';
import { computed, onMounted, ref, watch } from 'vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { EnactEncounterRevisionPrc, Encounter, EncounterStage, Job, JobStatus, RequiredPatientData, FacilityRoom, User } from '@/api/__generated__/graphql';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { formatDateByDMY, formatRelativeDate } from '@/utils/dateUtils';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import { useEncountersStore } from '@/stores/data/encounters';
import StatusBadge from '@/components/badge/StatusBadge.vue';
import { useModalStore } from '@/stores/modal';
import { useUsersStore } from '@/stores/data/settings/users';
import { useLinesStore } from '@/stores/data/encounters/lines';
import { IconArrowCircleBrokenDownRight, IconAlertHexagon,IconChevronUp } from '@/components/icons/index';
import ActivityFeed from '@/views/common/activityFeed/ActivityFeed.vue';
import dayjs from 'dayjs';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import SkeletonItem from '@/components/skeletons/SkeletonItem.vue';
import { useLoaders } from '@/hooks/useLoaders';
import UserAvatar from '@/components/avatar/UserAvatar.vue';

const props = defineProps<{
  job: Job;
  isReview?: boolean;
  encounter?: Encounter;
}>();

const emit = defineEmits<{
  (e: 'width', val: string): void;
  (e: 'has-procedures', val: boolean): void;
  (e: 'disable-save-submit', val: boolean): void;
  (e: 'onFollowUpClick', encounterId: string): void;
  (e: 'unsaved-details', val: boolean): void;
  (e: 'unsaved-procedures', val: boolean): void;
}>();
const { isJobLoading: isLoading } = useLoaders();
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const isMounted = ref(false);

const facilitiesStore = useFacilitiesStore();
const modalStore = useModalStore();
const encountersStore = useEncountersStore();
const medicalRecordsStore = useMedicalRecordsStore();
const usersStore = useUsersStore();
const linesStore = useLinesStore();
const proceduresStore = useProceduresStore();

const notesCount = computed(() => props.job.notes?.length);
const photosCount = computed(() => props.encounter?.photos?.length);
const proceduresCount = computed(() => props.encounter?.procedures?.length);
const observationCount = computed(() => {
  const record = medicalRecordsStore.medicalRecords.find(
    (record) => record.id === props.encounter?.medicalRecordId
  );
  return record?.observations?.length || 0;
});
const disabledTab = computed(
  () => props.encounter?.stage === EncounterStage.Waiting || props.encounter?.stage === EncounterStage.Assigned
);

const activeTab = computed(() => {
  switch (props.encounter?.stage) {
    case EncounterStage.Attending:
    case EncounterStage.Charting:
    case EncounterStage.Completed:
      return 0;
    default:
      return 1;
  }
});


const tabs = computed(() => [
  ...(props.encounter
    ? [
        {
          label: 'Procedure',
          icon: `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 20 20" fill="none">
            <path d="M17.5 7.91667V6.5C17.5 5.09987 17.5 4.3998 17.2275 3.86502C16.9878 3.39462 16.6054 3.01217 16.135 2.77248C15.6002 2.5 14.9001 2.5 13.5 2.5H6.5C5.09987 2.5 4.3998 2.5 3.86502 2.77248C3.39462 3.01217 3.01217 3.39462 2.77248 3.86502C2.5 4.3998 2.5 5.09987 2.5 6.5V13.5C2.5 14.9001 2.5 15.6002 2.77248 16.135C3.01217 16.6054 3.39462 16.9878 3.86502 17.2275C4.3998 17.5 5.09987 17.5 6.5 17.5H7.91667M14.4885 14.7594L13.0733 17.3877C12.8419 17.8173 12.7263 18.0322 12.5853 18.0882C12.4629 18.1369 12.3247 18.1248 12.2126 18.0557C12.0835 17.976 12.0068 17.7444 11.8534 17.2812L9.58366 10.4261C9.44936 10.0205 9.38221 9.81764 9.43049 9.68258C9.47251 9.56503 9.56503 9.47252 9.68257 9.43049C9.81764 9.38221 10.0205 9.44936 10.4261 9.58367L17.2812 11.8534C17.7444 12.0068 17.976 12.0835 18.0556 12.2126C18.1248 12.3247 18.1369 12.4629 18.0882 12.5853C18.0321 12.7263 17.8173 12.8419 17.3877 13.0733L14.7594 14.4885C14.694 14.5238 14.6612 14.5414 14.6326 14.564C14.6071 14.5841 14.5841 14.6071 14.564 14.6326C14.5414 14.6612 14.5238 14.694 14.4885 14.7594Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
          </svg>`,
          badge: proceduresCount.value,
          disabled: disabledTab.value,
        },
      ]
    : []),
  {
    label: 'Notes',
    icon: `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 20 20" fill="none">
            <path d="M11.6668 9.1665H6.66683M8.3335 12.4998H6.66683M13.3335 5.83317H6.66683M16.6668 5.6665V14.3332C16.6668 15.7333 16.6668 16.4334 16.3943 16.9681C16.1547 17.4386 15.7722 17.821 15.3018 18.0607C14.767 18.3332 14.067 18.3332 12.6668 18.3332H7.3335C5.93336 18.3332 5.2333 18.3332 4.69852 18.0607C4.22811 17.821 3.84566 17.4386 3.60598 16.9681C3.3335 16.4334 3.3335 15.7333 3.3335 14.3332V5.6665C3.3335 4.26637 3.3335 3.56631 3.60598 3.03153C3.84566 2.56112 4.22811 2.17867 4.69852 1.93899C5.2333 1.6665 5.93336 1.6665 7.3335 1.6665H12.6668C14.067 1.6665 14.767 1.6665 15.3018 1.93899C15.7722 2.17867 16.1547 2.56112 16.3943 3.03153C16.6668 3.56631 16.6668 4.26637 16.6668 5.6665Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
          </svg>`,
    badge: notesCount.value,
  },
  ...(props.encounter
    ? [
        {
          label: 'Photos',
          icon: `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 20 20" fill="none">
            <path d="M1.6665 6.41799C1.6665 5.17453 2.67453 4.1665 3.91799 4.1665C4.56406 4.1665 5.13764 3.75309 5.34195 3.14017L5.4165 2.9165C5.45166 2.81103 5.46924 2.75829 5.48805 2.71151C5.72821 2.11411 6.291 1.70848 6.93368 1.66955C6.98401 1.6665 7.0396 1.6665 7.15079 1.6665H12.8489C12.9601 1.6665 13.0157 1.6665 13.066 1.66955C13.7087 1.70848 14.2715 2.11411 14.5116 2.71151C14.5304 2.75829 14.548 2.81103 14.5832 2.9165L14.6577 3.14017C14.862 3.75309 15.4356 4.1665 16.0817 4.1665C17.3251 4.1665 18.3332 5.17453 18.3332 6.41799V13.4998C18.3332 14.9 18.3332 15.6 18.0607 16.1348C17.821 16.6052 17.4386 16.9877 16.9681 17.2274C16.4334 17.4998 15.7333 17.4998 14.3332 17.4998H5.6665C4.26637 17.4998 3.56631 17.4998 3.03153 17.2274C2.56112 16.9877 2.17867 16.6052 1.93899 16.1348C1.6665 15.6 1.6665 14.9 1.6665 13.4998V6.41799Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
            <path d="M9.99984 13.7498C12.0709 13.7498 13.7498 12.0709 13.7498 9.99984C13.7498 7.92877 12.0709 6.24984 9.99984 6.24984C7.92877 6.24984 6.24984 7.92877 6.24984 9.99984C6.24984 12.0709 7.92877 13.7498 9.99984 13.7498Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
          </svg>`,
          badge: photosCount.value,
          disabled: disabledTab.value,
        },
      ]
    : []),

  // observation
  ...(props.encounter?.medicalRecordId
    ? [
        {
          label: 'Observations',
          icon: `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 20 20" fill="none">
          <path d="M16.6673 5.83268V5.66602C16.6673 4.26588 16.6673 3.56582 16.3948 3.03104C16.1552 2.56063 15.7727 2.17818 15.3023 1.9385C14.7675 1.66602 14.0674 1.66602 12.6673 1.66602H7.33398C5.93385 1.66602 5.23379 1.66602 4.69901 1.9385C4.2286 2.17818 3.84615 2.56063 3.60647 3.03104C3.33398 3.56582 3.33398 4.26588 3.33398 5.66602V14.3327C3.33398 15.7328 3.33398 16.4329 3.60647 16.9677C3.84615 17.4381 4.2286 17.8205 4.69901 18.0602C5.23379 18.3327 5.93385 18.3327 7.33398 18.3327H10.4173M15.0007 14.9993V10.416C15.0007 9.72566 15.5603 9.16602 16.2507 9.16602C16.941 9.16602 17.5007 9.72566 17.5007 10.416V14.9993C17.5007 16.3801 16.3814 17.4993 15.0007 17.4993C13.6199 17.4993 12.5007 16.3801 12.5007 14.9993V11.666" stroke="#6A5ACD" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>`,
          badge: observationCount.value,
          disabled: disabledTab.value,
        },
      ]
    : []),
]);

const medicalRecord = computed(() =>
  medicalRecordsStore.medicalRecords.find((mr) => mr.id === props.job?.medicalRecordId)
);

const selectedMedicalRecordId = ref(props.encounter?.medicalRecordId || null);

const orderingProvider = ref('');
const contact = ref('');

const facility = ref('');
const room = ref('');

const firstName = ref('');
const lastName = ref('');
const birthDate = ref('');
const mrn = ref('');

onMounted(() => {
  orderingProvider.value = props.job.orderingProvider || '';
  orderingProviderEdit.value = props.job.orderingProvider || '';
  
  contact.value = props.job?.contact || '';
  contactEdit.value = props.job?.contact || '';

  facility.value = props.job.location?.facilityId || '';
  room.value = props.job.location?.roomId || '';
  isMounted.value = true;
});
watch(
  () => props.job,
  (newJob) => {
    orderingProvider.value = newJob.orderingProvider || '';
    contact.value = newJob?.contact || '';
    facility.value = newJob.location?.facilityId || '';
    room.value = newJob.location?.roomId || '';
    if (newJob.location?.facilityId) {
      setRoomOptions(newJob.location.facilityId);
    }
  }
);
//
// Job Details Panel
//

const isEditJobDetails = ref(false);
function handleEditIconJobDetails() {
  orderingProviderEdit.value = orderingProvider.value;
  contactEdit.value = contact.value;
  isEditJobDetails.value = true;

  toggleSaveSubmit();
}
function handleEditJobDetails() {
  orderingProvider.value = orderingProviderEdit.value;
  contact.value = contactEdit.value;
  isEditJobDetails.value = false;

  toggleSaveSubmit();
}

//
// Location Panel
//
onMounted(() => {
  const facilityId = props.job.location?.facilityId;
  if (facilityId) {
    setRoomOptions(facilityId);
  }
});
// Facility List
const facilityOptions = computed(() =>
  facilitiesStore.facilities
    .filter((f) => !f.archived)
    .map((p) => ({
      value: p.id,
      name: p.name ?? '',
    }))
);
function getFacilityNameById(id: string): string {
  const facility = facilitiesStore.facilities.find((f) => f.id === id);
  return facility?.name ?? '';
}

const rooms = ref<FacilityRoom[]>([]);
// Compute room options based on the fetched facility
const roomOptions = computed(() =>
  rooms.value.map((r: FacilityRoom) => ({
    value: r?.id || '',
    name: r?.name || '',
  }))
);
function getRoomNameById(id: string): string {
  const room = roomOptions.value.find((r) => r.value === id);
  return room ? room.name : '';
}

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
function createNewRoom(val: string) {
  roomEdit.value = val; // select created item
}

const schemaLocation = yup.object({
  facilityEdit: yup.string().required('facility is a required field'),
  roomEdit: yup.string().required('room is a required field'),
});
const { handleSubmit: handleSubmitLocation } = useForm({
  validationSchema: schemaLocation,
});
const isEditLocation = ref(false);
const { value: facilityEdit, errorMessage: facilityEditError } = useField<string>('facilityEdit');
const { value: roomEdit, errorMessage: roomEditError } = useField<string>('roomEdit');

function handleEditIconLocation() {
  const isFacilityArchived = facilitiesStore.facilities.find(
    (f) => f.id === facility.value
  )?.archived;
  if (isFacilityArchived) {
    facilityEdit.value = getFacilityNameById(facility.value);
  } else {
    facilityEdit.value = facility.value;
  }
  roomEdit.value = room.value;
  isEditLocation.value = true;
  
  toggleSaveSubmit();
}
const roomAutoComplete = ref<InstanceType<typeof AutoComplete>>();
function handleUpdateFacility(val: string) {
  roomAutoComplete.value?.clearInput();
  setRoomOptions(val);
}

function handleEditLocation() {
  handleSubmitLocation(async () => {
    getFacilityNameById(facilityEdit.value) === ''
      ? (facility.value = facility.value)
      : (facility.value = facilityEdit.value);
    room.value = roomEdit.value;
    isEditLocation.value = false;
    toggleSaveSubmit();
  })();
}

function handleCancleEditLocation() {
  setRoomOptions(facility.value);
  isEditLocation.value = false;
  
  toggleSaveSubmit();
}

//
// Patient info/ Medical Record Panel
//


// Define a type for the settings object
type FieldSettings = {
  required: boolean;
};

type Settings = Record<string, FieldSettings>;


const requiredData = computed(() => {
  if (!isMounted.value) return []; 
  const procedures = proceduresStore.procedures.filter(x=> procedureTabRef.value?.encounterProcedures.map(x=> x.procedureId).includes(x.id));
  const facility = facilitiesStore.facilities.find(x=> x.id == props.job.location?.facilityId);
  return procedures.flatMap(x=> x.requiredData).concat(facility!.requiredData);
})

/* Medical Record Schema */
function createMedicalRecordValidationSchema(requiredData: (RequiredPatientData | null)[]): yup.ObjectSchema<any> {
  const settings: Settings = {
    mrnEdit: { required: requiredData.includes(RequiredPatientData.Mrn) },
    firstNameEdit: { required: requiredData.includes(RequiredPatientData.FirstName)  },
    lastNameEdit: { required: requiredData.includes(RequiredPatientData.LastName)  },
    birthDateEdit: { required: requiredData.includes(RequiredPatientData.DateOfBirth)  },
  };

  const schemaFields = Object.entries(settings).reduce<Record<string, yup.StringSchema>>(
    (fields, [key, options]) => {
      let fieldSchema = yup.string();

      if (options.required) {
        fieldSchema = fieldSchema.required(`This is a required field`);
      }

      fields[key] = fieldSchema;
      return fields;
    },
    {}
  );

  return yup.object(schemaFields);
}

const medicalRecordSchema = computed(() => createMedicalRecordValidationSchema(requiredData.value));

const { handleSubmit: handleSubmitMedicalRecord } = useForm({
  validationSchema: medicalRecordSchema,
});
const { value: mrnEdit, errorMessage: mrnEditError } = useField<string>('mrnEdit');
const { value: firstNameEdit, errorMessage: firstNameEditError } =
  useField<string>('firstNameEdit');
const { value: lastNameEdit, errorMessage: lastNameEditError } = useField<string>('lastNameEdit');
const { value: birthDateEdit, errorMessage: birthDateEditError } =
  useField<string>('birthDateEdit');

const isEditMedicalRecord = ref(false);
const medicalRecordOptions = computed(() =>
  medicalRecordsStore.medicalRecords
    .filter((mr) => mr.facilityId === props.encounter?.location?.facilityId)
    .map((mr) => ({
      value: mr?.number ?? '',
      name: mr?.number ?? '',
    }))
);
function createNewMedicalRecord(val: string) {
  selectedMedicalRecordId.value = null;
  mrnEdit.value = val;
  firstNameEdit.value = '';
  lastNameEdit.value = '';
  birthDateEdit.value = '';
}
function setMedicalRecord(mrn: string) {
  selectedMedicalRecordId.value = getMedicalRecordByMRN(mrn)?.id ?? null;
  firstNameEdit.value = getMedicalRecordByMRN(mrn)?.firstName ?? '';
  lastNameEdit.value = getMedicalRecordByMRN(mrn)?.lastName ?? '';
  birthDateEdit.value = getMedicalRecordByMRN(mrn)?.birthday
    ? new Date(getMedicalRecordByMRN(mrn)?.birthday).toISOString().split('T')[0]
    : '';
}
function getMedicalRecordByMRN(mrn: string): any {
  return medicalRecordsStore.medicalRecords.find((mr) => mr.number === mrn);
}

function handleEditIconMedicalRecord() {
  mrnEdit.value = mrn.value;
  firstNameEdit.value = firstName.value;
  lastNameEdit.value = lastName.value;
  birthDateEdit.value = birthDate.value;

  isEditMedicalRecord.value = true;
  
  toggleSaveSubmit();
}
function handleEditMedicalRecord() {
  handleSubmitMedicalRecord(async () => {
    mrn.value = mrnEdit.value;
    firstName.value = firstNameEdit.value;
    lastName.value = lastNameEdit.value;
    birthDate.value = birthDateEdit.value;

    isEditMedicalRecord.value = false;
    toggleSaveSubmit();
  })();
}

/* Job Information Schema */
function createJobInfoValidationSchema(requiredData: (RequiredPatientData | null)[]): yup.ObjectSchema<any> {
  const settings: Settings = {
    orderingProviderEdit: { required: requiredData.includes(RequiredPatientData.OrderingProvider) },
    contactEdit: { required: false  }
  };

  const schemaFields = Object.entries(settings).reduce<Record<string, yup.StringSchema>>(
    (fields, [key, options]) => {
      let fieldSchema = yup.string();

      if (options.required) {
        fieldSchema = fieldSchema.required(`This is a required field`);
      }

      fields[key] = fieldSchema;
      return fields;
    },
    {}
  );

  return yup.object(schemaFields);
}
const jobInfoSchema = computed(() => createJobInfoValidationSchema(requiredData.value));

useForm({ validationSchema: jobInfoSchema });
const { value: orderingProviderEdit, errorMessage: orderingProviderEditError } = useField<string>('orderingProviderEdit');
const { value: contactEdit } =   useField<string>('contactEdit');


function validateRequiredData(){
  const formValues = {
    medicalRecord: {
      mrnEdit: mrnEdit.value,
      firstNameEdit: firstNameEdit.value,
      lastNameEdit: lastNameEdit.value,
      birthDateEdit: birthDateEdit.value,
    },
    jobInfo: {
      orderingProviderEdit: orderingProviderEdit.value,
      contactEdit: contactEdit.value,
    },
  };

  // Perform validation concurrently
  const isMedicalRecordValid = medicalRecordSchema?.value.isValidSync(formValues.medicalRecord);
  const isJobInfoValid = jobInfoSchema?.value.isValidSync(formValues.jobInfo);

  // Handle invalid cases
  if (!isMedicalRecordValid) handleEditIconMedicalRecord();
  if (!isJobInfoValid) handleEditIconJobDetails();

  return isMedicalRecordValid && isJobInfoValid;
}


async function attendToPatient() {
  await saveProgress();
  encountersStore.attendToPatient({ encounterId: props.encounter!.id });
}

async function submitProcedures(): Promise<boolean> {
  // Validate required data
  if (!validateRequiredData()) {
    return false;
  }

  // Save progress
  const isSaved = await saveProgress();
  if (!isSaved) {
    return false;
  }

  // Submit procedures
  await encountersStore.submitProcedures({ encounterId: props.encounter!.id });
  return true;
}

watch(
  medicalRecord,
  (newRecord) => {
    if (newRecord) {
      firstName.value = newRecord.firstName || '';
      lastName.value = newRecord.lastName || '';
      birthDate.value = newRecord.birthday
        ? new Date(newRecord.birthday).toISOString().split('T')[0]
        : '';
      mrn.value = newRecord.number || '';
      
      mrnEdit.value = mrn.value;
      firstNameEdit.value = firstName.value;
      lastNameEdit.value = lastName.value;
      birthDateEdit.value = birthDate.value;    
    }
  },
  { immediate: true }
);


const isDetailsDirty = computed(() => {
  return (
    (orderingProvider.value !== props.job.orderingProvider && orderingProvider.value !== '') ||
    (contact.value !== props.job.contact && contact.value !== '') ||
    facility.value !== props.job.location?.facilityId ||
    room.value !== props.job.location?.roomId ||
    (!!mrn.value && mrn.value !== medicalRecord.value?.number) ||
    (!!firstName.value && firstName.value !== medicalRecord.value?.firstName) ||
    (!!lastName.value && lastName.value !== medicalRecord.value?.lastName) ||
    (!!birthDate.value && birthDate.value !== medicalRecord.value?.birthday)
  );
});

const procedureTabRef = ref<InstanceType<typeof ProcedureTab>>();
async function saveProgress() {
  if (isDetailsDirty.value) {
    await enactEncounterRevision();
  }
  return await procedureTabRef.value?.saveProgress();
}

async function enactEncounterRevision() {
  if (props.encounter) {
    const encRevision: EnactEncounterRevisionPrc = {
      encounterId: props.encounter.id,
      jobId: props.job.id,
      contact: contact.value,
      orderingProvider: orderingProvider.value,
      location: {
        facilityId: facility.value,
        roomId: existingRoomId(room.value) ? room.value : null,
        roomName: existingRoomId(room.value) ? null : room.value,
      },
      medicalRecord: {
        id: selectedMedicalRecordId.value ?? null,
        number: mrn.value,
        firstName: firstName.value,
        lastName: lastName.value,
        birthday: birthDate.value,
      },
    };
    await encountersStore.enactEncounterRevision(encRevision);
  }
}

function existingRoomId(value: string): boolean {
  return rooms.value.some((room) => room.id === value);
}
function getUserForAvatar(id: string) {
  return usersStore.users.find((user) => user.id === id) as User;
}

const associatedLines = computed(() => {
  return linesStore.lines.filter((line) => props.encounter?.lines?.includes(line.id));
});

const is72Hours = computed(() => {
  if (props.encounter?.stage === EncounterStage.Completed) {
    const completedProgress = props.encounter?.progress?.find((pro) => pro?.stage === EncounterStage.Completed);
    if (completedProgress?.enteredAt) {
      const enteredAt = completedProgress.enteredAt;
      const now = dayjs();
      const entered = dayjs(enteredAt);
      const diffInHours = now.diff(entered, 'hours');
      return diffInHours > 72;
    }
  }
  return false;
});

const allowEdit = computed(() =>
    props.encounter?.stage !== EncounterStage.Waiting &&
    props.encounter?.stage !== EncounterStage.Charting &&
    props.job.status !== JobStatus.Canceled &&
    !is72Hours.value
);

const allowProceduresEdit = computed(() => {
  return props.encounter?.stage !== EncounterStage.Waiting && !is72Hours.value;
});

const getResponseTime = computed(() => {
  if (!props.encounter?.progress) return '0MIN';
  const relevantStages = props.encounter.progress.filter(
    (stage) => stage?.stage && [EncounterStage.Waiting, EncounterStage.Assigned].includes(stage.stage)
  );
  const totalMinutes = relevantStages.reduce((sum, stage) => {
    return sum + (stage?.duration ?? 0);
  }, 0);
  const hours = Math.floor(totalMinutes / 60);
  const minutes = totalMinutes % 60;
  if (hours > 0) {
    return `${hours}H:${minutes.toString().padStart(2, '0')}MIN`;
  }
  return `${minutes}MIN`;
});

const assignedEnteredAtTime = computed(() => {
  const assignedStage = props.encounter?.progress?.find((stage) => stage?.stage === EncounterStage.Assigned);
  if (assignedStage?.enteredAt) {
    return dayjs(assignedStage.enteredAt).format('HH:mm');
  }
  return null;
});

const completedEnteredAtTime = computed(() => {
  const completedStage = props.encounter?.progress?.find((stage) => stage?.stage === EncounterStage.Completed);

  if (completedStage?.enteredAt) {
    return dayjs(completedStage.enteredAt).format('HH:mm');
  }
  return null;
});

function setModalWidth(val: string) {
  emit('width', val);
}

function hasProcedures(val: boolean) {
  emit('has-procedures', val);
}

function isProceduresUnsaved(val: boolean) {
  emit('unsaved-procedures', val);
}


function onFollowUpClickHandler(){
  emit('onFollowUpClick', props.job.origin?.encounterId);
}

const followUpDetail = computed(() => {
  const { origin } = props.job || {};
  const encounterId = origin?.encounterId;
  const encounterProcedureId = origin?.encounterProcedureId;

  // Find the relevant encounter
  const encounter = encountersStore.encounters.find(e => e.id === encounterId);

  // Extract procedure details
  const procedureId = encounter?.procedures?.find(p => p?.id === encounterProcedureId)?.procedureId;
  const procedure = proceduresStore.procedures.find(p => p.id === procedureId);

  // Extract user details
  const userId = encounter?.assignments?.at(-1)?.clinicianId;
  const user = usersStore.users.find(u => u.id === userId);

  // Extract performedAt
  const performedAt = encounter?.procedures?.find(p => p?.id === encounterProcedureId)?.performAt;

  // Return the formatted details
  return {
    procedure: procedure?.name,
    clinician: user?.firstName + ' ' + user?.lastName,
    performedAt: formatDateByDMY(performedAt)
  };
});


function toggleSaveSubmit() {  
  const isEditEnabled = isEditMedicalRecord.value || isEditJobDetails.value || isEditLocation.value

  emit('disable-save-submit', isEditEnabled);
}

watch(isDetailsDirty, (newValue) => {
  emit('unsaved-details', newValue)
});

defineExpose({
  saveProgress,
  attendToPatient,
  submitProcedures
});
</script>
<template>
  <div class="flex h-auto lg:h-full flex-col lg:flex-row">
    <!-- Sidebar pannel -->
     <!-- loading -->
    <div
      v-if="isLoading && modalStore.isEncounterSidebarOpen"
      class="h-full overflow-y-auto custom-scroll lg:border-l border-slate-300 transition-all duration-300 ease-in-out"
    >
    <div v-for="i in 3" :key="i" class="flex flex-col gap-6 px-4 pt-4 pb-7  border-b border-slate-200 ">
      <div class="flex items-center gap-2">
      <IconChevronUp color="#64748B" class="shrink-0" />
      <SkeletonItem backgroundColor="#CBD5E1" class="w-56 h-4 rounded-3xl"/>
      </div> 
      <div class="flex items-center gap-2">
        <SkeletonItem  class="w-28 h-4 rounded-3xl"/>
        <SkeletonItem  class="w-28 h-4 rounded-3xl"/>
      </div>
      <div class="flex items-center gap-2">
        <SkeletonItem  class="w-36 h-4 rounded-3xl"/>
        <SkeletonItem  class="w-16 h-4 rounded-3xl"/>
      </div>
    </div>
    </div>

    <div
      v-else
      id="encounter-sidebar"
      :class="[
        'h-full overflow-y-auto custom-scroll lg:border-l border-slate-300 transition-all duration-300 ease-in-out',
        modalStore.isEncounterSidebarOpen 
          ? 'w-full lg:w-80 opacity-100' 
          : 'w-0 opacity-0 hidden lg:block'
      ]"
    >
      <!-- Follow Up -->
      <AccordionDefault v-if="job.origin" id="2" active class="p-4 border-b border-slate-300">
        <template #header>
          <AccordionHeader
            title="Follow Up"
            @edit="onFollowUpClickHandler"
            :isLink="true"
            caption="Redirect to Encounter"
          ></AccordionHeader>
        </template>
        <div class="flex flex-col gap-4 pt-2">
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Procedure</label>
            <div v-if="job.origin?.encounterProcedureId" class="text-sm font-semibold text-slate-900">
              {{ followUpDetail.procedure }}
            </div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Clinician</label>
            <div class="text-sm font-semibold text-slate-900">
              {{followUpDetail.clinician}}
            </div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Performed On</label>
            <div class="text-sm font-semibold text-slate-900">
              {{followUpDetail.performedAt}}
            </div>
          </div>
        </div>
      </AccordionDefault>
      <!-- Connected to line -->
      <AccordionDefault
        v-for="(line, index) in associatedLines"
        :id="`line-${index}`"
        title="Connected to a line"
        active
        class="p-4 border-b border-slate-300"
      >
        <div class="flex flex-col gap-4 pt-2">
          <div class="flex flex-col gap-2">
            <fwb-badge
              v-if="line.externallyPlaced"
              class="rounded-lg bg-yellow-100 text-yellow-700 mr-0 px-2.5 py-0.5 justify-start text-sm font-medium uppercase"
            >
              <template #icon>
                <IconArrowCircleBrokenDownRight height="14" class="mr-1" />
              </template>
              External placement
            </fwb-badge>
            <fwb-badge
              v-if="line.infectedOn"
              pill
              class="rounded-lg bg-radical-red-100 text-radical-red-700 mr-0 px-2.5 justify-start text-sm font-medium uppercase"
            >
              <template #icon>
                <IconAlertHexagon class="mr-1" />
              </template>
              Has Infection
            </fwb-badge>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Line</label>
            <div class="text-sm font-semibold text-slate-900">
              {{ line.name || '-' }}
            </div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Date was inserted</label>
            <div class="text-sm font-semibold text-slate-900">
              {{ line.insertedOn || '-' }}
            </div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Dwell time</label>
            <div class="text-sm font-semibold text-slate-900">
              {{ line.dwellTime || '-' }}
            </div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Inserted by</label>
            <div class="text-sm font-semibold text-slate-900">
              {{ line.externallyPlacedBy || '-' }}
            </div>
          </div>
        </div>
      </AccordionDefault>
      <!-- Encounters Details -->
      <AccordionDefault
        v-if="props.encounter?.id"
        id="1"
        title="Encounter Details"
        active
        class="p-4 border-b border-slate-300"
      >
        <div class="flex flex-col gap-4 pt-2">
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Assigned to </label>
            <!-- Assigned to -->
            <div v-if="props.encounter && props.encounter.assignments?.length" class="flex gap-1">
              <user-avatar
                v-if="props.encounter?.assignments"
                v-for="(assignment, index) in encounter?.assignments"
                :key="assignment?.clinicianId || index"
                :user="getUserForAvatar(assignment?.clinicianId)"
                rounded
                size="sm"
              />
            </div>
          </div>
          <div
            v-if="props.encounter.stage !== EncounterStage.Charting && props.encounter.stage !== EncounterStage.Completed"
            class="flex items-center gap-2"
          >
            <label class="w-2/5 text-xs font-medium text-slate-500">Priority</label>
            <StatusBadge v-if="props.encounter?.priority" :badge="encounter?.priority" />
          </div>

          <div v-if="props.encounter.stage === EncounterStage.Completed" class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Start - End</label>
            <div class="text-sm font-semibold text-slate-900">
              {{ assignedEnteredAtTime }} - {{ completedEnteredAtTime }}
            </div>
          </div>

          <div
            v-if="props.encounter.stage === EncounterStage.Charting || props.encounter.stage === EncounterStage.Completed"
            class="flex items-center gap-2"
          >
            <label class="w-2/5 text-xs font-medium text-slate-500">Response</label>
            <div class="text-sm font-semibold text-slate-900">
              {{ getResponseTime }}
            </div>
          </div>
        </div>
      </AccordionDefault>
      <!-- Location -->
      <AccordionDefault id="2" active class="p-4 border-b border-slate-300">
        <template #header>
          <AccordionHeader
            v-if="allowEdit"
            title="Location"
            @edit="handleEditIconLocation"
            :is-edit="isEditLocation"
          ></AccordionHeader>
          <span v-else class="text-base font-semibold">Location</span>
        </template>
        <div v-if="!isEditLocation" class="flex flex-col gap-4 pt-2">
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Facility</label>
            <div v-if="job.location?.facilityId" class="text-sm font-semibold text-slate-900">
              {{ getFacilityNameById(facility) }}
            </div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Room</label>
            <div v-if="job.location?.roomId" class="text-sm font-semibold text-slate-900">
              {{ getRoomNameById(room) || room }}
            </div>
          </div>
        </div>
        <div v-else class="flex flex-col gap-4 pt-2">
          <AutoComplete
            v-model="facilityEdit"
            :options="facilityOptions"
            label="Facility"
            @update:modelValue="handleUpdateFacility"
            :error="!!facilityEditError"
            :error_message="facilityEditError"
          ></AutoComplete>
          <AutoComplete
            ref="roomAutoComplete"
            v-model="roomEdit"
            :options="roomOptions"
            create_option
            @create="createNewRoom"
            label="Room"
            :error="!!roomEditError"
            :error_message="roomEditError"
          ></AutoComplete>
          <div class="flex gap-4 items-center justify-end">
            <fwb-button @click="handleCancleEditLocation" color="light" pill> Cancel </fwb-button>
            <fwb-button @click="handleEditLocation" class="bg-primary-600 hover:bg-brand-600" pill>
              Confirm
            </fwb-button>
          </div>
        </div>
      </AccordionDefault>
      <!-- Medical Record -->
      <AccordionDefault id="3" active class="p-4 border-b border-slate-300">
        <template #header>
          <AccordionHeader
            v-if="allowEdit"
            title="Medical Record"
            @edit="handleEditIconMedicalRecord"
            :is-edit="isEditMedicalRecord"
          ></AccordionHeader>
          <span v-else class="text-base font-semibold">Medical Record</span>
        </template>
        <div v-if="!isEditMedicalRecord" class="flex flex-col gap-4 pt-2">
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">MRN</label>
            <div class="text-sm font-semibold text-slate-900">{{ mrn }}</div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">First Name</label>
            <div class="text-sm font-semibold text-slate-900">{{ firstName }}</div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Last Name</label>
            <div class="text-sm font-semibold text-slate-900">{{ lastName }}</div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Birth Date</label>
            <div class="text-sm font-semibold text-slate-900">{{ birthDate }}</div>
          </div>
        </div>
        <div v-else class="flex flex-col gap-4 pt-2">
          <!-- <div>
            <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white">MRN</label>
            <fwb-input v-model="mrnEdit" placeholder="Write text here" />
          </div> -->
          <AutoComplete
            v-model="mrnEdit"
            :options="medicalRecordOptions"
            create_option
            @create="createNewMedicalRecord"
            @update:modelValue="setMedicalRecord"
            label="MRN"
            :error="!!mrnEditError"
            :error_message="mrnEditError"
          ></AutoComplete>
          <div>
            <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
              >First Name</label
            >
            <fwb-input
              v-model="firstNameEdit"
              placeholder="Write text here"
              :class="firstNameEditError ? inputErrorClasses : ''"
            />
            <span v-if="firstNameEditError" class="text-sm text-radical-red-600">{{
              firstNameEditError
            }}</span>
          </div>
          <div>
            <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
              >Last Name</label
            >
            <fwb-input
              v-model="lastNameEdit"
              placeholder="Write text here"
              :class="lastNameEditError ? inputErrorClasses : ''"
            />
            <span v-if="lastNameEditError" class="text-sm text-radical-red-600">{{
              lastNameEditError
            }}</span>
          </div>
          <div>
            <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
              >Birth Date</label
            >
            <fwb-input
              v-model="birthDateEdit"
              type="date"
              :max="new Date().toISOString().split('T')[0]"
              placeholder="Write text here"
              :class="birthDateEditError ? inputErrorClasses : ''"
            />
            <span v-if="birthDateEditError" class="text-sm text-radical-red-600">{{
              birthDateEditError
            }}</span>
          </div>
          <div class="flex gap-4 items-center justify-end">
            <fwb-button @click="() => { isEditMedicalRecord = false; toggleSaveSubmit(); }" color="light" pill>
              Cancel
            </fwb-button>
            <fwb-button
              @click="handleEditMedicalRecord"
              class="bg-primary-600 hover:bg-brand-600"
              pill
            >
              Confirm
            </fwb-button>
          </div>
        </div>
      </AccordionDefault>
      <!-- Job Details -->
      <AccordionDefault id="4" active class="p-4 border-b border-slate-300">
        <template #header>
          <AccordionHeader
            v-if="allowEdit"
            title="Job Information"
            @edit="handleEditIconJobDetails"
            :is-edit="isEditJobDetails"
          ></AccordionHeader>
          <span v-else class="text-base font-semibold">Job Information </span>
        </template>
        <div v-if="!isEditJobDetails" class="flex flex-col gap-4 pt-2">
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Ordering Provider</label>
            <div v-if="orderingProvider" class="text-sm font-semibold text-slate-900">
              {{ orderingProvider }}
            </div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-2/5 text-xs font-medium text-slate-500">Contact</label>
            <div v-if="contact" class="text-sm font-semibold text-slate-900">
              {{ contact }}
            </div>
          </div>
        </div>
        <div v-else class="flex flex-col gap-4 pt-2">
          <div>
            <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
              >Ordering Provider</label
            >
            <fwb-input v-model="orderingProviderEdit" placeholder="Write text here" :class="orderingProviderEditError ? inputErrorClasses : ''"/>
            <span v-if="orderingProviderEditError" class="text-sm text-radical-red-600">{{
              orderingProviderEditError
            }}</span>

          </div>
          <div>
            <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
              >Contact</label
            >
            <fwb-input v-model="contactEdit" placeholder="Write text here" />
          </div>
          <div class="flex gap-4 items-center justify-end">
            <fwb-button @click="() => { isEditJobDetails = false; toggleSaveSubmit(); }" color="light" pill> Cancel </fwb-button>
            <fwb-button
              @click="handleEditJobDetails"
              class="bg-primary-600 hover:bg-brand-600"
              pill
            >
              Confirm
            </fwb-button>
          </div>
        </div>
      </AccordionDefault>
      <!-- Updated/Created -->
      <div v-if="props.job?.createdAt" class="p-4 flex flex-col gap-2">
        <div class="text-xs font-medium text-slate-500">
          Updated:
          {{
            formatRelativeDate(props.job.modifiedAt ? props.job.modifiedAt : props.job.createdAt)
          }}
        </div>
        <div class="text-xs font-medium text-slate-500">
          Created: {{ formatRelativeDate(props.job.createdAt) }}
        </div>
      </div>
    </div>
    <!-- Tabs pannel -->
    <div
      class="h-full w-full lg:order-first lg:flex-1 overflow-y-auto custom-scroll"
    >
    <!-- loading -->
     <div v-if="isLoading" class="p-4 lg:p-6 flex flex-col gap-4 w-full">
      <div class=" flex sm:gap-8 gap-2 border-b border-slate-300 pb-4">
        <div v-for="i in 2" :key="i" class="flex items-center gap-2">
          <SkeletonItem backgroundColor="#F1F5F9" class="w-[22px] h-5  rounded"/>
          <SkeletonItem  class="sm:w-16 w-12 h-5  rounded-3xl"/>
          <SkeletonItem backgroundColor="#F1F5F9" class="w-[22px] h-5  rounded"/>
        </div>
      </div>
      <div class="border-b border-slate-300  pb-6">
      <SkeletonItem backgroundColor="#F8FAFC" class="w-full h-24  rounded-lg "/>
      </div>
      <div class="flex gap-4">
        <div><SkeletonItem backgroundColor="#CBD5E1" class="w-8 h-8  rounded-full "/></div>
        <div class="flex flex-col gap-9 flex-1">
          <SkeletonItem  class="w-20 h-4  rounded-3xl"/>
          <SkeletonItem backgroundColor="#CBD5E1" class="md:w-3/4 w-full h-4  rounded-3xl"/>
        </div>
      </div>
    </div>

      <div v-else class="p-4 lg:p-6">
        <Tabs v-if="props.encounter" :tabs="tabs" :active-tab="activeTab">
          <template #tab-0>
            <div class="py-4 lg:py-6">
              <ProcedureTab
                ref="procedureTabRef"
                :encounter="props.encounter"
                :is-review="props.isReview"
                :is-editable="allowProceduresEdit && allowEdit"
                @has-procedures="hasProcedures"
                @is-procedures-unsaved="isProceduresUnsaved"
              />
            </div>
          </template>
          <template #tab-1>
            <div class="py-4 lg:py-6">
              <NotesTab :job="props.job" :encounter="props.encounter" />
            </div>
          </template>
          <template #tab-2>
            <div class="py-4 lg:py-6">
              <PhotosTab @width="setModalWidth" :encounter="props.encounter" :job="props.job" />
            </div>
          </template>
          <template #tab-3>
            <div class="py-4 lg:py-6">
              <ObservationsTab
                :job="props.job"
                @width="setModalWidth"
                :encounter="props.encounter"
              />
            </div>
          </template>
        </Tabs>
        <Tabs v-else :tabs="tabs">
          <template #tab-0>
            <div class="py-4 lg:py-6">
              <NotesTab :job="props.job" />
            </div>
          </template>
        </Tabs>
      </div>

    </div>
    <!-- Activity Panel-->
    <div
      :class="[
        'h-full overflow-y-auto custom-scroll lg:border-l border-slate-300 transition-all duration-300 ease-in-out',
        modalStore.isEncounterActitityOpen 
          ? 'w-full lg:w-80 opacity-100' 
          : 'w-0 opacity-0 hidden lg:block'
      ]"
    >
    <!-- loading -->
     <div v-if="isLoading" class="p-4">
      <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-56 w-32 h-4 rounded-3xl"/>
      <div class="flex gap-2 mt-4">
        <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full"/>
        <div>
          <SkeletonItem  class="w-24 h-4 rounded-3xl"/>
          <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-60 w-40 h-4 rounded-3xl mt-2"/>
        </div>
      </div>
     </div>
      <!-- Activity Feed -->
      <div v-else class="p-4">
        <span class="text-base font-semibold">Activity</span>
        <div class="pl-2.5 mt-4">
          <ActivityFeed :aggregate-type="'Job'" :aggregate-id="props.job.id" :job="job" :encounter="encounter" />
        </div>
      </div>
    </div>
  </div>
</template>
