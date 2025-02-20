import { ref } from 'vue';
import { defineStore } from 'pinia';
import { SortEnumType } from '@/api/__generated__/graphql';
import dayjs from 'dayjs';
import LocalStorageService from '@/utils/localStorageService';

const storage = new LocalStorageService('Jobs_View');

interface Filter {
  id: string;
  name: string;
  count: number;
}
interface FilterStatus {
  name: string;
  count: number;
}
export interface allFilters {
  assigned: Filter[];
  facility: Filter[];
  purposes: Filter[];
  statuses: FilterStatus[];
}

const defaultFilters = {
  assigned: [],
  facility: [],
  purposes: [],
  statuses: [],
};

export const useJobsViewStore = defineStore('JobsViewStore', () => {  
  const filters = ref<allFilters>(getFilters());
  const sortOrder = ref(storage.getItem("sortOrder") ?? SortEnumType.Asc);
  const dateFilter = ref(storage.getItem("dateFilter") ?? getDefaultDate());
  const groupBy = ref(storage.getItem("groupBy") ?? '');

  function getFilters(): allFilters {
    const json = storage.getItem('filters');
    return json ? JSON.parse(json) : defaultFilters;
  }

  function getDefaultDate() {    
    const today = new Date();
    return `${dayjs(today).format('DD MMM YYYY')} - ${dayjs(today).format('DD MMM YYYY')}`
  }

  const setFilters = (f: allFilters): void => {
    filters.value = f;
    storage.setItem('filters', JSON.stringify(f));
  };

  const setDateFilter = (v: string): void => {
    dateFilter.value = v;
    storage.setItem('dateFilter', v);
  }
  
  const setGroupBy = (v: string): void => {
    groupBy.value = v;
    storage.setItem('groupBy', v);
  }

  const setSortOrder = (v: string): void => {
    sortOrder.value = v;
    storage.setItem('sortOrder', v);
  };

  const resetFilters = (): void => {
    filters.value = getFilters();
    sortOrder.value = SortEnumType.Asc;

    storage.clear();
  };

  return {
    filters,
    dateFilter,
    sortOrder,
    groupBy,
    setFilters,
    setDateFilter,
    setSortOrder,
    setGroupBy,
    resetFilters
  };
});
