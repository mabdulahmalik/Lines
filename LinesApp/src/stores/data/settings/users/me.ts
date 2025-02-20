import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  ModifyMyAvatarCmd,
  ModifyMyPreferenceCmd,
  ModifyMyProfileCmd,
  ModifyMyStatusCmd,
  useBroadcastSubscription,
  useDirectSubscription,
  useGetMeQuery,
  useModifyMyAvatarMutation,
  useModifyMyPreferenceMutation,
  useModifyMyProfileMutation,
  useModifyMyStatusMutation,
  User,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';

const toast = useToast();

export const useMeStore = defineStore('me', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const me = ref<User>();

  const { onResult, refetch } = useGetMeQuery();

  onResult((result: any) => {
    me.value = result?.data?.me || [];
    isLoading.value = result.loading;
  });

  const { onResult: handleBroadcast } = useBroadcastSubscription();
  handleBroadcast((result) => {
    if (!result.data?.broadcast?.payload) {
      return;
    }
    const broadcast = result.data.broadcast;
    const payload = JSON.parse(result.data.broadcast.payload);
    if (
      broadcast.eventName === 'ObjectModified' &&
      payload?.id &&
      payload?.name == 'User' &&
      payload?.id === me.value?.id
    ) {
      getMe();
    }
  });

  // Direct subscription
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

  const getMe = () => {
    const { onResult } = useGetMeQuery({fetchPolicy: 'network-only'});
    return onResult;
  };

  // Mutations
  const modifyMyStatus = async (payload: ModifyMyStatusCmd) => {
    const { mutate } = useModifyMyStatusMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyMyStatus?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
    await refetch();
  };

  const modifyMyPreference = async (payload: ModifyMyPreferenceCmd) => {
    const { mutate } = useModifyMyPreferenceMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyMyPreference?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
    await refetch();
  };

  const modifyMyProfile = async (payload: ModifyMyProfileCmd) => {
    const { mutate } = useModifyMyProfileMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyMyProfile?.correlationId ?? null;
    if (correlationId) {
      currentCorrelationId.value = correlationId;
    }
  };

  const modifyMyAvatar = async (payload: ModifyMyAvatarCmd) => {
    const { mutate } = useModifyMyAvatarMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyMyAvatar?.correlationId ?? null;
    if (correlationId) {
      currentCorrelationId.value = correlationId;
    }
  };

  return {
    me,
    isLoading,
    modifyMyStatus,
    modifyMyPreference,
    modifyMyProfile,
    modifyMyAvatar,
  };
});
