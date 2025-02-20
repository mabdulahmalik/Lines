import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  ActivateUserCmd,
  DeactivateUserCmd,
  ModifyUserCmd,
  useActivateUserMutation,
  useBroadcastSubscription,
  useDeactivateUserMutation,
  useDirectSubscription,
  useGetUsersQuery,
  useModifyUserMutation,
  User,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';

const toast = useToast();

export const useUsersStore = defineStore('users', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const users = ref<User[]>([]);

  const { onResult } = useGetUsersQuery();

  onResult((result: any) => {
    users.value = result?.data?.users?.items || [];
    isLoading.value = result.loading;
  });

  const { onResult: handleBroadcast } = useBroadcastSubscription();
  handleBroadcast((result) => {
    if (!result.data?.broadcast?.payload) {
      return;
    }
    const broadcast = result.data.broadcast;
    const payload = JSON.parse(result.data.broadcast.payload);
    if (broadcast.eventName === 'ObjectModified' && payload?.id && payload?.name == 'User') {
      getUser(payload.id)((res: any) => {
        if (res?.data?.users?.items?.length == 1) {
          users.value = [
            ...users.value.filter((p) => p.id !== payload.id),
            res.data.users.items[0],
          ];
          selectedUser.value = res.data.users.items[0];
        }
      });
    } else if (broadcast.eventName === 'UserActivationChanged' && payload?.userId) {
      getUser(payload.userId)((res: any) => {
        if (res?.data?.users?.items?.length == 1) {
          users.value = [
            ...users.value.filter((p) => p.id !== payload.userId),
            res.data.users.items[0],
          ];
          selectedUser.value = res.data.users.items[0];
        }
      });
    }
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

  const getUser = (id: string) => {
    const { onResult } = useGetUsersQuery({ where: { id: { eq: id } } } as any, {
      fetchPolicy: 'network-only',
    });
    return onResult;
  };

  // Mutations
  const modifyUser = async (payload: ModifyUserCmd) => {
    const { mutate } = useModifyUserMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyUser?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const activateUser = async (payload: ActivateUserCmd) => {
    const { mutate } = useActivateUserMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.activateUser?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const deactivateUser = async (payload: DeactivateUserCmd) => {
    const { mutate } = useDeactivateUserMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.deactivateUser?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const selectedUser = ref<User>({
    id: '',
    active: false,
    firstName: '',
    lastName: '',
    email: '',
    roles: null,
    avatar: '',
    identity: '',
    inTraining: false,
    loggedInAt: '',
    registeredAt: '',
    preferences: null,
    passwordChangedAt: '',
    status: null,
    phone: '',
  });
  function clearSelectedUser() {
    selectedUser.value = {
      id: '',
      active: false,
      firstName: '',
      lastName: '',
      email: '',
      roles: null,
      avatar: '',
      identity: '',
      inTraining: false,
      loggedInAt: '',
      registeredAt: '',
      preferences: null,
      passwordChangedAt: '',
      status: null,
      phone: '',
    };
  }

  return {
    users,
    isLoading,
    selectedUser,
    clearSelectedUser,
    getUser,
    modifyUser,
    activateUser,
    deactivateUser,
  };
});
