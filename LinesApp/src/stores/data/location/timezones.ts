import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import { useGetTimeZonesQuery } from '@/api/__generated__/graphql';

export const useTimezonesStore = defineStore('timezones', () => {
  provideApolloClient(client);

  const timeZones = ref<string[]>([]);

  // Fetch timeZones
  const { onResult: getTimeZonesResult } = useGetTimeZonesQuery();
  getTimeZonesResult((result: any) => {
    timeZones.value = result?.data?.timeZones || [];
  });

  return {
    timeZones,
  };
});
