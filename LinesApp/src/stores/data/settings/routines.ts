import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  Routine,
  useGetRoutinesQuery,
  CreateRoutineCmd,
  useCreateRoutineMutation,
  useBroadcastSubscription,
  ModifyRoutineCmd,
  useModifyRoutineMutation,
  useDirectSubscription,
  useActivateRoutineMutation,
  ActivateRoutineCmd,
  DeactivateRoutineCmd,
  useDeactivateRoutineMutation,
  DeleteRoutineCmd,
  useDeleteRoutineMutation,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';
const toast = useToast();
export const useRoutinesStore = defineStore('routines', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const routines = ref<Routine[]>([]);

  const { onResult } = useGetRoutinesQuery();

  onResult((result: any) => {
    routines.value = result?.data?.routines?.items || [];
    isLoading.value = result.loading;
  });

  const { onResult: handleBroadcast } = useBroadcastSubscription();
  handleBroadcast((result) => {
    if (!result.data?.broadcast?.payload) return;
    const broadcast = result.data.broadcast;
    const payload = JSON.parse(result.data.broadcast.payload);
    if (broadcast.eventName === 'ObjectCreated' && payload?.id && payload?.name == 'Routine') {
      getRoutine(payload.id)((result: any) => {
        if (result?.data?.routines?.items?.length == 1) {
          routines.value = [
            ...routines.value.filter((r) => r.id !== payload.id),
            result?.data?.routines?.items[0],
          ];
        }
      });
    } else if (
      broadcast.eventName === 'ObjectModified' &&
      payload?.id &&
      payload?.name == 'Routine'
    ) {
      getRoutine(payload.id)((result: any) => {
        if (result?.data?.routines?.items?.length == 1) {
          routines.value = [
            ...routines.value.filter((r) => r.id !== payload.id),
            result?.data?.routines?.items[0],
          ];
        }
      });
    } else if (broadcast.eventName === 'RoutineActivationChanged' && payload?.id) {
      routines.value = routines.value.map((routine) => {
        if (routine.id === payload.id) {
          return {
            ...routine,
            isActive: payload.active,
          };
        }
        return routine;
      });
    } else if (
      broadcast.eventName === 'ObjectDeleted' &&
      payload?.ids &&
      payload?.name == 'Routine'
    ) {
      routines.value = routines.value.filter((r) => !payload.ids.includes(r.id));
      if (selectedRoutine.value && payload.ids.includes(selectedRoutine.value.id)) {
        clearSelectedRoutine();
      }
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

  const getRoutine = (id: string) => {
    const { onResult } = useGetRoutinesQuery({ where: { id: { eq: id } } } as any, {
      fetchPolicy: 'network-only',
    });
    return onResult;
  };

  const createRoutine = async (payload: CreateRoutineCmd) => {
    const { mutate } = useCreateRoutineMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.createRoutine?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };
  const activateRoutine = async (payload: ActivateRoutineCmd) => {
    const { mutate } = useActivateRoutineMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.activateRoutine?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const deactivateRoutine = async (payload: DeactivateRoutineCmd) => {
    const { mutate } = useDeactivateRoutineMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.deactivateRoutine?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const modifyRoutine = async (payload: ModifyRoutineCmd) => {
    const { mutate } = useModifyRoutineMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyRoutine?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const deleteRoutine = async (payload: DeleteRoutineCmd) => {
    const { mutate } = useDeleteRoutineMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.deleteRoutine?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const selectedRoutine = ref<Routine>({
    id: null,
    name: '',
    description: '',
    isActive: false,
    purposeId: null,
    createdAt: null,
    modifiedAt: null,
    origin: [],
    recurrence: [],
    rolling: null,
    termini: [],
    assignmentCount: null,
    settings: [],
  });
  function clearSelectedRoutine() {
    selectedRoutine.value = {
      id: null,
      name: '',
      description: '',
      isActive: false,
      purposeId: null,
      createdAt: null,
      modifiedAt: null,
      origin: [],
      recurrence: [],
      rolling: null,
      termini: [],
      assignmentCount: null,
      settings: [],
    };
  }

  return {
    routines,
    selectedRoutine,
    clearSelectedRoutine,
    getRoutine,
    createRoutine,
    activateRoutine,
    deactivateRoutine,
    modifyRoutine,
    deleteRoutine,
    isLoading,
  };
});
