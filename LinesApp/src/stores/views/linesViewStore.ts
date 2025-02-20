import { ref } from 'vue';
import { defineStore } from 'pinia';
import LocalStorageService from '@/utils/localStorageService';

const storage = new LocalStorageService('Lines_View');

export const useLinesViewStore = defineStore('LinesViewStore', () => {
  const selectedDwelling = ref<string>(storage.getItem('selectedDwelling') ?? 'IN');
  const searchQuery = ref<string>(storage.getItem('searchQuery') ?? '');

  const setSelectedDwelling = (status: string): void => {
    selectedDwelling.value = status;
    storage.setItem('selectedDwelling', status);
  };

  const setSearchQuery = (query: string): void => {
    searchQuery.value = query;
    storage.setItem('searchQuery', query);
  };

  const resetFilters = (): void => {
    selectedDwelling.value = 'All';
    searchQuery.value = '';
    
    storage.clear();
  };

  return {
    selectedDwelling,
    searchQuery,
    setSelectedDwelling,
    setSearchQuery,
    resetFilters
  };
});
