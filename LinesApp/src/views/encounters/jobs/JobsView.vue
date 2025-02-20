<script setup lang="ts">
import { onMounted, onUnmounted, ref, watch, computed, ComponentPublicInstance } from 'vue';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import { FwbButton, FwbBadge } from 'flowbite-vue';
import JobCard from '@/components/card/JobCard.vue';
import PageContainer from '@/containers/PageContainer.vue';
import PageHeader from '@/components/header/PageHeader.vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import Filter from './partials/filter/JobsIndex.vue';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import VueTailwindDatepicker from 'vue-tailwind-datepicker';
import ViewJobModal from './partials/modal/ViewJobModal.vue';
import Modal from '@/components/modal/Modal.vue';
import CheckListItem from '@/components/list/CheckListItem.vue';
import {
  IconFilterLines,
  IconColumns01,
  IconCalendar,
  IconXClose,
  IconArrowNarrowUp,
  IconArrowNarrowDown,
  IconChevronUp,
} from '@/components/icons/index';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import {
  Encounter,
  EncounterStage,
  Job,
  JobStatus,
  SortEnumType,
} from '@/api/__generated__/graphql';
import { useEncountersStore } from '@/stores/data/encounters';
import EmptyStateScreen from '@/components/empty-states/EmptyStateScreen.vue';
import emptyJobsImage from '@/assets/images/encounters/empty-jobs.svg';
import search from '@/assets/images/encounters/search.svg';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { useUsersStore } from '@/stores/data/settings/users';
import SkeletonItem from '@/components/skeletons/SkeletonItem.vue';
import CardSkeleton from '@/components/skeletons/CardSkeleton.vue';
import { useLoaders } from '@/hooks/useLoaders';
import { formatDatetime } from '@/utils/dateUtils';
import dayjs from 'dayjs';
import { useJobsViewStore } from '@/stores/views/jobsViewStore';

const breadcrumbStore = useBreadcrumbStore();
const jobsStore = useJobsStore();
const encountersStore = useEncountersStore();
const purposesStore = usePurposesStore();
const facilitiesStore = useFacilitiesStore();
const usersStore = useUsersStore();
const { isJobsLoading: isLoading } = useLoaders();

const ISO_DATE_FORMAT = "yyyy-MM-DD";

onMounted(() => {
  breadcrumbStore.breadcrumbs = [
    { title: 'Encounters', to: '/encounters/live-queue' },
    { title: 'Jobs' },
  ];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

const viewStore = useJobsViewStore();

// Sort by Asc,Desc order

const initialSortAscDesc = computed({
  get: () => viewStore.sortOrder,
  set: (x) => viewStore.setSortOrder(x),
});
const toggleSortAscDecscOrder = () => {
  initialSortAscDesc.value =
    initialSortAscDesc.value === SortEnumType.Asc ? SortEnumType.Desc : SortEnumType.Asc;
};

const sortByAscDescJobs = computed(() => {
  return [...jobsBasedOnAllFilters.value].sort((a, b) => {
    const dateA = new Date(a.createdAt).getTime();
    const dateB = new Date(b.createdAt).getTime();

    if (initialSortAscDesc.value === SortEnumType.Asc) {
      return dateB - dateA;
    } else {
      return dateA - dateB;
    }
  });
});

// Group By

const modalGroupByRef = ref<InstanceType<typeof Modal>>();
const openGroupByModal = () => {
  modalGroupByRef.value?.setModalOpen(true);
};

const groupBy = computed({
  get: () => viewStore.groupBy,
  set: (x) => viewStore.setGroupBy(x),
});

const changeGroupBy = (val: string) => {
  groupBy.value = val;
};
const removeGroupBy = () => {
  groupBy.value = '';
};

// group by Status
interface Badge {
  id: string;
  name: string;
  active: boolean;
  archived?: boolean;
}
const statusBadges = ref<Badge[]>([
  { id: JobStatus.Underway, active: true, name: JobStatus.Underway },
  { id: JobStatus.Scheduled, active: true, name: JobStatus.Scheduled },
  { id: JobStatus.Completed, active: true, name: JobStatus.Completed },
  { id: JobStatus.Canceled, active: true, name: JobStatus.Canceled },
]);

const filteredJobsItemsByStatus = (id: string | null): Job[] => {
  return sortByAscDescJobs.value.filter((item) => item.status === id);
};

const statusBadgesWithJobs = computed(() => {
  return statusBadges.value.filter((badge) => filteredJobsItemsByStatus(badge.id).length > 0);
});

// group by Purpose
const purposeBadges = computed(() =>
  purposesStore.purposes.map((purpose) => ({
    id: purpose.id,
    active: true,
    name: purpose.name,
    archived: purpose.archived,
  }))
);

const filteredJobsItemsByPurpose = (id: string | null): Job[] => {
  const matchingPurpose = purposesStore.purposes.find((purpose) => purpose.id === id);
  if (matchingPurpose) {
    return sortByAscDescJobs.value.filter((item) => item.purposeId === matchingPurpose.id);
  }
  return [];
};

const purposesBadgesWithJobs = computed(() => {
  return purposeBadges.value.filter((badge) => filteredJobsItemsByPurpose(badge.id).length > 0);
});

// group by Facility
const facilityBadges = computed(() =>
  facilitiesStore.facilities.map((facility) => ({
    id: facility.id,
    active: true,
    name: facility.name,
    archived: facility.archived,
  }))
);

const dateBadges = computed(() => {
  const isAscending = initialSortAscDesc.value === SortEnumType.Asc;

  const sortedDates = Array.from(
    new Set(jobsStore.jobs.map((job) => formatDatetime(job.createdAt, ISO_DATE_FORMAT)))
  ).sort((a, b) => {
    const [dateA, dateB] = [new Date(a).getTime(), new Date(b).getTime()];
    return isAscending ? dateB - dateA : dateA - dateB;
  });

  return sortedDates.map((date) => ({
    id: date,
    name: date,
    active: true,
  }));
});




const filteredJobsItemsByFacility = (id: string | null): Job[] => {
  const matchingFacility = facilitiesStore.facilities.find((facility) => facility.id === id);
  if (matchingFacility) {
    return sortByAscDescJobs.value.filter(
      (item) => item.location?.facilityId === matchingFacility.id
    );
  }
  return [];
};

const facilityBadgesWithJobs = computed(() => {
  return facilityBadges.value.filter((badge) => filteredJobsItemsByFacility(badge.id).length > 0);
});

const filteredJobsItemsByDate = (createdAt: string | null): Job[] => {
  const isAscending = initialSortAscDesc.value === SortEnumType.Asc;

  return sortByAscDescJobs.value
    .filter((job) => formatDatetime(job.createdAt, ISO_DATE_FORMAT) === createdAt)
    .sort((a, b) => {
      const [dateA, dateB] = [new Date(a.createdAt).getTime(), new Date(b.createdAt).getTime()];
      return isAscending ? dateB - dateA : dateA - dateB;
    });
};



// group by Assigned To

const assignedBadgesWithJobs = computed(() => {
  const unassignedJobs = sortByAscDescJobs.value.filter(
    (job) =>
      !encountersStore.encounters.some(
        (encounter) =>
          encounter.jobId === job.id &&
          encounter.assignments?.some((assignment) => assignment?.clinicianId)
      )
  );
  const unassignedBadge =
    unassignedJobs.length > 0
      ? {
          id: 'unassigned',
          active: true,
          name: 'Unassigned',
          jobs: unassignedJobs,
        }
      : null;

  const assignedBadges = usersStore.users
    .map((user) => ({
      id: user.id,
      active: true,
      name: user.firstName + ' ' + user.lastName,
      jobs: filteredJobsItemsByAssignee(user.id),
    }))
    .filter((badge) => badge.jobs.length > 0)
    .sort((a, b) => a.name.localeCompare(b.name));

  const badges = [...assignedBadges];
  if (unassignedBadge) {
    badges.push(unassignedBadge);
  }
  return badges;
});

const filteredJobsItemsByAssignee = (id: string | null): Job[] => {
  if (!id) return [];
  const jobIds = encountersStore.encounters
    .filter((encounter) =>
      encounter.assignments?.some((assignment) => assignment?.clinicianId === id)
    )
    .map((encounter) => encounter.jobId);
  return sortByAscDescJobs.value.filter((job) => jobIds.includes(job.id));
};

const getJobsBadges = (group: string | null) => {
  if (group === 'Status') {
    return statusBadgesWithJobs.value;
  } else if (group === 'Purpose') {
    return purposesBadgesWithJobs.value;
  } else if (group === 'Facility') {
    return facilityBadgesWithJobs.value;
  } else if (group === 'Assigned To') {
    return assignedBadgesWithJobs.value;
  } else if (group === 'Date Created') {
    return dateBadges.value;
  }
  return [];
};
const getFilteredJobsItems = (id: string | null, group: string | null): Job[] => {
  if (group === 'Status') {
    return filteredJobsItemsByStatus(id);
  } else if (group === 'Purpose') {
    return filteredJobsItemsByPurpose(id);
  } else if (group === 'Facility') {
    return filteredJobsItemsByFacility(id);
  } else if (group === 'Assigned To') {
    if (id === 'unassigned') {
      return sortByAscDescJobs.value.filter(
        (job) =>
          !encountersStore.encounters.some(
            (encounter) =>
              encounter.jobId === job.id &&
              encounter.assignments?.some((assignment) => assignment?.clinicianId)
          )
      );
    }
    return filteredJobsItemsByAssignee(id);
  } else if (group === 'Date Created') {
    return filteredJobsItemsByDate(id);
  }
  return [];
};

// View Job Modal

const viewJobModalRef = ref<InstanceType<typeof ViewJobModal>>();
const filterDrawerRef = ref<InstanceType<typeof ModalDrawer>>();

const associatedEncounter = ref<Encounter | undefined>(undefined);

function handleClickJobCard(selectedJob: Job) {
  jobsStore.selectedJob = selectedJob;

  const selectedEncounter = encountersStore.encounters.find(
    (encounter) => encounter.jobId === selectedJob.id
  );

  associatedEncounter.value = selectedEncounter || undefined;
  console.log('selectedEncounter', associatedEncounter.value);

  if (selectedEncounter?.stage === EncounterStage.Charting) {
    viewJobModalRef.value?.setChartingModalOpen(true);
  } else {
    viewJobModalRef.value?.setModalOpen(true);
  }
}

watch(
  () => encountersStore.encounters,
  (newEncounters) => {
    const selectedJob = jobsStore.selectedJob;
    if (selectedJob) {
      const updatedEncounter = newEncounters.find(
        (encounter) => encounter.jobId === selectedJob.id
      );
      associatedEncounter.value = updatedEncounter || undefined;
    }
  },
  { deep: true }
);


// Date Picker
const modalDateRangeRef = ref<InstanceType<typeof Modal>>();
const today = new Date();

const dateValue = computed({
  get: () => viewStore.dateFilter,
  set: (x) => viewStore.setDateFilter(x),
});

const isTodaySelected = computed(() => {
  const [startDate, endDate] = dateValue.value.split(' - ');
  return startDate === endDate && startDate === dayjs(today).format('DD MMM YYYY');
});

const showNoJobsForToday = computed(() => {
  return (
    jobsStore.jobs.length &&
    isTodaySelected.value &&
    !jobsBasedOnToday.value.length &&
    Object.values(allFilters.value).every((arr) => arr.length === 0)
  );
});
const formatter = ref({
  date: 'DD MMM YYYY',
  month: 'MMM',
});

const predefinedDateRanges = () => {
  const today = new Date();
  const startOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);
  const endOfMonth = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  const startOfLastMonth = new Date(today.getFullYear(), today.getMonth() - 1, 1);
  const endOfLastMonth = new Date(today.getFullYear(), today.getMonth(), 0);

  return [
    {
      label: "Next 7 Days",
      atClick: () => {
        const startDate = new Date();
        const endDate = new Date();
        endDate.setDate(startDate.getDate() + 6);
        return [startDate, endDate];
      },
    },
    {
      label: "Today",
      atClick: () => [new Date(), new Date()],
    },
    {
      label: "Yesterday",
      atClick: () => {
        const yesterday = new Date();
        yesterday.setDate(today.getDate() - 1);
        return [yesterday, yesterday];
      },
    },
    {
      label: "Last 7 Days",
      atClick: () => [new Date(today.setDate(today.getDate() - 6)), new Date()],
    },
    {
      label: "Last 15 Days",
      atClick: () => {
        const date = new Date();
        return [new Date(date.setDate(date.getDate() - 14)), new Date()];
      },
    },
    {
      label: "Last 30 Days",
      atClick: () => [new Date(today.setDate(today.getDate() - 29)), new Date()],
    },
    {
      label: "This Month",
      atClick: () => [startOfMonth, endOfMonth],
    },
    {
      label: "Last Month",
      atClick: () => [startOfLastMonth, endOfLastMonth],
    },
    {
      label: "Last Years",
      atClick: () => [new Date(today.setFullYear(today.getFullYear() - 1)), new Date()],
    },
  ];
};

const getDateValue = (value: string) => {
  if (!value) return value;

  const [startDate, endDate] = value.split(' - ');
  if (startDate === endDate && startDate === dayjs(today).format('DD MMM YYYY')) {
    return 'Today';
  }
  return startDate === endDate ? startDate : value;
};

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
  assigned: Filter[];
  facility: Filter[];
  purposes: Filter[];
  statuses: FilterStatus[];
}

const allFilters = ref<allFilters>(viewStore.filters);

// Assigned To
const assignedList = computed(() => {
  return usersStore.users
    .map((user) => {
      const count = encountersStore.encounters.filter(
        (item: any) => item.assignments[0]?.clinicianId === user.id
      ).length;
      return { id: user.id, name: user.firstName +' '+ user.lastName, count };
    })
    .filter((assign) => assign.count > 0);
});

// Facility
const facilityList = computed(() => {
  if (
    !jobsStore.jobs ||
    jobsStore.jobs.length === 0 ||
    !facilitiesStore.facilities ||
    facilitiesStore.facilities.length === 0
  ) {
    return [];
  }
  // Count jobs by facilityId
  const facilityCounts: Record<string, number> = jobsStore.jobs.reduce((acc, job) => {
    const facilityId = job.location?.facilityId;
    if (facilityId) {
      acc[facilityId] = (acc[facilityId] || 0) + 1;
    }
    return acc;
  }, {} as Record<string, number>);

  // Map facilityId and count to facility name and count
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
  (newFacilities) => {
    if (newFacilities.length > 0) {
      facilityList.value;
    }
  },
  { immediate: true }
);

// Statues
const statusList = ref([
  { name: JobStatus.Underway, count: 0 },
  { name: JobStatus.Scheduled, count: 0 },
  { name: JobStatus.Completed, count: 0 },
  { name: JobStatus.Canceled, count: 0 },
]);

const statuesForFilter = computed(() => {
  return statusList.value
    .map((status) => {
      const count = jobsStore.jobs.filter((item) => item.status === status.name).length;
      return { name: status.name, count };
    })
    .filter((status) => status.count > 0);
});

// Purposes
const purposesList = computed(() => {
  return purposesStore.purposes
    .map((purpose: any) => {
      const count = jobsStore.jobs.filter((item) => item.purposeId === purpose.id).length;
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
    allFilters.value.assigned.length +
    allFilters.value.facility.length +
    allFilters.value.purposes.length +
    allFilters.value.statuses.length
  );
});

function handleFiltersData(filters: allFilters) {
  allFilters.value = filters;
  console.log('selectedFilters in jobs page: ', filters);
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

// filters jobs based on selectedFacility
const jobsBasedOnFacility = computed(() => {
  const selectedFacilityIds = allFilters.value.facility?.map((filter) => filter.id).filter(Boolean);
  if (!selectedFacilityIds || selectedFacilityIds.length === 0) {
    return jobsStore.jobs;
  }
  return jobsStore.jobs.filter((job) => selectedFacilityIds.includes(job.location?.facilityId));
});

// filters Jobs based on selectedPurposes
const jobsBasedOnPurposes = computed(() => {
  const selectedPurposeIds = allFilters.value.purposes?.map((filter) => filter.id).filter(Boolean);
  if (!selectedPurposeIds || selectedPurposeIds.length === 0) {
    return jobsStore.jobs;
  }
  return jobsStore.jobs.filter((job) => selectedPurposeIds.includes(job.purposeId));
});

// Filters Jobs based on selectedStatus
const jobsBasedOnStatuses = computed(() => {
  const selectedFilterNames = allFilters.value.statuses?.map((filter) =>
    filter?.name?.toUpperCase()
  );
  return selectedFilterNames.length > 0
    ? jobsStore.jobs.filter((job) => selectedFilterNames.includes(job.status?.toUpperCase() || ''))
    : jobsStore.jobs;
});

// filters jobs based on selectedAssined to
const jobsBasedOnAssigned = computed(() => {
  const selectedAssignedIds = allFilters.value.assigned?.map((filter) => filter.id).filter(Boolean);
  if (!selectedAssignedIds || selectedAssignedIds.length === 0) {
    return jobsStore.jobs;
  }
  const encountersWithSelectedAssigned = encountersStore.encounters.filter((encounter) => {
    return (encounter.assignments || []).some((assignment: any) =>
      selectedAssignedIds.includes(assignment.clinicianId)
    );
  });
  const filteredJobIds = encountersWithSelectedAssigned.map((encounter) => encounter.jobId);
  return jobsStore.jobs.filter((job) => filteredJobIds.includes(job.id));
});

const jobsBasedOnRange = computed(() => {
  return jobsStore.jobs.filter((job) => {
    const jobDate = dayjs(job.createdAt).valueOf();
    const [startDateString, endDateString] = dateValue.value.split(' - ');
    const startDate = dayjs(startDateString).startOf('day').valueOf(); // Set to 00:00
    const endDate = dayjs(endDateString).endOf('day').valueOf(); // Set to 23:59
    return jobDate >= startDate && jobDate <= endDate;
  });
});

const jobsBasedOnToday = computed(() => {
  return jobsStore.jobs.filter((job) => {
    const jobDate = dayjs(job.createdAt).valueOf();
    const todayStart = dayjs().startOf('day').valueOf(); // Set to 00:00
    const todayEnd = dayjs().endOf('day').valueOf(); // Set to 23:59
    return jobDate >= todayStart && jobDate <= todayEnd;
  });
});

//  All Selected Filters
const jobsBasedOnAllFilters = computed(() => {
  const hasAssignedFilter = allFilters.value.assigned?.length > 0;
  const hasFacilityFilter = allFilters.value.facility?.length > 0;
  const hasPurposeFilter = allFilters.value.purposes?.length > 0;
  const hasStatusFilter = allFilters.value.statuses?.length > 0;

  const rangeFilteredJobs = jobsBasedOnRange.value;
  const assignedFilteredJobs = hasAssignedFilter ? jobsBasedOnAssigned.value : jobsStore.jobs;
  const facilityFilteredJobs = hasFacilityFilter ? jobsBasedOnFacility.value : jobsStore.jobs;
  const purposesFilteredJobs = hasPurposeFilter ? jobsBasedOnPurposes.value : jobsStore.jobs;
  const statusesFilteredJobs = hasStatusFilter ? jobsBasedOnStatuses.value : jobsStore.jobs;

  const combinedJobs = facilityFilteredJobs.filter(
    (job: Job) =>
      rangeFilteredJobs.includes(job) &&
      purposesFilteredJobs.includes(job) &&
      statusesFilteredJobs.includes(job) &&
      assignedFilteredJobs.includes(job)
  );
  return combinedJobs;
});
// Filters too restrictive
const clearAllFilters = () => {
  allFilters.value = {
    assigned: [],
    facility: [],
    purposes: [],
    statuses: [],
  };
};

const onFollowUpClickHandler = (encounterId: string) => {
  viewJobModalRef?.value?.closeModal();

  const selectedEncounter = encountersStore.encounters.find((x) => x.id == encounterId);
  const job = jobsStore.jobs.find((x) => x.id == selectedEncounter?.jobId);

  jobsStore.selectedJob = job!;
  associatedEncounter.value = selectedEncounter || undefined;

  if (selectedEncounter?.stage === EncounterStage.Charting) {
    viewJobModalRef.value?.setChartingModalOpen(true);
  } else {
    viewJobModalRef.value?.setModalOpen(true);
  }
};

const accordionRefs = ref<ComponentPublicInstance[]>([]);
const scrollToAccordion = (index: number) => {
  const element = accordionRefs.value[index]?.$el as HTMLElement | null;
  if (element) {
    element.scrollIntoView({ behavior: "smooth", block: "end" });
  }
};
</script>

<template>  
<div>
    <!-- Empty jobs screen -->
    <div v-if="!isLoading && !jobsStore.jobs.length">
    <EmptyStateScreen
    title="We'll have more <span class='text-purple-800 px-0.5'>Jobs</span> soon"
    description="So don't go far! New Jobs may arrive at any moment and you wouldn't want to miss them. So please stay close."
    >
    <template #image>
    <img :src="emptyJobsImage" alt="empty-jobs" />
    </template>
    </EmptyStateScreen>
    </div>

  <div v-else>
    <PageHeader class="h-16 flex items-center">
        <!-- loading -->
        <div v-if="isLoading" class="flex items-center ">
        <div class="flex items-center gap-2">
        <Skeleton-item class="w-4 h-4 rounded-md" />
        <Skeleton-item class="w-16 h-4 rounded-3xl" />
        </div>
        </div>

      <div v-else class="flex justify-between items-center py-1 overflow-x-auto lg:overflow-x-visible  flex-1">
        <div class="flex lg:gap-4 gap-2 items-center">
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

        <!-- Sort -->
        <fwb-button
          @click="toggleSortAscDecscOrder"
          pill
          class="bg-brand-100 text-brand-600 hover:bg-brand-100 font-semibold"
        >
          <template #prefix>
            <template v-if="initialSortAscDesc === SortEnumType.Asc">
              <IconArrowNarrowUp />
            </template>
            <template v-else>
              <IconArrowNarrowDown />
            </template>
          </template>
          <span class="whitespace-nowrap"> Sort: Date </span>
        </fwb-button>

        <!-- Group by Desktop-->
        <Dropdown close-inside class="hidden lg:block">
          <template #trigger>
            <fwb-button pill class="bg-brand-100 text-brand-600 hover:bg-brand-100 font-semibold">
              <template #prefix>
                <IconColumns01 />
              </template>
              <div class="flex items-center">
                <span class="whitespace-nowrap"> Group by </span>
                <div v-if="groupBy" class="flex items-center">
                  <span class="whitespace-nowrap"> : {{ groupBy }} </span>
                  <span @click.stop="removeGroupBy" class="text-slate-900 ml-2"
                    ><IconXClose height="20" width="20"
                  /></span>
                </div>
              </div>
            </fwb-button>
          </template>
          <DropdownMenu class="w-40">
            <DropdownItem @click="changeGroupBy('Assigned To')"> Assigned To </DropdownItem>
            <DropdownItem @click="changeGroupBy('Facility')"> Facility </DropdownItem>
            <DropdownItem @click="changeGroupBy('Purpose')"> Purpose </DropdownItem>
            <DropdownItem @click="changeGroupBy('Status')"> Status </DropdownItem>
            <DropdownItem @click="changeGroupBy('Date Created')"> Date Created </DropdownItem>
          </DropdownMenu>
        </Dropdown>
        <!-- Group by Mobile-->
        <fwb-button
          @click="openGroupByModal"
          pill
          class="lg:hidden bg-brand-100 text-brand-600 hover:bg-brand-100 font-semibold"
        >
          <template #prefix>
            <IconColumns01 />
          </template>
          <div class="flex items-center">
            <span class="whitespace-nowrap"> Group by </span>
            <div v-if="groupBy" class="flex items-center">
              <span class="whitespace-nowrap"> : {{ groupBy }} </span>
              <span @click.stop="removeGroupBy" class="text-slate-900 ml-2"
                ><IconXClose height="20" width="20"
              /></span>
            </div>
          </div>
        </fwb-button>

        <!-- Date -->
        <vue-tailwind-datepicker
          v-model="dateValue"
          v-slot="{ value, placeholder }"
          :formatter="formatter"
          :shortcuts="predefinedDateRanges"
          separator=" - "
          placeholder="Select Date"
          class="z-[99] hidden lg:block"
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
        <!-- Total jobs -->
          <div class="total-jobs-badge flex justify-center items-center text-sm font-medium text-yellow-700 rounded-md bg-yellow-100 py-1.5 px-3 flex-nowrap text-nowrap ">
           {{ jobsStore.jobs.length }} results
          </div>
      </div>
    </PageHeader>
    <PageContainer>
      <!-- Job Modal drawer -->
      <ViewJobModal
        ref="viewJobModalRef"
        :job="jobsStore.selectedJob"
        :encounter="associatedEncounter"
        @onFollowUpClick="onFollowUpClickHandler"
      ></ViewJobModal>
      <!-- Date range modal for mobile -->
      <Modal ref="modalDateRangeRef" title="Select Date Range" no_footer :z_index="55">
        <template #body>
          <div class="h-full overflow-y-auto max-h-[calc(100vh-150px)]">
            <vue-tailwind-datepicker
              v-model="dateValue"
              v-slot="{ value, placeholder }"
              :formatter="formatter"
              :shortcuts="predefinedDateRanges"
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
      <!-- Group by Modal -->
      <Modal ref="modalGroupByRef" title="Group by" no_footer :z_index="55" :close_outside="false">
        <template #body>
          <div class="flex flex-col gap-2 p-4">
            <CheckListItem
              :checked="groupBy === 'Assigned To'"
              @click="changeGroupBy('Assigned To')"
            >
              Assigned To
            </CheckListItem>
            <CheckListItem :checked="groupBy === 'Facility'" @click="changeGroupBy('Facility')">
              Facility
            </CheckListItem>
            <CheckListItem :checked="groupBy === 'Purpose'" @click="changeGroupBy('Purpose')">
              Purpose
            </CheckListItem>
            <CheckListItem :checked="groupBy === 'Status'" @click="changeGroupBy('Status')">
              Status
            </CheckListItem>
          </div>
        </template>
      </Modal>

      <!-- loading Skeleton  -->
      <div v-if="isLoading" class="flex flex-col max-w-4xl gap-4">
        <!-- accordion skeleton -->
        <div class="flex items-center gap-2 justify-between">
          <div class="flex items-center gap-2">
            <IconChevronUp color="#64748B" />
            <SkeletonItem class="w-20 h-4 rounded"/>
          </div> 
          <hr class="w-full border-slate-300" />
           <div><SkeletonItem class="w-[22px] h-5 rounded" /></div> 
        </div>
        <!-- cards skeleton -->
        <div v-for="i in 5" :key="i">
        <CardSkeleton />
        </div>
      </div>

        <!-- No new jobs for today -->
        <div v-else-if="showNoJobsForToday" class="overflow-hidden">
        <EmptyStateScreen
        title="No new <span class='text-purple-800 px-0.5'> Jobs </span> for Today Yet..."
        >
        <template #image>
        <img :src="search" alt="search-image" />
        </template>
        </EmptyStateScreen>
        </div>
       <!-- Filters too restrictive screen -->
        <div v-else-if="!jobsBasedOnAllFilters.length" class="overflow-x-hidden ">
          <EmptyStateScreen
            title="No <span class='text-purple-800 px-0.5'>Jobs</span> found"
            description="No Jobs match the provided filter criteria. Try being less strict with your criteria or <span class='text-purple-800 px-0.5'>Clear all Filters</span> to see all Jobs within the provided Date Range."
            buttonText="Clear all filters"
            @action="clearAllFilters"
          >
            <template #image>
              <img :src="search" alt="search-image" />
            </template>
          </EmptyStateScreen>
        </div>
      
      <div v-else class="flex items-start gap-4 lg:gap-10 flex-col md:flex-row">
        <!-- Quick jump to Mobile -->
        <div v-if="groupBy" class="md:hidden bg-white w-[calc(100%+32px)] -ml-4 py-1.5 px-4 border-b border-slate-300 -mt-4 sticky -top-4 z-10">
          <AccordionDefault
              ref="accordionRefs"
              id="quick-jump"
              :active="false"
              custom_header
            >
              <template #customHeader="{open}" >
                <div class="w-full flex items-center justify-between pb-1">
                  <span class="text-sm font-semibold"> Quick jump to</span>
                  <div class="text-slate-500 hover:text-slate-900">
                    <IconChevronUp :class="{'rotate-180': open}"/>
                  </div>
                </div>
              </template>
              <div class="space-y-3 pb-2 max-h-60 overflow-y-auto">
                <div
                  v-for="(badge, index) in getJobsBadges(groupBy)"
                  :key="index"
                  class="flex items-center justify-between gap-2"
                >
                  <span class="text-base font-semibold text-brand-600 cursor-pointer" @click="scrollToAccordion(index)">{{ badge.name }}</span>
                  <fwb-badge type="dark" size="sm" class="rounded text-xs font-semibold m-0">
                    {{
                      'archived' in badge && badge.archived
                        ? 'Archived'
                        : getFilteredJobsItems(badge.id, groupBy).length
                    }}
                  </fwb-badge>
                </div>
              </div>

          </AccordionDefault>
        </div>
        <!-- Job cards -->
        <div class="flex flex-col w-full max-w-4xl gap-4 lg:mb-4 mb-10">
          <template v-if="!groupBy">
            <JobCard
              v-for="(job, index) in sortByAscDescJobs"
              :job="job"
              :key="index"
              :encounter="encountersStore.encounters.find((encounter) => encounter.jobId === job.id)"
              @click="handleClickJobCard(job)"
            />
          </template>
          <!-- Group by -->
          <template v-if="groupBy">
            <AccordionDefault
              v-for="(badge, index) in getJobsBadges(groupBy)"
              ref="accordionRefs"
              :id="`badge-${index}`"
              :active="badge.active"
              :count="
                'archived' in badge && badge.archived
                  ? 'Archived'
                  : getFilteredJobsItems(badge.id, groupBy).length
              "
              :def_header="true"
              :badge="badge.name"
              class="gap-4"
              :key="index"
            >
              <JobCard
                v-for="(job, index) in getFilteredJobsItems(badge.id, groupBy)"
                :key="index"
                :job="job"
                @click="handleClickJobCard(job)"
              />
            </AccordionDefault>
          </template>
        </div>
        <!-- Quick jump to Desktop -->
        <div v-if="groupBy" class="hidden md:block w-64 min-w-56 rounded-lg border bg-white border-slate-300 p-4 mt-5 sticky top-0">
          <div class="font-semibold text-sm mb-4">Quickly jump to</div>
          <div class="space-y-4">
            <!-- onclick scroll to accordion -->
            <div
              v-for="(badge, index) in getJobsBadges(groupBy)"
              :key="index"
              class="flex items-center justify-between gap-2"
            >
              <span class="text-base font-semibold text-brand-600 cursor-pointer" @click="scrollToAccordion(index)">{{ badge.name }}</span>
              <fwb-badge type="dark" size="sm" class="rounded text-xs font-semibold m-0">
                {{
                  'archived' in badge && badge.archived
                    ? 'Archived'
                    : getFilteredJobsItems(badge.id, groupBy).length
                }}
              </fwb-badge>
            </div>
          </div>
        </div>
      </div>
    </PageContainer>
    <!-- Filter drawer -->
    <ModalDrawer ref="filterDrawerRef" max_width="lg" title="Filter" :close_outside="true">
      <template #body>
        <Filter
          ref="filterRef"
          :assigned="assignedList"
          :facility="facilityList"
          :purposes="purposesList"
          :status="statuesForFilter"
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
/* Total jobs badge */
@media screen and (max-width: 1140px) {
  .total-jobs-badge{
    position: fixed;
    bottom: 0px;
    left: 0;
    right: 0;
    border-radius: 0;
  }
}
@media screen and (max-width: 1024px) {
  .total-jobs-badge{
    bottom: 64px; 
  }
}
</style>
