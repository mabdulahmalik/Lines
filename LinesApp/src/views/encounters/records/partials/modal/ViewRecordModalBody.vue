<script setup lang="ts">
import { MedicalRecord, SortEnumType,User } from '@/api/__generated__/graphql';
import { useModalStore } from '@/stores/modal';
import VueTailwindDatepicker from 'vue-tailwind-datepicker';
import { IconFilterLines, IconCalendar,  IconChevronUp, IconArrowNarrowUp,
IconArrowNarrowDown, IconSearchOutline, } from '@/components/icons/index';
import { FwbButton, FwbInput } from 'flowbite-vue';
import { ref, computed, watch } from 'vue';
import { formatRelativeDate, formatDateByDMY } from '@/utils/dateUtils';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import AccordionHeader from '@/views/encounters/jobs/partials/AccordionHeader.vue';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import Observations from '../observations/index.vue';
import PageHeader from '@/components/header/PageHeader.vue';
import index from '@/views/common/activityFeed/linesFeed/index.vue';
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


const props = defineProps<{
  record: MedicalRecord;
}>();

const emit = defineEmits<{
  (e: 'width', val: string): void;
  (e: 'close'): void;
  (e: 'isEditModeMedicalRecord', val: boolean): void;
}>();
const { isRecordLoading: isLoading } = useLoaders();

const modalStore = useModalStore();
const medicalRecordsStore = useMedicalRecordsStore();
const usersStore = useUsersStore();
const activitiesStore = useActivitiesStore();
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

const medicalRecord = computed(() =>
  medicalRecordsStore.medicalRecords.find((mr) => mr.id === props.record?.id)
);
const selectedMedicalRecordId = ref(props.record?.id || null);

const firstName = ref('');
const lastName = ref('');
const birthDate = ref('');
const mrn = ref('');

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
      selectedMedicalRecordId.value = newRecord.id || null;
    }
  },
  { immediate: true }
);

// Left Panel

// Sort Asc Desc
const activitySort = ref(SortEnumType.Desc);
const toggleSortAscDecscOrder = () => {
  activitySort.value =
  activitySort.value === SortEnumType.Asc ? SortEnumType.Desc : SortEnumType.Asc;
};

// Accordions
const activeAccordionStep = ref(1);
const setActiveAccordionStep = (step: number) => {
  activeAccordionStep.value = step;
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

// Medical Record
const schemaMedicalRecord = yup.object({
  firstNameEdit: yup.string().required('This is a required field'),
  lastNameEdit: yup.string().required('This is a required field'),
  birthDateEdit: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitMedicalRecord } = useForm({
  validationSchema: schemaMedicalRecord,
});
const { value: firstNameEdit, errorMessage: firstNameEditError } =
  useField<string>('firstNameEdit');
const { value: lastNameEdit, errorMessage: lastNameEditError } = useField<string>('lastNameEdit');
const { value: birthDateEdit, errorMessage: birthDateEditError } =
  useField<string>('birthDateEdit');

const isEditMedicalRecord = ref(false);
watch(isEditMedicalRecord, (newValue) => {
  emit('isEditModeMedicalRecord', newValue);
});

function handleEditIconMedicalRecord() {
  firstNameEdit.value = firstName.value;
  lastNameEdit.value = lastName.value;
  birthDateEdit.value = birthDate.value;

  isEditMedicalRecord.value = true;
}
function handleEditMedicalRecord() {
  handleSubmitMedicalRecord(async () => {
    firstName.value = firstNameEdit.value;
    lastName.value = lastNameEdit.value;
    birthDate.value = birthDateEdit.value;

    isEditMedicalRecord.value = false;
  })();
}

// Modify record
const modifyMedicalRecord = () => {
  medicalRecordsStore.modifyMedicalRecord({
    id: selectedMedicalRecordId.value,
    birthday: birthDate.value,
    firstName: firstName.value,
    lastName: lastName.value,
  });
};

defineExpose({
  modifyMedicalRecord,
});
</script>
<template>
  <div class="flex h-auto lg:h-full flex-col lg:flex-row">
    <!-- Left Pannel -->
      <!-- loading -->
    <div v-if="isLoading" class=" h-full w-full lg:border-r border-b border-slate-300 overflow-y-auto custom-scroll ">
      <PageHeader class="lg:border-t-0 lg:bottom-auto bottom-[70px] min-h-16 flex">
          <div class="flex items-center gap-2 py-1 overflow-x-auto lg:overflow-x-visible">
            <SkeletonItem backgroundColor="#F1F5F9" class="sm:w-28 w-20 sm:h-8 h-5 rounded-3xl"/>
            <SkeletonItem backgroundColor="#F1F5F9" class="sm:w-28 w-20 sm:h-8 h-5 rounded-3xl"/>
            <SkeletonItem backgroundColor="#F1F5F9" class="sm:w-28 w-20 sm:h-8 h-5 rounded-3xl"/>
          </div>
        </PageHeader>
        <div class="p-4">
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
     </div>

    <div
    v-else
      :class="[
        'h-full w-full lg:order-first order-last lg:flex-1 lg:border-r border-slate-300 overflow-y-auto custom-scroll overflow-x-hidden flex flex-col',
         activeAccordionStep === 1 ? 'justify-between' : 'justify-start'
        ]"
    >
      <!-- Feeds -->
      <AccordionDefault
        :active="activeAccordionStep === 1"
        @open="setActiveAccordionStep(1)"
        title="Feed"
        id="feedAccordion"
        :class="[
        'p-4 border-b border-slate-300 sm:mb-0 mb-36',
        { 'hidden sm:block': activeAccordionStep !== 1 }
        ]"
      >
        <div>
          <!-- Filters -->
          <div>
            <hr class="mt-2 h-[1px] border-t border-slate-300 -mx-4 w-[calc(100%+32px)]" />
            <PageHeader
              class="sm:border-y-0 sm:bottom-auto sm:relative bottom-[125px] left-0 right-0"
            >
              <div class="flex gap-4 py-1 overflow-x-auto lg:overflow-x-visible">
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
                  <DropdownText class="py-2 mb-1 border-y border-slate-200 bg-slate-100 text-slate-500"> 
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
                class="z-[99] records-date-picker hidden lg:block"
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
                    {{ getDateValue(dateValue) }}
                  </span>
                </fwb-button>
              </div>
            </PageHeader>
            <hr
              class="h-[1px] border-t border-slate-300 -mx-4 w-[calc(100%+32px)] sm:block hidden"
            />
          </div>
          <!-- Feed -->
          <index :record="record" :activitySort="activitySort" :dateRange="dateValue" :filteredUsers="appliedUsers"/>
        </div>
      </AccordionDefault>

      <!-- Observations -->
      <AccordionDefault
        :active="activeAccordionStep === 2"
        @open="setActiveAccordionStep(2)"
        title="Observations"
        id="observationsAccordion"
        :class="[
        'p-4 border-slate-300',
        activeAccordionStep === 2 ? 'border-b' : 'hidden sm:block border-t'
        ]"
      >
        <hr class="h-[1px] border-t border-slate-300 -mx-4 mt-3 w-[calc(100%+32px)]" />
        <div class="mt-6">
          <Observations :record="props.record" />
        </div>
      </AccordionDefault>

      <!-- Tabs For mobile -->
      <div class="py-2 px-4 sm:hidden border-t w-full bg-white absolute bottom-[71px] z-[9]">
        <div
          class="text-sm font-semibold cursor-pointer w-full border rounded-md border-brand-100 flex items-center justify-between text-center text-slate-500"
        >
          <span
            @click="setActiveAccordionStep(1)"
            class="w-2/4 h-full py-2 px-3"
            :class="
              activeAccordionStep === 1
                ? 'bg-brand-50  text-slate-900 border-r border-brand-100'
                : ''
            "
            >Feed</span
          >
          <span
            @click="setActiveAccordionStep(2)"
            class="w-2/4 h-full py-2 px-3"
            :class="
              activeAccordionStep === 2
                ? 'bg-brand-50 text-slate-900  border-l border-brand-100'
                : ''
            "
            >Observations</span
          >
        </div>
      </div>
    </div>
    <!-- Right Panel -->
    <!-- loading -->
    <div
      v-if="isLoading && modalStore.isRecordSidebarOpen"
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
      v-if="modalStore.isRecordSidebarOpen"
      class="h-full w-full lg:order-last lg:w-80 overflow-y-auto custom-scroll border-b border-slate-300 lg:border-b-0"
    >
      <!-- Medical Record -->
      <AccordionDefault id="1" active class="p-4 border-b border-slate-300">
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

      <!-- Attendance -->
      <AccordionDefault id="2" title="Attendance" active class="p-4 border-b border-slate-300">
        <div class="flex flex-col gap-4 pt-2">
          <div class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">First Seen</label>
            <div class="text-sm font-medium text-slate-900">
              {{ formatDateByDMY(props.record?.firstSeenOn) }}
            </div>
          </div>

          <div class="flex items-center gap-2">
            <label class="w-1/2 text-xs font-medium text-slate-500">Last Seen</label>
            <div class="text-sm font-medium text-slate-900">
              {{ formatDateByDMY(props.record?.lastSeenOn) }}
            </div>
          </div>
          <!-- Follow up -->
          <div class="flex flex-col gap-4 rounded-lg bg-cyan-50 p-2">
            <div class="flex items-center gap-2">
              <label class="w-1/2 text-xs font-medium text-slate-500">Follow Up</label>
              <div class="text-sm font-medium text-slate-900">
                <!-- {{ formatDateByDMY(props.line.followUp?.date) }} -->
              </div>
            </div>
            <div class="flex items-center gap-2">
              <label class="w-1/2 text-xs font-medium text-slate-500">Follow Up Purpose</label>
              <div class="text-sm font-medium text-slate-900 truncate text-ellipsis w-24">
                <!-- {{ getPurposeName(props.line.followUp?.purposeId) }} -->
              </div>
            </div>
          </div>
        </div>
      </AccordionDefault>
      <!-- Updated/Created -->
      <div class="p-4 flex flex-col gap-2">
        <div class="text-xs font-medium text-slate-500">
          Updated:
          {{
            formatRelativeDate(
              props.record.modifiedAt ? props.record.modifiedAt : props.record.createdAt
            )
          }}
        </div>
        <div class="text-xs font-medium text-slate-500">
          Created: {{ formatRelativeDate(props.record.createdAt) }}
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
              <fwb-input v-model="userSearchQuery"   placeholder="Search for" class="w-full p-2">
                <template #prefix>
                  <IconSearchOutline color="#475569"/>
                </template>
              </fwb-input>
              <div class="p-2 mb-1 border-y border-slate-200 bg-slate-100 text-slate-500"> 
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
:deep(.records-date-picker #headlessui-popover-panel-3 div.absolute.z-50) {
  position: fixed !important;
  top: auto;
  left: unset;
}
@media screen and (max-width: 1215px) {
  :deep(.records-date-picker #headlessui-popover-panel-3 div.absolute.z-50) {
    left: 80px;
  }
  :deep(.records-date-picker .vtd-datepicker:before) {
    opacity: 0;
  }
}
</style>