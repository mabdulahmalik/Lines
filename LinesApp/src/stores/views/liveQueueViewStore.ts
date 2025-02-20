import { ref } from 'vue';
import { defineStore } from 'pinia';
import LocalStorageService from '@/utils/localStorageService';

const storage = new LocalStorageService('LiveQueue_View');

export interface Filter {
  id: string;
  name: string;
  count: number;
}
export interface FilterStatus {
  name: string;
  count: number;
}
export interface allFilters {
  facility: Filter[];
  purposes: Filter[];
  statuses: FilterStatus[];
}

const defaultFilters = {
  facility: [],
  purposes: [],
  statuses: [],
};

export const useLiveQueueViewStore = defineStore('LiveQueueViewStore', () => {  
  const filters = ref<allFilters>(getFilters());
  const selectedTab = ref<string>(storage.getItem('selectedTab') ?? 'All');

  function getFilters(): allFilters {
    const json = storage.getItem('filters');
    return json ? JSON.parse(json) : defaultFilters;
  }  

  const setFilters = (f: allFilters): void => {
    filters.value = f;
    storage.setItem('filters', JSON.stringify(f));
  };

  const setSelectedTab = (value: string): void => {
    selectedTab.value = value;
    storage.setItem('selectedTab', value);
  };

  const resetFilters = (): void => {
    selectedTab.value = 'All';
    filters.value = defaultFilters;

    storage.clear();
  };

  return {
    filters,
    selectedTab,
    setFilters,
    setSelectedTab,
    resetFilters
  };
});
