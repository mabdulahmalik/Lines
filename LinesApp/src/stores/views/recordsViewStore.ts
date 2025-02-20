import { ref } from 'vue';
import { defineStore } from 'pinia';
import LocalStorageService from '@/utils/localStorageService';

const storage = new LocalStorageService('Records_View');

export const useRecordsViewStore = defineStore('RecordsViewStore', () => {
  const selectedStatus = ref<string>(storage.getItem('selectedStatus') ?? 'All');
  const searchQuery = ref<string>(storage.getItem('searchQuery') ?? '');

  const setSelectedStatus = (status: string): void => {
    selectedStatus.value = status;
    storage.setItem('selectedStatus', status);
  };

  const setSearchQuery = (query: string): void => {
    searchQuery.value = query;
    storage.setItem('searchQuery', query);
  };

  const resetFilters = (): void => {
    selectedStatus.value = 'All';
    searchQuery.value = '';

    storage.clear();
  };

  return {
    selectedStatus,
    searchQuery,
    setSelectedStatus,
    setSearchQuery,
    resetFilters
  };
});
