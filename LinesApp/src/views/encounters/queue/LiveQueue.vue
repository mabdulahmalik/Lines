<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from 'vue';
import { FwbButton, FwbTooltip } from 'flowbite-vue';
import QueueCard from '@/components/card/QueueCard.vue';
import PageHeader from '@/components/header/PageHeader.vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import ChartingPanel from '../jobs/partials/ChartingPanel.vue';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import {
  IconFilterLines,
  IconClipboardCheckFilled,
  IconClipboardCheckUnFilled,
  IconChevronUp,
} from '@/components/icons/index';
import Filter from '../jobs/partials/filter/LiveQueueIndex.vue';
import ViewJobModal from '../jobs/partials/modal/ViewJobModal.vue';
import { useEncountersStore } from '@/stores/data/encounters';
import {
  Encounter,
  EncounterAlertType,
  EncounterPriority,
  EncounterStage,
  Job,
  JobStatus,
} from '@/api/__generated__/graphql';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import EmptyStateScreen from '@/components/empty-states/EmptyStateScreen.vue';
import emptyLivequeueImage from '@/assets/images/encounters/empty-livequeue.svg';
import search from '@/assets/images/encounters/search.svg';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { useUsersStore } from '@/stores/data/settings/users';
import ButtonGroupRadio from '@/components/form/ButtonGroupRadio.vue';
import SkeletonItem from '@/components/skeletons/SkeletonItem.vue';
import CardSkeleton from '@/components/skeletons/CardSkeleton.vue';
import { useLoaders } from '@/hooks/useLoaders';
import MiniCardSkeleton from '@/components/skeletons/MiniCardSkeleton.vue';
import { useLiveQueueViewStore } from '@/stores/views/liveQueueViewStore';

const breadcrumbStore = useBreadcrumbStore();
const jobsStore = useJobsStore();
const encountersStore = useEncountersStore();
const purposesStore = usePurposesStore();
const facilitiesStore = useFacilitiesStore();
const usersStore = useUsersStore();
const viewStore = useLiveQueueViewStore();

const { isLiveQueueLoading: isLoading } = useLoaders();

onMounted(() => {
  breadcrumbStore.breadcrumbs = [
    { title: 'Encounters', to: '/encounters/live-queue' },
    { title: 'Live Queue' },
  ];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

interface Badge {
  name: EncounterPriority;
  active: boolean;
}
const badges = ref<Badge[]>([
  { active: true, name: EncounterPriority.Stat },
  { active: true, name: EncounterPriority.Normal },
  { active: true, name: EncounterPriority.Routine },
]);

//  Exclude CHARTING and COMPLETED stage encounters
const activeEncounters = computed(() => {
  return encountersStore.encounters.filter((encounter: Encounter) => {
    const job = getJobForEncounter(encounter.jobId);
    return (
      encounter.stage !== EncounterStage.Charting &&
      encounter.stage !== EncounterStage.Completed &&
      job?.status !== JobStatus.Canceled // Exclude canceled jobs if job exists
    );
  });
});
// Exclude Completed stage encounters
const activeEncountersWithCharting = computed(() => {
  return encountersStore.encounters.filter((encounter: Encounter) => {
    const job = getJobForEncounter(encounter.jobId);
    return (
      encounter.stage !== EncounterStage.Completed &&
      job?.status !== JobStatus.Canceled // Exclude canceled jobs if job exists
    );
  });
});

// Badges with Encounters
const badgesWithEncounters = computed(() => {
  return badges.value.filter((badge: Badge) => filteredEncounters(badge.name).length > 0);
});

const filteredEncounters = (badgeName: EncounterPriority): Encounter[] => {
  if (hasAppliedFiltersStatus.value) {
    return encountersBasedOnAllFilters.value.filter(
      (encounter) => encounter.priority === badgeName
    );
  } else {
    return tabsFilterEncounters.value.filter((encounter) => encounter.priority === badgeName);
  }
};
const viewJobModalRef = ref<InstanceType<typeof ViewJobModal>>();
const filterDrawerRef = ref<InstanceType<typeof ModalDrawer>>();

function handelClickQueueCard(encounter: Encounter) {
  encountersStore.selectedEncounter = encounter;

  if (encounter.stage === EncounterStage.Charting) {
    viewJobModalRef.value?.setChartingModalOpen(true);
  } else {
    viewJobModalRef.value?.setModalOpen(true);
  }
}

const allJobs = computed(() => jobsStore.jobs);
function getJobForEncounter(id: string): Job {
  return allJobs.value.find((job) => job.id === id) as Job;
}

const getJobAsProp = computed(() => {
  if (!encountersStore.selectedEncounter.jobId) return {} as Job;
  return allJobs.value.find((job) => job.id === encountersStore.selectedEncounter.jobId) as Job;
});

// Encounters has a single Priority
const singlePriorityEncounters = computed(() => {
  if (!encountersStore.encounters.length) return false;
  const firstPriority = encountersBasedOnAllFilters.value[0]?.priority;
  return encountersBasedOnAllFilters.value.every(
    (encounter) => encounter.priority === firstPriority
  );
});

// Filters

interface Filter {
  id: string;
  name: string;
  count: number;
}
interface FilterStatus {
  name: string;
  count: number;
}
interface allFilters {
  facility: Filter[];
  purposes: Filter[];
  statuses: FilterStatus[];
}
const allFilters = ref<allFilters>(viewStore.filters);

// Statues
const statusList = ref([
  { name: EncounterStage.Waiting, count: 0 },
  { name: EncounterStage.Assigned, count: 0 },
  { name: EncounterStage.Attending, count: 0 },
]);

const statusCounts = computed(() => {
  return statusList.value
    .map((status: FilterStatus) => {
      const count = activeEncounters.value?.filter((item) => item.stage === status.name).length;
      return { name: status.name, count };
    })
    .filter((status) => status.count > 0);
});

// Facility
const facilityList = computed(() => {
  const encounterJobIds = activeEncounters.value.map((encounter) => encounter.jobId);
  const filteredJobs = jobsStore.jobs.filter((job) => encounterJobIds.includes(job.id));
  if (!facilitiesStore.facilities || facilitiesStore.facilities.length === 0) {
    return [];
  }
  const facilityCounts: Record<string, number> = filteredJobs.reduce((acc, job) => {
    const facilityId = job.location?.facilityId;
    if (facilityId) {
      acc[facilityId] = (acc[facilityId] || 0) + 1;
    }
    return acc;
  }, {} as Record<string, number>);

  return Object.entries(facilityCounts)
    .map(([facilityId, count]) => {
      const facility = facilitiesStore.facilities.find((f) => f.id === facilityId);
      if (!facility) {
        console.warn(`Facility with ID ${facilityId} not found in facilitiesStore.`);
        return null;
      }
      return { id: facility.id, name: facility.name, count };
    })
    .filter((item): item is { id: string; name: string; count: number } => item !== null);
});

watch(
  () => facilitiesStore.facilities,
  () => {
    facilityList.value;
  },
  { immediate: true }
);

// Purposes
const purposesList = computed(() => {
  const encounterJobIds = activeEncounters.value.map((encounter: Encounter) => encounter.jobId);
  const filteredJobs = jobsStore.jobs.filter((job) => encounterJobIds.includes(job.id));
  return purposesStore.purposes
    .map((purpose: any) => {
      const count = filteredJobs.filter((job) => job.purposeId === purpose.id).length;
      return { id: purpose.id, name: purpose.name, count };
    })
    .filter((purpose) => purpose.count > 0);
});

// Total filters results count
const hasFilterData = ref(false);
const handleHasFilterData = (val: boolean) => {
  hasFilterData.value = val;
};

const badgeFiltersCount = computed(() => {
  return (
    allFilters.value.facility.length +
    allFilters.value.purposes.length +
    allFilters.value.statuses.length
  );
});

const hasAppliedFiltersStatus = computed(() => {
  return (
    allFilters.value.facility.length > 0 ||
    allFilters.value.purposes.length > 0 ||
    allFilters.value.statuses.length > 0
  );
});

function handleFiltersData(filters: allFilters) {
  allFilters.value = filters;
  // console.log('selectedFilters in live queue page: ', filters);
}

// Apply filters
const filterRef = ref();
const applyFilters = () => {
  filterRef.value?.submitFilters();
  filterDrawerRef.value?.setModalOpen(false);
};

const clearFilters = () => {
  filterRef.value?.clearFilters();
};

// Filters encounters based on selectedStatus
const encountersBasedOnStatuses = computed(() => {
  const selectedFilterNames = allFilters.value.statuses?.map((filter) =>
    filter?.name?.toUpperCase()
  );
  return selectedFilterNames.length > 0
    ? activeEncounters.value.filter((job) =>
        selectedFilterNames.includes(job.stage?.toUpperCase() || '')
      )
    : activeEncounters.value;
});

// Filters encounters based on selectedPurposes
const encountersBasedOnPurposes = computed(() => {
  const selectedPurposeIds = allFilters.value.purposes?.map((filter) => filter.id).filter(Boolean);
  if (!selectedPurposeIds || selectedPurposeIds.length === 0) {
    return activeEncounters.value;
  }
  const encounterJobIds = encountersStore.encounters.map((encounter) => encounter.jobId);
  const filteredJobs = jobsStore.jobs.filter(
    (job) => encounterJobIds.includes(job.id) && selectedPurposeIds.includes(job.purposeId)
  );
  // Return encounters that match the filtered jobs
  return activeEncounters.value.filter((encounter) =>
    filteredJobs.some((job) => job.id === encounter.jobId)
  );
});

// Filters encounters based on selectedFacility
const encountersBasedOnFacility = computed(() => {
  const selectedFacilityIds = allFilters.value.facility?.map((filter) => filter.id).filter(Boolean);
  if (!selectedFacilityIds || selectedFacilityIds.length === 0) {
    return activeEncounters.value;
  }
  const encounterJobIds = encountersStore.encounters.map((encounter) => encounter.jobId);
  const filteredJobs = jobsStore.jobs.filter(
    (job) =>
      encounterJobIds.includes(job.id) && selectedFacilityIds.includes(job.location?.facilityId)
  );
  return activeEncounters.value.filter((encounter) =>
    filteredJobs.some((job) => job.id === encounter.jobId)
  );
});

//  All Selected Filters
const encountersBasedOnAllFilters = computed(() => {
  const hasFacilityFilter = allFilters.value.facility?.length > 0;
  const hasPurposeFilter = allFilters.value.purposes?.length > 0;
  const hasStatusFilter = allFilters.value.statuses?.length > 0;

  const facilityFilteredEncounters = hasFacilityFilter
    ? encountersBasedOnFacility.value
    : activeEncounters.value;

  const purposesFilteredEncounters = hasPurposeFilter
    ? encountersBasedOnPurposes.value
    : activeEncounters.value;

  const statusesFilteredEncounters = hasStatusFilter
    ? encountersBasedOnStatuses.value
    : activeEncounters.value;

  const combinedEncounters = facilityFilteredEncounters.filter(
    (encounter: Encounter) =>
      purposesFilteredEncounters.includes(encounter) &&
      statusesFilteredEncounters.includes(encounter)
  );
  if (hasAppliedFiltersStatus.value) {
    return combinedEncounters.filter(
      (encounter: Encounter) => encounter.stage !== EncounterStage.Charting && encounter.stage !== EncounterStage.Completed
    );
  } else {
    return tabsFilterEncounters.value.filter(
      (encounter: Encounter) => encounter.stage !== EncounterStage.Charting && encounter.stage !== EncounterStage.Completed
    );
  }
});

// Filters too restrictive
const clearAllFilters = () => {
  allFilters.value = {
    facility: [],
    purposes: [],
    statuses: [],
  };
  selectedTabStatus.value = 'All';
};

// Charting panel
const chartingPanelRef = ref<InstanceType<typeof ChartingPanel>>();
const isChartingPanelOpen = ref(true);
const toggleChartingPanelDrawer = () => {
  isChartingPanelOpen.value = !isChartingPanelOpen.value;
  chartingPanelRef.value?.setModalOpen(isChartingPanelOpen.value);
};

// current user
const currentUser = computed(() => {
  return usersStore.users.find((user) =>
    encountersStore.encounters.some((encounter) =>
      encounter.assignments?.some((assignment) => assignment?.clinicianId === user.id)
    )
  );
});

// Free to grab
const chartingEncounters = computed(() => {
  if (!currentUser.value) return [];
  return encountersStore.encounters
    .filter(
      (encounter: Encounter) =>
        encounter.stage === EncounterStage.Charting &&
        !encounter.assignments?.some(
          (assignment) => assignment?.clinicianId === currentUser.value?.id
        )
    )
    .sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime());
});

// For me
const forMeEncounters = computed(() => {
  if (!currentUser.value) return [];
  return encountersStore.encounters
    .filter(
      (encounter: Encounter) =>
        encounter.stage === EncounterStage.Charting &&
        encounter.assignments?.some(
          (assignment) => assignment?.clinicianId === currentUser.value?.id
        )
    )
    .sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime());
});

// Charting panel Open & Close State

const updateChartingPanelState = (encounters: any) => {
  const hasEncounters = encounters.length > 0;
  isChartingPanelOpen.value = hasEncounters;
  chartingPanelRef.value?.setModalOpen(hasEncounters);
};
watch(chartingEncounters, (newVal) => updateChartingPanelState(newVal), { immediate: true });
watch(forMeEncounters, (newVal) => updateChartingPanelState(newVal), { immediate: true });
onMounted(() => {
  updateChartingPanelState(chartingEncounters.value);
  updateChartingPanelState(forMeEncounters.value);
});

// Tabs Filters
const selectedTabStatus = computed({
  get: () => viewStore.selectedTab,
  set: (x) => viewStore.setSelectedTab(x),
});
const allEncountersCount = computed(() => activeEncounters.value?.length || 0);
const holdEncountersCount = computed(() => {
  return (
    activeEncounters.value?.filter((encounter: Encounter) =>
      encounter.alerts?.some((alert) => alert?.type === EncounterAlertType.OnHold)
    ).length || 0
  );
});
const assistanceEncountersCount = computed(() => {
  return (
    activeEncounters.value?.filter((encounter: Encounter) =>
      encounter.alerts?.some((alert) => alert?.type === EncounterAlertType.AssistanceRequested)
    ).length || 0
  );
});

const tabStatusOptions = computed(() => [
  { value: 'All', label: 'All', count: allEncountersCount.value },
  { value: EncounterAlertType.OnHold, label: 'On Hold', count: holdEncountersCount.value },
  {
    value: EncounterAlertType.AssistanceRequested,
    label: 'Help Needed',
    count: assistanceEncountersCount.value,
  },
]);

watch(
  selectedTabStatus,
  (newVal) => {
    if (newVal === EncounterAlertType.OnHold) {
      isChartingPanelOpen.value = false;
      chartingPanelRef.value?.setModalOpen(false);
    }
    if (newVal === EncounterAlertType.AssistanceRequested) {
      isChartingPanelOpen.value = false;
      chartingPanelRef.value?.setModalOpen(false);
    }
  },
  { immediate: true }
);

const tabsFilterEncounters = computed(() => {
  return activeEncounters.value?.filter((encounter: Encounter) => {
    if (selectedTabStatus.value === 'All') {
      return true;
    }
    if (selectedTabStatus.value === EncounterAlertType.OnHold) {
      return encounter.alerts?.some((alert) => alert?.type === EncounterAlertType.OnHold);
    }
    if (selectedTabStatus.value === EncounterAlertType.AssistanceRequested) {
      return encounter.alerts?.some(
        (alert) => alert?.type === EncounterAlertType.AssistanceRequested
      );
    }
    return false;
  });
});
</script>

<template>
<div>
  <!-- Empty State screen -->
  <div v-if="!isLoading && !activeEncounters.length && !activeEncountersWithCharting.length">
    <EmptyStateScreen
      title="Congratulations, the <span class='text-purple-800 px-0.5'>Queue</span> is empty!"
      description="But don't go far! New Encounters may arrive at any moment and you wouldn't want to miss them. So please stay close."
    >
      <template #image>
        <img :src="emptyLivequeueImage" alt="empty-livequeue" />
      </template>
    </EmptyStateScreen>
  </div>
  
  <div v-else>
    <PageHeader
      :class="['sm:bottom-0 bottom-[62px]  min-h-16 ', selectedTabStatus === 'All' ? '' : 'sm:block hidden']"
    >
    <!-- loading -->
    <div v-if="isLoading" class="flex items-center justify-between flex-1 mt-3">
      <div class="flex items-center  gap-2 ">
        <Skeleton-item class="w-4 h-4 rounded-md" />
        <Skeleton-item class="w-16 h-4 rounded-3xl" />
      </div>
      <div>
        <Skeleton-item class="w-5 h-5 rounded-md" />
      </div>
    </div>
      <div
      v-else
        class="flex sm:flex-row flex-col gap-4 items-center py-1 overflow-x-auto lg:overflow-x-visible"
      >
        <!-- Desktop Tabs -->
        <div
          :class="[
            'sm:order-1 order-2 sm:block hidden',
            hasAppliedFiltersStatus ? 'pointer-events-none' : '',
          ]"
        >
          <ButtonGroupRadio v-model="selectedTabStatus" :options="tabStatusOptions" />
        </div>
        <div
          v-if="selectedTabStatus === 'All'"
          class="sm:order-2 order-1 flex justify-between items-center flex-1 w-full"
        >
          <!-- Filter -->
          <fwb-button
            @click="filterDrawerRef?.setModalOpen(true)"
            color="light"
            pill
            square
            :class="
              [
                'font-semibold border-transparent px-3 ms-1',
                badgeFiltersCount
                  ? 'bg-brand-100 text-brand-600 hover:bg-brand-100'
                  : 'text-slate-900',
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
          <!-- Charting Icon -->
          <div class="mr-2">
            <fwb-tooltip placement="bottom">
              <template #trigger>
                <div class="relative cursor-pointer" @click="toggleChartingPanelDrawer">
                  <template v-if="isChartingPanelOpen">
                    <IconClipboardCheckFilled />
                  </template>
                  <template v-else>
                    <IconClipboardCheckUnFilled />
                  </template>
                  <!-- Badge -->
                  <span
                    class="charting-badge absolute top-[-7px] right-[-7px] inline-flex items-center justify-center bg-brand-600 text-white w-[17px] h-[17px] text-[11px] font-medium rounded px-1"
                    >{{ forMeEncounters.length + chartingEncounters.length }}</span
                  >
                </div>
              </template>
              <template #content>
                <template v-if="isChartingPanelOpen">
                  <span class="text-sm font-medium"> Hide 'Charting' Encounters</span>
                </template>
                <template v-else>
                  <span class="text-sm font-medium">Show 'Charting' Encounters</span></template
                >
              </template>
            </fwb-tooltip>
          </div>
        </div>
      </div>
    </PageHeader>

    <!-- On Mobile view Tabs  -->
    <PageHeader
      :class="[
        'sm:hidden flex justify-center overflow-x-auto min-h-16',
        hasAppliedFiltersStatus ? 'pointer-events-none' : '',
      ]"
    >
    <!-- loading -->
    <div v-if="isLoading" class="flex items-center justify-center flex-1 ">
      <div class="flex items-center  gap-2">
        <Skeleton-item class="w-32 h-4 rounded-3xl" />
      </div>
    </div>
    <div  v-else>
      <ButtonGroupRadio v-model="selectedTabStatus" :options="tabStatusOptions" />
    </div>
    </PageHeader>

    <!-- Page Container -->
    <div
      :class="
        [
          'flex flex-row  justify-between items-stretch overflow-x-hidden  mb-16 lg:mb-0  overflow-auto sm:max-h-[calc(100vh-192px)] lg:min-h-[calc(100vh-152px)] ',
          selectedTabStatus === 'All' ? 'max-h-[calc(100vh-246px)]' : 'max-h-[calc(100vh-185px)]',
        ].join(' ')
      "
    >
      <div class="flex w-full h-full">
        <!-- Left Column -->
        <div class="lg:p-10 p-4 h-full w-full">
          <!-- Skeleton For main cards -->
          <div v-if="isLoading" class="my-4 flex flex-col max-w-4xl gap-4">
             <!-- accordion skeleton -->
             <div class="flex items-center gap-2 justify-between">
                <div class="flex items-center gap-2">
                  <IconChevronUp color="#64748B" />
                  <SkeletonItem class="w-20 h-4 rounded"/>
                </div> 
                <hr class="w-full border-slate-300" />
                  <SkeletonItem class="w-[22px] h-5 rounded" />
            </div>
            <div v-for="i in 5" :key="i">
              <CardSkeleton />
            </div>
          </div>
          <div v-else-if="encountersBasedOnAllFilters.length && tabsFilterEncounters.length">
            <!--  Encounters has single Priority  -->
            <div v-if="singlePriorityEncounters" class="flex flex-col max-w-4xl gap-4">
              <template v-for="encounter in encountersBasedOnAllFilters" :key="encounter.id">
                <QueueCard
                  v-if="getJobForEncounter(encounter.jobId)"
                  :encounter="encounter"
                  :job="getJobForEncounter(encounter.jobId)"
                  @click="handelClickQueueCard(encounter)"
                />
              </template>
            </div>
            <!--  Encounters has multiple Priority  -->
            <div v-else class="max-w-4xl">
              <AccordionDefault
                v-for="(badge, index) in badgesWithEncounters"
                :id="`badge-${index}`"
                :active="badge.active"
                :count="filteredEncounters(badge.name).length"
                :def_header="true"
                :badge="badge.name"
                class="gap-4"
              >
                <template v-for="encounter in filteredEncounters(badge.name)" :key="encounter.id">
                  <QueueCard
                    v-if="getJobForEncounter(encounter.jobId)"
                    :encounter="encounter"
                    :job="getJobForEncounter(encounter.jobId)"
                    @click="handelClickQueueCard(encounter)"
                  />
                </template>
              </AccordionDefault>
            </div>
          </div>          
          <!-- Filters too restrictive screen -->
          <div v-else>
            <EmptyStateScreen
              title="No <span class='text-purple-800 px-0.5'>Encounters</span> found"
              description="No Encounters match the provided filter criteria. Try being less strict with your criteria or <span class='text-purple-800 px-0.5'>Clear all Filters</span> to see the full Queue."
              buttonText="Clear all Filters"
              @action="clearAllFilters"
            >
              <template #image>
                <img :src="search" alt="search-image" />
              </template>
            </EmptyStateScreen>
          </div>          
        </div>

        <!-- Right Column -->
        <ChartingPanel
          ref="chartingPanelRef"
          title="Charting"
          @close="isChartingPanelOpen = false"
          :defaultOpen="isChartingPanelOpen"
        >
          <template #body>
          <!-- Skeleton For Charting cards -->
           <div v-if="isLoading" class="lg:p-10 p-4 flex flex-col gap-4">
               <!-- accordion skeleton -->
               <div class="flex items-center gap-2 justify-between">
                <div class="flex items-center gap-2">
                  <IconChevronUp color="#64748B" />
                  <SkeletonItem class="w-20 h-4 rounded"/>
                </div> 
                <hr class="w-full border-slate-300" />
                 <div><SkeletonItem class="w-5 h-5 rounded" /></div>
            </div>
            <div v-for="i in 5" :key="i">
              <MiniCardSkeleton />
            </div>
          </div>
            
            <div v-else-if="chartingEncounters.length || forMeEncounters.length" class="lg:p-10 p-4">
              <!-- For me -->
              <AccordionDefault
                v-if="forMeEncounters.length"
                id="For-me"
                :def_header="true"
                badge="For me"
                :count="forMeEncounters.length"
                active
                class="gap-4"
              >
                <template v-for="encounter in forMeEncounters" :key="encounter.id" class="gap-4">
                  <QueueCard
                    v-if="getJobForEncounter(encounter.jobId)"
                    :encounter="encounter"
                    :job="getJobForEncounter(encounter.jobId)"
                    @click="handelClickQueueCard(encounter)"
                  />
                </template>
              </AccordionDefault>

              <!-- Free to grab -->
              <AccordionDefault
                v-if="chartingEncounters.length"
                id="Free-to-grab"
                :def_header="true"
                badge="Free to grab"
                :count="chartingEncounters.length"
                active
                class="gap-4"
              >
                <template v-for="encounter in chartingEncounters" :key="encounter.id" class="gap-4">
                  <QueueCard
                    v-if="getJobForEncounter(encounter.jobId)"
                    :encounter="encounter"
                    :job="getJobForEncounter(encounter.jobId)"
                    @click="handelClickQueueCard(encounter)"
                  />
                </template>
              </AccordionDefault>
            </div>
            <div v-else>
              <!-- Empty Charting screen -->
              <EmptyStateScreen title="Nothing to Chart" description="We're all caught up!">
                <template #image>
                  <img :src="emptyLivequeueImage" alt="empty-livequeue" />
                </template>
              </EmptyStateScreen>
            </div>
          </template>
        </ChartingPanel>
      </div>
    </div>

    <!-- View/Open Job Modal -->
    <ViewJobModal
      ref="viewJobModalRef"
      :is-live-queue="true"
      :encounter="encountersStore.selectedEncounter"
      :job="getJobAsProp"
    ></ViewJobModal>

    <!-- Filter drawer -->
    <ModalDrawer ref="filterDrawerRef" max_width="lg" title="Filter" :close_outside="true">
      <template #body>
        <Filter
          ref="filterRef"
          :status="statusCounts"
          :purposes="purposesList"
          :facility="facilityList"
          :selectedFilters="{ ...allFilters }"
          @filtersData="handleFiltersData"
          @hasFilterData="handleHasFilterData"
        ></Filter>
      </template>
      <template #footer>
        <div class="p-4 lg:px-6 flex items-center gap-4 w-full">
          <fwb-button
            v-if="badgeFiltersCount"
            @click="clearFilters"
            color="light"
            pill
            class="w-48 text-nowrap"
          >
            Clear selection
          </fwb-button>

          <fwb-button @click="applyFilters" class="bg-primary-600 hover:bg-brand-600 w-full" pill>
            {{ hasFilterData ? ' See results' : 'See All' }}
          </fwb-button>
        </div>
      </template>
    </ModalDrawer>
  </div>
</div>
</template>

<style scoped>
/* Safari-specific CSS */
@supports not (-webkit-touch-callout: none) {
  .charting-badge {
    position: absolute;
    top: -7px;
    right: -7px;
  }
}
@media screen and (-webkit-min-device-pixel-ratio: 0) {
  .charting-badge {
    position: absolute;
    top: -7px;
    right: -7px;
  }
}
</style>
