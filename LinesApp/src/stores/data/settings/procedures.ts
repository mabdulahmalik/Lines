import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  CreateProcedureCmd,
  ModifyProcedureCmd,
  Procedure,
  useCreateProcedureMutation,
  useGetProceduresQuery,
  useModifyProcedureMutation,
  useBroadcastSubscription,
  useDirectSubscription,
  DeleteProcedureCmd,
  useDeleteProcedureMutation,
  useArchiveProcedureMutation,
  ArchiveProcedureCmd,
  UnarchiveProcedureCmd,
  useUnarchiveProcedureMutation,
  SortProcedureCmd,
  useSortProcedureMutation,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';

const toast = useToast();
export const useProceduresStore = defineStore('procedures', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const procedures = ref<Procedure[]>([]);

  const { onResult } = useGetProceduresQuery();

  onResult((result: any) => {
    procedures.value = result?.data?.procedures?.items || [];
    isLoading.value = result.loading;
  });

  const { onResult: handleBroadcast } = useBroadcastSubscription();

  handleBroadcast((result) => {
    if (!result.data?.broadcast?.payload) return;
    const broadcast = result.data.broadcast;
    const payload = JSON.parse(result.data.broadcast.payload);
    if (broadcast.eventName === 'ObjectCreated' && payload?.id && payload?.name == 'Procedure') {
      getProcedure(payload.id)((res: any) => {
        if (res?.data?.procedures?.items?.length == 1) {
          procedures.value = [
            ...procedures.value.filter((proc) => proc.id !== payload.id),
            res.data.procedures.items[0],
          ];
        }
      });
    } else if (
      broadcast.eventName === 'ObjectModified' &&
      payload?.id &&
      payload?.name == 'Procedure'
    ) {
      getProcedure(payload.id)((res: any) => {
        if (res?.data?.procedures?.items?.length == 1 && !res.loading) {
          procedures.value = [
            ...procedures.value.filter((proc) => proc.id !== payload.id),
            res.data.procedures.items[0],
          ];
        }
      });
    } else if (
      broadcast.eventName === 'ObjectDeleted' &&
      payload?.ids &&
      payload?.name == 'Procedure'
    ) {
      procedures.value = procedures.value.filter((p) => !payload.ids.includes(p.id));
    } else if (
      result.data.broadcast.eventName === 'ObjectArchiveStateChanged' &&
      payload?.ids &&
      payload?.name === 'Procedure'
    ) {
      procedures.value = procedures.value.map((f) => {
        if (payload.ids.includes(f.id)) {
          return {
            ...f,
            archived: !f.archived,
          };
        }
        return f;
      });
      if (payload.ids.includes(selectedProcedure.value.id)) {
        selectedProcedure.value = {
          ...selectedProcedure.value,
          archived: payload.archived,
        };
      }
    } else if (
      result.data.broadcast.eventName === 'ObjectSorted' &&
      payload?.id &&
      payload?.name === 'Procedure'
    ) {
      const proceduresCopy = [...procedures.value];
      const currentIndex = proceduresCopy.findIndex((procedure) => procedure.id === payload.id);
      const newIndex = payload.position - 1;
      if (
        currentIndex !== -1 &&
        newIndex >= 0 &&
        newIndex < proceduresCopy.length
      ) {
        const [procedureToMove] = proceduresCopy.splice(currentIndex, 1);
        proceduresCopy.splice(newIndex, 0, procedureToMove);
      }
      procedures.value = proceduresCopy;
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

  const getProcedure = (id: string) => {
    const { onResult } = useGetProceduresQuery({ where: { id: { eq: id } } } as any, {
      fetchPolicy: 'network-only',
    });
    return onResult;
  };

  const createProcedure = async (payload: CreateProcedureCmd) => {
    const { mutate } = useCreateProcedureMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.createProcedure?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const modifyProcedure = async (payload: ModifyProcedureCmd) => {
    const { mutate } = useModifyProcedureMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyProcedure?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
    // await refetch();
  };

  const deleteProcedure = async (payload: DeleteProcedureCmd) => {
    const { mutate } = useDeleteProcedureMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.deleteProcedure?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const archiveProcedure = async (payload: ArchiveProcedureCmd) => {
    const { mutate } = useArchiveProcedureMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.archiveProcedure?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const unarchiveProcedure = async (payload: UnarchiveProcedureCmd) => {
    const { mutate } = useUnarchiveProcedureMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.unarchiveProcedure?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const sortProcedure = async (payload: SortProcedureCmd) => {
    const { mutate } = useSortProcedureMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.sortProcedure?.correlationId ?? null;
    if (correlationId) {
      currentCorrelationId.value = correlationId;
    }
  };

  const selectedProcedure = ref<Procedure>({
    id: null,
    name: '',
    fields: null,
    requiredData: null,
    settings: null,
    createdAt: null,
    modifiedAt: null,
    references: null,
    archived: false,
  });
  function clearSelectedProcedure() {
    selectedProcedure.value = {
      id: null,
      name: '',
      fields: null,
      requiredData: null,
      settings: null,
      createdAt: null,
      modifiedAt: null,
      references: null,
      archived: false,
    };
  }

  return {
    procedures,
    selectedProcedure,
    clearSelectedProcedure,
    createProcedure,
    modifyProcedure,
    deleteProcedure,
    archiveProcedure,
    unarchiveProcedure,
    sortProcedure,
    isLoading,
  };
});
