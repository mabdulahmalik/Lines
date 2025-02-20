import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  CancelUserInviteCmd,
  Invitation,
  InviteUsersCmd,
  ResendUserInviteCmd,
  useCancelUserInviteMutation,
  useDirectSubscription,
  useGetInvitationQuery,
  useInviteUsersMutation,
  useResendUserInviteMutation,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';

const toast = useToast();

export const useInvitationsStore = defineStore('invitations', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const invitations = ref<Invitation[]>([]);

  const { onResult } = useGetInvitationQuery();

  onResult((result: any) => {
    invitations.value = result?.data?.invitations?.items || [];
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

  // Mutations

  const inviteUsers = async (payload: InviteUsersCmd) => {
    const { mutate } = useInviteUsersMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.inviteUsers?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };
  
  const cancelUserInvite = async (payload: CancelUserInviteCmd) => {
    const { mutate } = useCancelUserInviteMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.cancelUserInvite?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const resendUserInvite = async (payload: ResendUserInviteCmd) => {
    const { mutate } = useResendUserInviteMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.resendUserInvite?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  return {
    invitations,
    isLoading,
    inviteUsers,
    cancelUserInvite,
    resendUserInvite,
  };
});
