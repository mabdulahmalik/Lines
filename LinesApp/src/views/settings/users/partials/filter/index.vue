<script setup lang="ts">
import VerticalTabs from "@/components/tabs/VerticalTabs.vue";
import { TabType } from "@/types/tab";
import { ref, onMounted, computed, watch } from "vue";
import Identity from "./Identity.vue";
import Role from "./Role.vue";
import LastLogin from "./LastLogin.vue";

const props = defineProps<{
  identities?: Array<{ name: string }>;
  role?: Array<{ name: string }>;
  lastLogin?: Array<{ name: string }>;
  selectedFilters: allFilters;
}>();
const emit = defineEmits(["filtersData", "hasFilterData"]);

interface Filter {
  name: string;
}

interface allFilters {
  identities: Filter[];
  role: Filter[];
  lastLogin: Filter[];
}

const tabs = computed<TabType[]>(() => [
  {
    label: "Identity",
    sub_label: identitiesCount.value,
  },
  {
    label: "Role",
    sub_label: roleCount.value,
  },
  {
    label: "Last Login",
    sub_label: lastLoginCount.value,
  },
]);

//  Filter counts
const identitiesCount = computed(() =>
  filters.value.identities.length ? filters.value.identities.length + " filter(s)" : ""
);
const roleCount = computed(() =>
  filters.value.role.length ? filters.value.role.length + " filter(s)" : ""
);
const lastLoginCount = computed(() =>
  filters.value.lastLogin.length ? filters.value.lastLogin.length + " filter(s)" : ""
);

const defaultFilters = {
  identities: [],
  role: [],
  lastLogin: [],
};
const filters = ref<allFilters>(defaultFilters);

onMounted(() => {
  filters.value = props.selectedFilters
    ? JSON.parse(JSON.stringify(props.selectedFilters))
    : defaultFilters;
});

const hasFilterData = computed(() => {
  return (
    filters.value.identities.some((filter) => filter.name.length) ||
    filters.value.role.some((filter) => filter.name.length) ||
    filters.value.lastLogin.some((filter) => filter.name.length)
  );
});

watch(
  () => hasFilterData.value,
  (hasData) => {
    emit("hasFilterData", hasData);
  },
  { deep: true }
);

// Submit & clear filters
const submitFilters = () => {
  emit("filtersData", filters.value);
};

const clearFilters = () => {
  filters.value.identities = [];
  filters.value.role = [];
  filters.value.lastLogin = [];
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
          <Identity :identities="props.identities" v-model="filters.identities" />
        </div>
      </template>
      <template #tab-1>
        <div class="p-6 lg:p-6">
          <Role :role="props.role" v-model="filters.role" />
        </div>
      </template>
      <template #tab-2>
        <div class="p-4 lg:p-6">
          <LastLogin :lastLogin="props.lastLogin" v-model="filters.lastLogin" />
        </div>
      </template>
    </VerticalTabs>
  </div>
</template>
