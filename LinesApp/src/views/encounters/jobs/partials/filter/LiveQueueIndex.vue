<script setup lang="ts">
import VerticalTabs from '@/components/tabs/VerticalTabs.vue';
import Facility from './Facility.vue';
import Purpose from './Purpose.vue';
import Status from './Status.vue';
import { TabType } from '@/types/tab';
import { ref, onMounted, computed, watch } from 'vue';
import { useLiveQueueViewStore } from '@/stores/views/liveQueueViewStore';

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

const props = defineProps<{
  facility?: Array<{ id: string; name: string; count: number }>;
  purposes?: Array<{ id: string; name: string; count: number }>;
  status?: Array<{ name: string; count: number }>;
  selectedFilters: allFilters;
}>();

const emit = defineEmits(['filtersData', 'hasFilterData']);

const viewStore = useLiveQueueViewStore();

const tabs = computed<TabType[]>(() => [
  {
    label: 'Facility',
    sub_label: facilityCount.value,
  },
  {
    label: 'Purpose',
    sub_label: purposeCount.value,
  },
  {
    label: 'Stage',
    sub_label: statusCount.value,
  },
]);

//  Filter counts

const facilityCount = computed(() =>
  filters.value.facility.length ? filters.value.facility.length + ' filter(s)' : ''
);
const purposeCount = computed(() =>
  filters.value.purposes.length ? filters.value.purposes.length + ' filter(s)' : ''
);
const statusCount = computed(() =>
  filters.value.statuses.length ? filters.value.statuses.length + ' filter(s)' : ''
);

const defaultFilters = {
  facility: [],
  purposes: [],
  statuses: [],
};

const filters = ref<allFilters>(viewStore.filters);

onMounted(() => {
  filters.value = props.selectedFilters
    ? JSON.parse(JSON.stringify(props.selectedFilters))
    : defaultFilters;
});

// Check if any filters have data across the three tabs
const hasFilterData = computed(() => {
  return (
    filters.value.facility.some((filter) => filter.count > 0) ||
    filters.value.purposes.some((filter) => filter.count > 0) ||
    filters.value.statuses.some((filter) => filter.count > 0)
  );
});

// Watch for changes in hasFilterData and emit true or false
watch(
  () => hasFilterData.value,
  (hasData) => {
    emit('hasFilterData', hasData);
  },
  { deep: true }
);


// Submit & clear filters
function submitFilters() {
  viewStore.setFilters(filters.value);
  emit('filtersData', filters.value);
}

const clearFilters = () => {
  filters.value.facility = [];
  filters.value.purposes = [];
  filters.value.statuses = [];
};

defineExpose({
  submitFilters,
  clearFilters,
});
</script>

<template>
  <div class="h-full">
    <VerticalTabs :tabs="tabs" min_width="36" full_height>
      <template #tab-0>
        <div class="p-6 lg:p-6">
          <Facility :facility="props.facility" v-model="filters.facility" />
        </div>
      </template>
      <template #tab-1>
        <div class="p-4 lg:p-6">
          <Purpose :purposes="props.purposes" v-model="filters.purposes" />
        </div>
      </template>
      <template #tab-2>
        <div class="p-4 lg:p-6">
          <Status :status="props.status" v-model="filters.statuses" />
        </div>
      </template>
    </VerticalTabs>
  </div>
</template>
