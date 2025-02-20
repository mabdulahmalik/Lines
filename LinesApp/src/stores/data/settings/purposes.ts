import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  ArchivePurposeCmd,
  CreatePurposeCmd,
  DeletePurposeCmd,
  Purpose,
  RenamePurposeCmd,
  SortPurposeCmd,
  UnarchivePurposeCmd,
  useArchivePurposeMutation,
  useBroadcastSubscription,
  useCreatePurposeMutation,
  useDeletePurposeMutation,
  useDirectSubscription,
  useGetPurposesQuery,
  useRenamePurposeMutation,
  useSortPurposeMutation,
  useUnarchivePurposeMutation,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';

const toast = useToast();

export const usePurposesStore = defineStore('purposes', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const insertOnTop = ref(false);

  const purposes = ref<Purpose[]>([]);

  const { onResult } = useGetPurposesQuery();

  onResult((result: any) => {
    purposes.value = result?.data?.purposes?.items || [];
    isLoading.value = result.loading;
  });

  const { onResult: handleBroadcast } = useBroadcastSubscription();

  handleBroadcast((result) => {
    if (!result.data?.broadcast?.payload) {
      return;
    }
    const broadcast = result.data.broadcast;
    const payload = JSON.parse(result.data.broadcast.payload);
    if (broadcast.eventName === 'ObjectCreated' && payload?.id && payload?.name == 'Purpose') {
      getPurpose(payload.id)((res: any) => {
        if (res?.data?.purposes?.items?.length == 1) {
          const otherPurposes = purposes.value.filter((p) => p.id !== payload.id);

          if (insertOnTop.value) {
            purposes.value = [res.data.purposes.items[0], ...otherPurposes];
          } else {
            purposes.value = [...otherPurposes, res.data.purposes.items[0]];
          }
        }
      });
    } else if (
      broadcast.eventName === 'ObjectModified' &&
      payload?.id &&
      payload?.name == 'Purpose'
    ) {
      getPurpose(payload.id)((res: any) => {
        if (res?.data?.purposes?.items?.length == 1) {
          purposes.value = [
            ...purposes.value.filter((p) => p.id !== payload.id),
            res.data.purposes.items[0],
          ];
        }
      });
    } else if (
      broadcast.eventName === 'ObjectDeleted' &&
      payload?.ids &&
      payload?.name == 'Purpose'
    ) {
      purposes.value = purposes.value.filter((p) => !payload.ids.includes(p.id));
    } else if (
      result.data.broadcast.eventName === 'ObjectArchiveStateChanged' &&
      payload?.ids &&
      payload?.name === 'Purpose'
    ) {
      purposes.value = purposes.value.map((f) => {
        if (payload.ids.includes(f.id)) {
          return {
            ...f,
            archived: !f.archived,
          };
        }
        return f;
      });
    } else if (
      result.data.broadcast.eventName === 'ObjectSorted' &&
      payload?.id &&
      payload?.name === 'Purpose'
    ) {
      const purposesCopy = [...purposes.value];
      const currentIndex = purposesCopy.findIndex((purpose) => purpose.id === payload.id);
      const newIndex = payload.position - 1;
      if (
        currentIndex !== -1 &&
        newIndex >= 0 &&
        newIndex < purposesCopy.length
      ) {
        const [purposeToMove] = purposesCopy.splice(currentIndex, 1);
        purposesCopy.splice(newIndex, 0, purposeToMove);
      }
      purposes.value = purposesCopy;
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

  const getPurpose = (id: string) => {
    const { onResult } = useGetPurposesQuery({ where: { id: { eq: id } } } as any);
    return onResult;
  };

  const createPurpose = async (payload: CreatePurposeCmd) => {
    const { mutate } = useCreatePurposeMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.createPurpose?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
      insertOnTop.value = payload.insertOnTop;
    }
  };
  const renamePurpose = async (payload: RenamePurposeCmd) => {
    const { mutate } = useRenamePurposeMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.renamePurpose?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const deletePurpose = async (payload: DeletePurposeCmd) => {
    const { mutate } = useDeletePurposeMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.deletePurpose?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const archivePurpose = async (payload: ArchivePurposeCmd) => {
    const { mutate } = useArchivePurposeMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.archivePurpose?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const unarchivePurpose = async (payload: UnarchivePurposeCmd) => {
    const { mutate } = useUnarchivePurposeMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.unarchivePurpose?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const sortPurpose = async (payload: SortPurposeCmd) => {
    const { mutate } = useSortPurposeMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.sortPurpose?.correlationId ?? null;
    if (correlationId) {
      currentCorrelationId.value = correlationId;
    }
  };

  return {
    purposes,
    isLoading,
    createPurpose,
    renamePurpose,
    deletePurpose,
    archivePurpose,
    unarchivePurpose,
    getPurpose,
    sortPurpose,
  };
});
