import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  ApplyProcedureToEncounterCmd,
  AssignMeToEncounterCmd,
  AttachPhotosToEncounterCmd,
  AttendToPatientCmd,
  CancelAssistanceRequestForEncounterCmd,
  CompleteEncounterCmd,
  DeescalateEncounterCmd,
  EnactEncounterRevisionPrc,
  Encounter,
  EncounterAlertType,
  EscalateEncounterCmd,
  ModifyProcedureForEncounterCmd,
  PlaceEncounterOnHoldCmd,
  RemoveHoldFromEncounterCmd,
  RemovePhotoFromEncounterCmd,
  RemoveProcedureFromEncounterCmd,
  RequestAssistanceForEncounterCmd,
  SubmitProceduresCmd,
  useApplyProcedureToEncounterMutation,
  useAssignMeToEncounterMutation,
  useAttachPhotosToEncounterMutation,
  useAttendToPatientMutation,
  useBroadcastSubscription,
  useCancelAssistanceRequestForEncounterMutation,
  useCompleteEncounterMutation,
  useDeescalateEncounterMutation,
  useDirectSubscription,
  useEnactEncounterRevisionMutation,
  useEscalateEncounterMutation,
  useGetEncountersQuery,
  useModifyProcedureForEncounterMutation,
  usePlaceEncounterOnHoldMutation,
  useRemoveHoldFromEncounterMutation,
  useRemovePhotoFromEncounterMutation,
  useRemoveProcedureFromEncounterMutation,
  useRequestAssistanceForEncounterMutation,
  useSubmitProceduresMutation,
  useWithdrawMeFromEncounterMutation,
  WithdrawMeFromEncounterCmd,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';

const toast = useToast();

export const useEncountersStore = defineStore('encounters', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const encounters = ref<Encounter[]>([]);

  const { onResult } = useGetEncountersQuery();

  onResult((result: any) => {
    encounters.value = result?.data?.encounters?.items || [];
    isLoading.value = result.loading;
  });

  const { onResult: handleBroadcast } = useBroadcastSubscription();

  handleBroadcast((result) => {
    if (!result.data?.broadcast?.payload) {
      return;
    }
    const broadcast = result.data.broadcast;
    const payload = JSON.parse(result.data.broadcast.payload);

    if (broadcast.eventName === 'ObjectCreated' && payload?.id && payload.name === 'Encounter') {
      getEncounter(payload.id)((result: any) => {
        if (result?.data?.encounters?.items?.length == 1) {
          encounters.value = [
            ...encounters.value.filter((enc) => enc.id !== payload.id),
            result.data.encounters.items[0],
          ];
        }
      });
    } else if (
      broadcast.eventName === 'ObjectModified' &&
      payload?.id &&
      payload.name === 'Encounter'
    ) {
      getEncounter(payload.id)((result: any) => {
        if (result?.data?.encounters?.items?.length == 1) {
          encounters.value = [
            ...encounters.value.filter((enc) => enc.id !== payload.id),
            result.data.encounters.items[0],
          ];
          if (selectedEncounter.value.id) {
            selectedEncounter.value = result.data.encounters.items[0];
          }
        }
      });
    } else if (
      broadcast.eventName === 'ObjectDeleted' &&
      payload?.ids &&
      payload.name === 'Encounter'
    ) {
      encounters.value = encounters.value.filter((e) => e.id !== payload.ids[0]);
      if (selectedEncounter.value.id) {
        clearSelectedEncounter();
      }
    } else if (broadcast.eventName === 'ObjectDeleted' && payload?.ids && payload.name === 'Job') {
      encounters.value = encounters.value.filter((e) => e.jobId !== payload.ids[0]);
      if (selectedEncounter.value.id) {
        clearSelectedEncounter();
      }
    } else if (broadcast.eventName === 'EncounterStatusChanged' && payload?.encounterId) {
      getEncounter(payload.encounterId)((result: any) => {
        if (result?.data?.encounters?.items?.length == 1) {
          encounters.value = [
            ...encounters.value.filter((enc) => enc.id !== payload.encounterId),
            result.data.encounters.items[0],
          ];
          if (selectedEncounter.value.id) {
            selectedEncounter.value = result.data.encounters.items[0];
          }
        }
      });
    } else if (broadcast.eventName === 'EncounterProceduresApplied' && payload?.encounterId) {
      const newProcedures = Array.isArray(payload.procedures) ? payload.procedures : [];
      encounters.value = encounters.value.map((encounter) => {
        if (encounter.id === payload.encounterId) {
          return {
            ...encounter,
            procedures: [...(encounter.procedures || []), ...newProcedures],
          };
        }
        return encounter;
      });
      if (selectedEncounter.value?.id === payload.encounterId) {
        selectedEncounter.value = {
          ...selectedEncounter.value,
          procedures: [...(selectedEncounter.value.procedures || []), ...newProcedures],
        };
      }
    } else if (broadcast.eventName === 'EncounterProceduresModified' && payload?.encounterId) {
      const updatedProcedure = payload.procedures?.[0];
      if (updatedProcedure) {
        encounters.value = encounters.value.map((encounter) => {
          if (encounter.id === payload.encounterId) {
            return {
              ...encounter,
              procedures: [
                ...(encounter.procedures?.filter((p) => p?.id !== updatedProcedure.id) || []),
                updatedProcedure,
              ],
            };
          }
          return encounter;
        });

        if (selectedEncounter.value?.id === payload.encounterId) {
          selectedEncounter.value = {
            ...selectedEncounter.value,
            procedures: [
              ...(selectedEncounter.value.procedures?.filter(
                (p) => p?.id !== updatedProcedure.id
              ) || []),
              updatedProcedure,
            ],
          };
        }
      }
    } else if (broadcast.eventName === 'EncounterProceduresUndone' && payload?.encounterId) {
      encounters.value = encounters.value.map((encounter) => {
        if (encounter.id === payload.encounterId) {
          return {
            ...encounter,
            procedures: (encounter.procedures || []).filter(
              (procedure) => !payload.ids.includes(procedure?.id)
            ),
          };
        }
        return encounter;
      });
      if (selectedEncounter.value?.id === payload.encounterId) {
        selectedEncounter.value = {
          ...selectedEncounter.value,
          procedures: (selectedEncounter.value.procedures || []).filter(
            (procedure) => !payload.ids.includes(procedure?.id)
          ),
        };
      }
    } else if (broadcast.eventName === 'EncounterCliniciansAssigned' && payload?.id) {
      getEncounter(payload.id)((result: any) => {
        if (result?.data?.encounters?.items?.length == 1) {
          encounters.value = [
            ...encounters.value.filter((enc) => enc.id !== payload.id),
            result.data.encounters.items[0],
          ];
          if (selectedEncounter.value.id) {
            selectedEncounter.value = result.data.encounters.items[0];
          }
        }
      });
    } else if (broadcast.eventName === 'EncounterCliniciansWithdrawn' && payload?.id) {
      getEncounter(payload.id)((result: any) => {
        if (result?.data?.encounters?.items?.length == 1) {
          encounters.value = [
            ...encounters.value.filter((enc) => enc.id !== payload.id),
            result.data.encounters.items[0],
          ];
          if (selectedEncounter.value.id) {
            selectedEncounter.value = result.data.encounters.items[0];
          }
        }
      });
    } else if (broadcast.eventName === 'EncounterPhotosAttached' && payload?.id) {
      encounters.value = encounters.value.map((encounter) =>
        encounter.id === payload.id
          ? {
              ...encounter,
              photos: [...(encounter.photos || []), ...payload.photos],
            }
          : encounter
      );
      if (selectedEncounter.value?.id === payload.id) {
        selectedEncounter.value = {
          ...selectedEncounter.value,
          photos: [...(selectedEncounter.value.photos || []), ...payload.photos],
        };
      }
    } else if (broadcast.eventName === 'EncounterPhotosDetached' && payload?.encounterId) {
      const photoIds = Array.isArray(payload.photoIds) ? payload.photoIds : [];
      encounters.value = encounters.value.map((encounter) => {
        if (encounter.id === payload.encounterId) {
          return {
            ...encounter,
            photos: Array.isArray(encounter.photos)
              ? encounter.photos.filter((photo) => !photoIds.includes(photo?.id))
              : [],
          };
        }
        return encounter;
      });
      if (selectedEncounter.value?.id === payload.encounterId) {
        selectedEncounter.value = {
          ...selectedEncounter.value,
          photos: Array.isArray(selectedEncounter.value.photos)
            ? selectedEncounter.value.photos.filter((photo) => !photoIds.includes(photo?.id))
            : [],
        };
      }
    } else if (broadcast.eventName === 'EncounterPriorityChanged' && payload?.encounterId) {
      getEncounter(payload.encounterId)((result: any) => {
        if (result?.data?.encounters?.items?.length == 1) {
          encounters.value = [
            ...encounters.value.filter((enc) => enc.id !== payload.encounterId),
            result.data.encounters.items[0],
          ];
          selectedEncounter.value = result.data.encounters.items[0];
        }
      });
    } else if (broadcast.eventName === 'EncounterAlertAdded' && payload?.encounterId) {
      getEncounter(payload.encounterId)((result: any) => {
        if (result?.data?.encounters?.items?.length == 1) {
          encounters.value = [
            ...encounters.value.filter((enc) => enc.id !== payload.encounterId),
            result.data.encounters.items[0],
          ];
          if (selectedEncounter.value.id) {
            selectedEncounter.value = result.data.encounters.items[0];
          }
        }
      });
    } else if (broadcast.eventName === 'EncounterAlertRemoved' && payload?.encounterId) {
      getEncounter(payload.encounterId)((result: any) => {
        if (result?.data?.encounters?.items?.length == 1) {
          encounters.value = [
            ...encounters.value.filter((enc) => enc.id !== payload.encounterId),
            result.data.encounters.items[0],
          ];
          if (selectedEncounter.value.id) {
            selectedEncounter.value = result.data.encounters.items[0];
          }
        }
      });
    } else if (broadcast.eventName === 'EncounterProgressed' && payload?.encounterId) {
      getEncounter(payload.encounterId)((result: any) => {
        if (result?.data?.encounters?.items?.length == 1) {
          encounters.value = [
            ...encounters.value.filter((enc) => enc.id !== payload.encounterId),
            result.data.encounters.items[0],
          ];
          if (selectedEncounter.value.id) {
            selectedEncounter.value = result.data.encounters.items[0];
          }
        }
      });
    }
  });

  const { onResult: handleDirectSubscription } = useDirectSubscription();
  handleDirectSubscription((result) => {
    console.log('direct result errors', result);
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

  const getEncounter = (id: string) => {
    const { onResult } = useGetEncountersQuery({ where: { id: { eq: id } } } as any, {
      fetchPolicy: 'network-only',
    });
    return onResult;
  };

  // Mutations
  const applyProcedureToEncounter = async (payload: ApplyProcedureToEncounterCmd) => {
    const { mutate } = useApplyProcedureToEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.applyProcedureToEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const assignMeToEncounter = async (payload: AssignMeToEncounterCmd) => {
    const { mutate } = useAssignMeToEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.assignMeToEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const attachPhotosToEncounter = async (payload: AttachPhotosToEncounterCmd) => {
    console.log('attachPhotoToEncounter', payload);
    const { mutate } = useAttachPhotosToEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.attachPhotosToEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const escalateEncounter = async (payload: EscalateEncounterCmd) => {
    const { mutate } = useEscalateEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.escalateEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const deescalateEncounter = async (payload: DeescalateEncounterCmd) => {
    const { mutate } = useDeescalateEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.deescalateEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const modifyProcedureForEncounter = async (payload: ModifyProcedureForEncounterCmd) => {
    const { mutate } = useModifyProcedureForEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyProcedureForEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const placeEncounterOnHold = async (payload: PlaceEncounterOnHoldCmd) => {
    const { mutate } = usePlaceEncounterOnHoldMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.placeEncounterOnHold?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const removeHoldFromEncounter = async (payload: RemoveHoldFromEncounterCmd) => {
    const { mutate } = useRemoveHoldFromEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.removeHoldFromEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const removePhotoFromEncounter = async (payload: RemovePhotoFromEncounterCmd) => {
    const { mutate } = useRemovePhotoFromEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.removePhotoFromEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const removeProcedureFromEncounter = async (payload: RemoveProcedureFromEncounterCmd) => {
    const { mutate } = useRemoveProcedureFromEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.removeProcedureFromEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const withdrawMeFromEncounter = async (payload: WithdrawMeFromEncounterCmd) => {
    const { mutate } = useWithdrawMeFromEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.withdrawMeFromEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const enactEncounterRevision = async (payload: EnactEncounterRevisionPrc) => {
    const { mutate } = useEnactEncounterRevisionMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.enactEncounterRevision?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const requestAssistanceForEncounter = async (payload: RequestAssistanceForEncounterCmd) => {
    const { mutate } = useRequestAssistanceForEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.requestAssistanceForEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const cancelAssistanceRequestForEncounter = async (
    payload: CancelAssistanceRequestForEncounterCmd
  ) => {
    const { mutate } = useCancelAssistanceRequestForEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.cancelAssistanceRequestForEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const attendToPatient = async (payload: AttendToPatientCmd) => {
    const { mutate } = useAttendToPatientMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.attendToPatient?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const submitProcedures = async (payload: SubmitProceduresCmd) => {
    const { mutate } = useSubmitProceduresMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.submitProcedures?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const completeEncounter = async (payload: CompleteEncounterCmd) => {
    const { mutate } = useCompleteEncounterMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.completeEncounter?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const selectedEncounter = ref<Encounter>({
    createdAt: null,
    id: null,
    jobId: null,
    modifiedAt: null,
    priority: null,
    assignments: null,
    photos: null,
    procedures: null,
    lines: [],
    location: null,
    medicalRecordId: null,
    purposeId: null,
    alerts: [],
    progress: [],
    stage: null,
  });
  function clearSelectedEncounter() {
    selectedEncounter.value = {
      createdAt: null,
      id: null,
      jobId: null,
      modifiedAt: null,
      priority: null,
      assignments: null,
      photos: null,
      procedures: null,
      lines: [],
      location: null,
      medicalRecordId: null,
      purposeId: null,
      alerts: [],
      progress: [],
      stage: null,
    };
  }

  return {
    encounters,
    isLoading,
    selectedEncounter,
    clearSelectedEncounter,
    getEncounter,
    applyProcedureToEncounter,
    assignMeToEncounter,
    attachPhotosToEncounter,
    escalateEncounter,
    deescalateEncounter,
    modifyProcedureForEncounter,
    placeEncounterOnHold,
    removeHoldFromEncounter,
    removePhotoFromEncounter,
    removeProcedureFromEncounter,
    withdrawMeFromEncounter,
    enactEncounterRevision,
    requestAssistanceForEncounter,
    cancelAssistanceRequestForEncounter,
    attendToPatient,
    submitProcedures,
    completeEncounter,
  };
});
