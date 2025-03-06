<script setup lang="ts">
import { Job, Line, FacilityRoom, EnactLineRevisionPrc,  SortEnumType,User } from '@/api/__generated__/graphql';
import { useModalStore } from '@/stores/modal';
import PageHeader from '@/components/header/PageHeader.vue';
import VueTailwindDatepicker from 'vue-tailwind-datepicker';
import {
  IconFilterLines,
  IconCalendar,
  IconAlert02,
  IconChevronUp,
  IconArrowNarrowUp,
  IconArrowNarrowDown,
  IconSearchOutline
} from '@/components/icons/index';
import { FwbButton, FwbInput } from 'flowbite-vue';
import { ref, onMounted, computed, watch } from 'vue';
import index from '@/views/common/activityFeed/linesFeed/index.vue';
import { formatDateByDMY } from '@/utils/dateUtils';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import AccordionHeader from '@/views/encounters/jobs/partials/AccordionHeader.vue';
import AutoComplete from '@/components/form/AutoComplete.vue';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useLinesStore } from '@/stores/data/encounters/lines';
import SkeletonItem from '@/components/skeletons/SkeletonItem.vue';
import { useLoaders } from '@/hooks/useLoaders';
import dayjs from 'dayjs';
import Modal from '@/components/modal/Modal.vue';
import {
  Dropdown,
  DropdownMenu,
  DropdownItem,
  DropdownDivider,
  DropdownText,
} from '@/components/dropdown/index';
import CustomCheckbox from '@/components/form/CustomCheckbox.vue';
import UserAvatar from '@/components/avatar/UserAvatar.vue';
import { useUsersStore } from '@/stores/data/settings/users';
import { useActivitiesStore } from '@/stores/data/common/activities';
import DateTimeFormatter, { DateTimeFormatMode } from '@/utils/dateTimeFormatter';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';

const props = defineProps<{
  line: Line;
  job?: Job;
  lineName: string;
  isLineNameDirty?:boolean;
}>();

const emit = defineEmits<{
  (e: 'width', val: string): void;
  (e: 'close'): void;
  (e: 'unsaved-details', val: boolean): void;
  (e: 'isEditModeMRLocation', val: boolean): void;
}>();

const { isLineLoading: isLoading } = useLoaders();
const modalStore = useModalStore();
const facilitiesStore = useFacilitiesStore();
const medicalRecordsStore = useMedicalRecordsStore();
const purposesStore = usePurposesStore();
const linesStore = useLinesStore();
const usersStore = useUsersStore();
const activitiesStore = useActivitiesStore();
const facilityTypesStore = useFacilityTypesStore();
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

const facility = ref('');
const room = ref('');

const firstName = ref('');
const lastName = ref('');
const birthDate = ref('');
const mrn = ref('');

const medicalRecord = computed(() =>
  medicalRecordsStore.medicalRecords.find((mr) => mr.id === props.line.medicalRecordId)
);

const selectedMedicalRecordId = ref(props.line.medicalRecordId || null);

onMounted(() => {
  facility.value = props.line.location?.facilityId || '';
  room.value = props.line.location?.roomId || '';
});
watch(
  () => props.line,
  (newLine) => {
    facility.value = newLine.location?.facilityId || '';
    room.value = newLine.location?.roomId || '';
    if (newLine.location?.facilityId) {
      setRoomOptions(newLine.location.facilityId);
    }
    selectedMedicalRecordId.value = newLine.medicalRecordId || null;
  }
);
watch(
  medicalRecord,
  (newRecord) => {
    if (!newRecord || newRecord == undefined) {
      firstName.value = '';
      lastName.value = '';
      birthDate.value = '';
      mrn.value = '';
    }
    if (newRecord) {
      firstName.value = newRecord.firstName || '';
      lastName.value = newRecord.lastName || '';
      birthDate.value = newRecord.birthday
        ? new Date(newRecord.birthday).toISOString().split('T')[0]
        : '';
      mrn.value = newRecord.number || '';
    }
  },
  { immediate: true }
);

const getPurposeName = (purposeId: any) => {
  if (!purposeId) {
    return '';
  }
  const purpose = purposesStore.purposes.find((p) => p.id === purposeId);
  return purpose ? purpose.name : '';
};


// Left Panel

// Sort Asc Desc
const activitySort = ref(SortEnumType.Desc);
const toggleSortAscDecscOrder = () => {
  activitySort.value =
  activitySort.value === SortEnumType.Asc ? SortEnumType.Desc : SortEnumType.Asc;
};

// Filters

const filterModalRef = ref<InstanceType<typeof Modal>>();
const userSearchQuery = ref("");
const selectedUsersFilter = ref<string[]>([]);
const appliedUsers = ref<string[]>([]);
const filterBtnRef = ref<any>(null);

// Extracts users performed an activity
const activityUserIds = new Set(activitiesStore.activities.map(activity => activity.userId));
const activityUsers = usersStore.users.filter(user => activityUserIds.has(user.id));

const filteredUsers = computed(() => {
  const trimmedQuery = userSearchQuery.value.trim().toLowerCase();
  if (!trimmedQuery) {
    return activityUsers;
  }
  return activityUsers.filter((user) => {
    const fullName = `${user.firstName} ${user.lastName}`.toLowerCase().trim();
    return fullName.includes(trimmedQuery);   
  });
});

const toggleUserSelection = (userId: string) => {
  const index = selectedUsersFilter.value.indexOf(userId);
  if (index > -1) {
    selectedUsersFilter.value.splice(index, 1); 
  } else {
    selectedUsersFilter.value.push(userId); 
  }
};

const badgeFiltersCount = computed(() => {
  return (
    appliedUsers.value.length
  );
});

const getUserForAvatar=(id: string)=> {
  return usersStore.users.find((user) => user.id === id) as User;
}

const applyFilters = () => {
  appliedUsers.value = [...selectedUsersFilter.value]; 
  filterModalRef.value?.setModalOpen(false);
  if (filterBtnRef.value) {
    filterBtnRef.value.$el?.click();
  }
};

const cancelFilters=()=>{
  userSearchQuery.value='';
  selectedUsersFilter.value=[];
  appliedUsers.value = [];
  filterModalRef.value?.setModalOpen(false);
  if (filterBtnRef.value) {
    filterBtnRef.value.$el?.click();
  }
}

// Date Picker
const modalDateRangeRef = ref<InstanceType<typeof Modal>>();
  const dateValue = ref("");
  const formatter = ref({
    date: 'DD MMM YYYY',
    month: 'MMM',
  });
  const today = dayjs();
  const last7Days = dayjs().subtract(6, 'day'); 

const getDateValue = (value: string) => {
  if (!value) return "Select Date";
  const [startDate, endDate] = value.split(' - ');
  if (startDate === endDate && startDate === dayjs(today).format('DD MMM YYYY')) {
    return 'Today';
  }
  if (
    startDate === last7Days.format('DD MMM YYYY') &&
    endDate === today.format('DD MMM YYYY')
  ) {
    return 'Last 7 Days';
  }
  return startDate === endDate ? startDate : value;
};

// Right Panel

// Location

onMounted(() => {
  const facilityId = props.line.location?.facilityId;
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
  })();
}

function handleCancleEditLocation() {
  setRoomOptions(facility.value);
  isEditLocation.value = false;
}
// Get Facility Type 
const getFacilityTypeById = (id: string) => {
  const facility = facilitiesStore.facilities.find(fac => fac.id === id);
  if (!facility) return null; 
  return facilityTypesStore.facilityTypes.find(t => t.id === facility.typeId) || null;
};
// Medical Record

const schemaMedicalRecord = yup.object({
  mrnEdit: yup.string().required('mrn is a required field'),
  firstNameEdit: yup.string().required('This is a required field'),
  lastNameEdit: yup.string().required('This is a required field'),
  birthDateEdit: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitMedicalRecord } = useForm({
  validationSchema: schemaMedicalRecord,
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
    .filter((mr) => mr.facilityId === props.line?.location?.facilityId)
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
}
function handleEditMedicalRecord() {
  handleSubmitMedicalRecord(async () => {
    mrn.value = mrnEdit.value;
    firstName.value = firstNameEdit.value;
    lastName.value = lastNameEdit.value;
    birthDate.value = birthDateEdit.value;
    isEditMedicalRecord.value = false;
  })();
}

const isDetailsDirty = computed(() => {
  return (
    facility.value !== props.line.location?.facilityId ||
    room.value !== props.line.location?.roomId ||
    (!!mrn.value && mrn.value !== medicalRecord.value?.number) ||
    (!!firstName.value && firstName.value.trim() !== (medicalRecord.value?.firstName || "").trim()) ||
    (!!lastName.value && lastName.value.trim() !== (medicalRecord.value?.lastName || "").trim()) ||
    (!!birthDate.value && birthDate.value !== medicalRecord.value?.birthday)
  );
});
watch(isDetailsDirty, (newValue) => {
  emit('unsaved-details', newValue);
});

const saveLinesRevision = () => {
  const medicalRecord = {
      birthday: birthDate.value? birthDate.value : null,
      firstName: firstName.value? firstName.value : null,
      id: selectedMedicalRecordId.value ? selectedMedicalRecordId.value : null,
      lastName: lastName.value? lastName.value : null,
      number: mrn.value? mrn.value : null,
  };
  const hasMedicalRecordData = Object.values(medicalRecord).some(val => val !== null);
  const lineRevision: EnactLineRevisionPrc = {
    id: props.line.id,
    location: {
      facilityId: facility.value,
      roomId: room.value,
      roomName: getRoomNameById(room.value),
    },
    medicalRecord: hasMedicalRecordData ? medicalRecord : null,
    name: props.lineName,
  };
  if(isDetailsDirty.value || props.isLineNameDirty){
    linesStore.enactListRevision(lineRevision);
  }
};

// Edit Mode
watch([isEditMedicalRecord, isEditLocation], () => {
  const shouldEmitTrue = isEditMedicalRecord.value || isEditLocation.value;
  emit('isEditModeMRLocation', shouldEmitTrue);
});

defineExpose({
  saveLinesRevision,
});
</script>
<template>
  <div class="flex h-auto lg:h-full flex-col lg:flex-row">
    <!-- Left Pannel -->
    <div
      class="h-full w-full lg:order-first order-last lg:flex-1 lg:border-r border-slate-300 overflow-y-auto custom-scroll pb-4 overflow-x-hidden"
    >
      <!-- Filters -->
        <PageHeader class="lg:border-t-0 lg:bottom-auto bottom-[70px] min-h-16 flex">
          <!-- loading -->
           <div v-if="isLoading" class="flex items-center gap-2 py-1 overflow-x-auto lg:overflow-x-visible">
              <SkeletonItem backgroundColor="#F1F5F9" class="sm:w-28 w-20 sm:h-8 h-5 rounded-3xl"/>
              <SkeletonItem backgroundColor="#F1F5F9" class="sm:w-28 w-20 sm:h-8 h-5 rounded-3xl"/>
              <SkeletonItem backgroundColor="#F1F5F9" class="sm:w-28 w-20 sm:h-8 h-5 rounded-3xl"/>
           </div>
          <div v-else class="flex gap-4 items-center py-1 overflow-x-auto lg:overflow-x-visible">
            <!-- Filter For desktop -->
            <Dropdown class="lg:block hidden">
              <template #trigger>
              <fwb-button
              ref="filterBtnRef"
              pill
              square
              :class="
              [
              'font-semibold border-transparent px-3 ms-1',
              badgeFiltersCount
              ? 'bg-brand-100 text-brand-600 hover:bg-brand-100'
              : 'bg-brand-100 text-brand-600 hover:bg-brand-100 font-semibold',
              ].join(' ')
              "
              >
              <template #prefix>
              <IconFilterLines />
              </template>
              Filter
              <template v-if="badgeFiltersCount" #suffix>
                <div
                class="text-white bg-brand-600 rounded-full text-[12px] leading-[13px] font-medium w-[26px] h-[26px] flex items-center justify-center"
                >
                {{ badgeFiltersCount }}
                </div>
              </template>
              </fwb-button>  
              </template>
              <DropdownMenu class="w-80">
                <div class="p-2"> 
                <fwb-input v-model="userSearchQuery"  placeholder="Search for" class="w-full">
                <template #prefix>
                  <IconSearchOutline color="#475569"/>
                </template>
                </fwb-input>
              </div>
                <DropdownText class="py-2 mb-1 border-y border-slate-200 bg-slate-50 text-slate-500"> 
                  Users
                </DropdownText>
              <div class="max-h-60 min-h-36 overflow-y-auto custom-scroll px-1">
                <DropdownItem  v-for="user in filteredUsers" :key="user.id" @click="toggleUserSelection(user.id)" class="flex gap-2 items-center" >
                  <CustomCheckbox
                  :value="user.id"
                  v-model="selectedUsersFilter"
                  label=""
                  />
                  <div class="flex gap-2 items-center">
                    <user-avatar v-if="getUserForAvatar(user.id)" :user="getUserForAvatar(user.id)" rounded size="sm" />
                    <div class="font-medium text-sm text-slate-700">{{ user.firstName }} {{ user.lastName }}</div>
                  </div>
                </DropdownItem>
                <div v-if="filteredUsers.length === 0" class="text-center py-4">No results found</div>
              </div>
                <DropdownDivider />
                <DropdownText class="justify-end py-2 flex gap-2">
                  <fwb-button  @click="cancelFilters"  class="bg-white hover:bg-slate-50 text-slate-900 border border-transparent hover:border-slate-200" pill>Cancel </fwb-button>
                  <fwb-button
                    @click="applyFilters"
                    class="bg-brand-600 hover:bg-brand-700"
                    pill
                    >
                    Apply filters
                  </fwb-button>
                </DropdownText>
              </DropdownMenu>
            </Dropdown>
            <!-- Filter For mobile -->
            <fwb-button
              @click="filterModalRef?.setModalOpen(true)"
              pill
              square
              :class="
              [
              'font-semibold border-transparent px-3 ms-1 lg:hidden',
              badgeFiltersCount
              ? 'bg-brand-100 text-brand-600 hover:bg-brand-100'
              : 'bg-brand-100 text-brand-600 hover:bg-brand-100 font-semibold',
              ].join(' ')
              "
              >
              <template #prefix>
              <IconFilterLines />
              </template>
              Filter
              <template v-if="badgeFiltersCount" #suffix>
                <div
                class="text-white bg-brand-600 rounded-full text-[12px] leading-[13px] font-medium w-[26px] h-[26px] flex items-center justify-center"
                >
                {{ badgeFiltersCount }}
                </div>
              </template>
            </fwb-button>
            <!-- Sort -->
            <fwb-button
              @click="toggleSortAscDecscOrder"
              pill
              class="bg-brand-100 text-brand-600 hover:bg-brand-100 font-semibold"
              >
                <template #prefix>
                <template v-if="activitySort === SortEnumType.Desc">
                <IconArrowNarrowUp />
                </template>
                <template v-else>
                <IconArrowNarrowDown />
                </template>
                </template>
                <span class="whitespace-nowrap"> Sort: Date </span>
             </fwb-button>
            <!-- Date -->
            <vue-tailwind-datepicker
              v-model="dateValue"
              v-slot="{ value, placeholder }"
              :formatter="formatter"
              separator=" - "
              placeholder="Select Date"
              class="z-[99] lines-date-picker hidden lg:block"
            >
              <fwb-button
                pill
                class="bg-brand-100 text-brand-600 hover:bg-brand-100 font-semibold mr-1"
              >
                <template #prefix>
                  <IconCalendar />
                </template>
                <span class="whitespace-nowrap">
                  {{ getDateValue(value) || placeholder }}
                </span>
              </fwb-button>
            </vue-tailwind-datepicker>
               <!-- Date for Mobile -->
              <fwb-button
                pill
                class="lg:hidden bg-brand-100 text-brand-600 hover:bg-brand-100 font-semibold mr-1"
                @click="modalDateRangeRef?.setModalOpen(true)"
              >
                <template #prefix>
                  <IconCalendar />
                </template>
                <span class="whitespace-nowrap">
                  {{ getDateValue(dateValue)}}
                </span>
              </fwb-button>
          </div>
        </PageHeader>
      <!-- Feeds -->
       <!-- loading -->
       <div v-if="isLoading" class="p-4">
        <div class="flex gap-4 items-center border-b border-slate-300 pb-4 mb-4">
          <div> 
            <SkeletonItem backgroundColor="#CBD5E1" class="w-9 h-9 rounded-full"/>
          </div>
          <div class="flex flex-col gap-2 "> 
            <SkeletonItem  class="w-20 h-4 rounded-3xl"/>
            <div class="flex gap-2">
              <SkeletonItem backgroundColor="#CBD5E1" class="md:w-64 sm:w-52 w-20 h-4 rounded-3xl"/>
              <SkeletonItem backgroundColor="#CBD5E1" class="w-12 h-4 rounded-3xl"/>
              <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-32 w-10 h-4 rounded-3xl"/>
            </div>
          </div>
        </div>
        <div class="flex gap-4 items-center border-b border-slate-300 pb-4 mb-4">
          <div> 
            <SkeletonItem backgroundColor="#CBD5E1" class="w-9 h-9 rounded-full"/>
          </div>
          <div class="flex flex-col gap-2 "> 
            <SkeletonItem  class="sm:w-36 w-16 h-4 rounded-3xl"/>
            <div class="flex gap-2">
              <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-36 w-16 h-4 rounded-3xl"/>
              <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-32 w-16 h-4 rounded-3xl"/>
              <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-14 w-10 h-4 rounded-3xl"/>
            </div>
          </div>
        </div>
        <div class="flex gap-4 items-center border-b border-slate-300 pb-4 mb-4">
          <div> 
            <SkeletonItem backgroundColor="#CBD5E1" class="w-9 h-9 rounded-full"/>
          </div>
          <div class="flex flex-col gap-2 "> 
            <SkeletonItem  class="sm:w-48 w-28 h-4 rounded-3xl"/>
            <div class="flex gap-2">
              <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-36 w-16 h-4 rounded-3xl"/>
              <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-32 w-16 h-4 rounded-3xl"/>
              <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-14 w-10 h-4 rounded-3xl"/>
            </div>
          </div>
        </div>
        <div class="flex gap-4 items-center border-b border-slate-300 pb-4 mb-4">
          <div> 
            <SkeletonItem backgroundColor="#CBD5E1" class="w-9 h-9 rounded-full"/>
          </div>
          <div class="flex flex-col gap-2 "> 
            <SkeletonItem  class="sm:w-36 w-24 h-4 rounded-3xl"/>
            <div class="flex gap-2">
              <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-48 w-16 h-4 rounded-3xl"/>
              <SkeletonItem backgroundColor="#CBD5E1" class="sm:w-14 w-10 h-4 rounded-3xl"/>
            </div>
          </div>
        </div>
      </div>

      <div v-else class="px-4">
        <index :line="props.line" :activitySort="activitySort" :dateRange="dateValue" :filteredUsers="appliedUsers" />
      </div>
    </div>
    <!-- Right Panel  -->
     <!-- loading -->
    <div
      v-if="isLoading && modalStore.isLineSidebarOpen"
      class="h-full w-full lg:order-last lg:w-80 overflow-y-auto custom-scroll border-b border-slate-300 lg:border-b-0 "
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

  <div v-else>
    <div
      v-if="modalStore.isLineSidebarOpen "
      class="h-full w-full lg:order-last lg:w-80 overflow-y-auto custom-scroll border-b border-slate-300 lg:border-b-0"
    >
      <!-- Has infection -->
      <AccordionDefault
        v-if="props.line.infectedOn"
        id="1"
        active
        custom_header
        class="p-4 border-b border-slate-200 bg-rose-50"
      >
        <template #customHeader="{ open }">
          <div class="flex items-center justify-between w-full gap-2">
            <div class="flex gap-2">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
                fill="none"
              >
                <path
                  class="transform origin-center transition duration-200 ease-out"
                  :class="{ 'rotate-0': open, 'rotate-180': !open }"
                  d="M18 15L12 9L6 15"
                  stroke="#64748B"
                  stroke-width="1.5"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                />
              </svg>
              <span class="font-semibold text-base text-radical-red-700">Has infection</span>
            </div>
            <div class="text-radical-red-700"><IconAlert02 /></div>
          </div>
        </template>

        <div class="flex flex-col gap-4 pt-2">
          <div class="flex items-end gap-2">
            <label class="w-1/2 text-sm font-medium text-slate-500">Date Infected</label>
            <div class="text-sm font-medium text-slate-900">
              {{ DateTimeFormatter.formatDatetime(props.line.infectedOn, DateTimeFormatMode.Limited)}}
            </div>
          </div>
        </div>
      </AccordionDefault>
      <!-- Attendance -->
      <AccordionDefault id="1" title="Attendance" active class="p-4 border-b border-slate-300">
        <div class="flex flex-col gap-4 pt-2">
          <div class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Dwell time</label>
            <div class="text-sm font-medium text-slate-900">{{ props.line?.dwellTime }} days</div>
          </div>

          <div  class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Inserted by</label>
            <div class="text-sm font-medium text-slate-900 flex items-center gap-2">
              <span class="truncate text-ellipsis w-24"> {{ props.line?.externallyPlacedBy }}</span>
            </div>
          </div>

          <div class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Insertion Date</label>
            <div class="text-sm font-medium text-slate-900">
              {{ DateTimeFormatter.formatDatetime(props.line?.insertedOn, DateTimeFormatMode.Limited) }}
            </div>
          </div>

          <div  class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Removed by</label>
            <div class="text-sm font-medium text-slate-900 flex items-center gap-2">
              <!-- <span class="truncate text-ellipsis w-24"> Jese </span> -->
            </div>
          </div>

          <div  class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Removal Date</label>
            <div class="text-sm font-medium text-slate-900">
              {{ formatDateByDMY(props.line?.removedOn) }}
            </div>
          </div>

          <div v-if="props.line?.followUp" class="flex flex-col gap-4 rounded-lg bg-cyan-50 p-2">
            <div class="flex items-center gap-2">
              <label class="w-1/2 text-xs font-medium text-slate-500">Follow Up</label>
              <div class="text-sm font-medium text-slate-900">
                {{ formatDateByDMY(props.line.followUp?.date) }}
              </div>
            </div>
            <div class="flex items-center gap-2">
              <label class="w-1/2 text-xs font-medium text-slate-500">Follow Up Purpose</label>
              <div class="text-sm font-medium text-slate-900 truncate text-ellipsis w-24">
                {{ getPurposeName(props.line.followUp?.purposeId) }}
              </div>
            </div>
          </div>
        </div>
      </AccordionDefault>
      <!-- Location -->
      <AccordionDefault id="2" active class="p-4 border-b border-slate-300">
        <template #header>
          <AccordionHeader
            title="Location"
            @edit="handleEditIconLocation"
            :is-edit="isEditLocation"
          ></AccordionHeader>
        </template>
        <div v-if="!isEditLocation" class="flex flex-col gap-4 pt-2">
          <div class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Facility</label>
            <div v-if="line.location?.facilityId" class="text-sm font-medium text-slate-900">
              {{ getFacilityNameById(facility) }}
            </div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Room</label>
            <div v-if="line.location?.roomId" class="text-sm font-medium text-slate-900">
              {{ getRoomNameById(room) || room }}
            </div>
          </div>
            <!-- Show more -->
            <div v-if="getFacilityTypeById(facility)?.properties?.length">
            <AccordionDefault id="show-more" custom_header>
            <template #customHeader="{ open }">
                <div class="flex gap-2  w-full items-center text-brand-600 font-medium text-sm text-nowrap">
                <div>
                <span v-if="!open">See more</span> 
                <span v-else>See less</span> 
                </div>
                <svg
                xmlns="http://www.w3.org/2000/svg"
                width="20"
                height="20"
                viewBox="0 0 24 24"
                fill="none"
                class="shrink-0"
                >
                <path
                :class="`transform origin-center transition duration-200 ease-out ${open ? 'rotate-0' : 'rotate-180'}`"
                d="M18 15L12 9L6 15"
                stroke="#6A5ACD"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
                />
                </svg>
                <hr class="w-full h-[1px] border-slate-300">
                </div>
              </template>
              <div class="flex flex-col gap-4 pt-2">
                <div class="flex items-center gap-2">
                <label class="w-2/5 text-xs font-medium text-slate-500">Type</label>
                <div class="text-sm font-semibold text-slate-900">
                <span
                class="text-xs bg-[#DEF7EC] text-[#057A55] px-[10px] py-[2px] leading-[18px] font-medium rounded-full text-nowrap"
                >
                {{ getFacilityTypeById(facility)?.name }}
                </span>
                </div>
                </div>
                <!-- properties -->
                <div class="flex items-center gap-2">
                <label class="w-2/5 text-sm font-semibold text-slate-900">Properties</label>
                <div class="text-sm font-semibold text-slate-900">
                Values
                </div>
                </div>
                <div v-for="property in getFacilityTypeById(facility)?.properties" :key="property?.id" class="flex items-center gap-2">
                <label class="w-2/5 text-xs font-medium text-slate-500">{{ property?.name }}</label>
                <div class="text-sm font-semibold text-slate-900">
                {{ property?.options?.length }}
                </div>
                </div>
              </div>
            </AccordionDefault>
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
            title="Medical Record"
            @edit="handleEditIconMedicalRecord"
            :is-edit="isEditMedicalRecord"
          ></AccordionHeader>
        </template>
        <div v-if="!isEditMedicalRecord" class="flex flex-col gap-4 pt-2">
          <div class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Number</label>
            <div class="text-sm font-medium text-slate-900">{{ mrn }}</div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Name</label>
            <div class="text-sm font-medium text-slate-900">
              {{ firstName }} &nbsp;{{ lastName }}
            </div>
          </div>
          <div class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Date of Birth</label>
            <div class="text-sm font-medium text-slate-900">{{ formatDateByDMY(birthDate) }}</div>
          </div>
        </div>
        <div v-else class="flex flex-col gap-4 pt-2">
          <AutoComplete
            v-model="mrnEdit"
            :options="medicalRecordOptions"
            create_option
            @create="createNewMedicalRecord"
            @update:modelValue="setMedicalRecord"
            label="Number"
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
              >Date of Birth</label
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
            <fwb-button @click="isEditMedicalRecord = false" color="light" pill>
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
      <!-- Updated/Created -->
      <div class="p-4 flex flex-col gap-2">
        <div class="text-xs font-medium text-slate-500">
          Updated:
          {{
            DateTimeFormatter.formatDatetime(props.line.modifiedAt ? props.line.modifiedAt : props.line.createdAt)
          }}
        </div>
        <div class="text-xs font-medium text-slate-500">
          Created: {{ DateTimeFormatter.formatDatetime(props.line.createdAt) }}
        </div>
      </div>
    </div>
  </div>
    <!-- Date range modal for mobile -->
    <Modal ref="modalDateRangeRef" title="Select Date Range" no_footer :z_index="55">
      <template #body>
        <div class="h-full overflow-y-auto max-h-[calc(100vh-200px)]">
          <vue-tailwind-datepicker
            v-model="dateValue"
            v-slot="{ value, placeholder }"
            :formatter="formatter"
            separator=" - "
            placeholder="Select Date"
            no-input
            class="z-[99]"
            @update:model-value="modalDateRangeRef?.setModalOpen(false)"
          >
            <fwb-button
              pill
              class="bg-brand-100 text-brand-600 hover:bg-brand-100 font-semibold mr-1"
            >
              <template #prefix>
                <IconCalendar />
              </template>
              <span class="whitespace
              -nowrap">
                {{ getDateValue(value) || placeholder }}  
              </span>
            </fwb-button>
          </vue-tailwind-datepicker>
        </div>
      </template>
    </Modal>
     <!-- Filter modal for mobile -->
      <teleport to="body">
      <Modal ref="filterModalRef" title="Filter" :z_index="55">
         <template #body>
          <div class="p-2">
              <fwb-input v-model="userSearchQuery"   placeholder="Search for" class="w-full p-2">
                <template #prefix>
                  <IconSearchOutline color="#475569"/>
                </template>
              </fwb-input>
            </div>
              <div class="p-2 mb-1 border-y border-slate-200 bg-slate-50 text-slate-500"> 
                Users
              </div>
              <div class="p-1 max-h-60 min-h-36 overflow-y-auto custom-scroll px-2">
              <div  v-for="user in filteredUsers" :key="user.id" @click="toggleUserSelection(user.id)" class="p-2 flex gap-2 items-center hover:bg-[#F0F2FD] rounded-md" >
                <CustomCheckbox
                  :value="user.id"
                  v-model="selectedUsersFilter"
                  label=""
                  />
                <div class="flex gap-2 items-center">
                  <user-avatar v-if="getUserForAvatar(user.id)" :user="getUserForAvatar(user.id)" rounded size="sm" />
                  <div class="font-medium text-sm text-slate-700">{{ user.firstName }} {{ user.lastName }}</div>
                </div>
              </div>
              <div v-if="filteredUsers.length === 0" class="text-center py-4">No results found</div>
            </div>
          </template>
        <template #footer>
          <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button  @click="cancelFilters"  class="bg-white hover:bg-slate-50 text-slate-900 border border-transparent hover:border-slate-200" pill>Cancel </fwb-button>
            <fwb-button
              @click="applyFilters"
              class="bg-brand-600 hover:bg-brand-700"
              pill
            >
            Apply filters
            </fwb-button>
          </div>
        </template>
      </Modal>
      </teleport>
  </div>
</template>

<style scoped>
/* Date picker */
:deep(.lines-date-picker #headlessui-popover-panel-3 div.absolute.z-50) {
  position: fixed !important;
  top: auto;
  left: unset;
}
@media screen and (max-width: 1215px) {
  :deep(.lines-date-picker #headlessui-popover-panel-3 div.absolute.z-50) {
    left: 80px;
  }
  :deep(.lines-date-picker .vtd-datepicker:before) {
    opacity: 0;
  }
}
</style>

