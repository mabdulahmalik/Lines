import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  ClearLineInfectionCmd,
  CloseLineCmd,
  EnactLineRevisionPrc,
  Line,
  PlaceLineExternallyCmd,
  PlaceLineInternallyCmd,
  RecordLineInfectionCmd,
  useBroadcastSubscription,
  useClearLineInfectionMutation,
  useCloseLineMutation,
  useDirectSubscription,
  useEnactListRevisionMutation,
  useGetLinesQuery,
  usePlaceLineExternallyMutation,
  usePlaceLineInternallyMutation,
  useRecordLineInfectionMutation,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';

const toast = useToast();

export const useLinesStore = defineStore('lines', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const lines = ref<Line[]>([]);

  const { onResult } = useGetLinesQuery();

  onResult((result: any) => {
    setTimeout(() => {
      lines.value = result?.data?.lines?.items || [];
      isLoading.value = result.loading;
    }, 1000);
  });

  const { onResult: handleBroadcast } = useBroadcastSubscription();

  handleBroadcast((result) => {
    if (!result.data?.broadcast?.payload) {
      return;
    }
    const broadcast = result.data.broadcast;
    const payload = JSON.parse(result.data.broadcast.payload);
    if (broadcast.eventName === 'ObjectCreated' && payload?.id && payload.name === 'Line') {
      getLine(payload.id)((res: any) => {
        if (res?.data?.lines?.items?.length == 1) {
          lines.value = [
            ...lines.value.filter((l) => l.id !== payload.id),
            res.data.lines.items[0],
          ];
        }
      });
    } else if (broadcast.eventName === 'ObjectModified' && payload?.id && payload.name === 'Line') {
      getLine(payload.id)((res: any) => {
        if (res?.data?.lines?.items?.length == 1) {
          lines.value = [
            ...lines.value.filter((l) => l.id !== payload.id),
            res.data.lines.items[0],
          ];
          if (selectedLine.value?.id === payload.id) {
            selectedLine.value = res.data?.lines?.items[0];
          }
        }
      });
    } else if (broadcast.eventName === 'LineInfectionStatusChanged' && payload?.id) {
      lines.value = lines.value.map((line) => {
        if (line.id === payload.id) {
          return {
            ...line,
            infectedOn: payload.hasInfection ? payload.infectedOn : null,
          };
        }
        return line;
      });
      if (selectedLine.value?.id === payload.id) {
        selectedLine.value = {
          ...selectedLine.value,
          infectedOn: payload.hasInfection ? payload.infectedOn : null,
        };
      }
    } else if (broadcast.eventName === 'LinePlacementChanged' && payload?.id) {
      lines.value = lines.value.map((line) => {
        if (line.id === payload.id) {
          return {
            ...line,
            externallyPlaced: payload.externallyPlaced,
            externallyPlacedBy: payload.placedBy,
            insertedOn: payload.placedOn,
          };
        }
        return line;
      });
      if (selectedLine.value?.id === payload.id) {
        selectedLine.value = {
          ...selectedLine.value,
          externallyPlaced: payload.externallyPlaced,
          externallyPlacedBy: payload.placedBy,
          insertedOn: payload.placedOn,
        };
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

  const getLine = (id: string) => {
    const { onResult } = useGetLinesQuery({ where: { id: { eq: id } } } as any, {
      fetchPolicy: 'network-only',
    });
    return onResult;
  };

  // Mutations
  const recordLineInfection = async (payload: RecordLineInfectionCmd) => {
    const { mutate } = useRecordLineInfectionMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.recordLineInfection?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const clearLineInfection = async (payload: ClearLineInfectionCmd) => {
    const { mutate } = useClearLineInfectionMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.clearLineInfection?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const placeLineInternally = async (payload: PlaceLineInternallyCmd) => {
    const { mutate } = usePlaceLineInternallyMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.placeLineInternally?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const placeLineExternally = async (payload: PlaceLineExternallyCmd) => {
    const { mutate } = usePlaceLineExternallyMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.placeLineExternally?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const closeLine = async (payload: CloseLineCmd) => {
    const { mutate } = useCloseLineMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.closeLine?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const enactListRevision = async (payload: EnactLineRevisionPrc) => {
    const { mutate } = useEnactListRevisionMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.enactListRevision?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const selectedLine = ref<Line>({
    followUp: null,
    dwellTime: null,
    insertedOn: null,
    removedOn: null,
    createdAt: null,
    dwelling: null,
    externallyPlaced: null,
    externallyPlacedBy: null,
    infectedOn: null,
    location: null,
    id: null,
    medicalRecordId: null,
    modifiedAt: null,
    name: null,
    type: null,
    insertedWith: null,
    removedWith: null,
  });
  function clearSelectedLine() {
    selectedLine.value = {
      followUp: null,
      dwellTime: null,
      insertedOn: null,
      removedOn: null,
      createdAt: null,
      dwelling: null,
      externallyPlaced: null,
      externallyPlacedBy: null,
      infectedOn: null,
      location: null,
      id: null,
      medicalRecordId: null,
      modifiedAt: null,
      name: null,
      type: null,
      insertedWith: null,
      removedWith: null,
    };
  }

  return {
    lines,
    isLoading,
    selectedLine,
    clearSelectedLine,
    getLine,
    recordLineInfection,
    clearLineInfection,
    placeLineInternally,
    placeLineExternally,
    closeLine,
    enactListRevision,
  };
});
