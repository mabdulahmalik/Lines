<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from 'vue';
import { BaseTable, THead, TBody, TR, TD, TH } from '@/components/table/index';
import { FwbButton, FwbBadge } from 'flowbite-vue';
import PageContainer from '@/containers/PageContainer.vue';
import PageHeader from '@/components/header/PageHeader.vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { IconArrowCircleBrokenDownRight, IconAlertHexagon } from '@/components/icons';
import LineStatusBadge from '@/components/badge/LineStatusBadge.vue';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import { IconSearchOutline } from '@/components/icons/index';
import ViewLineModal from '../lines/partials/modal/ViewLineModal.vue';
import ButtonGroupRadio from '@/components/form/ButtonGroupRadio.vue';
import TableSearchInput from '@/components/form/TableSearchInput.vue';
import { useLinesStore } from '@/stores/data/encounters/lines';
import { Line, MedicalRecord, FacilityRoom } from '@/api/__generated__/graphql';
import { useFacilitiesStore } from '@/stores/data/facilities';
import EmptyStateScreen from '@/components/empty-states/EmptyStateScreen.vue';
import emptyLinesImage from '@/assets/images/encounters/empty-lines.svg';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import { formatDateByDMY } from '@/utils/dateUtils';
import SkeletonItem from '@/components/skeletons/SkeletonItem.vue';
import { useLoaders } from '@/hooks/useLoaders';
import { useLinesViewStore } from '@/stores/views/linesViewStore';

const breadcrumbStore = useBreadcrumbStore();
const linesStore = useLinesStore();
const facilitiesStore = useFacilitiesStore();
const medicalRecordsStore = useMedicalRecordsStore();
const { isLinesLoading: isLoading } = useLoaders();
const viewStore = useLinesViewStore();

onMounted(() => {
  breadcrumbStore.breadcrumbs = [
    { title: 'Encounters', to: '/encounters/live-queue' },
    { title: 'Lines' },
  ];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

const lineItems = computed(() => linesStore.lines);
const viewLineModalRef = ref<InstanceType<typeof ModalDrawer>>();
const modalSearchRef = ref<InstanceType<typeof ModalDrawer>>();
function openSearchModal() {
  modalSearchRef.value?.setModalOpen(true);
}

function handelTblRowClick(selectedLine: Line) {
  linesStore.selectedLine = selectedLine;
  viewLineModalRef.value?.setModalOpen(true);
}

const selectedDwellingOption = computed({
  get: () => viewStore.selectedDwelling,
  set: (x) => viewStore.setSelectedDwelling(x),
});

const dwellingOptions = [
  { value: 'IN', label: 'In' },
  { value: 'OUT', label: 'Out' },
];

const searchQuery = computed({
  get: () => viewStore.searchQuery,
  set: (val) => viewStore.setSearchQuery(val),
});

const inputQuery = ref(searchQuery.value);

const filteredLineItems = computed(() => {
  return linesStore.lines.filter((line) => {
    const matchesStatus = line.dwelling === selectedDwellingOption.value;
    const patientName = getMedicalRecordNameById(line.medicalRecordId).toLowerCase();
    const roomName = getRoomName(line.location?.roomId).toLowerCase();
    const matchesSearchQuery =
      patientName.includes(searchQuery.value.toLowerCase()) ||
      roomName.includes(searchQuery.value.toLowerCase()) ||
      line.name?.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
      line.type?.toLowerCase().includes(searchQuery.value.toLowerCase());

    return matchesStatus && matchesSearchQuery;
  });
});

function performSearch(query: string) {
  searchQuery.value = query;
  modalSearchRef.value?.setModalOpen(false);
}

const searchOptions = computed(() =>
  lineItems.value.flatMap((item) => [
    { value: getMedicalRecordNameById(item.medicalRecordId) || '', property: 'PatientName' },
    { value: item.name || '', property: 'LineName' },
    { value: getRoomName(item.location?.roomId) || '', property: 'Room' },
    { value: item.type || '', property: 'Type' },
  ])
);

function getMedicalRecordNameById(recordId: string): string {
  const record = medicalRecordsStore.medicalRecords.find((r) => r.id === recordId);
  if (record?.firstName && record?.lastName) {
    return `${record.firstName} ${record.lastName}`;
  }
  return '';
}
watch(
  () => linesStore.lines,
  (newLines) => {
    const facilityIds = [...new Set(newLines.map((line) => line.location?.facilityId))];
    facilityIds.forEach((facilityId) => {
      if (facilityId) {
        getRoomsForFacility(facilityId);
      }
    });
  },
  { immediate: true }
);
const roomsCache = ref<FacilityRoom[]>([]);

function getRoomsForFacility(facilityId: string) {
  facilitiesStore.getRooms(facilityId)((result) => {
    if (result.data && result.data.facilityRooms?.items) {
      roomsCache.value = [
        ...roomsCache.value,
        ...(result.data.facilityRooms.items.filter((room) => room !== null) as FacilityRoom[]),
      ];
    }
  });
}

function getRoomName(roomId: string): string {
  const room = roomsCache.value.find((r) => r.id === roomId);
  return room?.name ?? 'Loading...';
}
// Empty state
const addLinesAction = () => {
  console.log('Add Lines Action');
};

function getMedicalRecord(recordId: string): MedicalRecord | undefined {
  const record = medicalRecordsStore.medicalRecords.find((r) => r.id === recordId);
  return record ?? undefined;
}

// Follow up
const showFollowUpColumn = computed(() => lineItems.value.some((line) => line.followUp !== null));
</script>

<template>
<div>
   <div v-if="!isLoading && !linesStore.lines.length ">
    <EmptyStateScreen
      title="This is where you’ll find the lines"
      description="The purpose of intravenous (IV) therapy is to replace fluid and electrolytes, provide medications, and replenish blood volume."
      buttonText="Add Line"
      @action="addLinesAction"
    >
      <template #image>
        <img :src="emptyLinesImage" alt="empty-lines" />
      </template>
    </EmptyStateScreen>
  </div>

  <div v-else>
    <PageHeader class="min-h-16 flex items-center">
    <!-- loading -->
       <div v-if="isLoading" class="flex items-center justify-between flex-1 ">
          <div class="flex items-center gap-2 py-3">
            <SkeletonItem class="w-4 h-4 rounded-md" />
            <SkeletonItem class="sm:w-16 w-12 h-4 rounded-3xl" />
          </div>
          <div class="flex items-center gap-2 py-3 lg:hidden">
            <SkeletonItem class="w-4 h-4 rounded-md" />
            <SkeletonItem class="sm:w-16 w-12 h-4 rounded-3xl" />
          </div>
          <div class="flex items-center gap-2 py-3 lg:hidden">
            <SkeletonItem class="w-4 h-4 rounded-md" />
            <SkeletonItem class="sm:w-16 w-12 h-4 rounded-3xl" />
          </div>
        </div>
      <div
      v-else
        class="flex gap-4 items-center justify-between py-1 overflow-x-auto lg:overflow-x-visible flex-1"
      >
        <div class="flex gap-2 lg:gap-4 justify-between lg:justify-none w-full lg:w-auto">
          <!-- In, Out -->
          <ButtonGroupRadio v-model="selectedDwellingOption" :options="dwellingOptions" />
          <!-- Mobile search button -->
          <fwb-button
            @click="openSearchModal"
            color="light"
            class="lg:hidden px-0 border-none hover:bg-white focus:ring-0"
          >
            <template #prefix>
              <IconSearchOutline width="20" height="20" class="text-gray-500 dark:text-gray-400" />
            </template>
            Search
          </fwb-button>
        </div>
        <!-- Desktop Search -->
        <TableSearchInput
          v-model="inputQuery"
          :options="searchOptions"
          placeholder="Search"
          @search="performSearch"
          class="hidden lg:block"
        />
      </div>
    </PageHeader>
    <PageContainer class="max-w-[100vw]">
      <div class="max-w-full">
        <div class="relative overflow-x-auto">
          <base-table>
            <t-head class="uppercase">
              <t-r>
                <t-h v-if="showFollowUpColumn && selectedDwellingOption === 'IN'" class="w-32 text-nowrap">Follow Up</t-h>
                <t-h class="text-nowrap">Dwell Time</t-h>
                <t-h class="min-w-44 text-nowrap">Patient NAME</t-h>
                <t-h class="min-w-32">Room</t-h>
                <t-h class="min-w-44 text-nowrap">Line name</t-h>
                <t-h class="min-w-44 text-nowrap">type of line</t-h>
                <t-h class="text-nowrap">Dwelling</t-h>
              </t-r>
            </t-head>
            <t-body>
              <!-- loading -->
                <t-r v-if="isLoading"  v-for="i in 5" :key="i">
                  <t-d v-if="showFollowUpColumn && selectedDwellingOption === 'IN'">
                    <SkeletonItem class="w-16 h-4 rounded-3xl" />
                  </t-d>
                  <t-d >
                    <SkeletonItem class="w-16 h-4 rounded-3xl" />
                  </t-d>
                  <t-d>
                    <div class="flex gap-2">
                      <SkeletonItem backgroundColor="#CBD5E1" class="w-[200px] h-4 rounded-3xl" />
                      <SkeletonItem backgroundColor="#CBD5E1" class="w-4 h-4 rounded-full" />
                    </div>
                  </t-d>
                  <t-d >
                    <SkeletonItem class="w-32 h-4 rounded-3xl" />
                  </t-d>
                  <t-d >
                    <SkeletonItem class="w-32 h-4 rounded-3xl" />
                  </t-d>
                  <t-d >
                    <SkeletonItem class="w-32 h-4 rounded-3xl" />
                  </t-d>
                  <t-d >
                    <SkeletonItem backgroundColor="#CBD5E1" class="w-12 h-4 rounded-3xl" />
                  </t-d>
                </t-r>
              <t-r v-else v-for="(line, index) in filteredLineItems" :key="index">
                <t-d v-if="showFollowUpColumn && selectedDwellingOption === 'IN'">{{
                  formatDateByDMY(line.followUp?.date) || '-'
                }}</t-d>
                <t-d>{{ line.dwellTime ? `${line.dwellTime} ${line.dwellTime === 1 ? 'day' : 'days'}` : '-' }}</t-d>
                <t-d>
                  <div class="flex gap-2 text-brand-600">
                    <a @click="handelTblRowClick(line)" href="#">
                      {{ getMedicalRecord(line.medicalRecordId)?.firstName || '(Unknown)' }}
                      {{ getMedicalRecord(line.medicalRecordId)?.lastName }}
                    </a>
                    <fwb-badge
                      v-if="line.externallyPlaced"
                      class="bg-yellow-100 text-yellow-700 self-center mr-0"
                    >
                      <template #icon>
                        <IconArrowCircleBrokenDownRight />
                      </template>
                    </fwb-badge>
                    <fwb-badge
                      v-if="line.infectedOn"
                      class="bg-radical-red-100 text-radical-red-700 self-center mr-0"
                    >
                      <template #icon>
                        <IconAlertHexagon />
                      </template>
                    </fwb-badge>
                  </div>
                </t-d>
                <t-d>{{ getRoomName(line.location?.roomId) }}</t-d>
                <t-d>{{ line.name || '(Unspecified)' }}</t-d>
                <t-d>{{ line.type }}</t-d>
                <t-d>
                  <LineStatusBadge v-if="line.dwelling" :badge="line.dwelling"></LineStatusBadge>
                </t-d>
              </t-r>
            </t-body>
            <t-body v-if="!filteredLineItems.length && !isLoading">
              <t-r>
                <t-d colspan="6" class="text-center">No Lines to Display</t-d>
              </t-r>
            </t-body>
          </base-table>
        </div>
      </div>
      <!-- Open Line Modal -->
      <ViewLineModal ref="viewLineModalRef" :line="linesStore.selectedLine" />
      <!-- Search Modal -->
      <ModalDrawer
        ref="modalSearchRef"
        max_width="lg"
        title="Search"
        no_footer
        :close_outside="true"
      >
        <template #body>
          <div class="flex flex-col gap-2 p-4">
            <TableSearchInput
              v-model="inputQuery"
              :options="searchOptions"
              placeholder="Search"
              @search="performSearch"
            />
          </div>
        </template>
      </ModalDrawer>
    </PageContainer>
  </div>
</div>
</template>
