import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  Role,
  useDirectSubscription,
  useGetRolesQuery,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';

const toast = useToast();

export const useRolesStore = defineStore('roles', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const roles = ref<Role[]>([]);

  const { onResult } = useGetRolesQuery();

  onResult((result: any) => {
    roles.value = result?.data?.roles?.items || [];
    isLoading.value = result.loading;
  });

  const { onResult: handleDirectSubscription } = useDirectSubscription();
  handleDirectSubscription((result) => {
    if (!result.data?.direct?.payload) return;
    const payload = JSON.parse(result.data.direct.payload);
    const correlationId = result.data.direct.correlationId;
    if (
      result.data.direct.eventName === 'OperationErrored' &&
      correlationId === currentCorrelationId.value
    ) {
      toast.error(`An error occurred at server.`);
      const errors = payload?.errors || [];
      if (errors.length > 0) {
        console.error(errors);
      }
    }
  });

  return {
    roles,
    isLoading,
  };
});
