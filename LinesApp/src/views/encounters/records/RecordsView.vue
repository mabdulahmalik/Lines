<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from 'vue';
import { BaseTable, THead, TBody, TR, TD, TH } from '@/components/table/index';
import { FwbButton, FwbBadge } from 'flowbite-vue';
import PageContainer from '@/containers/PageContainer.vue';
import PageHeader from '@/components/header/PageHeader.vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { IconArrowCircleBrokenDownRight, IconAlertHexagon } from '@/components/icons';
import RecordStatusBadge from '@/components/badge/RecordStatusBadge.vue';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import { IconSearchOutline } from '@/components/icons/index';
import { Record } from '@/types/records';
import ViewRecordModal from './partials/modal/ViewRecordModal.vue';
import ButtonGroupRadio from '@/components/form/ButtonGroupRadio.vue';
import TableSearchInput from '@/components/form/TableSearchInput.vue';
import EmptyStateScreen from '@/components/empty-states/EmptyStateScreen.vue';
import emptyRecordsImage from '@/assets/images/encounters/empty-records.svg';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import { MedicalRecord } from '@/api/__generated__/graphql';
import { useLinesStore } from '@/stores/data/encounters/lines';
import SkeletonItem from '@/components/skeletons/SkeletonItem.vue';
import { useLoaders } from '@/hooks/useLoaders';
import { useRecordsViewStore } from '@/stores/views/recordsViewStore';

const breadcrumbStore = useBreadcrumbStore();
const medicalRecordsStore = useMedicalRecordsStore();
const linesStore = useLinesStore();
const { isRecordsLoading: isLoading } = useLoaders();

const viewStore = useRecordsViewStore();

onMounted(() => {
  breadcrumbStore.breadcrumbs = [
    { title: 'Encounters', to: '/encounters/live-queue' },
    { title: 'Records' },
  ];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

const medicalRecords = computed(() => medicalRecordsStore.medicalRecords);

function externallyPlaced(recordId: string): boolean {
  const associatedLines = linesStore.lines.filter((line) => line.medicalRecordId === recordId);
  return associatedLines.some((line) => line.externallyPlaced);
}

function hasInfection(recordId: string): boolean {
  const associatedLines = linesStore.lines.filter((line) => line.medicalRecordId === recordId);
  return associatedLines.some((line) => line.infectedOn);
}

const viewRecordModalRef = ref<InstanceType<typeof ModalDrawer>>();
const modalSearchRef = ref<InstanceType<typeof ModalDrawer>>();
function openSearchModal() {
  modalSearchRef.value?.setModalOpen(true);
}

function handelTblRowClick(selectedRecord: MedicalRecord) {
  medicalRecordsStore.selectedMedicalRecord = selectedRecord;
  viewRecordModalRef.value?.setModalOpen(true);
}

const selectedStatusOption = computed({
  get: () => viewStore.selectedStatus,
  set: (x) => viewStore.setSelectedStatus(x),
});

const statusOptions = [
  { value: 'Active', label: 'Active' },
  { value: 'Inactive', label: 'Inactive' },
  { value: 'All', label: 'All' },
];

const searchQuery = computed({
  get: () => viewStore.searchQuery,
  set: (val) => viewStore.setSearchQuery(val),
});

const inputQuery = ref(searchQuery.value);

const filteredMedicalRecords = computed(() => {
  return medicalRecords.value.filter((record) => {
    const matchesStatus =
      selectedStatusOption.value === 'All' ||
      (record.active && selectedStatusOption.value === 'Active') ||
      (!record.active && selectedStatusOption.value === 'Inactive');

    const matchesSearchQuery =
      `${record.firstName} ${record.lastName}`
        ?.toLowerCase()
        .includes(searchQuery.value.toLowerCase()) ||
      record.number?.toLowerCase().includes(searchQuery.value.toLowerCase());
    return matchesStatus && matchesSearchQuery;
  });
});

function performSearch(query: string) {
  searchQuery.value = query;
  modalSearchRef.value?.setModalOpen(false);
}

const searchOptions = computed(() =>
  medicalRecordsStore.medicalRecords.flatMap((item) => [
    { value: `${item.firstName || ''} ${item.lastName || ''}`, property: 'PatientName' },
    { value: item.number || '', property: 'Mrn' },
  ])
);



// Empty state
const emptyRecordsAction = () => {
  console.log('call to action');
};
</script>

<template>
<div>
  <div v-if="!isLoading && !medicalRecords.length">
    <EmptyStateScreen
      title="All Records are here"
      description="The purpose of intravenous (IV) therapy is to replace fluid and electrolytes, provide medications, and replenish blood volume."
      buttonText="Call to action"
      @action="emptyRecordsAction"
    >
      <template #image>
        <img :src="emptyRecordsImage" alt="empty-records" />
      </template>
    </EmptyStateScreen>
  </div>

  <div v-else>
    <PageHeader class="min-h-16 flex items-center">
      <!-- loading -->
        <div v-if="isLoading" class="flex items-center justify-between flex-1 ">
          <div class="flex items-center gap-2 py-3">
            <Skeleton-item class="w-4 h-4 rounded-md" />
            <Skeleton-item class="sm:w-16 w-12 h-4 rounded-3xl" />
          </div>
          <div class="flex items-center gap-2 py-3 lg:hidden">
            <Skeleton-item class="w-4 h-4 rounded-md" />
            <Skeleton-item class="sm:w-16 w-12 h-4 rounded-3xl" />
          </div>
          <div class="flex items-center gap-2 py-3 lg:hidden">
            <Skeleton-item class="w-4 h-4 rounded-md" />
            <Skeleton-item class="sm:w-16 w-12 h-4 rounded-3xl" />
          </div>
        </div>
      <div
      v-else
        class="flex gap-4 items-center justify-between py-1 overflow-x-auto lg:overflow-x-visible flex-1"
      >
        <div class="flex gap-2 lg:gap-4 justify-between lg:justify-none w-full lg:w-auto">
          <!-- All, In, Out -->
          <ButtonGroupRadio v-model="selectedStatusOption" :options="statusOptions" />
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
    <PageContainer  class="max-w-[100vw]">
      <div class="max-w-full">
        <div class="relative overflow-x-auto">
          <base-table>
            <t-head class="uppercase">
              <t-r>
                <t-h class="w-40 text-nowrap">Last Seen</t-h>
                <t-h class="w-40">MRN</t-h>
                <t-h class="min-w-44 text-nowrap">Patient NAME</t-h>
                <t-h class="w-44 text-nowrap">Lines (Total/Open)</t-h>
                <t-h class="w-32 text-nowrap">Status</t-h>
              </t-r>
            </t-head>
            <t-body>
               <!-- loading -->
               <t-r v-if="isLoading"  v-for="i in 5" :key="i">
                  <t-d >
                    <SkeletonItem class="w-16 h-4 rounded-3xl" />
                  </t-d>
                  <t-d>
                    <div class="flex gap-2 items-center">
                      <SkeletonItem backgroundColor="#CBD5E1" class="lg:w-[200px] w-32 h-4 rounded-3xl" />
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
                    <SkeletonItem backgroundColor="#CBD5E1" class="w-12 h-4 rounded-3xl" />
                  </t-d>
                </t-r>
              <t-r v-else v-for="(record, index) in filteredMedicalRecords" :key="index">
                <t-d>{{ record.lastSeenOn }}</t-d>
                <t-d>
                  <a @click="handelTblRowClick(record)" href="#" class="text-brand-600">
                    {{ record.number }}
                  </a>
                </t-d>
                <t-d>
                  <div class="flex gap-2">
                    <span> {{ record.lastName }}, {{ record.firstName }} </span>
                    <fwb-badge
                      v-if="externallyPlaced(record.id)"
                      class="bg-yellow-100 text-yellow-700 self-center mr-0"
                    >
                      <template #icon>
                        <IconArrowCircleBrokenDownRight />
                      </template>
                    </fwb-badge>
                    <fwb-badge
                      v-if="hasInfection(record.id)"
                      class="bg-radical-red-100 text-radical-red-700 self-center mr-0"
                    >
                      <template #icon>
                        <IconAlertHexagon />
                      </template>
                    </fwb-badge>
                  </div>
                </t-d>

                <t-d>{{ record.linesTotal }} / {{ record.linesIn }}</t-d>
                <t-d>
                  <RecordStatusBadge v-if="record.active" badge="Active"></RecordStatusBadge>
                  <RecordStatusBadge v-else badge="Inactive"></RecordStatusBadge>
                </t-d>
              </t-r>
            </t-body>
            <t-body v-if="!filteredMedicalRecords.length && !isLoading">
              <t-r>
                <t-d colspan="6" class="text-center">No Records to Display</t-d>
              </t-r>
            </t-body>
          </base-table>
        </div>
      </div>
      <!-- Open Record Modal -->
      <ViewRecordModal
        ref="viewRecordModalRef"
        :record="medicalRecordsStore.selectedMedicalRecord"
      ></ViewRecordModal>
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
