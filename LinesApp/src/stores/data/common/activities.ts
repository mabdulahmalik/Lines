import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  Activity,
  useBroadcastSubscription,
  useDirectSubscription,
  useGetActivitiesQuery,
} from '@/api/__generated__/graphql';

export const useActivitiesStore = defineStore('activities', () => {
  provideApolloClient(client);

  const activities = ref<Activity[]>([]);
  const isLoading = ref(true);

  const { onResult } = useGetActivitiesQuery({ order: { timestamp: "DESC" } } as any);

  onResult((result: any) => {
    activities.value = result?.data?.activities?.items || [];
    isLoading.value = result.loading;
  });
    
  const { onResult: handleBroadcast } = useBroadcastSubscription();
  handleBroadcast(() => {
    getLatestActivities()((result: any) => {
      activities.value = result?.data?.activities?.items || [];
    });
  });

  const { onResult: handleDirectSubscription } = useDirectSubscription();
  handleDirectSubscription(() => {
    getLatestActivities()((result: any) => {
      //TODO: Append items
      activities.value = result?.data?.activities?.items || [];
    });
  });

  const getLatestActivities = () => {
    //const timestamp = activities.value[activities.value.length -1].timestamp;

    const { onResult } = useGetActivitiesQuery({
      //where: {timestamp: {gt: timestamp}}, 
      order: { timestamp: "DESC" } } as any, {
      fetchPolicy: 'network-only',
    });
    return onResult;
  }

  return {
    activities,
    isLoading,
  };
});
